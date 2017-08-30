using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.FeatureTouring.Models;
using ThinkSharp.FeatureTouring.ViewModels;

namespace ThinkSharp.FeatureTouring.Test.Navigation
{
    public class TestTourViewModel : TourViewModel
    {
        public TestTourViewModel(ITourRun run) : base(run)
        { }
    }
    [TestClass]
    public class FeatureTourTest
    {
        [TestMethod]
        public void TestSetFactoryMethod()
        {
            ITourRun tourRun = null;
            FeatureTouring.Navigation.FeatureTour.SetViewModelFactoryMethod(
                run =>
                {
                    tourRun = run;
                    return new TestTourViewModel(run);
                });
            var tour = new Tour()
            {
                Name = "Test",
                Steps = new[]
                {
                    new Step("ID1", "Header", "Content")
                },
            };

            tour.Start();

            Assert.IsNotNull(tourRun, "TourRun should not be null - ViewModelFactoryMethod was not called.");
            
            //BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            //FieldInfo field = typeof(TourRun).GetField("myTourViewModel", bindFlags);
            //var vm = field.GetValue(tourRun) as TestTourViewModel;
            var vm = tourRun.GetPrivateField<TestTourViewModel>("myTourViewModel");

            Assert.IsNotNull(vm, "ViewModel should not be null. Custom view model creation failed.");
        }
    }
}
