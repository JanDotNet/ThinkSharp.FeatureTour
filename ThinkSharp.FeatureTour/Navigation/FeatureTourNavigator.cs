// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics;
using ThinkSharp.FeatureTouring.Models;
using ThinkSharp.FeatureTouring.ViewModels;
using ThinkSharp.FeatureTouring.Logging;

namespace ThinkSharp.FeatureTouring.Navigation
{
    /// <summary>
    /// Abstracts the navigation object for controling the current tour.
    /// </summary>
    public interface IFeatureTourNavigator
    {
        /// <summary>
        /// Returns an object providing navigation (next step, previous step, close tour) if the current step has the specified ste ID.
        /// </summary>
        /// <param name="stepID">
        /// The step ID</param>
        /// <returns>
        /// <see cref="ITourNavigator"/> object that provides navigation methods.
        /// </returns>
        ITourNavigator IfCurrentStepEquals(string stepID);

        /// <summary>
        /// Returns an object for attaching doable actions for the specified step ID.
        /// </summary>
        /// <param name="stepID">
        /// The step ID.
        /// </param>
        /// <returns>
        /// <see cref="ITourDoable"/> object that for attaching doables.
        /// </returns>
        ITourDoable ForStep(string stepID);

        /// <summary>
        /// Returns an object for executing custom logic before the step with the speicified step ID was entered.
        /// </summary>
        /// <param name="stepID">
        /// The step ID.
        /// </param>
        /// <returns>
        /// <see cref="ITourExecution"/> object for executing custom logic.
        /// </returns>
        ITourExecution OnStepEntering(string stepID);

        /// <summary>
        /// Returns an object for executing custom logic after the step with the speicified step ID was entered.
        /// </summary>
        /// <param name="stepID">
        /// The step ID.
        /// </param>
        /// <returns>
        /// <see cref="ITourExecution"/> object for executing custom logic.
        /// </returns>
        ITourExecution OnStepEntered(string stepID);

        /// <summary>
        /// Returns an object for executing custom logic after the step with the speicified step ID was left.
        /// </summary>
        /// <param name="stepID">
        /// The step ID.
        /// </param>
        /// <returns>
        /// <see cref="ITourExecution"/> object for executing custom logic.
        /// </returns>
        ITourExecution OnStepLeft(string stepID);

        /// <summary>
        /// Returns an object for executing custom logic if the tour was closed.
        /// </summary>
        /// <returns>
        /// <see cref="ITourExecution"/> object for executing custom logic.
        /// </returns>
        ITourExecution OnClosed();

        /// <summary>
        /// Closes the current tour.
        /// </summary>
        /// <returns>
        /// true if the current run was active and has been closed; otherwise false.
        /// </returns>
        bool Close();
    }

    /// <summary>
    /// Entry point for controlling the tour programmatically.
    /// Use <see cref="GetNavigator"/> to get the <see cref="IFeatureTourNavigator"/> object.
    /// </summary>
    public class FeatureTour : IFeatureTourNavigator
    {
        private static ITourRun theCurrentTourRun = null;

        private static readonly ITourNavigator theNullNavigator = new NullTourNavigator();
        private static readonly ITourExecution theNullExecution = new NullTourExecution();
        private static readonly ITourDoable theNullDoable = new NullTourDoable();

        private static readonly ActionRepository theExecutionRepository = new ActionRepository();
        private static readonly ActionRepository theDoableRepository = new ActionRepository();

        private const string STEP_ENTERED = "OnStepEnterd";
        private const string STEP_ENTERING = "OnStepEntering";
        private const string STEP_LEAVED = "OnStepLeaved";
        private const string DOABLE = "Doable";
        private const string CLOSED = "OnClosed";

        internal static Func<ITourRun, TourViewModel> ViewModelFactoryMethod { get; private set; }


        // public interface
        // //////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the <see cref="IFeatureTourNavigator"/> object that can be used to control navigation.
        /// </summary>
        /// <returns></returns>
        public static IFeatureTourNavigator GetNavigator()
        {
            return new FeatureTour();
        }

        /// <summary>
        /// Sets a factory method to use subclassed <see cref="TourViewModel"/>s with additional
        /// properties / behaviors. May be meaningfull in combination with custom templates.
        /// </summary>
        /// <param name="factoryMethod">
        /// The factory method to create view model or null to use the default one.
        /// </param>
        public static void SetViewModelFactoryMethod(Func<ITourRun, TourViewModel> factoryMethod)
        {
            ViewModelFactoryMethod = factoryMethod;
        }

        /// <summary>
        /// Returns an object providing navigation (next step, previous step, close tour) if the current step has the specified ste ID.
        /// </summary>
        /// <param name="stepID">
        /// The step ID</param>
        /// <returns>
        /// <see cref="ITourNavigator"/> object that provides navigation methods.
        /// </returns>
        public ITourNavigator IfCurrentStepEquals(string stepID)
        {
            var currentRun = theCurrentTourRun;
            if (currentRun == null || currentRun.CurrentStep == null || currentRun.CurrentStep.ID != stepID) 
                return theNullNavigator;
            return new TourNavigator(currentRun);
        }

