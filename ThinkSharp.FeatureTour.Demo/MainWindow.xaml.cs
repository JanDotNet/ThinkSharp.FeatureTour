// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Windows.Controls;
using ThinkSharp.FeatureTouring.Navigation;

namespace ThinkSharp.FeatureTouring
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = MainWindowViewModel.Instance;

            myTbResult.TextChanged += TbResultTextChanged;
            myTbPath.TextChanged += TbPathTextChanged;
            myCbOptions.SelectionChanged += CbOptionsSelectionChanged;

            var navigator = FeatureTour.GetNavigator();

            navigator.OnStepEntering(ElementID.Rectangle).Execute(s =>
            {
                (DataContext as MainWindowViewModel).Placement = (Placement) s.Tag;
                TabControl.SelectedIndex = 0;

            });
            navigator.OnStepEntering(ElementID.TextBoxResult).Execute(s => TabControl.SelectedIndex = 1);
            navigator.OnStepEntering(ElementID.ButtonPushMe).Execute(s => TabControl.SelectedIndex = 2);
            navigator.OnStepEntering(ElementID.CustomView).Execute(s => TabControl.SelectedIndex = 3);

            navigator.OnStepEntered(ElementID.TextBoxResult).Execute(s => myTbResult.Focus());
            navigator.OnStepEntered(ElementID.TextBoxPath).Execute(s => myTbPath.Focus());
            navigator.OnStepEntered(ElementID.ComboBoxOption).Execute(s => myCbOptions.Focus());
            navigator.OnStepEntered(ElementID.ButtonClear).Execute(s => myBtnClear.Focus());
        }

        private void CbOptionsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myCbOptions.SelectedIndex == 1)
            {
                var navigator = FeatureTour.GetNavigator();
                navigator.IfCurrentStepEquals(ElementID.ComboBoxOption).GoNext();
            }
        }

        private void TbPathTextChanged(object sender, TextChangedEventArgs e)
        {
            var path = myTbPath.Text.Trim(' ', '\\');
            var expectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Trim(' ', '\\');
            if (path.Equals(expectedPath, StringComparison.InvariantCultureIgnoreCase))
            {
                var navigator = FeatureTour.GetNavigator();
                navigator.IfCurrentStepEquals(ElementID.TextBoxPath).GoNext();
            }
        }

        private void TbResultTextChanged(object sender, TextChangedEventArgs e)
        {
            var result = myTbResult.Text.Trim();
            if (result == "21")
            {
                var navigator = FeatureTour.GetNavigator();
                navigator.IfCurrentStepEquals(ElementID.TextBoxResult).GoNext();
            }
        }
    }
}
