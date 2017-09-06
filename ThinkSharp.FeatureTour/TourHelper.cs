// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Windows;
using System.Windows.Controls;
using ThinkSharp.FeatureTouring.Navigation;

namespace ThinkSharp.FeatureTouring
{
    /// <summary>
    /// Provides attached properties to configure XAML elements to be used with FeatureTour.
    /// </summary>
    public static class TourHelper
    {
        private static readonly VisualElementManager theVisualElementManager;
        private static readonly IPopupNavigator thePopupNavigator;
        private static readonly IWindowManager theWindowManager;

        static TourHelper()
        {
            thePopupNavigator = new PupupNavigator();
            theWindowManager = new WindowManager(thePopupNavigator);
            theVisualElementManager = new VisualElementManager(theWindowManager);
        }

        #region ElementID

        /// <summary>
        /// Helper for reading ElementID property from a UIElement.
        /// </summary>
        /// <param name="obj">UIElement to read ElementID property from.</param>
        public static string GetElementID(UIElement obj)
        {
            return (string)obj.GetValue(ElementIDProperty);
        }

        /// <summary>
        /// Helper for setting ElementID property on a UIElement.
        /// </summary>
        /// <param name="obj">UIElement to set ElementID property on.</param>
        /// <param name="value">ElementID property value.</param>
        public static void SetElementID(UIElement obj, string value)
        {
            obj?.SetValue(ElementIDProperty, value);
        }

        /// <summary>
        /// ElementID property. This is an attached property.
        /// ElementID can be used to mark visual elements and associate them with the popup.
        /// </summary>
        public static readonly DependencyProperty ElementIDProperty =
            DependencyProperty.RegisterAttached("ElementID", typeof(string), typeof(TourHelper), 
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.None, (s, e) =>
            {
                var element = s as FrameworkElement;
                if (element == null) throw new InvalidOperationException("ElementID is only valid for objects of type FrameworkElement");
                theVisualElementManager.ElementAdded(element);
            }));

        #endregion

        #region Placement

        /// <summary>
        /// Helper for reading Placement property from a UIElement.
        /// </summary>
        /// <param name="obj">UIElement to read Placement property from.</param>
        public static Placement GetPlacement(UIElement obj)
        {
            return (Placement)obj.GetValue(PlacementProperty);
        }

        /// <summary>
        /// Helper for setting Placement property on a UIElement.
        /// </summary>
        /// <param name="obj">UIElement to set Placement property on.</param>
        /// <param name="value">Placement property value.</param>
        public static void SetPlacement(UIElement obj, Placement value)
        {
            obj.SetValue(PlacementProperty, value);
        }

        /// <summary>
        /// Placement property. This is an attached property.
        /// Placement can be used to define the popup placement for a visual element.
        /// </summary>
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.RegisterAttached("Placement", typeof(Placement), typeof(TourHelper), new PropertyMetadata(Placement.TopLeft, (s, e) =>
            {
                var element = s as UIElement;
                if (element != null) theVisualElementManager.ElementPropertyChanged(element, (el, ve) => ve.Placement = GetPlacement(el));
            }));

        #endregion

        #region WindowTransisionBehavior

        /// <summary>
        /// Helper for reading WindowTransisionBehavior property from a UIElement.
        /// </summary>
        /// <param name="obj">UIElement to read Placement property from.</param>
        public static WindowTransisionBehavior GetWindowTransisionBehavior(UIElement obj)
        {
            return (WindowTransisionBehavior)obj.GetValue(WindowTransisionBehaviorProperty);
        }

        /// <summary>
        /// Helper for setting WindowTransisionBehavior property on a UIElement.
        /// </summary>
        /// <param name="obj">UIElement to set WindowTransisionBehavior property on.</param>
        /// <param name="value">WindowTransisionBehavior property value.</param>
        public static void SetWindowTransisionBehavior(UIElement obj, WindowTransisionBehavior value)
        {
            obj.SetValue(WindowTransisionBehaviorProperty, value);
        }

        /// <summary>
        /// WindowTransisionBehavior property. This is an attached property.
        /// WindowTransisionBehavior can be used to define the tour behavior if another window gets activated.
        /// </summary>
        public static readonly DependencyProperty WindowTransisionBehaviorProperty =
            DependencyProperty.RegisterAttached("WindowTransisionBehavior", typeof(WindowTransisionBehavior), typeof(TourHelper), new PropertyMetadata(WindowTransisionBehavior.Automatic, (s, e) =>
            {
                var element = s as UIElement;
                if (element != null) theVisualElementManager.ElementPropertyChanged(element, (el, ve) => ve.WindowTransisionBehavior = GetWindowTransisionBehavior(el));
            }));

        #endregion
        
        #region static manager properties

        internal static IVisualElementManager VisualElementManager => theVisualElementManager;
        internal static IPopupNavigator PopupNavigator => thePopupNavigator;
        internal static IWindowManager WindowManager => theWindowManager;

        #endregion
    }
}