        /// <summary>
        /// Returns an object for attaching doable actions for the specified step ID.
        /// </summary>
        /// <param name="stepID">
        /// The step ID.
        /// </param>
        /// <returns>
        /// <see cref="ITourDoable"/> object that for attaching doables.
        /// </returns>
        public ITourDoable ForStep(string stepID)
        {
            if (string.IsNullOrEmpty(stepID))
                return theNullDoable;
            return new TourDoable(theDoableRepository, GetName(stepID, DOABLE));
        }

        /// <summary>
        /// Returns an object for executing custom logic before the step with the speicified step ID was entered.
        /// </summary>
        /// <param name="stepID">
        /// The step ID.
        /// </param>
        /// <returns>
        /// <see cref="ITourExecution"/> object for executing custom logic.
        /// </returns>
        public ITourExecution OnStepEntering(string stepID)
        {
            if (string.IsNullOrEmpty(stepID))
                return theNullExecution;
            return new TourExecution(theExecutionRepository, GetName(stepID, STEP_ENTERING));
        }

        /// <summary>
        /// Returns an object for executing custom logic after the step with the speicified step ID was entered.
        /// </summary>
        /// <param name="stepID">
        /// The step ID.
        /// </param>
        /// <returns>
        /// <see cref="ITourExecution"/> object for executing custom logic.
        /// </returns>
        public ITourExecution OnStepEntered(string stepID)
        {
            if (string.IsNullOrEmpty(stepID))
                return theNullExecution;
            return new TourExecution(theExecutionRepository, GetName(stepID, STEP_ENTERED));
        }

        /// <summary>
        /// Returns an object for executing custom logic after the step with the speicified step ID was left.
        /// </summary>
        /// <param name="stepID">
        /// The step ID.
        /// </param>
        /// <returns>
        /// <see cref="ITourExecution"/> object for executing custom logic.
        /// </returns>
        public ITourExecution OnStepLeft(string stepID)
        {
            if (string.IsNullOrEmpty(stepID))
                return theNullExecution;
            return new TourExecution(theExecutionRepository, GetName(stepID, STEP_LEAVED));
        }

        /// <summary>
        /// Returns an object for executing custom logic if the tour was closed.
        /// </summary>
        /// <returns>
        /// <see cref="ITourExecution"/> object for executing custom logic.
        /// </returns>
        public ITourExecution OnClosed()
        {
            return new TourExecution(theExecutionRepository, CLOSED);
        }
        
        /// <summary>
        /// Closes the current tour.
        /// </summary>
        /// <returns>
        /// true if the current run was active and has been closed; otherwise false.
        /// </returns>
        public bool Close()
        {
            var currentRun = theCurrentTourRun;
            if (currentRun == null) return false;
            currentRun.Close();
            return true;
        }


        // internal interface
        // //////////////////////////////////////////////////////////////////////

        internal static void SetTourRun(ITourRun run)
        {
            var currentRun = theCurrentTourRun;
            currentRun?.Close();
            theCurrentTourRun = run;
        }

        internal static Step CurrentStep
        {
            get
            {
                var currentRun = theCurrentTourRun;
                if (currentRun == null) return null;
                return currentRun.CurrentStep;
            }
        }

        internal static void OnStepEntering(Step step)
        {
            if (step == null) return;
            var name = GetName(step.ID, STEP_ENTERING);
            Log.Debug("OnStepEntering: '" + name + "'");
            theExecutionRepository.Execute(name, step);
        }

        internal static void OnStepEntered(Step step)
        {
            if (step == null) return;
            var name = GetName(step.ID, STEP_ENTERED);
            Log.Debug("OnStepEntered: '" + name + "'");
            theExecutionRepository.Execute(name, step);
        }

        internal static void OnStepLeaved(Step step)
        {
            if (step == null) return;
            var name = GetName(step.ID, STEP_LEAVED);
            Log.Debug("OnStepLeaved: '" + name + "'");
            theExecutionRepository.Execute(name, step);
        }

        internal static void OnClosed(Step step)
        {
            Log.Debug("OnClosed");
            theExecutionRepository.Execute(CLOSED, step);
        }

        internal static void SetCurrentRunNull()
        {
            theCurrentTourRun = null;
        }

        private static string GetName(string stepID, string postfix)
        {
            return string.Format("stepChange_{0}_{1}", stepID, postfix);
        }

        internal static void Do(Step step)
        {
            var name = GetName(step.ID, DOABLE);
            Log.Debug("Do: '" + name + "'");
            theDoableRepository.Execute(name, step);
        }

        internal static bool CanDo(Step step)
        {
            return theDoableRepository.CanExecute(GetName(step.ID, DOABLE), step);
        }

        internal static bool HasDoableAttached(Step step)
        {
            return theDoableRepository.Contains(GetName(step.ID, DOABLE));
        }
    }
}
