using System;
using System.Diagnostics.Contracts;

namespace ZHello.Algorithm
{
    //common language specification
    public static class CLS
    {
        public static ushort MAXIMUM_CAPACITY_Ushort = 1 << (sizeof(ushort) - 1);
        public static short MAXIMUM_CAPACITY_Short = 1 << (sizeof(short) - 1);
        public static int MAXIMUM_CAPACITY_Int = 1 << (sizeof(int) - 1);
        public static uint MAXIMUM_CAPACITY_Uint = 1 << (sizeof(uint) - 1);
        public static long MAXIMUM_CAPACITY_Long = 1 << (sizeof(long) - 1);
        public static ulong MAXIMUM_CAPACITY_Ulong = 1 << (sizeof(ulong) - 1);

        public static int tableSizeFor_or(int c)
        {
            int n = c - 1;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            n |= n >> 32;
            return (n < 0) ? 1 : (n >= MAXIMUM_CAPACITY_Int) ? MAXIMUM_CAPACITY_Int : n + 1;
        }

        public static int tableSizeFor(int c)
        {
            int n = c - 1;
            int s = sizeof(int);
            for (int i = 0; i < s; i++)
            {
                n |= n >> (1 << i);
            }
            return (n < 0) ? 1 : (n >= MAXIMUM_CAPACITY_Int) ? MAXIMUM_CAPACITY_Int : n + 1;
        }

        public static ulong tableSizeFor(ulong c)
        {
            ulong n = c - 1;
            int s = sizeof(ulong);
            for (int i = 0; i < s; i++)
            {
                n |= n >> (1 << i);
            }
            return (n < 0) ? 1 : (n >= MAXIMUM_CAPACITY_Ulong) ? MAXIMUM_CAPACITY_Ulong : n + 1;
        }
    }

    /// <summary>
    /// 高效编程的奥秘
    /// 位操作技巧
    /// </summary>
    internal class HackersDelight
    {
        private static void Out(dynamic value, int intbase = 2)
        {
            //前置条件判断
            Contract.Requires(intbase == 2 || intbase == 8 || intbase == 10 || intbase == 16);
            if (value is string)
            {
                Console.Write(value);
            }
            else if (value is byte || value is short
                || value is ushort || value is short
                || value is uint || value is int
                || value is ulong || value is long)
            {
                Console.WriteLine(Convert.ToString(value, intbase));
            }
            else
            {
                Console.WriteLine(value.ToSting());
            }
        }

        private static void Print(string msg, dynamic value = null, int intBase = 2)
        {
            if (value != null)
            {
                Out(msg);
                Out(value, intBase);
            }
            else
            {
                Out(msg + "\n");
            }
        }

        private static void Main()
        {
            ushort x = ushort.Parse("6086");
            Print(string.Format("初始数字:{0},二进制数字:", x), x);
            Print("取反:~x:", ~x);
            Print("取负:-x:", -x);
            Print("~(-x)+1:", ~(-x) + 1);
            Print("反转最右侧的1:", x & (x - 1));
            Print("反转最右侧的0:", x | (x + 1));
            Print("反转最右侧尾部的1:", x & (x + 1));
            Print("反转最右侧尾部的0:", x | (x - 1));
            Print("用1标记最右侧的0:", ~x & (x + 1));
            Print("用0标记最右侧的1:", ~x | (x - 1));
            Print("用1标记最右侧尾部的0:", ~x & (x - 1));
            Print("用1标记最右侧尾部的0:", ~(x | -x));
            Print("用1标记最右侧尾部的0:", ~x & ~(-x));
            Print("用1标记最右侧尾部的0:", (x & -x) - 1);
            Print("用0标记最右侧尾部的1:", ~x | (x + 1));
            Print("析出最右侧的1:", x & (-x));
            Print("标记最右侧的1并将尾部0填充为1:", x ^ (x - 1));
            Print("标记最右侧的0并将尾部1填充为0:", x ^ (x + 1));
            Print("反转最右侧连续的1:", ((x | (x - 1)) + 1) & x);
            Print("反转最右侧连续的1:", ((x & -x) + x) & x);
            Print("De Morgan Laws Extended\n");
            Print("~(x & y) = ~x | ~y");
            Print("~(x | y) = ~x & ~y");
            Print("~(x + y) = ~x - y");
            Print("~(x - y) = ~x + y");
            Print("~(x + 1) = ~x - 1");
            Print("~(x - 1) = ~x + 1");
            Print("~-x  = x - 1");
            Print("~(x ^ y) = ~x ^ y = x E y");
            Print("~(x E y) = ~x E y = x ^ y");
            //a= --a
            //~~a = ~(-a)+1
            //a-1 = ~(-a)-> -a = ~(a-1)
        }
    }
}