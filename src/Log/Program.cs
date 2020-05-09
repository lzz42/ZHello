using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace Log
{
    class Program
    {
        [DllImport("Kernel32.dll")] //引入命名空间 using System.Runtime.InteropServices;  
        public static extern bool Beep(int frequency, int duration);

        static void Main(string[] args)
        {
            //TestLog4Net();
            TestBeep();
            //TestWindowsLog();
            //var i = Console.ReadLine();
        }

        static void TestBeep()
        {
            Console.WriteLine("Beep 测试");
            while (true)
            {
                Thread.Sleep(3000);
                Console.WriteLine("Beep!");
                Beep(1000, 800);
            }
        }

        static void TestLog4Net()
        {
            while (true)
            {
                try
                {
                    Convert.ToInt32("sss");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Log4NetHelper.Log.Error(ex, "");
                }
                Thread.Sleep(200);
            }
        }

        static void TestNLog()
        {
            while (true)
            {
                try
                {
                    Convert.ToInt32("sss");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    NLogHelper.Log.Error(ex, "");
                }
                Thread.Sleep(200);
            }
        }

        static void TestWindowsLog()
        {
            while (true)
            {
                try
                {
                    Convert.ToInt32("sss");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    WindowsLog.GetLogger().Debug(ex, "this is debug msg.");
                    WindowsLog.GetLogger().Error(ex, "this is error msg.");
                    WindowsLog.GetLogger().Info(ex, "this is info msg.");
                    WindowsLog.GetLogger().Warn(ex, "this is warn msg.");
                }
                Thread.Sleep(200);
            }
        }
    }
}
