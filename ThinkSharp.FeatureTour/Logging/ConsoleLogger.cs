// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics;

namespace ThinkSharp.FeatureTouring.Logging
{
    /// <summary>
    /// Logger that writes messages to the console (<see cref="Console.WriteLine(string)"/>).
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private readonly string myPrefix;

        /// <summary>
        /// Creates a new instance of the class. This version does not use a prefix.
        /// </summary>
        public ConsoleLogger() : this("")
        { }

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="prefix">
        /// The prefix to use for all logging entries.
        /// </param>
        public ConsoleLogger(string prefix)
        {
            myPrefix = prefix;
        }

        /// <summary>
        /// Log debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Debug(object message)
        {
            if (Debugger.IsAttached)
                WriteLog(message);
        }

        /// <summary>
        /// Log debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Debug(object message, Exception exception)
        {
            if (Debugger.IsAttached)
                WriteLog(message + "; Exception: " + exception);
        }

        /// <summary>
        /// Log error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Error(object message)
        {
            if (Debugger.IsAttached)
                WriteLog(message);
        }

        /// <summary>
        /// Log error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Error(object message, Exception exception)
        {
            if (Debugger.IsAttached)
                WriteLog(message + "; Exception: " + exception);
        }
        public void Info(object message)
        {
            if (Debugger.IsAttached)
                WriteLog(message);
        }

        /// <summary>
        /// Log informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Info(object message, Exception exception)
        {
            if (Debugger.IsAttached)
                WriteLog(message + "; Exception: " + exception);
        }
        public void Warn(object message)
        {
            if (Debugger.IsAttached)
                WriteLog(message);
        }

        /// <summary>
        /// Log warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Warn(object message, Exception exception)
        {
            if (Debugger.IsAttached)
                WriteLog(message + "; Exception: " + exception);
        }

        private void WriteLog(object logContent)
        {
            Console.WriteLine(myPrefix + ":" + logContent);
        }
    }
}
