using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace UnitTestProject
{
    public interface ITarget
    {
        void SomeMethod();
        bool SomeMethod(string file);
        int SomeMethod(int a, int b);
        bool SomeMethod(out int a);
        bool SomeMethod(ref string str);
        bool SomeMethod(int a, out int b, ref int c, params int[] d);
    }

    public class TestTarget : ITarget
    {
        public int SomeMethod(int a, int b)
        {
            System.Diagnostics.Contracts.Contract.Requires<ArgumentOutOfRangeException>(a + b > 0);
            Thread.Sleep(a + b);
            return a + b;
        }

        public bool SomeMethod(string file)
        {
            return string.IsNullOrEmpty(file);
        }

        public void SomeMethod()
        {
            Thread.Sleep(Environment.TickCount % 100);
        }

        public bool SomeMethod(out int a)
        {
            a = Environment.TickCount % 100;
            return a % 2 == 1;
        }

        public bool SomeMethod(ref string str)
        {
            System.Diagnostics.Contracts.Contract.Requires<ArgumentOutOfRangeException>(str!=null);
            var tc = Environment.TickCount;
            str += tc % 1000;
            return tc % 2 == 1;
        }

        public bool SomeMethod(int a, out int b, ref int c, params int[] d)
        {
            System.Diagnostics.Contracts.Contract.Requires<ArgumentOutOfRangeException>(a > 0);
            Thread.Sleep(a);
            b = a * 10;
            c = c * 100 + a;
            return d != null ? d.Length % 2 == 1 : false;
        }
    }

    public static class StaicTarget
    {
        public static int Add(int a, int b)
        {
            System.Diagnostics.Contracts.Contract.Requires<ArgumentOutOfRangeException>(a + b > 0);
            Thread.Sleep(a + b);
            return a + b;
        }

        public static bool Open(string file)
        {
            return string.IsNullOrEmpty(file);
        }

        public static void SomeMethod()
        {
            Thread.Sleep(Environment.TickCount % 100);
        }

        public static bool SomeMethod(out int a)
        {
            a = Environment.TickCount % 100;
            return a % 2 == 1;
        }

        public static bool SomeMethod(ref string str)
        {
            System.Diagnostics.Contracts.Contract.Requires<ArgumentOutOfRangeException>(str != null);
            var tc = Environment.TickCount;
            str += tc % 1000;
            return tc % 2 == 1;
        }

        public static bool SomeMethod(int a, out int b, ref int c, params int[] d)
        {
            System.Diagnostics.Contracts.Contract.Requires<ArgumentOutOfRangeException>(a > 0);
            Thread.Sleep(a);
            b = a * 10;
            c = c * 100 + a;
            return d != null ? d.Length % 2 == 1 : false;
        }
    }
}
