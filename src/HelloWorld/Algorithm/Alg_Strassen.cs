using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Algorithm
{
    public class Alg_Strassen
    {

        /// <summary>
        /// O(n^3)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int[,] Multiply(int[,] a, int[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0))
                return null;
            int row = a.GetLength(0);
            var col = b.GetLength(1);
            var r = new int[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    for (int k = 0; k < a.GetLength(1); k++)
                    {
                        r[i,j] += a[i,k] * b[k,j];
                    }
                }
            }
            return r;
        }

        /// <summary>
        /// O(n^lg7)
        /// 2.80<lg7<2.81
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int[,] Multiply_Square_Strassen(int[,] a, int[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0))
                return null;
            int row = a.GetLength(0);
            var col = b.GetLength(1);
            var r = new int[row, col];
            if (row == 1)
            {
                r[0, 0] = a[0, 0] * b[0, 0];
                return r;
            }
            if (row != col || row % 2 == 1 || row == 0)
                return null;
            //检测是否为2幂指数
            if ((~(row - 1)) != 0)
                return null;
            //检测是否为2幂指数
            if ((row & (row - 1)) != 0)
                return null;
            int n = row;
            

            return r;
        }

    }
}
