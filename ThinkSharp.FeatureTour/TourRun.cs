// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using ThinkSharp.FeatureTouring.Logging;
using ThinkSharp.FeatureTouring.Models;
using ThinkSharp.FeatureTouring.Navigation;
using ThinkSharp.FeatureTouring.ViewModels;

namespace ThinkSharp.FeatureTouring
{
    /// <summary>
    /// Interface for the TourViewModel to access the run.
    /// </summary>
    public interface ITourRun
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="includeUnloaded"></param>
        /// <returns></returns>
        bool NextStep(bool includeUnloaded);
        bool PreviousStep();
        void Close();
        void DoIt();

        bool CanNextStep();
        bool CanDoIt();

        Step CurrentStep { get; }
    }

    internal class TourRun : ITourRun
    {
        private readonly Tour myTour;
        private readonly IVisualElementManager myVisualElementManager;
        private readonly IWindowManager myWindowManager;
        private readonly IPopupNavigator myPopupNavigator;
        private TourViewModel myTourViewModel;
        private StepNode myCurrentStepNode = null;
        private Guid myCurrentWindowID = Guid.Empty;

        enum HandleWindowTransitionResult { ShowPopup, HidePopup, DoNothing }

        //  .ctor
        // ////////////////////////////////////////////////////////////////////

        internal TourRun(Tour tour, IVisualElementManager visualElementManager, IWindowManager windowManager, IPopupNavigator popupNavigator)
        {
            if (tour == null) throw new ArgumentNullException("tour");
            if (tour.Steps == null) throw new ArgumentNullException("tour.Steps");
            if (tour.Steps.Length == 0) throw new ArgumentException("Unable to start tour without steps");
            if (tour.Steps.Any(s => s == null)) throw new ArgumentException("Steps must not be null");
            if (tour.Steps.Any(s => s.ElementID == null)) throw new ArgumentException("Step.ElementID must not be null");
            if (visualElementManager == null) throw new ArgumentNullException("visualElementManager");
            if (windowManager == null) throw new ArgumentNullException("windowManager");
            if (popupNavigator == null) throw new ArgumentNullException("popupNavigator");

            myTour = tour;
            myVisualElementManager = visualElementManager;
            myWindowManager = windowManager;
            myPopupNavigator = popupNavigator;

            windowManager.WindowActivated += WindowActivated;
            windowManager.WindowDeactivated += WindowDeactivated;
            myCurrentWindowID = windowManager.GetActiveWindowID();
            InitStepNodes();
        }
        
        // Methods
        // //////////////////////////////////////////////////////////////////////
        
        private void InitStepNodes()
        {
            StepNode prevStepNode = null;
            var counter = 1;
            foreach (var step in myTour.Steps)
            {
                var stepNode = new StepNode(step)
                {
                    Previous = prevStepNode,
                    StepNo = counter++
                };
                if (prevStepNode == null)
                    myCurrentStepNode = stepNode;
                else
                    prevStepNode.Next = stepNode;
                prevStepNode = stepNode;
            }
        }

        private void WindowDeactivated(object sender, WindowActivationChangedEventArgs e)
        {
        }

        private void WindowActivated(object sender, WindowActivationChangedEventArgs e)
        {
            // Window changed
            var previousWindowID = myCurrentWindowID;
            if (e.WindowID != myCurrentWindowID)
            {
                myCurrentWindowID = e.WindowID;
                var result = HandleWindowTransitionChange(previousWindowID);
                switch (result)
                {
                    case HandleWindowTransitionResult.HidePopup:
                        e.ShowPopup = false;
                        break;
                    case HandleWindowTransitionResult.ShowPopup:
                        e.ShowPopup = true;
                        break;
                }
            }
        }

        private HandleWindowTransitionResult HandleWindowTransitionChange(Guid previousWindowID)
        {
            // If current element is already on the new window, we don't need to go.
            // CASE: Window is just reactivated
            var currentElement = myVisualElementManager.GetVisualElement(myCurrentStepNode.Step.ElementID, false);
            if (currentElement == null)
            {
                Log.Warn("Could not find visual element with ElementID '" + myCurrentStepNode.Step.ElementID + "'");
                return HandleWindowTransitionResult.DoNothing;
            }
            if (currentElement.WindowID == myCurrentWindowID)
                return HandleWindowTransitionResult.ShowPopup;

            var behavior = currentElement.WindowTransisionBehavior;
            if (behavior == WindowTransisionBehavior.None)
            {
                return HandleWindowTransitionResult.DoNothing;
            }
            if (behavior == WindowTransisionBehavior.Automatic)
            {
                var wasPreviousParent = myWindowManager.IsParentWindow(previousWindowID, myCurrentWindowID);
                behavior = wasPreviousParent ? WindowTransisionBehavior.NextHide : WindowTransisionBehavior.NextPreviousHide;
            }

            if (behavior == WindowTransisionBehavior.NextHide ||
                behavior == WindowTransisionBehavior.NextPreviousHide)
            {
                // otherwise, we will try to to the next element on the new window
                // CASE: Open modal dialog with elements
                var nextStep = myCurrentStepNode.NextStep;
                if (nextStep != null)
                {
                    var nextElement = myVisualElementManager.GetVisualElement(nextStep.ElementID, true);
                    // next element belongs to the new window
                    if (nextElement != null && nextElement.WindowID == myCurrentWindowID)
                    {
                        NextStep(true);
                        return HandleWindowTransitionResult.ShowPopup;
                    }
                }
            }

            if (behavior == WindowTransisionBehavior.PreviousHide ||
                behavior == WindowTransisionBehavior.NextPreviousHide)
            {
                // otherwise we will try to go to the nears previous step for the current window
                // CASE: Open modal dialog but do not pass all steps on that dialog and close the dialog
                StepNode prevStepNode = myCurrentStepNode;
                while ((prevStepNode = prevStepNode.Previous) != null)
                {
                    var prevElement = myVisualElementManager.GetVisualElement(prevStepNode.Step.ElementID, true);
                    if (prevElement == null || prevElement.WindowID != myCurrentWindowID)
                        continue;
                    SetStep(prevStepNode);
                    return HandleWindowTransitionResult.ShowPopup;
                }
            }

            // Otherwise do not show the popup because we have no meaningful content for the current window.
            return HandleWindowTransitionResult.HidePopup;
        }

        internal bool Start()
        {
            var factoryMethod = FeatureTour.ViewModelFactoryMethod;
            myTourViewModel = factoryMethod == null 
                ? new TourViewModel(this) 
                : factoryMethod(this);
            
            myPopupNavigator.StartTour(myTourViewModel);

            // Go to first step
            var success = SetStep(myCurrentStepNode);
            if (!success) Close();
            return success;
        }

        private string GetSteps()
        {
            return $"Step {myCurrentStepNode.StepNo}/{myTour.Steps.Length}";
        }

        private bool SetStep(StepNode nextStep, bool includeUnloaded = false)
        {
            if (nextStep == null)
            {
                Log.Debug("SetStep: nextStep is null");
                return false;
            }
            Log.Debug("SetStep: '" + nextStep.Step.ID + "'");

            if (myCurrentStepNode != nextStep)
            {
                FeatureTour.OnStepLeaved(myCurrentStepNode.Step);
                myCurrentStepNode = nextStep;
            }
            var step = myCurrentStepNode.Step;
            FeatureTour.OnStepEntering(step);

            var app = Application.Current;

            VisualElement element;
            if (app == null)
                element = myVisualElementManager.GetVisualElement(step.ElementID, includeUnloaded);
            else
                element = app.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, new Func<object>(() => myVisualElementManager.GetVisualElement(step.ElementID, includeUnloaded))) as VisualElement;
            if (element == null)
            {
                LogWarningCouldNotFindElementFor(step);
                return false;
            }

            using (myPopupNavigator.MovePopupTo(element))
            {
                InitializeViewModel(step, element);

                // required to ensure that the view is updated before the popup is shown
                // otherwise the update is visible in the popup (which looks ugly)
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));
                
                FeatureTour.OnStepEntered(step);
            }

            return true;
        }

        private static void LogWarningCouldNotFindElementFor(Step step)
        {
            var msg = "Could not find visual element with ElementID '" + step.ElementID + "'. Popup may not occur.";
            msg += " Ensure that the visual element is available in the current view.";
            Log.Warn(msg);
        }

        private void InitializeViewModel(Step step, VisualElement element)
        {
            Debug.Assert(myTourViewModel != null, "myTourViewModel != null");
            myTourViewModel.Header = step.Header;
            myTourViewModel.Content = step.Content;
            myTourViewModel.ContentTemplate = element.GetTemplate(step.ContentDataTemplateKey);
            myTourViewModel.HeaderTemplate = element.GetTemplate(step.HeaderDataTemplateKey);
            myTourViewModel.Steps = GetSteps();
            myTourViewModel.CurrentStepNo = myCurrentStepNode.StepNo;
            myTourViewModel.TotalStepsCount = myTour.Steps.Length;
            myTourViewModel.ShowDoIt = ShowDoIt();
            myTourViewModel.ShowNext = step.ShowNextButton ?? myTour.ShowNextButtonDefault;

            if (myCurrentStepNode.Next == null)
                myTourViewModel.SetCloseText();
            myTourViewModel.Placement = element.Placement;
        }

        // Properties (for testing)
        // ////////////////////////////////////////////////////////////////////

        public Step CurrentStep => myCurrentStepNode.Step;
        internal int CurrentStepNo => myCurrentStepNode.StepNo;

        // Interface implementations
        // ////////////////////////////////////////////////////////////////////

        #region ITutorialRun

        public bool NextStep(bool includeUnloaded = false)
        {
            return SetStep(myCurrentStepNode.Next, includeUnloaded);
        }

        public bool PreviousStep()
        {
            return SetStep(myCurrentStepNode.Previous);
        }

        public void Close()
        {
            myPopupNavigator.ExitTour();
            myWindowManager.WindowActivated -= WindowActivated;
            myWindowManager.WindowDeactivated -= WindowDeactivated;

            FeatureTour.OnStepLeaved(myCurrentStepNode.Step);
            FeatureTour.OnClosed(myCurrentStepNode.Step);
            FeatureTour.SetCurrentRunNull();
        }

        internal bool CanPreviousStep()
        {
            return CanGoToStep(myCurrentStepNode.PreviousStep);
        }

        public bool CanNextStep()
        {
            return CanGoToStep(myCurrentStepNode.NextStep);
        }

        private bool CanGoToStep(Step step)
        {
            if (step == null)
                return false;

            if (myTour.EnableNextButtonAlways)
                return true;

            // step entering is usually used to create a state where the visual element is available.
            if (FeatureTour.HasStepEnteringAttached(step))
                return true;

            var visualElement = myVisualElementManager.GetVisualElement(step.ElementID, true);
            if (visualElement == null || visualElement.WindowID != myCurrentWindowID)
                return false;

            return true;
        }

        public void DoIt()
        {
            FeatureTour.Do(myCurrentStepNode.Step);
        }

        public bool CanDoIt()
        {
            return FeatureTour.CanDo(myCurrentStepNode.Step);
        }

        public bool ShowDoIt()
        {
            return FeatureTour.HasDoableAttached(myCurrentStepNode.Step);
        }

        #endregion
    }
}
