using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ZHello.Async
{
    /*
     多线程异常处理问题
         */


    /// <summary>
    /// 
    /// </summary>
    public class SyncException
    {
        public static void Main(string[] args)
        {
            TaskTest();
            Console.ReadKey();
        }

        public static async Task<int> GetVAsync()
        {
            await Task.Run(() => { Work(); });
            return 0;
        }

        public static void TaskTest()
        {

            var source = new CancellationTokenSource();

            var task = new Task(() => { Work(); }, source.Token);
            task.Start();
            try
            {
                //捕捉wait异常
                task.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //使用延续任务处理异常
            task.ContinueWith((t) =>
            {
                if (t.IsFaulted)
                {
                    Console.WriteLine(t.Exception.InnerException.Message);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
            //
            var awtiter = task.GetAwaiter();
            awtiter.OnCompleted(() =>
            {
                //此处异常直接抛出
                try
                {
                    awtiter.GetResult();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            task.ContinueWith((t) =>
            {
                DealTaskException(t);
            }, TaskContinuationOptions.OnlyOnCanceled);
            //匿名任务异常
            Task.Run(() => { Work(); });
            //该机制只有在GC进行垃圾回收时出现
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Console.WriteLine(e.Exception.Message);
            };
            GC.Collect();

            TaskCompletionSource<Task<int>> tc = new TaskCompletionSource<Task<int>>();
            new Thread(() =>
            {
                Work();
                tc.SetResult(new Task<int>(() => { return 9; }));
            }).Start();
            Console.Write(tc.Task.Result);
        }

        public static void DealTaskException(Task task)
        {
            if (task.IsFaulted)
            {
                Console.WriteLine(task.Exception.InnerException.Message);
            }
            else if (task.IsCanceled)
            {
                Console.WriteLine(task.Exception.InnerException.Message);
            }
        }

        public static void ThreadTest()
        {
            var t = new Thread(Work);
            t.Start();
            var e = t.ExecutionContext;

        }

        public static void ThreadPoolTest()
        {
            ThreadPool.QueueUserWorkItem((obj) => { Work(); }, null);

        }

        public static void Work()
        {
            //Int32.Parse("ok");
            //throw new Exception("test");
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {

                }
            }
        }
    }
}
