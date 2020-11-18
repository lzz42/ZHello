using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
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

    /* 线程的开销
     线程内核对象（Thread kernal object）：
        该数据结构：线程描述属性信息+线程上下文（一个内存块，CPU寄存器集合，x86机器约700Byte,x64机器12040Byte,IA64机器约2500Byte）
     线程环境块(thread environment block TEB):
        用户模式中分配和初始化的内存块，大小为一个内存页（x86-x64为4K，IA64为8K）
        包含异常处理链首：线程进入try时插入一个节点，退出try时删除一个节点
        包含线程本地存储数据，GDI OpenGL数据结构：
    用户模式栈（User-Mode Stack）:
        用于存储传递给方法的局部变量和参数，以及一个当前执行的方法返回时，要执行代码的地址，默认大小为1M
    内核模式栈（Kernal-Mode Stack）:
        传递给内核模式的实参、内部函数参数、函数局部变量、返回地址，32位12KB,64位24KB
    DLL线程连接和线程分离通知：
        创建或者终止一个线程时，都会调用该进程内所有加载的DLL的DllMain方法，并传递DLL_THREAD_ATTACH或者DLL_THREAD_DETACH标识
        托管DLL不会收到通知（没有DllMain函数），非托管DLL可以通过调用Win32 DisableThreadLibraryCalls来禁用通知
    Windows线程切换操作：
        1.保存当前CPU寄存器值到当前运行线程的线程上下文
        2.调度其他线程，若该线程为其他进程，还需要切换该进程的虚拟地址空间
        3.加载该线程的上下文到CPU寄存器
    上下文切换是净开销
    再次调度同一线程时，不需要上下文切换
    CLR线程与Windows线程
        目前一个CLR线程直接对应于一个Windows线程
    创建专用线程的条件（Thread类）：
        1.线程需要以非普通线程优先级运行（所有线程池线程都以普通优先级运行）
        2.需要现场表现为一个前提线程，防止线程在任务未终结前进程终止（线程池始终是后台线程）
        3.计算限制任务需要长时间运行
        4.要启动一个线程，并可能调用Abort方法提前终止
    使用线程的理由：
        1.使用线程将代码同其他代码隔离，提供应用程序的可靠性
        2.简化代码
        3.实现并发执行
    线程优先级：
        Windows优先级从0~到31（最高），系统轮询检测高优先级进行调度，调度时会抢占低优先级的CPU，即使时间片未使用完
        进程的优先级类6个：Idle、Below Normal、Normal、Above Normal、Hight、RealTime,(一般默认为Normal)
        RealTime优先级很高，能干扰操作系统任务，需要“提高调度优先级”的特权
        线程优先级7个：Idle、Lowest、Below Normal、Normal、Above Normal、Hightest、Time-Critical
        基础优先级 = 优先级类 + 优先级
        基础优先级自动调整：16~31系统不会自动提升，0~15才会被动态提升
         */

    public static class ResetEvent_Semaphore_SpinLock
    {
        public static bool MLock = false;

        public static bool MLockToken = false;

        public static SpinLock MSpinLock = new SpinLock(false);

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

        public static void SpinLockMethod()
        {
            MSpinLock.Enter(ref MLockToken);
            MSpinLock.TryEnter(40, ref MLockToken);
            MSpinLock.Exit();
        }
    }

    /// <summary>
    /// 数据的线程本地存储
    /// </summary>
    public static class ThreadLocalT
    {
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

        public struct struct1
        {
            public int A { get; set; }
            public string Str { get; set; }
        }

        public class Obj
        {
            public int A { get; set; } = 10;

            public string Str { get; set; } = "ok";

            public struct1 stru { get; set; } = new struct1 { A = 20, Str = "no" };

            public Obj()
            {
            }
        }

        /// <summary>
        /// 原子操作
        /// </summary>
        public class Atom_Operaion
        {
            public volatile int IntAtom = 0;
            public volatile string StrAtom = "ok";
            public volatile Atom_Operaion Atom;

            public static void P_Invoke()
            {
                Thread.BeginThreadAffinity();
                //该代码必须使用当前物理操作系统线程来执行
                //P/Invoke Native Code
                Thread.EndThreadAffinity();

                //设置上下文数据
                CallContext.SetData("k1", "v1");
                ThreadPool.QueueUserWorkItem(state =>
                {
                    //此时可以访问到上下文数据
                    Console.WriteLine($"Name={CallContext.GetData("k1")}");
                });
                //阻止当前线程的上下文流动
                //提升应用程序性能
                ExecutionContext.SuppressFlow();
                ThreadPool.QueueUserWorkItem(state =>
                {
                    //此时将访问不到上下文数据
                    Console.WriteLine($"Name={CallContext.GetData("k1")}");
                });
            }

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
    }

    public abstract class Poller
    {
        private ManualResetEvent PauseEvent;
        private ManualResetEvent StopEvent;
        private ManualResetEventSlim PauseEventSlim;
        private ManualResetEventSlim StopEventSlim;

        private SpinLock ResSpinLock;
        private SpinWait ResSpinWait;

        private object lockObj1 = new object();
        private object lockObj2 = new object();

        private Semaphore Semaphore1;
        private SemaphoreSlim SemaphoreSlim2;

        private Mutex Mutex1;
        private Mutex Mutex2;

        private Queue<int> IntQueue1;
        public Action PollWork { get; set; }
        protected Thread mainThread { get; set; }

        public Poller()
        {
            mainThread = new Thread(Work);

            IntQueue1 = new Queue<int>(10);

            ResSpinLock = new SpinLock();
            ResSpinWait = new SpinWait();

            Semaphore1 = new Semaphore(1, 1);
            SemaphoreSlim2 = new SemaphoreSlim(1, 1);

            Mutex1 = new Mutex();
            bool crn = false;
            /*
             * 同步基元命名：
             Global\前缀：与系统进程共享
             Local\前缀：或者默认，与同一会话的进程共享，会话即一个登陆会话
            */
            Mutex2 = new Mutex(true, @"Global\{B8A9C6AC-FB28-43F0-8D9C-C8F1E7F8C53D}");

            Init();
        }

        public void Dispose()
        {
            Stop();

            PauseEvent.Dispose();
            StopEvent.Dispose();

            PauseEventSlim.Dispose();
            StopEventSlim.Dispose();

            ResSpinLock.Exit();

            Semaphore1.Release();
            Semaphore1.Dispose();
            SemaphoreSlim2.Release();
            SemaphoreSlim2.Dispose();

            Mutex1.Close();
            Mutex1.Dispose();
            Mutex2.Close();
            Mutex2.Dispose();
        }

        public void AddDataSafe(int d)
        {
            EntryCritical();
            //lock
            lock (lockObj1)
            {
                IntQueue1.Enqueue(d);
            }
            ExitCritical();
        }

        public void Pause()
        {
            StopEvent.Reset();
            PauseEvent.Reset();

            StopEventSlim.Reset();
            PauseEventSlim.Reset();
        }

        public void Start()
        {
            StopEvent.Reset();
            PauseEvent.Set();

            StopEventSlim.Reset();
            PauseEventSlim.Set();

            if (mainThread.ThreadState == System.Threading.ThreadState.Unstarted)
            {
                mainThread.Start();
            }
        }

        public void Stop()
        {
            PauseEvent.Reset();
            StopEvent.Set();
        }

        public void Work()
        {
            //ManualResetEvent
            while (true)
            {
                if (StopEvent.WaitOne(0))
                    break;
                PauseEvent.WaitOne();
                PollWork?.Invoke();
            }
            //ManualResetEventSlim
            while (true)
            {
                if (StopEventSlim.Wait(0))
                    break;
                PauseEventSlim.Wait(-1);
                PollWork?.Invoke();
            }
        }

        private void Init()
        {
            PauseEvent = new ManualResetEvent(false);
            StopEvent = new ManualResetEvent(false);
            PauseEventSlim = new ManualResetEventSlim(false);
            StopEventSlim = new ManualResetEventSlim(false);
        }

        /// <summary>
        /// 退出临界区
        /// </summary>
        private void ExitCritical()
        {
            ResSpinLock.Exit();

            Monitor.Exit(lockObj2);
            Semaphore1.Release();
            SemaphoreSlim2.Release();
        }

        /// <summary>
        /// 进入临界区
        /// </summary>
        private void EntryCritical()
        {
            //自旋锁
            var k = false;
            ResSpinLock.TryEnter(100, ref k);
            if (!k)
            {
                //手动进行自旋
                int max = 1024;
                while (true)
                {
                    if (max <= 0)
                    {
                        throw new Exception("获取SpinLock锁超过指定次数");
                    }
                    max++;
                    ResSpinWait.Reset();
                    ResSpinWait.SpinOnce();
                    ResSpinLock.TryEnter(20, ref k);
                    if (k)
                        break;
                }

                //系统封装自旋
                var r = SpinWait.SpinUntil(() =>
                {
                    ResSpinLock.TryEnter(10, ref k);
                    return k;
                }, 1000);
                if (!r)
                {
                    throw new Exception("获取SpinLock锁超时");
                }
            }
            //minitor
            Monitor.Enter(lockObj2);
            //semphare
            if (!Semaphore1.WaitOne(1000))
            {
                throw new Exception("获取Sempahre资源超时");
            }
            if (!SemaphoreSlim2.Wait(1000))
            {
                throw new Exception("获取SempahreSlim资源超时");
            }
        }
    }
}