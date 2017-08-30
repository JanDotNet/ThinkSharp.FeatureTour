using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FeatureTouring.Helper;
using ThinkSharp.FeatureTouring.Models;

namespace ThinkSharp.FeatureTouring.Navigation
{
    /// <summary>
    /// Interface for an object that allows to execute actions.
    /// </summary>
    public interface ITourExecution
    {
        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <param name="action">
        /// The action to execute.
        /// </param>
        void Execute(Action<Step> action);
    }
    internal class NullTourExecution : ITourExecution
    {
        public void Execute(Action<Step> action)
        {
        }
    }
    internal class TourExecution : ITourExecution
    {
        private readonly ActionRepository myActionRepository;
        private readonly string myName;

        public TourExecution(ActionRepository actionRepository, string name)
        {
            myActionRepository = actionRepository;
            myName = name;
        }

        public void Execute(Action<Step> action)
        {
            myActionRepository.AddAction(myName, action);
        }
    }
}
