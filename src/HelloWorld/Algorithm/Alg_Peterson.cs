using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZHello.Algorithm
{
    /// <summary>
    /// Peterson 软件互斥算法
    /// </summary>
    public class Alg_Peterson
    {
        private int Turn = 0;
        private bool[] Flag { get; set; }

        public Alg_Peterson()
        {
            Flag = new bool[2] { false, false };
        }

        public void Visit_Safe(int i)
        {
            int j = 1 - i;
            do
            {
                //进入区
                Flag[i] = true;
                Turn = j;
                while(Flag[j] && Turn == j)
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

    public class Peterson_Algorithm_N
    {
        private int N { get; set; }


        private int Turn = 0;
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
