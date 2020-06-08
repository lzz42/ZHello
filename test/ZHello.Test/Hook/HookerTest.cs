using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZHello.Hook;

namespace ZHello.Test.Hook
{
    [TestClass]
    public class HookerTest
    {
        public class OrignClass
        {
            public int Add(int x,int y)
            {
                Trace.WriteLine($"Call Add X:{x} Y:{y}");
                return x + y;
            }
        }

        public class HookClass
        {
            public void HookAdd()
            {
                Trace.WriteLine($"Before Call Add ");
            }

            public int Add(int x,int y)
            {
                Trace.WriteLine($"Repea Call Add X:{x} Y:{y}");
                return (x + y) * 100;
            }
        }

        [TestMethod]
        public void Main()
        {
            InLineHooker hooker = new InLineHooker();
            var m1 = typeof(OrignClass).GetMethod("Add");
            var m2 = typeof(HookClass).GetMethod("HookAdd");
            var m3 = typeof(HookClass).GetMethod("Add");

            hooker.HookMethod(m1, m2, m3);
            var orig = new OrignClass();

            int t0 = orig.Add(11, 22);
            int t1 = orig.Add(11, 22);
            int t2 = orig.Add(33, 44);
        }

    }
}
