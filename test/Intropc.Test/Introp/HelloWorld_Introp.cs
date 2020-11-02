using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Intropc.Test
{
    public class HelloWorld_Introp
    { 
        const string DLLPath = @"x64\HelloWorld.dll";
        const string x86DLLPath = @"x86\HelloWorld.dll";

        [DllImport(DLLPath, EntryPoint = "TestFunc",CallingConvention = CallingConvention.Cdecl)]
        public static extern int TestFuncX64(int a);

        [DllImport(x86DLLPath, EntryPoint = "TestFunc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TestFuncX86(int a);

        [DllImport(DLLPath, EntryPoint = "Funci",CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Funci(int a,int b);

        [DllImport(DLLPath, EntryPoint = "Funci2")]
        public static extern int Funci2(string str);

        [DllImport(DLLPath, EntryPoint = "Funcc")]
        public static extern IntPtr Funcc(int a);

        [DllImport(DLLPath, EntryPoint = "Funcc2")]
        public static extern char Funcc2();

        [DllImport(DLLPath, EntryPoint = "GetMyIntroStruct",CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetMyIntroStruct(IntPtr mystr, ref int c);

        [DllImport(DLLPath, EntryPoint = "SetMyIntroStruct")]
        public static extern int SetMyIntroStruct(IntPtr mystr, int c);

        public static string Utf8ToString(IntPtr nativeUtf8, uint? length = null)
        {
            if (nativeUtf8 == IntPtr.Zero)
                return String.Empty;
            uint len = 0;
            if (length.HasValue)
            {
                len = length.Value;
            }
            else
            {
                while (Marshal.ReadByte(nativeUtf8, (int)len) != 0)
                {
                    ++len;
                }
            }
            byte[] buffer = new byte[len];
            Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        public static IntPtr StringToUTF8Inptr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return IntPtr.Zero;
            }
            return Marshal.StringToHGlobalAnsi(str);
        }

        public static int Convert(MyIntroStruct s1,out MyIntroStruct2 s2)
        {
            s2 = new MyIntroStruct2();
            s2.BB = s1.B;
            s2.C = s1.C;
            s2.CharsA = Utf8ToString(s1.CharsA);
            s2.CharsB = Utf8ToString(s1.CharsB);
            return 0;
        }
        
        public static void Call_Get()
        {
            var list = new List<MyIntroStruct>();
            //call SetMyIntroStruct
            var size = Marshal.SizeOf(typeof(MyIntroStruct));
            IntPtr ptr = IntPtr.Zero;
            //分配大小
            int count = 1;
            GetMyIntroStruct(ptr, ref count);
            if (count > 0)
            {
                ptr = Marshal.AllocHGlobal(count * size);                
            }
            if (ptr != IntPtr.Zero)
            {
                GetMyIntroStruct(ptr, ref count);
            }
            for (int i = 0; i < count; i++)
            {
                IntPtr p = IntPtr.Add(ptr, i * size);
                MyIntroStruct mm = (MyIntroStruct)Marshal.PtrToStructure(p, typeof(MyIntroStruct));
                list.Add(mm);
            }
            Marshal.FreeHGlobal(ptr);
            var res = new List<MyIntroStruct2>();
            list.ForEach(s =>
            {
                MyIntroStruct2 s2;
                Convert(s, out s2);
                res.Add(s2);
            });            
        }

        public static void Call_Set()
        {
            IntPtr ptr = IntPtr.Zero;
            ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MyIntroStruct)));
            MyIntroStruct ms = new MyIntroStruct();
            ms.B = false;
            ms.C = 998;            
            ms.CharsA = StringToUTF8Inptr("123456789");
            ms.CharsB = IntPtr.Zero;
            //ms.CharsB = StringToUTF8Inptr("KKKK");
            Marshal.StructureToPtr(ms, ptr, true);
            SetMyIntroStruct(ptr, 9);
            Marshal.FreeHGlobal(ptr);
            Marshal.FreeHGlobal(ms.CharsA);
            Marshal.FreeHGlobal(ms.CharsB);
        }

        public static void Main()
        {
            var a2 = Funci(7, 8);
            Console.WriteLine("a2:" + a2);
            var a3 = Funci2("abcd");
            Console.WriteLine("a3:" + a3);
            var a4 = Utf8ToString(Funcc(77));
            Console.WriteLine("a4:" + a4);
            var a5 = Funcc2();
            Console.WriteLine("a5:" + a5);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MyIntroStruct
    {        
        public IntPtr CharsA;//8
        public IntPtr CharsB;//8
        public int C;//4
        public bool A;//1
        public bool B;//1
        public char D;
        //8+4+4+4+4 = 28
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MyIntroStruct2
    {
        public string CharsA;
        public string CharsB;
        public int C;
        public bool A;
        public bool BB;
    }
}
