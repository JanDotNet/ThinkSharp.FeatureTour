// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ThinkSharp.FeatureTouring.Navigation;
using ThinkSharp.FeatureTouring.Touring;

namespace ThinkSharp.FeatureTouring
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ICommand _cmdStartPositioning;
        private ICommand _cmdStartIntroduction;
        private ICommand _cmdStartActiveTour;
        private ICommand _cmdStartDialogTour;
        private ICommand _cmdStartCustomViewTour;
        private ICommand _cmdStartOverView;
        private ICommand _cmdOpenDialog;
        private ICommand _cmdClear;
        private Placement _placement;
        private int _colorSchemaIndex;
        private int _tabIndex;
        private string _path;
        private string _result;
        private int _selectedIndex;
        private string _styleText;

        private readonly PopupStyle myPopupStyle = new PopupStyle();


        // .ctor

        private MainWindowViewModel()
        {
            FeatureTour.SetViewModelFactoryMethod(tourRun => new CustomTourViewModel(tourRun));

            var navigator = FeatureTour.GetNavigator();

            navigator.ForStep(ElementID.TextBoxPath).AttachDoable(s => Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            navigator.ForStep(ElementID.ComboBoxOption).AttachDoable(s => SelectedIndex = 1);
            navigator.ForStep(ElementID.TextBoxPath).AttachDoable(s => Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            PopupStyle.PropertyChanged += (s, e) => StyleText = GetStyleText();
            StyleText = GetStyleText();
        }

        private string GetStyleText()
        {
            var sb = new StringBuilder();

            sb.AppendLine("...");
            sb.AppendLine("xmlns:featureTouringControls=\"clr -namespace:ThinkSharp.FeatureTouring.Controls;assembly=ThinkSharp.FeatureTour\"");
            sb.AppendLine("...");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("<Style TargetType=\"featureTouringControls: TourControl\">");
            sb.AppendLine($"    <Setter Property=\"Background\" Value=\"{myPopupStyle.BackgroundColor}\"/>");
            sb.AppendLine($"    <Setter Property=\"BorderBrush\" Value=\"{myPopupStyle.BorderBrushColor}\"/>");
            sb.AppendLine($"    <Setter Property=\"Foreground\" Value=\"{myPopupStyle.ForegroundColor}\"/>");
            sb.AppendLine($"    <Setter Property=\"FontSize\" Value=\"{myPopupStyle.FontSize:0}\"/>");
            sb.AppendLine($"    <Setter Property=\"CornerRadius\" Value=\"{myPopupStyle.CornerRadius:0}\"/>");
            sb.AppendLine($"    <Setter Property=\"BorderThickness\" Value=\"{myPopupStyle.BorderThickness.Top:0}\"/>");
            sb.AppendLine("</Style>");
            return sb.ToString();
        }


        // Methods

        private void Clear()
        {
            Result = "";
            Path = "";
            SelectedIndex = 0;
            FeatureTour.GetNavigator()
                .IfCurrentStepEquals(ElementID.ButtonClear)
                .Close();
        }


        // Commands

        public ICommand CmdStartPositioning
        {
            get
            {
                if (_cmdStartPositioning == null)
                {
                    _cmdStartPositioning = new RelayCommand(TourStarter.StartPositioning);
                }
                return _cmdStartPositioning;
            }
        }
        
        public ICommand CmdStartIntroduction
        {
            get
            {
                if (_cmdStartIntroduction == null)
                {
                    _cmdStartIntroduction = new RelayCommand(TourStarter.StartIntroduction);
                }
                return _cmdStartIntroduction;
            }
        }

        public ICommand CmdStartActiveTour
        {
            get
            {
                if (_cmdStartActiveTour == null)
                {
                    _cmdStartActiveTour = new RelayCommand(TourStarter.StartActiveTour);
                }
                return _cmdStartActiveTour;
            }
        }

        public ICommand CmdStartDialogTour
        {
            get
            {
                if (_cmdStartDialogTour == null)
                {
                    _cmdStartDialogTour = new RelayCommand(TourStarter.StartDialogTour);
                }
                return _cmdStartDialogTour;
            }
        }

        public ICommand CmdStartCustomView
        {
            get
            {
                if (_cmdStartCustomViewTour == null)
                {
                    _cmdStartCustomViewTour = new RelayCommand(TourStarter.StartCustomViewTour);
                }
                return _cmdStartCustomViewTour;
            }
        }

        public ICommand CmdStartOverView
        {
            get
            {
                if (_cmdStartOverView == null)
                {
                    _cmdStartOverView = new RelayCommand(TourStarter.StartOverView);
                }
                return _cmdStartOverView;
            }
        }

        public ICommand CmdOpenDialog
        {
            get
            {
                if (_cmdOpenDialog == null)
                {
                    _cmdOpenDialog = new RelayCommand(() =>
                    {
                        var dlg = new Dialog();
                        dlg.ShowDialog();
                    });
                }
                return _cmdOpenDialog;
            }
        }

        public ICommand CmdClear
        {
            get
            {
                if (_cmdClear == null)
                {
                    _cmdClear = new RelayCommand(Clear);
                }
                return _cmdClear;
            }
        }


        // Properties

        public Placement Placement
        {
            get { return _placement; }
            set { Set("Placement", ref _placement, value); }
        }
        
        public int ColorSchemaIndex
        {
            get { return _colorSchemaIndex; }
            set { Set("ColorSchemaIndex", ref _colorSchemaIndex, value); }
        }

        public int TabIndex
        {
            get { return _tabIndex; }
            set { Set("TabIndex", ref _tabIndex, value); }
        }

        public string Path
        {
            get { return _path; }
            set { Set("Path", ref _path, value); }
        }

        public string Result
        {
            get { return _result; }
            set { Set("Result", ref _result, value); }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { Set("SelectedIndex", ref _selectedIndex, value); }
        }

        public string StyleText
        {
            get { return _styleText; }
            set { Set("StyleText", ref _styleText, value); }
        }

        public PopupStyle PopupStyle => myPopupStyle;

        public static MainWindowViewModel Instance { get; } = new MainWindowViewModel();
    }
}
