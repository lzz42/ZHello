using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Hook
{
    /*
     */
    public abstract class Hooker
    {

    }

    public class obj
    {
        public static void MethodAddr()
        {
            unsafe
            {
                //获取函数汇编调用地址
                MethodInfo m = null;
                var p = m.MethodHandle.GetFunctionPointer().ToPointer();
            }
        }
    }
}
