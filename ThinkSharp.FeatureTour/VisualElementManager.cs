// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using ThinkSharp.FeatureTouring.Controls;
using ThinkSharp.FeatureTouring.Helper;
using ThinkSharp.FeatureTouring.Logging;

namespace ThinkSharp.FeatureTouring
{
    internal class WindowActivationChangedEventArgs : EventArgs
    {
        public WindowActivationChangedEventArgs(Guid windowID, bool showPoup)
        {
            ShowPopup = showPoup;
            WindowID = windowID;
        }
        public bool ShowPopup { get; set; }
        public Guid WindowID { get; private set; }
    }
    internal interface IPlacementAware
    {
        /// <summary>
        /// Gets or sets the desired <see cref="Placement"/> of the pop-up.
        /// </summary>
        Placement Placement { get; set; }

        /// <summary>
        /// Gets or sets the actual <see cref="Placement"/>. The framework may change the desired placement (e.g. if the pop-up can not be placed on left-side).
        /// </summary>
        Placement ActualPlacement { get; set; }
    }

    internal interface IVisualElementManager
    {
        /// <summary>
        /// Gets all <see cref="VisualElement"/> objects managed by the <see cref="VisualElementManager"/>.
        /// </summary>
        /// <param name="includeUnloaded">
        /// true to return also unloaded (currently not visible) element; false to return only loaded (current visible) elements</param>
        /// <returns></returns>
        IEnumerable<VisualElement> GetVisualElements(bool includeUnloaded);

        /// <summary>
        /// Gets the <see cref="VisualElement"/> with the specified <see cref="elementID"/> or null if the element is nor available.
        /// </summary>
        /// <param name="elementID">
        /// The element ID to get the <see cref="VisualElement"/> for.
        /// </param>
        /// <param name="includeUnloaded">
        /// true if also unloaded (not yet visible) elements should be returned; otherwise false.
        /// </param>
        /// <returns></returns>
        VisualElement GetVisualElement(string elementID, bool includeUnloaded);
    }

    internal class VisualElementManager : IVisualElementManager
    {
        private readonly IWindowManager myWindowManager;
        private readonly List<VisualElement> myVisualElements = new List<VisualElement>();
        
        public VisualElementManager(IWindowManager windowManager)
        {
            myWindowManager = windowManager;
            myWindowManager.WindowRemoved += (s, eargs) => myVisualElements.RemoveAll(ve => ve.WindowID == eargs.WindowID);
        }

        IEnumerable<VisualElement> IVisualElementManager.GetVisualElements(bool includeUnloaded)
        {
            foreach (var visualElement in myVisualElements.ToArray())
            {
                FrameworkElement element;
                if (visualElement.TryGetElement(out element))
                {
                    // Do only return loaded elements but do not remove unloaded elements
                    // because it is possible that elements get temporary unloaded (e.g. on
                    // a tab of a TabControl.
                    if (element.IsLoaded || includeUnloaded)
                        yield return visualElement;
                }
            }
        }

        VisualElement IVisualElementManager.GetVisualElement(string elementID, bool includeUnloaded)
        {
            return (this as IVisualElementManager).GetVisualElements(includeUnloaded).FirstOrDefault(e => e.ElementID == elementID);
        }

        
        internal void ElementAdded(FrameworkElement element)
        {
            var visualElement = GetVisualElement(element);
            myVisualElements.Add(visualElement);
        }

        internal void ElementPropertyChanged(UIElement element, Action<UIElement, VisualElement> propertySetter)
        {
            var elementID = TourHelper.GetElementID(element);
            if (string.IsNullOrEmpty(elementID))
                return;

            var visualElement = myVisualElements.FirstOrDefault(e => e.ElementID == elementID);
            if (visualElement != null)
                propertySetter(element, visualElement);
        }

        private VisualElement GetVisualElement(FrameworkElement element)
        {
            var elementID = TourHelper.GetElementID(element);
            var placement = TourHelper.GetPlacement(element);
            var transtionBehavior = TourHelper.GetWindowTransisionBehavior(element);

            RemoveElement(elementID);

            return new VisualElement(element)
            {
                Placement = placement,
                ElementID = elementID,
                WindowTransisionBehavior = transtionBehavior,
                WindowID = myWindowManager.GetWindowID(element, elementID)
            };
        }

        private void RemoveElement(string elementID)
        {
            foreach (var visualElement in myVisualElements.ToArray())
            {
                FrameworkElement element;
                if (!visualElement.TryGetElement(out element) ||
                    visualElement.ElementID == elementID)
                    myVisualElements.Remove(visualElement);
            }
        }
    }

