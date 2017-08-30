using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.FeatureTouring.Models;
using ThinkSharp.FeatureTouring.Navigation;

// ReSharper disable MethodTooLong

namespace ThinkSharp.FeatureTouring.Test
{
    [TestClass]
    public class TourRunTest
    {
        [TestMethod]
        public void TestStepsNull()
        {
            var tour = new Tour();
            tour.Steps = null;
            try
            {
                var tourRun = new TourRun(tour, new VisualElementManagerMock(), new WindowManagerMock(), new PopupNavigatorMock());
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void TestStepsEmpty()
        {
            var tour = new Tour();
            tour.Steps = new Step[0];
            try
            {
                var tourRun = new TourRun(tour, new VisualElementManagerMock(), new WindowManagerMock(), new PopupNavigatorMock());
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void TestStepsContainsNullElements()
        {
            var tour = new Tour();
            tour.Steps = new Step[] { null };
            try
            {
                var tourRun = new TourRun(tour, new VisualElementManagerMock(), new WindowManagerMock(), new PopupNavigatorMock());
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void TestStepsContainsElementIdWithNull()
        {
            var tour = new Tour();
            tour.Steps = new[] { new Step(null, "Header", "Content") };
            try
            {
                var tourRun = new TourRun(tour, new VisualElementManagerMock(), new WindowManagerMock(), new PopupNavigatorMock());
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void TestCompleteRun_2Steps()
        {
            var tour = new Tour();
            tour.Steps = new[]
            {
                new Step("1", "Header1", "Content1"),
                new Step("2", "Header2", "Content2"),
            };

            var manager = new VisualElementManagerMock(tour.Steps);
            var windowManager = new WindowManagerMock();
            var popupNavigator = new PopupNavigatorMock();
            var tourRun = new TourRun(tour, manager, windowManager, popupNavigator);
            tourRun.Start();

            Assert.AreEqual(tour.Steps[0], tourRun.CurrentStep);
            Assert.AreEqual(1, tourRun.CurrentStepNo);

            Assert.IsTrue(tourRun.CanNextStep());
            Assert.IsTrue(tourRun.NextStep());
            Assert.AreEqual(tour.Steps[1], tourRun.CurrentStep);
            Assert.AreEqual(2, tourRun.CurrentStepNo);

            Assert.IsFalse(tourRun.CanNextStep());
            Assert.IsFalse(tourRun.NextStep());
            tourRun.Close();

            AssertManagerIsClean(popupNavigator, windowManager);
        }

        [TestMethod]
        public void TestCompleteRun_2Steps_WithPreviouse()
        {
            var tour = new Tour();
            tour.Steps = new[]
            {
                new Step("1", "Header1", "Content1"),
                new Step("2", "Header2", "Content2"),
            };

            var manager = new VisualElementManagerMock(tour.Steps);
            var windowManager = new WindowManagerMock();
            var popupNavigator = new PopupNavigatorMock();
            var tourRun = new TourRun(tour, manager, windowManager, popupNavigator);

            Assert.IsNull(popupNavigator.ViewModel);
            
            tourRun.Start();
            Assert.IsNotNull(popupNavigator.ViewModel);

            AssertStep(tour, 0, tourRun, popupNavigator);

            Assert.IsTrue(tourRun.CanNextStep());
            Assert.IsTrue(tourRun.NextStep());
            AssertStep(tour, 1, tourRun, popupNavigator);

            Assert.IsTrue(tourRun.CanPreviousStep());
            Assert.IsTrue(tourRun.PreviousStep());
            AssertStep(tour, 0, tourRun, popupNavigator);

            Assert.IsFalse(tourRun.PreviousStep());
            Assert.IsFalse(tourRun.PreviousStep());
            tourRun.Close();

            AssertManagerIsClean(popupNavigator, windowManager);
        }

        [TestMethod]
        public void TestCompleteRun_2Steps_UsingNavigator()
        {
            var navigator = FeatureTour.GetNavigator();
            var tour = new Tour();
            tour.Steps = new[]
            {
                new Step("1", "Header1", "Content1"),
                new Step("2", "Header2", "Content2"),
            };

            var manager = new VisualElementManagerMock(tour.Steps);
            var windowManager = new WindowManagerMock();
            var popupNavigator = new PopupNavigatorMock();
            var tourRun = new TourRun(tour, manager, windowManager, popupNavigator);
            FeatureTour.SetTourRun(tourRun);
            tourRun.Start();

            Assert.AreEqual(tour.Steps[0], FeatureTour.CurrentStep);

            Assert.IsTrue(navigator.IfCurrentStepEquals(FeatureTour.CurrentStep.ID).GoNext());
            Assert.AreEqual(tour.Steps[1], FeatureTour.CurrentStep);

            Assert.IsFalse(navigator.IfCurrentStepEquals(FeatureTour.CurrentStep.ID).GoNext());
            Assert.AreEqual(tour.Steps[1], FeatureTour.CurrentStep);

            Assert.IsTrue(navigator.IfCurrentStepEquals(FeatureTour.CurrentStep.ID).GoPrevious());
            Assert.AreEqual(tour.Steps[0], FeatureTour.CurrentStep);

            Assert.IsFalse(navigator.IfCurrentStepEquals(FeatureTour.CurrentStep.ID).GoPrevious());
            Assert.AreEqual(tour.Steps[0], FeatureTour.CurrentStep);

            Assert.IsTrue(navigator.Close());
            Assert.AreEqual(null, FeatureTour.CurrentStep);

            AssertManagerIsClean(popupNavigator, windowManager);

            FeatureTour.SetCurrentRunNull();
        }

        [TestMethod]
        public void TestCompleteRun_2Steps_UsingPrediction()
        {
            var navigator = FeatureTour.GetNavigator();
            var tour = new Tour();
            tour.Steps = new[]
            {
                new Step("1", "Header1", "Content1", "ID1"),
                new Step("2", "Header2", "Content2", "ID2"),
            };

            var manager = new VisualElementManagerMock(tour.Steps);
            var windowManager = new WindowManagerMock();
            var popupNavigator = new PopupNavigatorMock();
            var tourRun = new TourRun(tour, manager, windowManager, popupNavigator);
            FeatureTour.SetTourRun(tourRun);
            tourRun.Start();

            Assert.AreEqual(tour.Steps[0], FeatureTour.CurrentStep);

            Assert.IsFalse(navigator.IfCurrentStepEquals("ID2").GoPrevious());
            Assert.AreEqual(tour.Steps[0], FeatureTour.CurrentStep);
            Assert.IsTrue(navigator.IfCurrentStepEquals("ID1").GoNext());
            Assert.AreEqual(tour.Steps[1], FeatureTour.CurrentStep);

            Assert.IsFalse(navigator.IfCurrentStepEquals("ID1").GoPrevious());
            Assert.AreEqual(tour.Steps[1], FeatureTour.CurrentStep);
            Assert.IsTrue(navigator.IfCurrentStepEquals("ID2").GoPrevious());
            Assert.AreEqual(tour.Steps[0], FeatureTour.CurrentStep);

            Assert.IsTrue(navigator.Close());
            Assert.AreEqual(null, FeatureTour.CurrentStep);

            AssertManagerIsClean(popupNavigator, windowManager);

            FeatureTour.SetCurrentRunNull();
        }

        private void AssertStep(Tour tour, int index, TourRun tourRun, PopupNavigatorMock popupNavigator)
        {
            Assert.AreEqual(tour.Steps[index], tourRun.CurrentStep);
            Assert.AreEqual(index + 1, tourRun.CurrentStepNo);
            AssertViewModelIsInitialized(tour.Steps[index], popupNavigator.ViewModel);
        }

        private void AssertViewModelIsInitialized(Step step, ViewModels.TourViewModel tourViewModel)
        {
            Assert.AreEqual(step.Content, tourViewModel.Content);
            Assert.AreEqual(step.Header, tourViewModel.Header);
        }

        private static void AssertManagerIsClean(PopupNavigatorMock popupNavigator, WindowManagerMock windowManager)
        {
            Assert.IsTrue(popupNavigator.Reseted);
            Assert.AreEqual(0, windowManager.WindowActivatedHandlersCount);
            Assert.AreEqual(0, windowManager.WindowDeactivatedHandlersCount);
        }
    }
}
