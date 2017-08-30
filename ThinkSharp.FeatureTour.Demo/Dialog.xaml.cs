// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ThinkSharp.FeatureTouring.Navigation;

namespace ThinkSharp.FeatureTouring
{
    public partial class Dialog
    {
        public Dialog()
        {
            InitializeComponent();
        }

        private void ButtonBaseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonMeToOnClick(object sender, RoutedEventArgs e)
        {
            var tour = FeatureTour.GetNavigator();

            tour.IfCurrentStepEquals(ElementID.ButtonPushMeToo).GoNext();
        }
    }
}