    internal interface IPopupNavigator
    {
        IDisposable MovePopupTo(VisualElement element);
        void StartTour(IPlacementAware viewModel);
        void ShowPopup();
        void HidePopup();
        void ExitTour();
        void UpdatePopupPosition();
    }

    internal class PupupNavigator : IPopupNavigator
    {
        private FeatureTourPopup myPopup;

        private class FeatureTourPopup
        {
            private readonly IPlacementAware myViewModel;
            private readonly Popup myPopup;
            
            private FrameworkElement myElement;

            private CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
            => PlacementHelper.CustomPopupPlacementCallback(popupSize, targetSize, myViewModel?.Placement ?? Placement.TopLeft);

            public FeatureTourPopup(IPlacementAware viewModel)
            {
                myViewModel = viewModel;

                myPopup = new Popup();
                myPopup.AllowsTransparency = true;
                myPopup.Child = new TourControl();
                (myPopup.Child as FrameworkElement).DataContext = myViewModel;
                myPopup.Placement = PlacementMode.Custom;
                myPopup.CustomPopupPlacementCallback += CustomPopupPlacementCallback;
                myPopup.Opened += (s, e) => UpdatePopupPosition();
            }

            public void Open()
            {
                UpdatePopupPosition();
                myPopup.IsOpen = true;   
            }
            public void Close() => myPopup.IsOpen = false;

            public void UpdatePopupPosition()
            {
                // Workaround to move the popup if the window is moved.
                var offset = myPopup.HorizontalOffset;
                myPopup.HorizontalOffset = offset + 1;
                myPopup.HorizontalOffset = offset;
                var vm = myViewModel;
                if (vm != null)
                {
                    var targetFrameworkElement = myPopup.PlacementTarget as FrameworkElement;
                    if (targetFrameworkElement != null && targetFrameworkElement.IsLoaded)
                    {
                        vm.ActualPlacement = myPopup.GetActualPlacement(vm.Placement);
                    }
                    else
                    {
                        myPopup.IsOpen = false;
                    }
                }
            }

            internal void UpdatePlacementTarget(FrameworkElement element)
            {
                DetachEventHandlers();

                myElement = element;

                AttachEventHandlers();

                if (myElement != null)
                    myPopup.PlacementTarget = myElement;
                else
                    Close();
            }

            private void DetachEventHandlers()
            {
                var element = myElement;
                if (element == null) return;

                element.Loaded -= ElementOnLoaded;
                element.Unloaded -= ElementOnUnloaded;

                myElement = null;
            }

            private void AttachEventHandlers()
            {
                var element = myElement;
                if (element == null) return;

                element.Loaded += ElementOnLoaded;
                element.Unloaded += ElementOnUnloaded;
            }

            private void ElementOnUnloaded(object sender, RoutedEventArgs routedEventArgs) => Close();
            private void ElementOnLoaded(object sender, RoutedEventArgs routedEventArgs) => Open();

            internal void Release()
            {
                Close();
                DetachEventHandlers();
            }
        }

        public IDisposable MovePopupTo(VisualElement visualElement)
        {
            HidePopup();
            Log.Debug("MovePopupTo: " + visualElement.ElementID);
            var element = (FrameworkElement)null;
            if (visualElement.TryGetElement(out element))
            {
                myPopup?.UpdatePlacementTarget(element);
            }
            else
            {
                Log.Warn("MovePopupTo: Could not find placement target with element ID: " + visualElement.ElementID);
            }
            return new DisposableAction(ShowPopup);
        }

        public void StartTour(IPlacementAware viewModel)
        {
            myPopup?.Release();
            myPopup = new FeatureTourPopup(viewModel);
        }

        public void ExitTour()
        {
            if (myPopup == null)
            {
                Log.Warn(nameof(ExitTour) + ": Unable to exit tour - tour has not been started.");
            }
            else
            {
                myPopup?.Release();
                myPopup = null;
            }
        }

        public void ShowPopup()
        {
            myPopup?.Open();
        }

        public void HidePopup()
        {
            myPopup?.Close();
        }

        public void UpdatePopupPosition()
        {
            myPopup?.UpdatePopupPosition();
        }
    }

    internal interface IWindowManager
    {
        Guid GetActiveWindowID();
        bool IsParentWindow(Guid parentID, Guid childID);
        Guid GetWindowID(UIElement element, string elementID);

