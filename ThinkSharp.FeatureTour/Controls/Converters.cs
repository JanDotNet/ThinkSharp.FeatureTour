// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ThinkSharp.FeatureTouring.Controls
{
    public class PlacementToAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var border = (BorderSide)parameter;
            var placement = (Placement)value;
            if (placement == Placement.Center ||
                placement == Placement.BottomCenter ||
                placement == Placement.TopCenter ||
                placement == Placement.LeftCenter ||
                placement == Placement.RightCenter)
            {
                if (border == BorderSide.Bottom || border == BorderSide.Top)
                    return HorizontalAlignment.Center;
                return VerticalAlignment.Center;
            }
            if (placement == Placement.BottomLeft ||
                placement == Placement.TopLeft ||
                placement == Placement.LeftTop ||
                placement == Placement.RightTop)
            {
                if (border == BorderSide.Bottom || border == BorderSide.Top)
                    return HorizontalAlignment.Left;
                return VerticalAlignment.Top;
            }

            if (border == BorderSide.Bottom || border == BorderSide.Top)
                return HorizontalAlignment.Right;
            return VerticalAlignment.Bottom;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PlacementToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var border = (BorderSide)parameter;
            var placement = (Placement)value;

            if (placement == Placement.BottomCenter ||
                placement == Placement.BottomLeft ||
                placement == Placement.BottomRight)
                return border == BorderSide.Top ? Visibility.Visible : Visibility.Collapsed;

            if (placement == Placement.TopCenter ||
                placement == Placement.TopLeft ||
                placement == Placement.TopRight)
                return border == BorderSide.Bottom ? Visibility.Visible : Visibility.Collapsed;

            if (placement == Placement.LeftBottom ||
                placement == Placement.LeftCenter ||
                placement == Placement.LeftTop)
                return border == BorderSide.Right ? Visibility.Visible : Visibility.Collapsed;

            if (placement == Placement.RightBottom ||
                placement == Placement.RightCenter ||
                placement == Placement.RightTop)
                return border == BorderSide.Left ? Visibility.Visible : Visibility.Collapsed;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ContentDefaultTemplateIfNullConverter : IValueConverter
    {
        public DataTemplate DefaultTemplate { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var customDataTemplate = value as DataTemplate;
            return customDataTemplate ?? DefaultTemplate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BorderThicknessToInnerPolygonTransform : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parametersAreValid = value is Thickness && parameter is BorderSide;
            if (!parametersAreValid) return 0;

            var thickness = (Thickness)value;
            var borderSide = (BorderSide)parameter;

            var borderThickness = ThicknessToStrokeThickness.GetThicknessForSide(borderSide, thickness);

            if (borderSide == BorderSide.Bottom || borderSide == BorderSide.Right)
                borderThickness *= -1;

            return borderThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ThicknessToStrokeThickness : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parametersAreValid = value is Thickness && parameter is BorderSide;
            if (!parametersAreValid) return 0;

            var thickness = (Thickness)value;
            var borderSide = (BorderSide)parameter;

            return GetThicknessForSide(borderSide, thickness);
        }

        internal static double GetThicknessForSide(BorderSide borderSide, Thickness thickness)
        {
            switch (borderSide)
            {
                case BorderSide.Bottom:
                    return thickness.Bottom;
                case BorderSide.Left:
                    return thickness.Left;
                case BorderSide.Right:
                    return thickness.Right;
                case BorderSide.Top:
                    return thickness.Top;
                default:
                    throw new NotImplementedException($"Case for state '{borderSide}' is not implemeted.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BorderThicknessToContainerMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parametersAreValid = value is Thickness && parameter is BorderSide;
            if (!parametersAreValid) return new Thickness(0);

            var thickness = (Thickness)value;
            var borderSide = (BorderSide)parameter;

            var borderThickness = GetThicknessForSide(borderSide, thickness);

            var marginFromCorner = 10 + borderThickness;

            switch (borderSide)
            {
                case BorderSide.Bottom: // 10, 0, 10, 0
                    return new Thickness(marginFromCorner, -1, marginFromCorner , 0);
                case BorderSide.Left: // 0, 10, 0, 10
                    return new Thickness(0, marginFromCorner, -1, marginFromCorner);
                case BorderSide.Right: // 0, 10, 0, 10
                    return new Thickness(-1, marginFromCorner, 0, marginFromCorner);
                case BorderSide.Top: // 10, 0, 10, 0
                    return new Thickness(marginFromCorner, 0, marginFromCorner, -1);
                default:
                    throw new NotImplementedException($"Case for state '{borderSide}' is not implemeted.");
            }
        }

        internal static double GetThicknessForSide(BorderSide borderSide, Thickness thickness)
        {
            switch (borderSide)
            {
                case BorderSide.Bottom:
                    return thickness.Bottom;
                case BorderSide.Left:
                    return thickness.Left;
                case BorderSide.Right:
                    return thickness.Right;
                case BorderSide.Top:
                    return thickness.Top;
                default:
                    throw new NotImplementedException($"Case for state '{borderSide}' is not implemeted.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
