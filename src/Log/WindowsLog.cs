using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log;

namespace Log
{
    public class WindowsLog : ILog
    {
        private EventLog mLog { get; set; }
        public bool IsErrorLog { get; private set; }

        public EntryWrittenEventHandler OnWrittenLog;

        private WindowsLog(string logName, string machineName, string source)
        {
            if (logName == null)
            {
                logName = AppDomain.CurrentDomain.FriendlyName;
            }
            if (machineName == null)
            {
                machineName = Environment.MachineName;
            }
            if (source == null)
            {
                source = AppDomain.CurrentDomain.FriendlyName;
            }
            //TODO:需要管理员权限方能查找事件源
            if (IsAdministratorRole())
            {
                if (source.Length > 254)
                {
                    source = source.Substring(0, 254);
                }
                if (!EventLog.SourceExists(source, machineName))
                {
                    var cData = new EventSourceCreationData(source, logName);
                    cData.MachineName = machineName;
                    EventLog.CreateEventSource(cData);
                }
                mLog = new EventLog(logName, machineName, source);
                mLog.EntryWritten += MLog_EntryWritten;
            }
            else
            {
                IsErrorLog = true;
            }
        }

        private static bool IsAdministratorRole()
        {
            System.Security.Principal.WindowsIdentity wid = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal printcipal = new System.Security.Principal.WindowsPrincipal(wid);
            return printcipal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        private void MLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            if (OnWrittenLog != null)
            {
                OnWrittenLog.Invoke(sender, e);
            }
        }

        private WindowsLog(string logName, string source)
            : this(logName, null, source)
        {

        }

        private WindowsLog(string logName)
            : this(logName, null, null)
        {

        }

        private WindowsLog()
            : this(null, null, null)
        {
        }

        private static object mLock = new object();

        private static ILog mLogger { get; set; }

        public static ILog GetLogger()
        {
            if (mLogger == null)
            {
                lock (mLock)
                {
                    if (mLogger == null)
                    {
                        mLogger = new WindowsLog();
                    }
                }
            }
            return mLogger;
        }

        public void Debug(Exception ex, string msg)
        {
            if (IsErrorLog)
            {
                return;
            }
            string message = null;
            if (ex != null)
            {
                message = ex.Message + ".\n" + ex.StackTrace + ".\n" + msg;
            }
            else
            {
                message = "NULL.\n" + msg;
            }
            if(message.Length> 31839)
            {
                message = message.Substring(0, 31839);
            }
            mLog.WriteEntry(message, EventLogEntryType.Information, 0, 0, null);
        }

        public void Info(Exception ex, string msg)
        {
            if (IsErrorLog)
            {
                return;
            }
            string message = null;
            if (ex != null)
            {
                message = ex.Message + ".\n" + ex.StackTrace + ".\n" + msg;
            }
            else
            {
                message = "NULL.\n" + msg;
            }
            if (message.Length > 31839)
            {
                message = message.Substring(0, 31839);
            }
            mLog.WriteEntry(message, EventLogEntryType.Information, 0, 1, null);
        }

        public void Warn(Exception ex, string msg)
        {
            if (IsErrorLog)
            {
                return;
            }
            string message = null;
            if (ex != null)
            {
                message = ex.Message + ".\n" + ex.StackTrace + ".\n" + msg;
            }
            else
            {
                message = "NULL.\n" + msg;
            }
            if (message.Length > 31839)
            {
                message = message.Substring(0, 31839);
            }
            mLog.WriteEntry(message, EventLogEntryType.Warning, 0, 2, null);
        }

        public void Error(Exception ex, string msg)
        {
            if (IsErrorLog)
            {
                return;
            }
            string message = null;
            if (ex != null)
            {
                message = ex.Message + ".\n" + ex.StackTrace + ".\n" + msg;
            }
            else
            {
                message = "NULL.\n" + msg;
            }
            if (message.Length > 31839)
            {
                message = message.Substring(0, 31839);
            }
            mLog.WriteEntry(message, EventLogEntryType.Error, 0, 3, null);
        }
    }
}
