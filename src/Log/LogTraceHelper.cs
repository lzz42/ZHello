using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public class LogTraceHelper:ILog
    {
        public bool IsErrorLog { get; set; }

        public static void Initialize()
        {
            var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            var listenerDtl = new System.Diagnostics.DefaultTraceListener
            {
                LogFileName = "TraceLog_" + name + "_" + DateTime.Now.ToString("yyyMMdd") + ".txt"
            };
            System.Diagnostics.Trace.Listeners.Add(listenerDtl);
            
            var listenerTwtl = new System.Diagnostics.TextWriterTraceListener();
            listenerTwtl.Writer = Console.Out;
            //listenerTwtl.Writer = new System.IO.StreamWriter(name + ".txt", true, Encoding.UTF8);
            System.Diagnostics.Trace.Listeners.Add(listenerTwtl);

            //var listenerEltl = new System.Diagnostics.EventLogTraceListener();
            //System.Diagnostics.Trace.Listeners.Add(listenerEltl);

            //listenerEltl.EventLog = new System.Diagnostics.EventLog(name, Environment.MachineName, name);
            //var listenerEptl = new System.Diagnostics.Eventing.EventProviderTraceListener("{89185535-C194-48D9-82FB-0B01F7147461}", name);
            //System.Diagnostics.Trace.Listeners.Add(listenerEptl);
        }

        public static ILog Log { get; private set; }

        static LogTraceHelper()
        {
            Log = new LogTraceHelper();
        }


        public void Debug(Exception ex, string msg)
        {
            System.Diagnostics.Trace.TraceInformation("\nMsg:{0};\nStack:{1}", ex.Message, ex.StackTrace);
        }

        public void Error(Exception ex, string msg)
        {
            System.Diagnostics.Trace.TraceError("\nMsg:{0};\nStack:{1}", ex.Message, ex.StackTrace);
        }

        public void Info(Exception ex, string msg)
        {
            System.Diagnostics.Trace.TraceInformation("\nMsg:{0};\nStack:{1}", ex.Message, ex.StackTrace);
        }

        public void Warn(Exception ex, string msg)
        {
            System.Diagnostics.Trace.TraceWarning("\nMsg:{0};\nStack:{1}", ex.Message, ex.StackTrace);
        }
    }
}
