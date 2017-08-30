using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FeatureTouring.Helper
{
    internal class DisposableAction : IDisposable
    {
        private readonly Action myAction;

        /// <summary>
        /// </summary>
        /// <param name="action">The action that is invoked when dispose is called.
        /// </param>
        public DisposableAction(Action action)
        {
            myAction = action;
        }

        public void Dispose()
        {
            if (myAction != null)
                myAction();
        }
    }
}
