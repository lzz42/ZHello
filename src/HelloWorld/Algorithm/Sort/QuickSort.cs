using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Algorithm.Sort
{
    public class QuickSort: Sort,ISort
    {
        /// <summary>
        /// 快速排序期望时间O(nlgn)，最坏情况O(n^2)
        /// 1.分解
        /// 2.解决
        /// 3.合并
        /// </summary>
        /// <param name="a"></param>
        public void Sort(int[] a)
        {
            QuickSortCore(a, 0, a.Length - 1);
        }

        private static void QuickSortCore(int[] a, int p, int r)
        {
            if (p < r)
            {
                int q = Partition(a, p, r);
                QuickSortCore(a, p, q - 1);
                QuickSortCore(a, q + 1, r);
            }
        }

        private static int Partition(int[] a, int p, int r)
        {
            int x = a[r];
            int i = p - 1;
            for (int j = p; j < r; j++)
            {
                if (a[j] <= x)
                {
                    i++;
                    Swap(ref a[j], ref a[i]);
                }
            }
            Swap(ref a[i + 1], ref a[r]);
            return i + 1;
        }
    }
}
