using System;
using System.Diagnostics;
using System.Threading;
using Timer = System.Timers.Timer;

namespace ZHello.DesignPattern
{
    #region Spec Pattern

    /// <summary>
    /// 断容器模式
    /// </summary>
    public class CircuitBreakerPattern
    {
        /*熔断器模式

         处理远程服务或者资源请求在无法使用时，导致资源阻塞，而提供的过载保护策略；
         解决：
         1.防止应用程序不断尝试执行可能会失败的操作；
         2.使应用程序能够诊断错误是否已修正，然后再次执行操作；
         做法：
         1.记录最近调用发生错误次数，并决定执行或者返回错误；
         2.定义错误修正方法；
        三种状态：
        闭合状态（正常状态）-close：对于请求直接调用服务；
        断开状态（错误状态）-open：对于请求立即返回错误；
        半断开状态（半正常半错误状态）-half-open：允许一定数量请求调用服务，若全部返回成功，则断容器切换到close状态；
            若存在返回失败，则断容器切换到断开状态并尝试进行错误修正；

        状态切换：
        close ---> open：允许请求，一段时间内调用服务失败次数达到阈值后，状态切换为open；---计数重置
        open ---> half-open :切换到open状态后，不允许请求（返回错误），超时时钟计时（该计时时间内可以进行错误修正操作），计时结束后，切换到half-open；
        half-open --->open:允许一定请求，若请求存在执行失败，则切换到open；
        half-open --->close:允许一定请求，若请求全部执行成功，则切换到close；---计数切换

        两个计数：
        close下错误计数：基于时间的计数，在特定时间间隔后重置，下次进入close重置；
        half-open下成功计数：下次进入half-open重置；

        过程：
        close：
        Entry：错误计数重置；
        Critical：执行请求，判断结果；
        Exit：

        open：
        Entry：开始恢复计时器,修正错误；
        Critical：返回错误；
        Exit：

        half-open：
        Entry：成功计数重置；
        Critical：执行请求，判断结果；
        Exit：
         */

        /// <summary>
        /// 熔断器状态
        /// </summary>
        public enum State
        {
            /// <summary>
            /// 关闭
            /// </summary>
            Close = 1 << 0,

            /// <summary>
            /// 半开
            /// </summary>
            half_open = 1 << 1,

            /// <summary>
            /// 打开
            /// </summary>
            open = 1 << 2,
        }

        /// <summary>
        /// 熔断器主体 has-a state instance
        /// </summary>
        public class CircuitBreaker
        {
            /// <summary>
            /// 熔断器当前状态
            /// </summary>
            public State State { get; set; }

            //添加状态标识类
            private CState StateC { get; set; }

            /// <summary>
            /// 工作函数
            /// </summary>
            public Action Request { get; set; }

            /// <summary>
            /// 恢复函数
            /// </summary>
            public Func<bool> Restore { get; set; }

            /// <summary>
            /// 当前错误数量
            /// </summary>
            public int ErrorNum { get; set; }

            /// <summary>
            /// 关闭状态下 请求失败转换数
            /// </summary>
            public int MaxFailNum { get; set; }

            /// <summary>
            /// 半开状态下 请求成功状态转换数
            /// </summary>
            public int MaxSuccessNum { get; set; }

            public int ErrorRestoreTime { get; set; }

            public CircuitBreaker(Action act, Func<bool> res)
            {
                ErrorNum = 0;
                MaxFailNum = 3;
                ErrorRestoreTime = 100;
                Request = act;
                Restore = res;
                ConvertState(State.Close);//初始状态为close
            }

            /// <summary>
            /// 方法执行体
            /// </summary>
            public void Process()
            {
                StateC.Process();
            }

