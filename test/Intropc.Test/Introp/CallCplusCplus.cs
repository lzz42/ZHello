using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Intropc.Test
{
    public class CallCplusCplus
    {
        public static bool IsV1 = true;
        private static string MGUID = "CallCplusCplus_{3489EEA6-8906-4B3A-8680-C7DE60B1B0C9}";
        private static string MGUID2 = "CallCplusCplus_{905D8A2D-C733-4CD7-8316-C64437CB355C}";
        private static Mutex mutex;
        private static Mutex mutex2;
        static CallCplusCplus()
        {
            try
            {
                bool isCreateNew = false;
                mutex = new Mutex(true, MGUID, out isCreateNew);
                if (!isCreateNew)
                {
                    bool isCreateNew2 = false;
                    mutex2 = new Mutex(true, MGUID2, out isCreateNew2);
                    if (!isCreateNew2)
                    {
                        throw new Exception("MGUID2 exit!!!");
                    }
                    IsV1 = false;
                }
                else
                {
                    IsV1 = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetStrLen(string str)
        {
            if (IsV1)
            {
                return Call_V1.GetStrLen(str);
            }
            else
            {
                //return Call_V2.GetStrLen(pStr);
            }
            return 0;
        }
        public static int Add(int a, int b)
        {
            if (IsV1)
            {
                return Call_V1.Add(a, b);
            }
            else
            {
                //return Call_V2.Add(a, b);
            }
            return 0;
        }

        public static char GetASCIIChar(int a)
        {
            if (IsV1)
            {
                return Call_V1.GetASCIIChar(a);
            }
            else
            {
                //return Call_V2.GetASCIIChar(a);
            }
            return '\n';
        }

        public static char GetChar()
        {
            if (IsV1)
            {
                return Call_V1.GetChar();
            }
            else
            {
                //return Call_V2.GetChar();
            }
            return '\n';
        }
        public static string GetString()
        {
            if (IsV1)
            {
                return Call_V1.GetString();
            }
            else
            {
                //var pStr = Call_V2.GetString();
                //return IntptrToString(pStr);
            }
            return null;
        }

        public static string IntptrToString(object obj)
        {
            if (obj is IntPtr)
            {
                var pStr = (IntPtr)obj;
                if (pStr == IntPtr.Zero)
                    return String.Empty;
                uint len = 0;
                while (Marshal.ReadByte(pStr, (int)len) != 0)
                {
                    ++len;
                }
                byte[] buffer = new byte[len];
                Marshal.Copy(pStr, buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
            else
            {
                return null;
            }
        }

        private static IntPtr StringToIntptr(string str)
        {
            if (str == null)
            {
                return IntPtr.Zero;
            }
            else
            {
                return Marshal.StringToHGlobalAnsi(str);
            }
        }

        public static bool IsX86Cpu()
        {
            var K = IntPtr.Size == 4;
            return !Environment.Is64BitProcess;
        }


        public class Call_V1
        {
            static readonly bool mIsX86 = true;

            static Call_V1()
            {
                mIsX86 = !Environment.Is64BitProcess;
            }

            public static int Add(int a, int b)
            {
                if (mIsX86)
                {
                    return AddX86(a, b);
                }
                else
                {
                    return AddX64(a, b);
                }
            }
            public static char GetASCIIChar(int a)
            {
                if (mIsX86)
                {
                    return GetASCIICharX86(a);
                }
                else
                {
                    return GetASCIICharX64(a);
                }
            }
            public static char GetChar()
            {
                if (mIsX86)
                {
                    return GetCharX86();
                }
                else
                {
                    return GetCharX64();
                }
            }
            public static string GetString()
            {
                if (mIsX86)
                {
                    var pStr = GetStringX86();
                    return IntptrToString(pStr);
                }
                else
                {
                    var pStr = GetStringX64();
                    return IntptrToString(pStr);
                }
            }
            public static int GetStrLen(string str)
            {
                var pStr = Marshal.StringToHGlobalAuto(str);
                if (mIsX86)
                {
                    return GetStrLenX86(pStr);
                }
                else
                {
                    return GetStrLenX64(pStr);
                }
            }

            const string DllPath = @"x86\V1\Helloworld.dll";
            [DllImport(DllPath, EntryPoint = "GetStringLen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
            public static extern int GetStrLenX86(IntPtr str);

            [DllImport(DllPath, EntryPoint = "GetASCIIChar", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern char GetASCIICharX86(int a);

            [DllImport(DllPath, EntryPoint = "Add", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern int AddX86(int a, int b);

            [DllImport(DllPath, EntryPoint = "GetChar", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern char GetCharX86();
            [DllImport(DllPath, EntryPoint = "GetString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern IntPtr GetStringX86();

            const string DllPathX64 = @"x64\V1\Helloworld.dll";

            [DllImport(DllPathX64, EntryPoint = "GetStringLen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
            public static extern int GetStrLenX64(IntPtr str);

            [DllImport(DllPathX64, EntryPoint = "GetASCIIChar", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern char GetASCIICharX64(int a);

            [DllImport(DllPathX64, EntryPoint = "Add", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern int AddX64(int a, int b);

            [DllImport(DllPathX64, EntryPoint = "GetChar", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern char GetCharX64();
            [DllImport(DllPathX64, EntryPoint = "GetString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern IntPtr GetStringX64();

        }

        public class Call_V2
        {
            const string DllPath = @"V2\Helloworld.dll";

            [DllImport(DllPath, EntryPoint = "GetStringLen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern int GetStrLen(IntPtr str);

            [DllImport(DllPath, EntryPoint = "GetASCIIChar", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern char GetASCIIChar(int a);

            [DllImport(DllPath, EntryPoint = "Add", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern int Add(int a, int b);

            [DllImport(DllPath, EntryPoint = "GetChar", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern char GetChar();

            [DllImport(DllPath, EntryPoint = "GetString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern IntPtr GetString();


        }

    }
}
