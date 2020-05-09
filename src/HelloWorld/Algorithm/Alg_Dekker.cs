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
    /// Dekker算法
    ///软互斥算法
    /// </summary>
    public class Alg_Dekker
    {

        public class Worker
        {
            public string Name { get; set; }
            public Worker(string name)
            {
                Name = name;
            }

            public void Working()
            {
                //Thread.Sleep(Environment.TickCount % 1000);
                Trace.WriteLine(string.Format("{0} is Working.", Name));
            }

            public void Waiting()
            {
                //Thread.Sleep(Environment.TickCount % 500);
                Trace.WriteLine(string.Format("{0} is Waiting.", Name));
            }
        }

        public static void Run_Dekker_Two()
        {
            var w1 = new Worker("Bob");
            var w2 = new Worker("Max");
            var dekker = new Dekker_Two(w1,w2);
            var t1 = new Task(() =>
            {
                Thread.Sleep(Environment.TickCount % 1000);
                dekker.RunD0();
            });
            var t2 = new Task(() =>
            {
                Thread.Sleep(200);
                dekker.RunD1();
            });
            t1.Start();
            t2.Start();
        }

        public class Dekker_Two
        {

            private readonly Worker[] Workers;
            public Dekker_Two(Worker lw, Worker rw)
            {
                Workers = new Worker[2] { lw, rw };
            }

            private readonly bool[] Flag = new bool[2] { false, false };
            private int turn = 0;

            public void RunD0()
            {
                Run(0, 1);
            }

            public void RunD1()
            {
                Run(1, 0);
            }

            public void Run(int i, int j)
            {
                do
                {
                    //尝试进入互斥区域
                    Flag[i] = true;
                    while (Flag[j])
                    {
                        if (turn == j)
                        {
                            Flag[i] = false;
                            //等待其他人工作完成
                            while (turn == j)
                            {
                                //防止循环等待时CPU占用过高
                                Thread.Sleep(5);
                            }
                            //放置当前工作标识
                            Flag[i] = true;
                        }
                    }
                    //进入互斥区域
                    Workers[i].Working();
                    //退出互斥区域
                    turn = j;//轮询标识转移
                    Flag[i] = false;//状态恢复
                } while (true);
            }
        }

    }
}
