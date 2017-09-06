// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;

namespace ThinkSharp.FeatureTouring.Logging
{
    public static class Log
    {
        private static ILogger theLogger = new ConsoleLogger();

        public static void SetLogger(ILogger logger)
        {
            theLogger = logger ?? new NullLogger();
        }

        public static void Debug(object message)
        {
            theLogger.Debug(message);
        }
        public static void Debug(object message, Exception exception)
        {
            theLogger.Debug(message, exception);
        }
        public static void Info(object message)
        {
            theLogger.Info(message);
        }
        public static void Info(object message, Exception exception)
        {
            theLogger.Info(message, exception);
        }
        public static void Warn(object message)
        {
            theLogger.Warn(message);
        }
        public static void Warn(object message, Exception exception)
        {
            theLogger.Debug(message, exception);
        }
        public static void Error(object message)
        {
            theLogger.Error(message);
        }
        public static void Error(object message, Exception exception)
        {
            theLogger.Error(message, exception);
        }
    }
}
