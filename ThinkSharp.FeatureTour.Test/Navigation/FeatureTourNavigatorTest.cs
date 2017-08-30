// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.FeatureTouring.Models;
using ThinkSharp.FeatureTouring.Navigation;
using ThinkSharp.FeatureTouring.ViewModels;

namespace ThinkSharp.FeatureTouring.Test.Navigation
{
    [TestClass]
    public class FeatureTourNavigatorTest
    {
        private class Counter
        {
            public int entered1 = 0;
            public int leaved1 = 0;
            public int entered2 = 0;
            public int leaved2 = 0;
            public int closed = 0;
        }

        private class Doable
        {
            public int do1 = 0;
            public int do2 = 0;
            public int do3 = 0;
        }

        [TestMethod]
        public void TestStepCount()
        {
            var tour = new Tour();
            tour.Steps = new[]
            {
                new Step("1", "Header1", "Content1", "ID1"),
                new Step("2", "Header2", "Content2", "ID2"),
                new Step("3", "Header2", "Content2", "ID2"),
            };

            var manager = new VisualElementManagerMock(tour.Steps);
            var tourRun = new TourRun(tour, manager, new WindowManagerMock(), new PopupNavigatorMock());
            FeatureTour.SetTourRun(tourRun);
            tourRun.Start();

            var vm = tourRun.GetPrivateField<TourViewModel>("myTourViewModel");

            Assert.AreEqual(false, vm.HasTourFinished);
            Assert.AreEqual(3, vm.TotalStepsCount);
            Assert.AreEqual(1, vm.CurrentStepNo);

            tourRun.NextStep();

            Assert.AreEqual(false, vm.HasTourFinished);
            Assert.AreEqual(3, vm.TotalStepsCount);
            Assert.AreEqual(2, vm.CurrentStepNo);

            tourRun.NextStep();

            Assert.AreEqual(true, vm.HasTourFinished);
            Assert.AreEqual(3, vm.TotalStepsCount);
            Assert.AreEqual(3, vm.CurrentStepNo);

            tourRun.NextStep();

            Assert.AreEqual(true, vm.HasTourFinished);
            Assert.AreEqual(3, vm.TotalStepsCount);
            Assert.AreEqual(3, vm.CurrentStepNo);

            tourRun.PreviousStep();

            Assert.AreEqual(false, vm.HasTourFinished);
            Assert.AreEqual(3, vm.TotalStepsCount);
            Assert.AreEqual(2, vm.CurrentStepNo);

            FeatureTour.SetCurrentRunNull();
        }

        [TestMethod]
        public void OnStepEnteredLeavedClosed()
        {
            var c = new Counter();
            var navigator = FeatureTour.GetNavigator();
            navigator.OnStepEntered("ID1").Execute(s => c.entered1++);
            navigator.OnStepEntered("ID2").Execute(s => c.entered2++);
            navigator.OnStepLeft("ID1").Execute(s => c.leaved1++);
            navigator.OnStepLeft("ID2").Execute(s => c.leaved2++);
            navigator.OnClosed().Execute(s => c.closed++);

            var tour = new Tour();
            tour.Steps = new[]
            {
                new Step("1", "Header1", "Content1", "ID1"),
                new Step("2", "Header2", "Content2", "ID2"),
            };

            var manager = new VisualElementManagerMock(tour.Steps);
            var tourRun = new TourRun(tour, manager, new WindowManagerMock(), new PopupNavigatorMock());
            AssertIsCounter(c, 0, 0, 0, 0, 0);
            FeatureTour.SetTourRun(tourRun);
            AssertIsCounter(c, 0, 0, 0, 0, 0);
            tourRun.Start();
            AssertIsCounter(c, 1, 0, 0, 0, 0);
            tourRun.NextStep();
            AssertIsCounter(c, 1, 1, 1, 0, 0);
            tourRun.PreviousStep();
            AssertIsCounter(c, 2, 1, 1, 1, 0);
            tourRun.Close();
            AssertIsCounter(c, 2, 2, 1, 1, 1);

            FeatureTour.SetCurrentRunNull();
        }

        [TestMethod]
        public void OnStepDoable()
        {
            var d = new Doable();
            var navigator = FeatureTour.GetNavigator();
            navigator.ForStep("ID1").AttachDoable(s => d.do1++);
            navigator.ForStep("ID2").AttachDoable(s => d.do2++, s => true);
            navigator.ForStep("ID3").AttachDoable(s => d.do3++, s => false);

            var tour = new Tour();
            tour.Steps = new[]
            {
                new Step("1", "Header1", "Content1", "ID1"),
                new Step("2", "Header2", "Content2", "ID2"),
                new Step("3", "Header3", "Content3", "ID3"),
                new Step("4", "Header4", "Content4", "ID4"),
            };

            var manager = new VisualElementManagerMock(tour.Steps);
            var tourRun = new TourRun(tour, manager, new WindowManagerMock(), new PopupNavigatorMock());
            tourRun.Start();

            AssertIsDoable(d, 0, 0, 0);
            Assert.IsTrue(tourRun.ShowDoIt());
            Assert.IsTrue(tourRun.CanDoIt());
            tourRun.DoIt();
            AssertIsDoable(d, 1, 0, 0);

            tourRun.NextStep();
            Assert.IsTrue(tourRun.ShowDoIt());
            Assert.IsTrue(tourRun.CanDoIt());
            tourRun.DoIt();
            AssertIsDoable(d, 1, 1, 0);

            tourRun.NextStep();
            Assert.IsTrue(tourRun.ShowDoIt());
            Assert.IsFalse(tourRun.CanDoIt());
            tourRun.DoIt();
            AssertIsDoable(d, 1, 1, 1);

            tourRun.NextStep();
            Assert.IsFalse(tourRun.ShowDoIt());
            Assert.IsFalse(tourRun.CanDoIt());
            tourRun.DoIt();
            AssertIsDoable(d, 1, 1, 1);
        }

        private void AssertIsDoable(Doable d, int do1, int do2, int do3)
        {
            Assert.AreEqual(do1, d.do1);
            Assert.AreEqual(do2, d.do2);
            Assert.AreEqual(do3, d.do3);
        }

        private void AssertIsCounter(Counter c, int entered1, int leaved1, int entered2, int leaved2, int closed)
        {
            Assert.AreEqual(entered1, c.entered1, "entered1");
            Assert.AreEqual(leaved1, c.leaved1, "leaved1");
            Assert.AreEqual(entered2, c.entered2, "entered2");
            Assert.AreEqual(leaved2, c.leaved2, "leaved2");
            Assert.AreEqual(closed, c.closed, "closed");
        }
    }
}
