using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZHello
{
    public class HelloWorldEntry
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("hello world...");
            TaskManagerControl();
            Console.ReadLine();
            //IsDebugMode();
            //TestUser32Method();
            Console.ReadKey();
        }

        private static void InitMutex()
        {
            //local mutex: Local\[name]
            //global mutex:Global\[name]
            string name = @"Local\Helloworld";
            try
            {
                System.Threading.Mutex mutex;
                if (!System.Threading.Mutex.TryOpenExisting(name, out mutex))
                {
                    bool r = false;
                    mutex = new System.Threading.Mutex(true, name, out r);
                    if (r)
                    {
                        //create success
                    }
                    else
                    {
                        //create fail
                    }
                }
                else
                {
                    //create fail
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void IsDebugMode()
        {
            Console.WriteLine("Is Debug Mode?");
            if (Debugger.IsAttached)
            {
                Console.WriteLine("true");
            }
            else
            {
                Console.WriteLine("false");
            }
        }

        private static void ImmStatus()
        {
            var h = Process.GetCurrentProcess().Handle;
            if (ImmGetOpenStatus(h))
            {
                Console.WriteLine("Imm status:{0}", true);
            }
        }

        private static void PrintCurrentAsssemblyInfo()
        {
            var a = Assembly.GetCallingAssembly().GetCustomAttribute(typeof(AssemblyVersionAttribute));
            if (a != null && a is AssemblyVersionAttribute)
            {
                Console.WriteLine((a as AssemblyVersionAttribute).Version);
            }
        }

        private static void TestUser32Method()
        {
            while (true)
            {
                var cmd = Console.ReadLine();
                if (cmd.StartsWith("s "))
                {
                    var handle = cmd.Split(' ')[1];
                    if (int.TryParse(handle, out int c))
                    {
                        Console.WriteLine("ShowWindow 3");
                        SetForegroundWindow((IntPtr)c);
                        Console.WriteLine(ShowWindow((IntPtr)c, 3));
                    }
                }
                Console.WriteLine("waiting cmd...");
            }
        }

        private static void TaskManagerControl()
        {
            //CPU使用率=运行时间/空闲时间
            //空闲时间 线程睡眠时间ms Sleep
            //运行时间=(1*1000/单个CPU频率)*(代码行数/一个时钟周期)
            //2.5GHZ * 4
            //一个时钟周期时间 单位时间/单个CPU频率
            //
            //一个时钟周期可以运行的代码数量：2
            int time = 1000;
            Action a = new Action(() =>
            {
                unchecked
                {
                    long n = 25 * 100000000 * 2 / 2;
                    while (true)
                    {
                        for (long i = 0; i < n; i++)
                        {
                        }
                    }
                    //Thread.Sleep(500);
                }
            });
            var s = new Thread(new ThreadStart(a));
            //s.Start();
            Console.WriteLine("Sleep:" + time);
        }

        #region Window user32.dll

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindow")] //获取窗体句柄，hwnd为源窗口句柄
        /*wCmd指定结果窗口与源窗口的关系，它们建立在下述常数基础上：
              GW_CHILD
              寻找源窗口的第一个子窗口
              GW_HWNDFIRST
              为一个源子窗口寻找第一个兄弟（同级）窗口，或寻找第一个顶级窗口
              GW_HWNDLAST
              为一个源子窗口寻找最后一个兄弟（同级）窗口，或寻找最后一个顶级窗口
              GW_HWNDNEXT
              为源窗口寻找下一个兄弟窗口
              GW_HWNDPREV
              为源窗口寻找前一个兄弟窗口
              GW_OWNER
              寻找窗口的所有者
         */
        public static extern int GetWindow(
            int hwnd,
            int wCmd
            );

        [DllImport("User32.dll")]
        public static extern int GetWindowText(int hWnd, StringBuilder lpString, int nMaxCount);

        // 获取窗口信息
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        // 设置窗口属性
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hMenu, int nIndex, int dwNewLong);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion Window user32.dll

        #region IME输入法

        //https://docs.microsoft.com/en-us/windows/desktop/api/Imm/
        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hWnd); //获取当前正在输入的窗口的输入法句柄

        [DllImport("imm32.dll")]
        public static extern bool ImmGetOpenStatus(IntPtr hIMC);//判断当前输入法是否是打开的

        [DllImport("imm32.dll")]
        public static extern bool ImmGetConversionStatus(IntPtr hIMC, ref int conversion, ref int sentence); //获取当前输入法的状态

        [DllImport("imm32.dll")]
        public static extern bool ImmSetConversionStatus(IntPtr hIMC, int conversion, int sentence);//设置输入法的状态

        [DllImport("imm32.dll")]
        public static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);//释放上下文关联

        #endregion IME输入法
    }
}