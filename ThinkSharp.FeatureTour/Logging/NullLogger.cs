// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace ThinkSharp.FeatureTouring.Logging
{
    /// <summary>
    /// Logger that ignores logging
    /// </summary>
    public class NullLogger : ILogger
    {
        /// <summary>
        /// Log debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Debug(object message) { }

        /// <summary>
        /// Log debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Debug(object message, Exception exception) { }

        /// <summary>
        /// Log error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Error(object message) { }

        /// <summary>
        /// Log error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Error(object message, Exception exception) { }

        /// <summary>
        /// Log informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Info(object message) { }

        /// <summary>
        /// Log informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Info(object message, Exception exception) { }

        /// <summary>
        /// Log warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Warn(object message) { }

        /// <summary>
        /// Log warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Warn(object message, Exception exception) { }
    }
}
