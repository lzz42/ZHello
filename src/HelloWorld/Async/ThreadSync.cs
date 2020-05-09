#pragma warning disable CS3026,CS3001
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ZHello.Async
{
    /*线程同步相关
     
     1.采用线程同步事件EventWaitHandle的派生类：
        a.ManualResetEvent
        b.AotuResetEvent
        c.MaualResetEventSlim - 内使用ManualResetEvent
        d.Semaphore
        e.SemaphoreSlim
      2.线程本地存储
        ThreadLocal
      3.锁：
        a.lock
        b.Monitor
        c.Interlocked
        d.SpinLock、SimpleSpinLock
    */

    /*AutoResetEvent与ManualResetEvent的区别
     * 
     同步机制包括
     临界区（critical section），
     信号量（simphore），
     互斥量（mutex），
     管程（monitor）
    关于AutoResetEvent与ManualResetEvent
    ResetEvent线程通知事件
    可用于多线程同步 线程控制

    AutoResetEvent与ManualResetEvent的区别
    相同点
    1. 都从EventWaitHandle基类派生;
    2.Set方法将信号置为发送状态，Reset方法将信号置为不发送状态,WaitOne等待信号的发送。
    可以通过构造函数的参数值来决定其初始状态，若为true则非阻塞状态，为false为阻塞状态。
    如果某个线程调用WaitOne方法,则当信号处于发送状态时,该线程会得到信号, 继续向下执行
    不同点
    针对WaitOne()方法调用后的状态十分改变
    AutoResetEvent自动改变（置为不发送状态），则此时只有一个线程会继续执行；
    Manual则不会改变状态（保持原样即发送状态），则此时其他同步线程都可以执行；
    AutoResetEvent.WaitOne()每次只允许一个线程进入,当某个线程得到信号后,AutoResetEvent会自动又将信号置为不发送状态,
    则其他调用WaitOne的线程只有继续等待.也就是说,AutoResetEvent一次只唤醒一个线程;
    而ManualResetEvent则可以唤醒多个线程,因为当某个线程调用了ManualResetEvent.Set()方法后,
    其他调用WaitOne的线程获得信号得以继续执行,而ManualResetEvent不会自动将信号置为不发送。
    也就是说,除非手工调用了ManualResetEvent.Reset()方法,
    则ManualResetEvent将一直保持有信号状态,ManualResetEvent也就可以同时唤醒多个线程继续执行。

    关于Simphore与Simphoeresilm
    信号量与轻量级的信号量
    Simphore 控制对资源池的访问
    请求资源 WaitOne() 减少一次资源可用计数 若计数为0 则阻塞当前线程等待其他线程释放该资源
    释放资源Release() 增加一次可用计数

    关于Mutex
    
     */

    /*线程同步
     * 临界区问题
     * 进入区——》临界区——》退出区——》剩余区
     * 
     * 解决：
     * 1.互斥
     * 2.前进
     * 3.有限等待
     * 操作系统内处理临界区：抢占内核、非抢占内核
     * 
     * Peterson算法——》基于软件的临界区解决
     * 硬件同步：原子指令、信号量（semaphore）
     * 
     * 1.原子操作：volatile，InterLock
     * 2.锁：lock，Monitor，SpinLock，SpinWait
     * 3.互斥算法：
     * 4.特殊：Threadlocal
     * 5.信号量：Simphore,Simphoeresilm
     * 6.互斥体:Mutex
     * 
     * 原子操作 -   
    线程同步
    1.原子操作：一组能够保证操作原子性的API
    2.锁：
        自旋锁：单线程时执行快，但等待锁时浪费CPU；
        内核锁：系统内核维护的变量，内核管控线程变量访问，单线程时，用户线程需要切换到核心模读取变量，然后返回用户模式进行操作，造成资源浪费；
        混合锁：混合使用两种锁，先自旋，若资源被释放则不需要调用内核锁，Eg：Monitor是自旋锁，lock是内部调用Monitor；

    原子操作：volatile关键字Interlocked;
    锁：
    自旋锁：SpinLock,SimpleSpinLock，SpinWait
    内核锁：WaitHandle派生类：AutoResetEvent、Semaphore、Mutex；
    混合锁：Monitor,Lock

    其他：MethodImplAttribute 类和 SynchronizationAttribute 类

     */

    public interface IPoll : IDisposable
    {
        void Start();
        void Pause();
        void Stop();
        void Work();
    }

    public abstract class Poll : IPoll
    {
        protected Thread mainThread { get; set; }
        public Action PollWork { get; set; }
        public Poll()
        {
            mainThread = new Thread(Work);
        }
        public abstract void Dispose();
        public abstract void Pause();
        public abstract void Start();
        public abstract void Stop();
        public abstract void Work();
    }

    public class ManualResetEventPoller : Poll, IPoll
    {
        private ManualResetEvent _pauseEvent { get; set; }
        private ManualResetEvent _stopEvent { get; set; }
        public ManualResetEventPoller()
        {
            _pauseEvent = new ManualResetEvent(false);
            _stopEvent = new ManualResetEvent(false);
            Pause();
            mainThread.Start();
        }


        public override void Dispose()
        {
            Stop();
            _pauseEvent.Dispose();
            _stopEvent.Dispose();
        }

        public override void Pause()
        {
            _pauseEvent.Set();
        }

        public override void Start()
        {
            _pauseEvent.Reset();
            _stopEvent.Reset();
        }

        public override void Stop()
        {
            _stopEvent.Set();
        }

        public override void Work()
        {
            while (true)
            {
                while (_pauseEvent.WaitOne(1))
                {
                    if (_stopEvent.WaitOne(0))
                    {
                        System.Diagnostics.Trace.TraceInformation("Poll Exited");
                        break;
                    }
                }
                if (PollWork != null)
                {
                    PollWork.Invoke();
                }
            }
        }
    }

    public class ManualResetEventSlimPoller : Poll, IPoll
    {
        private ManualResetEventSlim _pauseEvent { get; set; }
        private ManualResetEventSlim _stopEvent { get; set; }
        public ManualResetEventSlimPoller()
        {
            _pauseEvent = new ManualResetEventSlim(false);
            _stopEvent = new ManualResetEventSlim(false);
            Pause();
            mainThread.Start();
        }


        public override void Dispose()
        {
            Stop();
            _pauseEvent.Dispose();
            _stopEvent.Dispose();
        }

        public override void Pause()
        {
            _pauseEvent.Set();
        }

        public override void Start()
        {
            _pauseEvent.Reset();
            _stopEvent.Reset();
        }

        public override void Stop()
        {
            _stopEvent.Set();
        }

        public override void Work()
        {
            while (true)
            {
                while (_pauseEvent.Wait(0))
                {
                    if (_stopEvent.Wait(0))
                    {
                        System.Diagnostics.Trace.TraceInformation("Poll Exited");
                        break;
                    }
                }
                if (PollWork != null)
                {
                    PollWork.Invoke();
                }
            }
        }
    }

    [Obsolete("未完成Poll", true)]
    public class AutoResetEventPoller : Poll
    {
        private AutoResetEvent _stopEvent { get; set; }
        private AutoResetEvent _pauseEvent { get; set; }
        public AutoResetEventPoller()
        {
            _stopEvent = new AutoResetEvent(false);
            _pauseEvent = new AutoResetEvent(false);
            Pause();
            mainThread.Start();
        }

        public override void Dispose()
        {
            _stopEvent.Dispose();
            _pauseEvent.Dispose();
        }

        public override void Pause()
        {
            _pauseEvent.Set();
        }

        public override void Start()
        {
            _pauseEvent.Reset();
            _stopEvent.Reset();
        }

        public override void Stop()
        {
            _stopEvent.Set();
        }

        public override void Work()
        {
            while (true)
            {
                while (_pauseEvent.WaitOne(0))
                {
                    if (_stopEvent.WaitOne(0))
                    {
                        System.Diagnostics.Trace.TraceInformation("Poll Exited");
                        break;
                    }
                }
                if (PollWork != null)
                {
                    PollWork.Invoke();
                }
            }
        }
    }

    public class SemaphoreStorage
    {
        private Semaphore chairs { get; set; }
        private Semaphore blocks { get; set; }
        private Queue<object> datas { get; set; }
        public SemaphoreStorage(int maxChairs, int maxBlocks)
        {
            chairs = new Semaphore(maxChairs, maxChairs);
            blocks = new Semaphore(maxBlocks, maxBlocks);
            datas = new Queue<object>();
        }

        public void WriteSync(object data)
        {
            chairs.WaitOne();
            blocks.WaitOne();
            //write data
            datas.Enqueue(data);
            chairs.Release();
        }

        public void ReadSync(out object data)
        {
            chairs.WaitOne();
            blocks.Release();
            //read data
            data = datas.Dequeue();
            chairs.Release();
        }
    }

    public static class ResetEvent_Semaphore_SpinLock
    {
        public static bool MLock = false;

        public static unsafe bool TestAndSet(bool* btarget)
        {
            bool res = *btarget;
            *btarget = true;
            return res;
        }

        public static unsafe void Method()
        {
            do
            {
                fixed (bool* temp = &MLock)
                {
                    while (TestAndSet(temp))
                    {
                        ;
                    }
                }
                //Critial Section
                MLock = false;
                //Remainder section

            } while (true);
        }

        public static unsafe void Swap(bool* a, bool* b)
        {
            bool t = *a;
            *a = *b;
            *b = t;
        }

        public static void WaitSemaphore(int t)
        {
            while (t <= 0)
            {
                ;
            }

            t--;
        }

        public static void SignalSemaphore(int t)
        {
            t++;
        }

        public static bool MLockToken = false;
        public static SpinLock MSpinLock = new SpinLock(false);

        public static void SpinLockMethod()
        {

            MSpinLock.Enter(ref MLockToken);
            MSpinLock.TryEnter(40, ref MLockToken);
            MSpinLock.Exit();
        }
    }

    /// <summary>
    /// Dekker 互斥算法
    /// </summary>
    public class Dekker
    {

    }
}