        event EventHandler<WindowActivationChangedEventArgs> WindowActivated;
        event EventHandler<WindowActivationChangedEventArgs> WindowDeactivated;
        event EventHandler<WindowActivationChangedEventArgs> WindowRemoved;
    }

    internal class WindowManager : IWindowManager
    {
        private readonly IPopupNavigator myPopupNavigator;
        private readonly Dictionary<Window, Guid> myReferencedWindows = new Dictionary<Window, Guid>();

        public event EventHandler<WindowActivationChangedEventArgs> WindowActivated;
        public event EventHandler<WindowActivationChangedEventArgs> WindowDeactivated;
        public event EventHandler<WindowActivationChangedEventArgs> WindowRemoved;

        public WindowManager(IPopupNavigator popupNavigator)
        {
            myPopupNavigator = popupNavigator;
        }

        public Guid GetActiveWindowID()
        {
            var current = Application.Current;

            var activeWindow = current?.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            if (activeWindow == null)
                return Guid.Empty;

            Guid windowID;
            if (!myReferencedWindows.TryGetValue(activeWindow, out windowID))
                return Guid.Empty;

            return windowID;
        }

        public bool IsParentWindow(Guid parentID, Guid childID)
        {
            var idxParent = myReferencedWindows.Values.ToList().IndexOf(parentID);
            var idxChild = myReferencedWindows.Values.ToList().IndexOf(childID);

            if (idxParent < 0 || idxChild < 0)
                return false;

            // parent window must be added first and therefore has a lower index.
            return idxParent < idxChild;
        }

        public Guid GetWindowID(UIElement element, string elementID)
        {
            var window = Window.GetWindow(element);
            if (window == null)
            {
                var lastWindow = myReferencedWindows.LastOrDefault();
                if (lastWindow.Key == null)
                    return Guid.Empty;
                return lastWindow.Value;
            }

            Guid guid;
            if (!myReferencedWindows.TryGetValue(window, out guid))
            {
                guid = IsMainWindow(window) ? Guid.Empty : Guid.NewGuid();
                myReferencedWindows.Add(window, guid);
                window.SizeChanged += WindowSizeChanged;
                window.LocationChanged += WindowLocationChanged;
                window.Deactivated += WinDeactivated;
                window.Activated += WinActivated;
                window.Closed += WindowClosed;
            }
            return guid;
        }

        private static bool IsMainWindow(Window window)
        {
            var app = Application.Current;
            return app != null && app.MainWindow == window;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            RemoveReferencedWindow(sender as Window);
        }

        private void RemoveReferencedWindow(Window window)
        {
            if (window != null)
            {
                // remove event handler
                window.SizeChanged -= WindowSizeChanged;
                window.LocationChanged -= WindowLocationChanged;
                window.Deactivated -= WinDeactivated;
                window.Activated -= WinActivated;
                window.Closed -= WindowClosed;

                // remove references (element and window)
                var guid = Guid.Empty;
                if (myReferencedWindows.TryGetValue(window, out guid))
                {
                    myReferencedWindows.Remove(window);
                    WindowRemoved?.Invoke(this, new WindowActivationChangedEventArgs(guid, false));
                }
            }
        }

        private void WindowLocationChanged(object sender, EventArgs e)
        {
            myPopupNavigator.UpdatePopupPosition();
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            myPopupNavigator.UpdatePopupPosition();
        }

        private void WinDeactivated(object sender, EventArgs e)
        {
            // close popup because it is on top (even of windows of other applications)
            var open = OnWindowActivationChanged(sender, WindowDeactivated, false);
            if (open)
                myPopupNavigator.ShowPopup();
            else
                myPopupNavigator.HidePopup();
        }

        private void WinActivated(object sender, EventArgs e)
        {
            var open = OnWindowActivationChanged(sender, WindowActivated, true);
            if (open)
                myPopupNavigator.ShowPopup();
            else
                myPopupNavigator.HidePopup();
        }

        private bool OnWindowActivationChanged(object sender, EventHandler<WindowActivationChangedEventArgs> handler, bool showPopup)
        {
            var h = handler;
            if (h == null)
                return false;

            var window = sender as Window;
            Guid guid;
            if (myReferencedWindows.TryGetValue(window, out guid))
            {
                var args = new WindowActivationChangedEventArgs(guid, showPopup);
                h(this, args);
                return args.ShowPopup;
            }
            return showPopup;
        }
    }
}
