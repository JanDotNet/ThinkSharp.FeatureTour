// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using ThinkSharp.FeatureTouring.Helper;
using ThinkSharp.FeatureTouring.Models;
using ThinkSharp.FeatureTouring.Logging;

namespace ThinkSharp.FeatureTouring.Navigation
{
    using ExecuteAction = Action<Step>;
    using CanExecuteAction = Func<Step, bool>;

    internal class ActionRepository
    {
        private readonly Dictionary<string, ExecuteAction> myExecuteActions = new Dictionary<string, ExecuteAction>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, CanExecuteAction> myCanExecuteActions = new Dictionary<string, CanExecuteAction>(StringComparer.InvariantCultureIgnoreCase);

        public IReleasable AddAction(string name, Action<Step> executeAction)
        {
            if (executeAction == null)
                throw new ArgumentNullException("executeAction");

            var execute = new ExecuteAction(executeAction);

            if (myExecuteActions.ContainsKey(name))
                Log.WarnFormat($"Action with name '{name}' already exists. Will be overwritten!");

            myExecuteActions[name] = execute;

            Log.Debug("ActionRepository: Action '" + name + "' added.");

            return new ReleasableAction(() =>
            {
                ExecuteAction executeStored;
                if (myExecuteActions.TryGetValue(name, out executeStored) && executeStored == execute)
                    myExecuteActions.Remove(name);
            });
        }

        public IReleasable AddAction(string name, Action<Step> executeAction, Func<Step, bool> canExecuteAction)
        {
            if (executeAction == null)
                throw new ArgumentNullException("executeAction");
            if (canExecuteAction == null)
                throw new ArgumentNullException("canExecuteAction");

            var execute = new ExecuteAction(executeAction);
            myExecuteActions[name] = execute;
            var canExecute = new CanExecuteAction(canExecuteAction);
            myCanExecuteActions[name] = canExecute;

            Log.Debug("ActionRepository: Action '" + name + "' added (with CanExecute).");

            return new ReleasableAction(() =>
            {
                ExecuteAction executeStored;
                if (myExecuteActions.TryGetValue(name, out executeStored) && executeStored == execute)
                    myExecuteActions.Remove(name);

                CanExecuteAction canExecuteStored;
                if (myCanExecuteActions.TryGetValue(name, out canExecuteStored) && canExecuteStored == canExecute)
                    myCanExecuteActions.Remove(name);
            });
        }

        public bool Contains(string name)
        {
            return myExecuteActions.ContainsKey(name);
        }

        internal void Clear()
        {
            myExecuteActions.Clear();
            myCanExecuteActions.Clear();
        }

        public void Execute(string name, Step step)
        {
            ExecuteAction action;
            if (!myExecuteActions.TryGetValue(name, out action))
            {
                Log.Debug("ActionRepository: Action '" + name + "' not available.");
                return;
            }

            action.Invoke(step);
        }

        public bool CanExecute(string name, Step step)
        {
            CanExecuteAction canAction;
            if (!myCanExecuteActions.TryGetValue(name, out canAction))
                return false;

            return canAction.Invoke(step);
        }

        // for unit testing
        internal int ExecuteCount { get { return myExecuteActions.Count; } }
        internal int CanExecuteCount { get { return myExecuteActions.Count; } }
    }
}
