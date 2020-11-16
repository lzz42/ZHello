using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ZHello.Base
{
    /// <summary>
    /// 函数调用信息
    /// </summary>
    public class CallInfo
    {
        public static string RecordMethodInfo()
        {
            var minfo = GetCallMethodInfo(2);
            return string.Format("Tid:{0},Assembly:{1},MethodInfo:{2}"
                , Thread.CurrentThread.ManagedThreadId
                , Assembly.GetCallingAssembly().FullName
                , minfo.ToString()
                );
        }

        public static string[] GetCallMethodInfo(int deep = 1)
        {
            StackTrace st = new StackTrace(true);
            var frames = st.GetFrames();
            if (deep >= frames.Length)
            {
                deep = frames.Length - 1;
            }
            StackFrame sf = st.GetFrame(deep);
            return new string[]
            {
                sf.GetMethod().Name,
                sf.GetFileName(),
                sf.GetFileLineNumber().ToString(),
            };
        }

        public static void GetCallMemberName([CallerMemberName] string name = "")
        {
        }

        public static void GetLineNumber([CallerLineNumber] int lineNumber = -1)
        {
        }

        public static void GetFilePath([CallerFilePath] string filePath = "")
        {
        }
    }

    public class @class
    {
        public static void @static(bool @bool)
        {
            if (@bool)
            {
                Trace.Write($"{@bool} is value");
            }
            else
            {
                Trace.Write($"{@bool} was value");
            }
        }

        public static void Main()
        {
            @class.@static(false);
            @cl\u0061ss.@st\u0061ti\u0063(false);
        }
    }
}