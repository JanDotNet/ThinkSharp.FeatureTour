using System;
using System.Diagnostics;
using System.Windows.Input;

namespace ThinkSharp.FeatureTouring.ViewModels
{
    /// <summary>
    ///   The class CommandBase is the base implementation of the ICommand interface.
    /// </summary>
    public abstract class CommandBase : ICommand
    {

        // .ctor
        // ////////////////////////////////////////////////////////////////////////////////////////////

        #region .ctor

        /// <summary>
        ///   Creates a new instance of the CommandBase class.
        /// </summary>
        protected CommandBase()
        {
            this.InitializeMyClass();
        }

        #endregion


        // Clean up
        // ////////////////////////////////////////////////////////////////////////////////////////////


        // Initialize
        // ////////////////////////////////////////////////////////////////////////////////////////////

        #region InitializeMyClass

        /// <summary>
        ///   Initializes the new instance.
        /// </summary>
        private void InitializeMyClass()
        {
        }

        #endregion


        // Methods
        // ////////////////////////////////////////////////////////////////////////////////////////////

        #region CanExecute

        /// <summary>
        ///   Determines whether the command can execute in its current state. 
        /// </summary>
        /// <param name="parameter">
        ///   The parameter that contains optional information.
        /// </param>
        /// <returns>
        ///   true if the command can be execute; otherwise false.
        /// </returns>
        public virtual Boolean CanExecute(Object parameter)
        {
            return true;
        }

        #endregion

        #region Execute

        /// <summary>
        ///   Executes the command.
        /// </summary>
        /// <param name="parameter">
        ///   The parameter that contains optional data.
        /// </param>
        public void Execute(Object parameter)
        {
            var lastCursor = Mouse.OverrideCursor;
            try
            {
                this.ExecuteInternal(parameter);
            }
            finally
            {
                Mouse.OverrideCursor = lastCursor;
            }
        }

        #endregion

        #region ExecuteInternal

        /// <summary>
        ///   The internal execute method.
        /// </summary>
        /// <param name="parameter">
        ///   The parameter to use for executing.
        /// </param>
        protected abstract void ExecuteInternal(Object parameter);

        #endregion


        // Properties
        // ////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The exception handler in case an Exception is throw in method ExecuteInternal.
        /// 
        /// A default exception handler <code>DefaultExecutionExceptionHandler</code>is 
        /// defined which logs the exception and displays a message box.
        /// 
        /// If another exception handler is set, it is valid for every ICommand derived from 
        /// class <code>CommandBase</code>. 
        /// 
        /// Another exception handler without displaying a message box should be defined for unit tests.
        /// </summary>
        public static Action<CommandBase, Exception> ExecutionExceptionHandler { get; set; }

        // Properties
        // ////////////////////////////////////////////////////////////////////////////////////////////

        #region CanExecuteChanged

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// 
        /// There is no problem if the caller tries to assign null or tries to remove handlers 
        /// which have not been added before.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                Debug.Assert(value != null, "Given event handler to add to CanExecuteChanged event cannot be null!");
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                Debug.Assert(value != null, "Given event handler to remove from CanExecuteChanged event cannot be null!");
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion
    }
}