using GalaSoft.MvvmLight;
using ThinkSharp.FeatureTouring.Models;

namespace ThinkSharp.FeatureTouring.Touring
{
    internal static class TourStarter
    {
        public static void StartIntroduction()
        {
            var tour = new Tour
            {
                Name = "Introduction",
                ShowNextButtonDefault = true,
                Steps = new[]
                {
                    new Step(ElementID.ButtonIntroduction, "Introduction", "Starts this tour."),
                    new Step(ElementID.ButtonActiveTour, "Active Tour", "Starts an active tour"),
                    new Step(ElementID.ButtonTourWithDialog, "Tour with Dialog", "Starts a tour that demonstrates how to created tours with dialogs."),
                    new Step(ElementID.ButtonPositioning, "Positioning", "Starts a tour that shows the popup positioning options"),
                    new Step(ElementID.ButtonCustomView, "CustomView", "Starts a tour that shows ho to define custom views"),
                    new Step(ElementID.ButtonOverView, "Tour Selection Menu", "Opens a menu screen that provides all available tours."),
                }
            };
            tour.Start();
        }

        public static void StartPositioning()
        {
            var tour = new Tour
            {
                Name = "PositioningTour",
                ShowNextButtonDefault = true,
                Steps = new[]
                {
                    new Step(ElementID.Rectangle, "Positioning", "TopLeft") {Tag = Placement.TopLeft},
                    new Step(ElementID.Rectangle, "Positioning", "TopCenter") {Tag = Placement.TopCenter},
                    new Step(ElementID.Rectangle, "Positioning", "TopRight") {Tag = Placement.TopRight},
                    new Step(ElementID.Rectangle, "Positioning", "RightTop") {Tag = Placement.RightTop},
                    new Step(ElementID.Rectangle, "Positioning", "RightCenter") {Tag = Placement.RightCenter},
                    new Step(ElementID.Rectangle, "Positioning", "RightBottom") {Tag = Placement.RightBottom},
                    new Step(ElementID.Rectangle, "Positioning", "BottomRight") {Tag = Placement.BottomRight},
                    new Step(ElementID.Rectangle, "Positioning", "BottomCenter") {Tag = Placement.BottomCenter},
                    new Step(ElementID.Rectangle, "Positioning", "BottomLeft") {Tag = Placement.BottomLeft},
                    new Step(ElementID.Rectangle, "Positioning", "LeftBottom") {Tag = Placement.LeftBottom},
                    new Step(ElementID.Rectangle, "Positioning", "LeftCenter") {Tag = Placement.LeftCenter},
                    new Step(ElementID.Rectangle, "Positioning", "LeftTop") {Tag = Placement.LeftTop},
                    new Step(ElementID.Rectangle, "Positioning", "Center") {Tag = Placement.Center},
                }
            };
            tour.Start();
        }

        public static void StartActiveTour()
        {
            var tour = new Tour
            {
                Name = "Active Tour",
                ShowNextButtonDefault = false,
                Steps = new[]
                {
                    new Step(ElementID.TextBoxResult, "Enter Calculation", "Enter the result of \"10 + 11\"."),
                    new Step(ElementID.TextBoxPath, "Select Path", "Enter path to the desktop."),
                    new Step(ElementID.ComboBoxOption, "Choose Option", "Choose \"OptionB\"."),
                    new Step(ElementID.ButtonClear, "Reset", "Press the button to reset the form and finish the tour."),
                }
            };
            tour.Start();
        }

        public static void StartDialogTour()
        {
            var tour = new Tour
            {
                Name = "Dialog Tour",
                ShowNextButtonDefault = true,
                Steps = new[]
                {
                    new Step(ElementID.ButtonPushMe, "Open Dialog", "Push the button to open a dialog and continue the tour. Note that the 'Next >>' button is disabled because the visual element that is associated with the next step is not yet available."),
                    new Step(ElementID.ButtonPushMeToo, "Dialog", "Push the button to continue") { ShowNextButton = false },
                    new Step(ElementID.ButtonClose, "Dialog", "Push the button to close the dialog."),
                    new Step(ElementID.ButtonPushMe, "Tour succeeded", "Tour finished."),
                }
            };
            tour.Start();
        }

        public static void StartCustomViewTour()
        {
            var customizeHeaderViewModell = new CustomizeHeaderViewModel();
            var tour = new Tour
            {
                Name = "Custom View",
                ShowNextButtonDefault = true,
                Steps = new[]
                {
                    new Step(ElementID.CustomView, "Image Content", null) { ContentDataTemplateKey = "ImageWithTextView" },
                    new Step(ElementID.CustomView, "Content With View Model", new BinaryCalculatorViewModel()) { ContentDataTemplateKey = "ViewWithViewModel" },
                    new Step(ElementID.CustomView, customizeHeaderViewModell, customizeHeaderViewModell)
                    {
                        HeaderDataTemplateKey = "CustomizeHeaderView",
                        ContentDataTemplateKey = "CustomizeHeaderContentView"
                    },
                }
            };
            tour.Start();
        }

        public static void StartOverView()
        {
            var tour = new Tour
            {
                Name = "Overview",
                ShowNextButtonDefault = false,
                Steps = new[]
                {
                    new Step(ElementID.CustomView, "Welcome - Select a tour", MainWindowViewModel.Instance)
                    {
                        ContentDataTemplateKey = "SelectTourDataTemplate"
                    },
                }
            };
            tour.Start();
        }
    }
}
