using System;

namespace ZHello.Algorithm.Sort
{
    public class MergeSort : Sort, ISort
    {
        /// <summary>
        /// 归并排序
        /// 时间复杂度O(nlgn)
        /// 1.分解 将队列分解为可以解决的两个基元队列
        /// 2.解决 解决基元队列
        /// 3.合并 合并所有的已解决的基元队列
        /// 算法导论 2.3.1 分治法
        /// </summary>
        /// <param name="a"></param>
        public void Sort(int[] a)
        {
            SortCore(a, 0, a.Length - 1);
        }

        private void SortCore(int[] a, int p, int r)
        {
            if (p < r)
            {
                int q = (int)Math.Floor((p + r) / 2f);
                SortCore(a, p, q);
                SortCore(a, q + 1, r);
                Merge(a, p, q, r);
            }
        }

        private void Merge(int[] a, int p, int q, int r)
        {
            int n1 = q - p + 1;
            int n2 = r - q;
            int[] L = new int[n1 + 1];
            int[] R = new int[n2 + 1];
            int i = 0, j = 0;
            for (i = 0; i < n1; i++)
            {
                L[i] = a[p + i];
            }
            for (j = 0; j < n2; j++)
            {
                R[j] = a[q + j + 1];
            }
            //添加哨兵
            L[n1] = int.MaxValue;
            R[n2] = int.MaxValue;
            i = 0;
            j = 0;
            for (int k = p; k <= r; k++)
            {
                if (L[i] <= R[j])
                {
                    a[k] = L[i];
                    i++;
                }
                else
                {
                    a[k] = R[j];
                    j++;
                }
            }
        }
    }
}