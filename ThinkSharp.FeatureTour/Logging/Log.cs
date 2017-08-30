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
        public static void DebugFormat(string format, params object[] args)
        {
            theLogger.DebugFormat(format, args);
        }
        public static void Info(object message)
        {
            theLogger.Info(message);
        }
        public static void Info(object message, Exception exception)
        {
            theLogger.Info(message, exception);
        }
        public static void InfoFormat(string format, params object[] args)
        {
            theLogger.InfoFormat(format, args);
        }
        public static void Warn(object message)
        {
            theLogger.Warn(message);
        }
        public static void Warn(object message, Exception exception)
        {
            theLogger.Debug(message, exception);
        }
        public static void WarnFormat(string format, params object[] args)
        {
            theLogger.WarnFormat(format, args);
        }
        public static void Error(object message)
        {
            theLogger.Error(message);
        }
        public static void Error(object message, Exception exception)
        {
            theLogger.Error(message, exception);
        }
        public static void ErrorFormat(string format, params object[] args)

        {
            theLogger.ErrorFormat(format, args);
        }
    }
}
