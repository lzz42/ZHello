using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading;

[assembly: CLSCompliant(true)]

namespace ZHello.Base
{
    public class EmitProgram
    {
        public static void FCurrentFunction()
        {
            Console.WriteLine(":@@@@:" + MethodInfo.GetCurrentMethod().Name);
        }

        public static void TestStackLenght(int num = 0)
        {
            try
            {
                Console.WriteLine(num);
                Thread.Sleep(1);
                num++;
                //int c = num;
                if (true)
                {
                    TestStackLenght(num);
                }
            }
            catch (StackOverflowException e)
            {
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Main(string[] args)
        {
            MakeHelloWorldFunc();
            Console.ReadKey();
        }

        #region IL指令的使用

        /// <summary>
        /// 使用IL指令动态生成方法
        /// </summary>
        public static void MakeHelloWorldFunc()
        {
            //定义一个名为HelloWorld的动态方法，没有返回值，没有参数
            DynamicMethod helloWorldMethod = new DynamicMethod("HelloWorld", null, null);
            //创建一个MSIL生成器，为动态方法生成代码
            ILGenerator helloWorldIL = helloWorldMethod.GetILGenerator();
            //将要输出的Hello World!字符创加载到堆栈上
            helloWorldIL.Emit(OpCodes.Ldstr, "Hello World!");
            //调用Console.WriteLine(string)方法输出Hello World!
            helloWorldIL.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            //方法结束，返回
            helloWorldIL.Emit(OpCodes.Ret);
            //完成动态方法的创建，并且获取一个可以执行该动态方法的委托
            Action HelloWorld = (Action)helloWorldMethod.CreateDelegate(typeof(Action));
            //执行动态方法，将在屏幕上打印Hello World!
            HelloWorld();
            Console.WriteLine("Hello World!");
        }

        #endregion IL指令的使用
    }

    #region Memory layout

    public class MemoryLayout
    {
        public static void StructSize()
        {
            unsafe
            {
                List<Type> cs, ss;
                GetAllTypes(out cs, out ss);
                List<object> instances = new List<object>();
                cs.ForEach(e =>
                {
                    var obj = AppDomain.CurrentDomain.CreateInstance(e.Assembly.FullName, e.FullName, true, BindingFlags.Default, null, null, null, null);
                    instances.Add(obj);
                });

                Console.WriteLine(Marshal.SizeOf(new object()));
                Console.WriteLine(sizeof(bool));
            }
        }

        public static void PrintTypeInfo()
        {
            Console.WriteLine("");
        }

        public static void PrintInstanceInfo(object obj)
        {
            Console.WriteLine("Unmanaged size:{0},Type:{1}", Marshal.SizeOf(obj), obj.GetType().Name);
        }

        public static void GetAllTypes(out List<Type> clss, out List<Type> structs)
        {
            clss = new List<Type>();
            structs = new List<Type>();
            var curtype = MethodInfo.GetCurrentMethod().DeclaringType;
            var nestedTypes = curtype.GetNestedTypes();
            for (int i = 0; i < nestedTypes.Length; i++)
            {
                if (nestedTypes[i].IsValueType && !nestedTypes[i].IsInterface)
                {
                    structs.Add(nestedTypes[i]);
                }
                else
                if (nestedTypes[i].IsClass)
                {
                    clss.Add(nestedTypes[i]);
                }
            }
        }

        [StructLayout(LayoutKind.Auto)]
        public struct LayoutKing_Auto_Struct
        {
            public bool bol;
            public int a;
            public byte b;
            public short c;
            public double d;
            public string str;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LayoutKing_Sequential_Struct
        {
            public bool bol;
            public int a;
            public byte b;
            public short c;
            public double d;
            public string str;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct Layout_Explicit_Struct
        {
            [FieldOffset(0)]
            public bool bol;

            [FieldOffset(1)]
            public int a;

            [FieldOffset(5)]
            public byte b;

            [FieldOffset(6)]
            public short c;

            [FieldOffset(8)]
            public double d;

            [FieldOffset(16)]
            public string str;
        }

        [StructLayout(LayoutKind.Auto)]
        public class LayoutKing_Auto
        {
            public bool bol;
            public int a;
            public byte b;
            public short c;
            public double d;
            public string str;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class LayoutKing_Sequential
        {
            public bool bol;
            public int a;
            public byte b;
            public short c;
            public double d;
            public string str;
        }

        [StructLayout(LayoutKind.Explicit)]
        public class Layout_Explicit
        {
            [FieldOffset(0)]
            public bool bol;

            [FieldOffset(1)]
            public int a;

            [FieldOffset(5)]
            public byte b;

            [FieldOffset(6)]
            public short c;

            [FieldOffset(8)]
            public double d;

            [FieldOffset(16)]
            public string str;
        }
    }

    #endregion Memory layout
}