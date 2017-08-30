// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace ThinkSharp.FeatureTouring.ViewModels
{
    /// <summary>
    ///   Base class for all ViewModel classes in the application.
    ///   It provides support for property change notifications.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private event PropertyChangedEventHandler myPropertyChanged;
        private event PropertyChangingEventHandler myPropertyChanging;


        //  Methods
        // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        #region OnPropertyChanged

        /// <summary>
        ///   Raises the PropertyChanged event with the specified property.
        /// </summary>
        /// <param name="property">
        ///   The name of the property that has been changed.
        /// </param>
        protected virtual void OnPropertyChanged(String property)
        {
            var handler = myPropertyChanged;
            if (handler == null) return;

            VerifyPropertyName(property);

            handler(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region OnPropertyChanging

        /// <summary>
        ///   Raises the PropertyChanging event with the specified property.
        /// </summary>
        /// <param name="property">
        ///   The name of the property that is changing.
        /// </param>
        protected virtual void OnPropertyChanging(String property)
        {
            var handler = myPropertyChanging;
            if (handler == null) return;

            VerifyPropertyName(property);

            handler(this, new PropertyChangingEventArgs(property));
        }

        #endregion


        #region SetValue

        /// <summary>
        ///   Checks if the specified values are different and sets the new value to the specified
        ///   property if they differ.
        /// </summary>
        /// <typeparam name="T">
        ///   The type of the property.
        /// </typeparam>
        /// <param name="propertyName">
        ///   The name of the property.
        /// </param>
        /// <param name="valueToAssign">
        ///   The ref variable that receives the new value.
        /// </param>
        /// <param name="newValue">
        ///   The new value.
        /// </param>
        /// <returns>
        ///   Returns a value that indicates if the value has been changed.
        /// </returns>
        protected Boolean SetValue<T>(String propertyName, ref T valueToAssign, T newValue)
        {
            if (valueToAssign == null && newValue == null)
                return false;

            if (valueToAssign != null && valueToAssign.Equals(newValue))
                return false;

            valueToAssign = newValue;

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary> 
        ///   Checks if the specified values are different and sets the new value to the specified 
        ///   property if they differ. 
        /// </summary> 
        /// <typeparam name="T"> 
        ///   The type of the property. 
        /// </typeparam> 
        /// <param name="propertyName"> 
        ///   The name of the property. 
        /// </param> 
        /// <param name="setter"> 
        ///   Setter to set new value. 
        /// </param> 
        /// <param name="oldValue"> 
        ///   The old value. 
        /// </param> 
        /// <param name="newValue"> 
        ///   The new value. 
        /// </param> 
        /// <returns> 
        ///   Returns a value that indicates if the value has been changed. 
        /// </returns> 
        protected Boolean SetValue<T>(String propertyName, Action<T> setter, T oldValue, T newValue)
        {
            if (oldValue == null && newValue == null)
                return false;

            if (oldValue != null && oldValue.Equals(newValue))
                return false;

            setter(newValue);

            OnPropertyChanged(propertyName);

            return true;
        }

        #endregion


        #region VerifyPropertyName

        /// <summary>
        ///   Verifies that the specified property name exists.
        /// </summary>
        /// <param name="propertyName">
        ///   The property name to check.
        /// </param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(String propertyName)
        {
            String msg;


            if (TypeDescriptor.GetProperties(this)[propertyName] != null) return;

            msg = "Invalid property name: " + propertyName;

            Debug.Fail(msg);

            throw new Exception(msg);
        }

        #endregion


        //  Events
        // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region PropertyChanged

        /// <summary>
        ///   Occurs when a property value has been changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                this.myPropertyChanged += value;
            }
            remove
            {
                this.myPropertyChanged -= value;
            }
        }



        #endregion

        #region PropertyChanging

        /// <summary>
        ///   Occurs when a property value is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging
        {
            add
            {
                this.myPropertyChanging += value;
            }
            remove
            {
                this.myPropertyChanging -= value;
            }
        }

        #endregion
    }
}