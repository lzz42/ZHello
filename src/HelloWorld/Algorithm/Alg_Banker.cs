using System;

namespace ZHello.Algorithm
{
    /// <summary>
    /// 银行家算法
    /// </summary>
    public static class Alg_Banker
    {
        //n Number of Process
        //m Number of Resources
        private static int M = 1;

        private static int N = 1;
        public static int[] Available = new int[M];
        public static int[][] Max = new int[N][];
        public static int[][] Allocation = new int[N][];
        public static int[][] Need = new int[N][];

        public static bool Bigger(int[] x, int[] y)
        {
            if (x.Length == y.Length)
            {
                bool c = true;
                for (int i = 0; i < x.Length; i++)
                {
                    c = c & (x[i] > y[i]);
                }
                return c;
            }
            return false;
        }

        public static bool BigOrEqual(int[] x, int[] y)
        {
            if (x.Length == y.Length)
            {
                bool c = true;
                for (int i = 0; i < x.Length; i++)
                {
                    c = c & (x[i] >= y[i]);
                }
                return c;
            }
            return false;
        }

        /// <summary>
        /// Safe Status Check
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool CheckSafeStatus(int m, int n)
        { //Step 1
            var work = Available;
            var finish = new bool[n];
            for (int i = 0; i < finish.Length; i++)
            {//Step 2
                if (!finish[i] && BigOrEqual(work, Need[i]))
                {//Step 3
                    for (int j = 0; j < work.Length; j++)
                    {
                        work[j] = work[j] + Allocation[i][j];
                    }
                    finish[i] = true;
                }
            }
            var res = true;
            //Step 4
            for (int i = 0; i < work.Length; i++)
            {
                res = res && finish[i];
            }
            return res;
        }

        /// <summary>
        /// source request of i
        /// </summary>
        /// <param name="i"></param>
        public static void RequestSource(int i)
        {
            int[] request = null;
            if (BigOrEqual(Need[i], request))
            {
                while (BigOrEqual(Available, request))
                {
                    //Wait
                    System.Threading.Thread.Sleep(10);
                }
                for (int j = 0; j < Available.Length; j++)
                {
                    Available[j] = Available[j] - request[j];
                }
                for (int j = 0; j < Allocation[i].Length; j++)
                {
                    Allocation[i][j] = Allocation[i][j] + request[j];
                }
                for (int j = 0; j < Need[i].Length; j++)
                {
                    Need[i][j] = Need[i][j] - request[j];
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }
}