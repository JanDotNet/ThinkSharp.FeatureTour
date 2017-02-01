using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using ThinkSharp.FeatureTouring.Navigation;

namespace ThinkSharp.FeatureTouring
{
    public class PopupStyle : ViewModelBase
    {
        private Thickness myBorderThickness = new Thickness(3);
        public Thickness BorderThickness
        {
            get { return myBorderThickness; }
            set { Set(() => BorderThickness, ref myBorderThickness, value); }
        }

        public double BorderThicknessValue
        {
            get { return myBorderThickness.Top; }
            set { BorderThickness = new Thickness(value); }
        }

        private double myCornerRadius = 3;
        public double CornerRadius
        {
            get { return myCornerRadius; }
            set { Set(() => CornerRadius, ref myCornerRadius, value); }
        }

        private double myFontSize = 12;
        public double FontSize
        {
            get { return myFontSize; }
            set
            {
                if (Set(() => FontSize, ref myFontSize, value))
                    FeatureTour.GetNavigator().Close();
            }
        }

        private Brush myForeground = new SolidColorBrush(Color.FromRgb(0x04, 0x35, 0x6c));
        public Brush Foreground
        {
            get { return myForeground; }
            set { Set(() => Foreground, ref myForeground, value); }
        }
        public Color? ForegroundColor
        {
            get { return (Foreground as SolidColorBrush)?.Color; }
            set { Foreground = value.HasValue ? new SolidColorBrush(value.Value) : Brushes.Transparent; }
        }

        private Brush myBackground = new SolidColorBrush(Color.FromRgb(0x68, 0x9a, 0xd3));
        public Brush Background
        {
            get { return myBackground; }
            set { Set(() => Background, ref myBackground, value); }
        }
        public Color? BackgroundColor
        {
            get { return (Background as SolidColorBrush)?.Color; }
            set { Background = value.HasValue ? new SolidColorBrush(value.Value) : Brushes.Transparent; }
        }

        private Brush myBorderBrush = new SolidColorBrush(Color.FromRgb(0x27, 0x4f, 0x7d));
        public Brush BorderBrush
        {
            get { return myBorderBrush; }
            set { Set(() => BorderBrush, ref myBorderBrush, value); }
        }
        public Color? BorderBrushColor
        {
            get { return (BorderBrush as SolidColorBrush)?.Color; }
            set { BorderBrush = value.HasValue ? new SolidColorBrush(value.Value) : Brushes.Transparent; }
        }
    }
}
