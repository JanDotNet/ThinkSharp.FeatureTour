// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using ThinkSharp.FeatureTouring.Helper;
using ThinkSharp.FeatureTouring.Models;

namespace ThinkSharp.FeatureTouring.Navigation
{
    /// <summary>
    /// Interface for an object that allows to add doable actions.
    /// </summary>
    public interface ITourDoable
    {
        /// <summary>
        /// Attaches a doable action.
        /// If a step has a doable action attach, the pop-up shows a 'DO' button that gives the user the possibility to execute the attached action.
        /// </summary>
        /// <param name="doAction">
        /// The doable action to execute.
        /// </param>
        /// <returns>
        /// The <see cref="IReleasable"/> that can be used to release the doable.</returns>
        IReleasable AttachDoable(Action<Step> doAction);

        /// <summary>
        /// Attaches a doable action.
        /// If a step has a doable action attach, the pop-up shows a 'DO' button that gives the user the possibility to execute the attached action.
        /// </summary>
        /// <param name="doAction">
        /// The doable action to execute.
        /// </param>
        /// <param name="canDoAction">
        /// The delegate that determines if the 'DO' button is enabled or not.
        /// </param>
        /// <returns>
        /// The <see cref="IReleasable"/> that can be used to release the doable.</returns>
        IReleasable AttachDoable(Action<Step> doAction, Func<Step, bool> canDoAction);
    }
    internal class NullTourDoable : ITourDoable
    {
        public IReleasable AttachDoable(Action<Step> doAction)
        {
            return ReleasableAction.Empty;
        }
        public IReleasable AttachDoable(Action<Step> doAction, Func<Step, bool> canDoAction)
        {
            return ReleasableAction.Empty;
        }
    }
    internal class TourDoable : ITourDoable
    {
        private readonly ActionRepository myActionRepository;
        private readonly string myName;

        public TourDoable(ActionRepository actionRepository, string name)
        {
            myActionRepository = actionRepository;
            myName = name;
        }

        public IReleasable AttachDoable(Action<Step> doAction)
        {
            return myActionRepository.AddAction(myName, doAction, s => true);
        }

        public IReleasable AttachDoable(Action<Step> doAction, Func<Step, bool> canDoAction)
        {
            return myActionRepository.AddAction(myName, doAction, canDoAction);
        }
    }
}
