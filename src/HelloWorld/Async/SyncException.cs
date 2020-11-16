using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ZHello.Async
{
    /*
     多线程异常处理问题
    */
    public class SyncException
    {
        public static async Task<int> GetVAsync()
        {
            await Task.Run(() => { Work(); });
            return 0;
        }

        public static void ThreadPoolException()
        {
            ThreadPool.SetMinThreads(4, 2);
            ThreadPool.QueueUserWorkItem(s => Work());
            ThreadPool.UnsafeQueueUserWorkItem(s => Work(), null);
            Thread.Sleep(5000);
        }

        public static void ThreadException()
        {
            var th = new Thread(() =>
            {
                Work();
            });
            th.Start();
            while (th.IsAlive)
            {
                Thread.Sleep(500);
            }
        }

        public static void TaskException()
        {
            var t = new Task(() =>
            {
                Work();
            });
            t.Start();
            try
            {
                t.Wait();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public static void Work()
        {
            int x = int.MaxValue - 100;
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    x += 10;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}