using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Log
{
    public class ES6LogHelper : ILog
    {
        public static ILog Log { get; private set; }
        public bool IsErrorLog { get; private set; }

        static ES6LogHelper()
        {
            Log = new ES6LogHelper();
        }

        private ES6LogHelper()
        {

        }

        public void Debug(Exception ex, string msg)
        {
            LogEntry entry = new LogEntry();
            entry.Title = msg;
            entry.Message = ex.Message;
            Logger.Write(entry);
        }

        public void Error(Exception ex, string msg)
        {
            LogEntry entry = new LogEntry();
            entry.Title = msg;
            entry.Message = ex.Message;
            Logger.Write(entry);
        }

        public void Info(Exception ex, string msg)
        {
            LogEntry entry = new LogEntry();
            entry.Title = msg;
            entry.Message = ex.Message;
            Logger.Write(entry);
        }

        public void Warn(Exception ex, string msg)
        {
            LogEntry entry = new LogEntry();
            entry.Title = msg;
            entry.Message = ex.Message;
            Logger.Write(entry);
        }
    }
}