/// <summary>
/// 数据的线程本地存储
/// </summary>
public static class ThreadLocalT
{
    public struct struct1
    {
        public int A { get; set; }
        public string Str { get; set; }
    }
    public class Obj
    {
        public Obj()
        {

        }
        public int A { get; set; } = 10;
        public string Str { get; set; } = "ok";
        public struct1 stru { get; set; } = new struct1 { A = 20, Str = "no" };
    }
    public static ThreadLocal<int> count = new ThreadLocal<int>();
    public static ThreadLocal<struct1> st = new ThreadLocal<struct1>();
    public static ThreadLocal<Obj> objs = new ThreadLocal<Obj>(() => { return new Obj(); });

    public static void Run()
    {

        Thread th1 = new Thread(() =>
            {
                for (int i = 0; i < 1 << 20; i++)
                {
                    count.Value++;
                    struct1 stt = st.Value;
                    stt.A++;
                    stt.Str += i;
                    st.Value = stt;
                    Obj o = objs.Value;
                    o.Str = "s" + i;
                    Thread.Sleep(67);
                    Trace.WriteLine("th1:" + objs.Value.Str);
                }
            });

        Thread th2 = new Thread(() =>
            {
                for (int i = 0; i < 1 << 20; i++)
                {
                    count.Value++;
                    struct1 stt = st.Value;
                    stt.A++;
                    stt.Str += i;
                    st.Value = stt;
                    Obj o = objs.Value;
                    o.Str = "s" + i;
                    Thread.Sleep(7);
                    Trace.WriteLine("th2:" + objs.Value.Str);
                }
            });
        th1.Start();
        Thread.Sleep(60);
        th2.Start();

        Thread.Sleep(100);
        Trace.WriteLine("Main:" + objs.Value.Str);
    }


    public class Atom_Operaion
    {
        public volatile int IntAtom = 0;
        public volatile string StrAtom = "ok";
        public volatile Atom_Operaion Atom;

        public void Atom_Enter()
        {
            double d_org = 2.2d;
            double d_new = 1.1d;
            double d_old;
            d_old = Interlocked.Exchange(ref d_org, d_new);
            SpinLock spinL = new SpinLock();
            bool r = true;
            spinL.Enter(ref r);
            spinL.Exit();

            SpinWait spin = new SpinWait();
            spin.SpinOnce();
        }
    }

    public class Locker
    {
        public class SpinLock_T
        {

        }
        public class KernalLock
        {
            public class MyKernalHandle : WaitHandle
            {

            }
        }
    }

}
