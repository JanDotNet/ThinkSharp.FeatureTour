// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FeatureTouring.ViewModels;

namespace ThinkSharp.FeatureTouring
{
    /// <summary>
    /// Defines all possible transitions if another window gets activated.
    /// </summary>
    public enum WindowTransisionBehavior
    {
        /// <summary>
        /// Default.
        /// If transition change is from parent to child window, <see cref="NextHide"/> is used.
        /// If transition change is from child to parent window, <see cref="NextPreviousHide"/> is used.
        /// </summary>
        Automatic,

        /// <summary>
        /// If the element of the next step is on the activated window, the tour goes to the next step.
        /// Otherwise, if one of the previous steps is on the activated window, the tour goes back to that previous step.
        /// Otherwise the tour is paused (pop-up hidden).
        /// </summary>
        NextPreviousHide,

        /// <summary>
        /// If the element of the next step is on the on the activated window, the tour goes to the next step.
        /// Otherwise the tour is paused (pop-up hidden).
        /// </summary>
        NextHide,

        /// <summary>
        /// If one of the previous steps is on the activated window, the tour goes back to that previous step.
        /// Otherwise the tour is paused (pop-up hidden).
        /// </summary>
        PreviousHide,

        /// <summary>
        /// If the activated window changes, nothing happens. In case of modal windows, pop-up may be visible but not responsive.
        /// </summary>
        None,
    }
}