            /// <summary>
            /// 切换熔断器状态
            /// </summary>
            /// <param name="s"></param>
            public void ConvertState(State s)
            {
                Trace.WriteLine("ConvertState::" + s);
                State = s;
                switch (s)
                {
                    case State.Close:
                        StateC = new CloseState(this);
                        break;

                    case State.half_open:
                        StateC = new Half_OpenState(this);
                        break;

                    case State.open:
                        StateC = new OpenState(this);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 熔断器状态抽象类 定义行为方法
        /// has-a circuit break instance
        /// </summary>
        public abstract class CState : IDisposable
        {
            /// <summary>
            /// 引用主要执行实例
            /// </summary>
            private CircuitBreaker _breaker { get; set; }

            private object _lock = new object();

            public CState(CircuitBreaker b)
            {
                _breaker = b;
            }

            ~CState()
            {
                Exit();
            }

            /// <summary>
            /// 进入临界区函数
            /// </summary>
            protected abstract void Entry();

            /// <summary>
            /// 临界区函数
            /// </summary>
            protected abstract void Critical();

            /// <summary>
            /// 退出临界区函数
            /// </summary>
            protected abstract void Exit();

            protected void Request()
            {
                _breaker.Request?.Invoke();
            }

            protected void Restore()
            {
                _breaker.Restore?.Invoke();
            }

            public void Process()
            {
                Monitor.Enter(_lock);
                Critical();
                Monitor.Exit(_lock);
            }

            public void ConvertState(State state)
            {
                Monitor.Enter(_lock);
                _breaker.ConvertState(state);
                Monitor.Exit(_lock);
            }

            public void Dispose()
            {
                Exit();
            }
        }

        /// <summary>
        /// 关闭状态
        /// </summary>
        public class CloseState : CState
        {
            public int ErrorNum = 0;
            public int MaxError = 3;
            protected Timer timer;

            public CloseState(CircuitBreaker c) : base(c)
            {
                ErrorNum = c.ErrorNum;
                MaxError = c.MaxFailNum;
                timer = new Timer(c.ErrorRestoreTime);
                timer.Elapsed += (sender, e) =>
                {
                    if (ErrorNum > 0)
                    {
                        ErrorNum--;
                    }
                };
                Entry();
            }

            protected override void Critical()
            {
                try
                {
                    Request();
                }
                catch (Exception ex)
                {
                    ++ErrorNum;
                    if (GoOpen())
                    {
                        ConvertState(State.open);
                    }
                }
            }

            protected override void Entry()
            {
                timer.Start();
                ErrorNum = 0;
            }

            protected override void Exit()
            {
                timer.Stop();
            }

            /// <summary>
            /// 是否需要状态转换
            /// </summary>
            /// <returns></returns>
            private bool GoOpen()
            {
                return ErrorNum > MaxError;
            }
        }

        /// <summary>
        /// 半开状态
        /// </summary>
        public class Half_OpenState : CState
        {
            public Half_OpenState(CircuitBreaker c) : base(c)
            {
                Entry();
            }

            public int SuccessNum = 0;

            private bool GoClose()
            {
                return true;
            }

            protected override void Critical()
            {
                try
                {
                    Request();
                    SuccessNum++;
                    if (GoClose())
                    {
                        ConvertState(State.Close);
                    }
                }
                catch (Exception ex)
                {
                    ConvertState(State.open);
                }
            }

            protected override void Entry()
            {
                SuccessNum = 0;
            }

            protected override void Exit()
            {
            }
        }

        /// <summary>
        /// 打开状态
        /// </summary>
        public class OpenState : CState
        {
            protected Timer timer;

            public OpenState(CircuitBreaker c) : base(c)
            {
                timer = new Timer(100);
                timer.Elapsed += (sender, e) =>
                {
                    ConvertState(State.half_open);
                    timer.Stop();
                };
                Entry();
            }

            protected override void Critical()
            {
                //TODO:return error
            }

            protected override void Entry()
            {
                timer.Start();
                //修复错误
                Restore();
            }

            protected override void Exit()
            {
                timer.Stop();
            }
        }

        public static void Used()
        {
            var slim = new SemaphoreSlim(3, 3);
            Action work = new Action(() =>
            {
                if (slim.Wait(50))
                {
                    Trace.WriteLine("已请求资源 TS：" + Environment.TickCount);
                    Thread.Sleep(500);
                }
                else
                {
                    throw new Exception()
                    {
                        Source = "没有可用资源",
                    };
                }
                slim.Release();
            });
            Func<bool> restore = new Func<bool>(() =>
            {
                slim.Release();
                Trace.WriteLine("restore::");
                return true;
            });
            var cb = new CircuitBreaker(work, restore);
            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine(string.Format("执行第{0}次调用", i));
                cb.Process();
                Thread.Sleep(200);
            }
            Console.ReadLine();
        }
    }

    #endregion Spec Pattern
}