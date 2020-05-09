using System.Threading;

namespace ZHello.Algorithm
{
    /// <summary>
    /// 哲学家吃饭问题
    /// </summary>
    public class PhilosopherEatingProblem
    {
        /// <summary>
        /// 哲学家状态
        /// </summary>
        public enum PhilosopherState
        {
            /// <summary>
            /// 思考
            /// </summary>
            Thinking,

            /// <summary>
            /// 吃饭
            /// </summary>
            Eating,
        }
        
        /// <summary>
        /// 哲学家
        /// </summary>
        public class Philosopher
        {
            private SemaphoreSlim x_sem = new SemaphoreSlim(0, 1);
            public SemaphoreSlim Mutex { get; set; }
            public SemaphoreSlim Next { get; set; }

            private int x_count;
            public int Next_Count { get; set; }
            public PhilosopherState State { get; set; }

            public void Wait()
            {
                x_count++;
                if (Next_Count > 0)
                {
                    Next.Release();
                }
                else
                {
                    Mutex.Release();
                }
                x_sem.Wait();
                x_count--;
            }

            public void Signal()
            {
                if (x_count > 0)
                {
                    Next_Count++;
                    x_sem.Release();
                    Next.Wait();
                    Next_Count--;
                }
            }
        }

        /// <summary>
        /// 监视者
        /// </summary>
        public class MinitorDp
        {
            public Philosopher[] Self;
            public int Count { get; set; }

            public void Init(int count)
            {
                if (count <= 0)
                {
                    return;
                }
                Self = new Philosopher[count];
                for (int i = 0; i < count; i++)
                {
                    Self[i].State = PhilosopherState.Thinking;
                }
            }

            public void Test(int i)
            {
                if (Self[(i + Count - 1) / Count].State != PhilosopherState.Eating
                    && Self[i].State == PhilosopherState.Thinking
                    && Self[(i + 1) % Count].State != PhilosopherState.Eating)
                {
                    Self[i].State = PhilosopherState.Eating;
                    Self[i].Signal();
                }
            }

            public void PickUp(int i)
            {
                Self[i].State = PhilosopherState.Thinking;
                Test(i);
                if (Self[i].State == PhilosopherState.Eating)
                {
                    Self[i].Wait();
                }
            }

            public void PutDown(int i)
            {
                Self[i].State = PhilosopherState.Thinking;
                Test((i + Count - 1) % Count);
                Test((i + 1) % Count);
            }
        }

        /// <summary>
        /// 资源收集者
        /// </summary>
        public class ResourceAllocator
        {
            private bool busy;
            private SemaphoreSlim x;

            public void Init()
            {
                busy = false;
            }

            public void Acquire(int time)
            {
                if (busy)
                {
                    x.Wait(time);
                }
                busy = true;
            }

            public void Release()
            {
                busy = false;
                x.Release();
            }
        }
    }
}