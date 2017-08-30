using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace ThinkSharp.FeatureTouring.Logging
{
    internal class Log4NetLogger : ILogger
    {
        private static readonly ILog Logger = LogManager.GetLogger("FeatureTour");

        public void Debug(object message) => Logger.Debug(message);
        public void Debug(object message, Exception exception) => Logger.Debug(message, exception);
        public void DebugFormat(string format, params object[] args) => Logger.DebugFormat(format, args);

        public void Error(object message) => Logger.Error(message);
        public void Error(object message, Exception exception) => Logger.Error(message, exception);
        public void ErrorFormat(string format, params object[] args) => Logger.ErrorFormat(format, args);

        public void Info(object message) => Logger.Info(message);
        public void Info(object message, Exception exception) => Logger.Info(message, exception);
        public void InfoFormat(string format, params object[] args) => Logger.InfoFormat(format, args);

        public void Warn(object message) => Logger.Warn(message);
        public void Warn(object message, Exception exception) => Logger.Warn(message, exception);
        public void WarnFormat(string format, params object[] args) => Logger.WarnFormat(format, args);
    }
}
