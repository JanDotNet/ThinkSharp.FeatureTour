using System;
using System.Diagnostics;

namespace ThinkSharp.FeatureTouring.Logging
{
    public class ConsoleLogger : ILogger
    {
        private readonly string myPrefix;

        public ConsoleLogger() : this("")
        { }

        public ConsoleLogger(string prefix)
        {
            myPrefix = prefix;
        }

        public void Debug(object message)
        {
            if (Debugger.IsAttached)
                WriteLog(message);
        }
        public void Debug(object message, Exception exception)
        {
            if (Debugger.IsAttached)
                WriteLog(message + "; Exception: " + exception);
        }
        public void DebugFormat(string format, params object[] args)
        {
            if (Debugger.IsAttached)
                WriteLogFormat(format, args);
        }
        public void Error(object message)
        {
            if (Debugger.IsAttached)
                WriteLog(message);
        }
        public void Error(object message, Exception exception)
        {
            if (Debugger.IsAttached)
                WriteLog(message + "; Exception: " + exception);
        }
        public void ErrorFormat(string format, params object[] args)
        {
            if (Debugger.IsAttached)
                WriteLogFormat(format, args);
        }
        public void Info(object message)
        {
            if (Debugger.IsAttached)
                WriteLog(message);
        }
        public void Info(object message, Exception exception)
        {
            if (Debugger.IsAttached)
                WriteLog(message + "; Exception: " + exception);
        }
        public void InfoFormat(string format, params object[] args)
        {
            if (Debugger.IsAttached)
                WriteLogFormat(format, args);
        }
        public void Warn(object message)
        {
            if (Debugger.IsAttached)
                WriteLog(message);
        }
        public void Warn(object message, Exception exception)
        {
            if (Debugger.IsAttached)
                WriteLog(message + "; Exception: " + exception);
        }
        public void WarnFormat(string format, params object[] args)
        {
            if (Debugger.IsAttached)
                WriteLogFormat(format, args);
        }

        private void WriteLogFormat(string logContent, params object[] parameters)
        {
            Console.WriteLine(myPrefix + ":" + logContent, parameters);
        }

        private void WriteLog(object logContent)
        {
            Console.WriteLine(myPrefix + ":" + logContent);
        }
    }
}
