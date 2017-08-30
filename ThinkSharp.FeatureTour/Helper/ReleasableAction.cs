// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FeatureTouring.Helper
{
    internal class ReleasableAction : IReleasable
    {
        private static readonly IReleasable theEmpty = new ReleasableAction(null);
        private readonly Action myAction;

        /// <summary>
        /// </summary>
        /// <param name="action">The action that is invoked when dispose is called.
        /// </param>
        public ReleasableAction(Action action)
        {
            myAction = action;
        }

        public void Release()
        {
            if (myAction != null)
                myAction();
        }

        public static IReleasable Empty { get { return theEmpty; } }
    }
}
