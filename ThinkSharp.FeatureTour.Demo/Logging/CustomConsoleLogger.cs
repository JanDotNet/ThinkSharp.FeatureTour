using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using ThinkSharp.Logging;

namespace ThinkSharp.FeatureTouring.Logging
{
    internal class CustomConsoleLogger : ILogger
    {
        private void LogToFile(string obj) => Console.WriteLine("FEATURETOUR - " + obj);

        public void Debug(object message) => LogToFile("DEBUG: " + message);
        public void Debug(object message, Exception exception) => LogToFile($"DEBUG: {message}; Exeption: {exception}");
        public void DebugFormat(string format, params object[] args) => LogToFile(string.Format("DEBUG: " + format, args));

        public void Error(object message) => LogToFile("ERROR: " + message);
        public void Error(object message, Exception exception) => LogToFile($"ERROR: {message}; Exeption: {exception}");
        public void ErrorFormat(string format, params object[] args) => LogToFile(string.Format("ERROR: " + format, args));

        public void Info(object message) => LogToFile("INFO: " + message);
        public void Info(object message, Exception exception) => LogToFile($"INFO: {message}; Exeption: {exception}");
        public void InfoFormat(string format, params object[] args) => LogToFile(string.Format("INFO: " + format, args));

        public void Warn(object message) => LogToFile("WARNING: " + message);
        public void Warn(object message, Exception exception) => LogToFile($"WARNING: {message}; Exeption: {exception}");
        public void WarnFormat(string format, params object[] args) => LogToFile(string.Format("WARNING: " + format, args));
    }
}
