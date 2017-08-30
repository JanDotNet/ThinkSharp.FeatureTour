// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ThinkSharp.FeatureTouring.Models;

namespace ThinkSharp.FeatureTouring.Controls
{
    public sealed class TourControl : Control
    {
        static TourControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (TourControl), new FrameworkPropertyMetadata(typeof (TourControl)));

            AddOwner(
                BackgroundProperty,
                BorderBrushProperty,
                BorderThicknessProperty,
                DataContextProperty,
                FontFamilyProperty,
                FontStretchProperty,
                FontStyleProperty,
                FontSizeProperty,
                FontWeightProperty);
        }

        private static void AddOwner(params DependencyProperty[] depdencyProperties)
        {
            foreach (var dp in depdencyProperties)
                dp.AddOwner(typeof(TourControl), new FrameworkPropertyMetadata { Inherits = false });
        }

        /// <summary>
        /// Gets or sets the content that is shown in the content area of the popup.
        /// Use the <see cref="Step.Content"/> to define the content to show. The default template shows the string representation of the content as text.
        /// Use the <see cref="Step.ContentDataTemplateKey"/> to define a data template for displaying the content.
        /// </summary>
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(TourControl), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the header that is shown in the header area of the popup.
        /// Use <see cref="Step.Header"/> to define the header to show. The default template shows the string representation of the header as text.
        /// Use <see cref="Step.HeaderDataTemplateKey"/> to define a data template for displaying the header.
        /// </summary>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(TourControl), new PropertyMetadata(string.Empty));
        
        public Placement Placement
        {
            get { return (Placement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register("Placement", typeof(Placement), typeof(TourControl), new PropertyMetadata(Placement.LeftBottom));
        
        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(TourControl), new PropertyMetadata(3.0));
    }

    public enum BorderSide
    {
        Left,
        Top,
        Right,
        Bottom,
    }
}
