// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ThinkSharp.FeatureTouring.Helper
{
    internal static class WpfUtils
    {
        public static T GetChildOfType<T>(this DependencyObject depObj) 
            where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        public static T GetChildOfType<T>(this DependencyObject depObj, string name)
            where T : FrameworkElement
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = child as T;
                if (result != null && result.Name == name)
                    return result;

                result = GetChildOfType<T>(child);
                if (result != null && result.Name == name)
                    return result;
            }
            return null;
        }

        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            return FindParent<T>(parentObject);
        }

        public static void DispatcherInvoke(Action action)
        {
            var app = Application.Current;
            if (app == null || app.Dispatcher == null)
                action();
            else
                app.Dispatcher.Invoke(action);
        }

        public static TParam DispatcherInvoke<TParam>(Func<TParam> action)
        {
            var app = Application.Current;
            if (app == null || app.Dispatcher == null)
                return action();
            
            return (TParam)app.Dispatcher.Invoke(action);
        }
    }
}
