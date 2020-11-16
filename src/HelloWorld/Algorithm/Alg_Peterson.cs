using System.Diagnostics;
using System.Threading;

namespace ZHello.Algorithm
{

    /*
    操作系统概念 6.3 Peterson算法
    软件临界区
    适用于两个进程在临界区与剩余区进行交替执行的问题
    */
    /// <summary>
    /// Peterson 软件互斥算法
    /// 两个进程的算法
    /// </summary>
    public class Alg_Peterson
    {
        public delegate void WorkDelegate(int i);

        private int Turn = 0;
        private bool[] Flag { get; set; }

        public event WorkDelegate OnWork;

        /// <summary>
        /// 两个线程的Peterson算法
        /// </summary>
        public Alg_Peterson()
        {
            Flag = new bool[2] { false, false };
        }

        public void Work_Safe(int i)
        {
            int j = 1 - i;
            do
            {
                //进入区
                Flag[i] = true;
                Turn = j;
                while (Flag[j] && Turn == j)
                {
                    Thread.Sleep(5);
                }
                //进入区

                //临界区
                //TODO:
                DoWork(i);
                //临界区

                //退出区
                Flag[i] = false;
                //退出区

                //剩余区
                //TODO:
                //剩余区
            } while (true);
        }

        public virtual void DoWork(int i)
        {
            if (OnWork != null)
            {
                OnWork(i);
            }
            Trace.WriteLine($"{i} is Working.");
        }
    }

    public class Peterson_Algorithm_N
    {
        private int Turn = 0;
        private int N { get; set; }
        private bool[] Flag { get; set; }

        public Peterson_Algorithm_N(int n)
        {
            N = n;
            Flag = new bool[n];
        }

        public void Visit_Safe(int i)
        {
            int j = 1 - i;
            do
            {
                //进入区
                Flag[i] = true;
                Turn = j;
                while (Flag[j] && Turn == j)
                {
                    Thread.Sleep(5);
                }
                //进入区

                //临界区
                //TODO:
                Visit_Resouce(i);
                //临界区

                //退出区
                Flag[i] = false;
                //退出区

                //剩余区
                //TODO:
                //剩余区
            } while (true);
        }

        private void Visit_Resouce(int i)
        {
            Trace.WriteLine("Visitor : " + i);
        }
    }
}