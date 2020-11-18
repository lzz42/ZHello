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

        public static void GetCallingInfo([CallerMemberName] string name = "", [CallerLineNumber] int lineNumber = -1, [CallerFilePath] string filePath = "")
        {

        }
    }

    /// <summary>
    /// 使用@将关键字作为普通字符串使用
    /// </summary>
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