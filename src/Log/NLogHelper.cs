using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Log
{
    public class NLogHelper: ILog
    {
        private static void ConfigNLog()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = "${basedir}/Log/${date:format=yyyy_MM_dd}.txt",
                Layout = "${longdate} ${level} \n ${message} \n ${exception}",
            };
            config.AddTarget(logfile);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);
            NLog.LogManager.Configuration = config;
        }
        public bool IsErrorLog { get; private set; }
        public static ILog Log { get; private set; }

        static NLogHelper()
        {
            ConfigNLog();
            Log = new NLogHelper();
        }

        private NLogHelper()
        {

        }

        public void Info(Exception ex,string msg)
        {
            var type = StackInfo.GetCallingType();
            NLog.LogManager.GetLogger(type.FullName).Info(ex,msg);
        }

        public void Debug(Exception ex, string msg)
        {
            var type = StackInfo.GetCallingType();
            NLog.LogManager.GetLogger(type.FullName).Debug(ex, msg);
        }

        public void Warn(Exception ex, string msg)
        {
            var type = StackInfo.GetCallingType();
            NLog.LogManager.GetLogger(type.FullName).Warn(ex, msg);
        }

        public void Error(Exception ex, string msg)
        {
            var type = StackInfo.GetCallingType();
            NLog.LogManager.GetLogger(type.FullName).Error(ex, msg);
        }

    }
}
