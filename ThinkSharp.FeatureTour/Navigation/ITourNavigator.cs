// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FeatureTouring.Navigation
{
    /// <summary>
    /// Interface for an object that allows to control the navigation of the tour.
    /// </summary>
    public interface ITourNavigator
    {
        /// <summary>
        /// Navigates to the next step.
        /// </summary>
        /// <returns>
        /// true if navigation succeeded; otherwise false.
        /// </returns>
        bool GoNext();

        /// <summary>
        /// Navigates to the previous step.
        /// </summary>
        /// <returns>
        /// true if navigation succeeded; otherwise false.
        /// </returns>
        bool GoPrevious();

        /// <summary>
        /// Closes the active tour.
        /// </summary>
        void Close();
    }
    internal class NullTourNavigator : ITourNavigator
    {
        public bool GoNext() { return false; }
        public bool GoPrevious() { return false; }
        public void Close() { }
    }
    internal class TourNavigator : ITourNavigator
    {
        private readonly ITourRun myRun;

        public TourNavigator(ITourRun run)
        {
            myRun = run;
        }

        public bool GoNext()
        {
            return myRun.NextStep(false);
        }

        public bool GoPrevious()
        {
            return myRun.PreviousStep();
        }

        public void Close()
        {
            myRun.Close();
        }
    }
}
