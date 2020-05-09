using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Core;
using log4net.Repository;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Threading;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

/* Using log4net
 * step 1: using log4net.dll；
 * step 2: 添加程序集特性,指定配置文件；
 * step 3: 添加配置文件
 * step 4: 获取ILog对象
 * step 5: 使用
 */

namespace Log
{
    public class Log4NetHelper : ILog
    {
        public static ILog Log { get; private set; }
        public bool IsErrorLog { get; private set; }
        static Log4NetHelper()
        {
            Log = new Log4NetHelper();
        }

        private Log4NetHelper()
        {
        }

        public void Info(Exception ex,string msg)
        {
            var type = StackInfo.GetCallingType();
            log4net.LogManager.GetLogger(type.FullName).Info(msg, ex);
        }

        public void Debug(Exception ex, string msg)
        {
            var type = StackInfo.GetCallingType();
            log4net.LogManager.GetLogger(type.FullName).Debug(msg, ex);
        }

        public void Warn(Exception ex, string msg)
        {
            var type = StackInfo.GetCallingType();
            log4net.LogManager.GetLogger(type.FullName).Warn(msg, ex);
        }

        public void Error(Exception ex, string msg)
        {
            var type = StackInfo.GetCallingType();
            log4net.LogManager.GetLogger(type.FullName).Error(msg, ex);
        }
    }
}
