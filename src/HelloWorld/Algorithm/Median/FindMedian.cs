using System;

namespace ZHello.Algorithm.Median
{
    public class FindMedian
    {
        /// <summary>
        /// 需要n-1次比较才能找到最大值
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int FindMax(int[] a)
        {
            int max = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] > max)
                    max = a[i];
            }
            return max;
        }

        public static int FindMin(int[] a)
        {
            int min = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] < min)
                    min = a[i];
            }
            return min;
        }

        public static void FindMaxMin(int[] a, out int max, out int min)
        {
            //2n-2次比较 找到max和min
            max = FindMax(a);
            min = FindMin(a);
            //总共3[n/2]次比较 找到max和min
            int i = 1;
            if ((a.Length & 1) == 0)
            {
                if (a[0] > a[1])
                {
                    max = a[0];
                    min = a[1];
                }
                else
                {
                    max = a[1];
                    min = a[0];
                }
                i = 2;
            }
            else
            {
                max = min = a[0];
            }
            for (int j = i; j < a.Length; j += 2)
            {
                if (a[j] > a[j + 1])
                {
                    if (a[j] > max)
                        max = a[j];
                    if (a[j + 1] < min)
                        min = a[j + 1];
                }
                else
                {
                    if (a[j] < min)
                        min = a[j];
                    if (a[j + 1] > max)
                        max = a[j + 1];
                }
            }
        }

        public static int FindX(int[] a, int i)
        {
            if (a == null || a.Length == 0)
                return -1;
            if (a.Length == 1)
                return a[0];
            if (i > a.Length)
                return a[a.Length - 1];
            return FindX_Ex(a, 0, a.Length - 1, i);
        }

        public static int FindX_Ex(int[] a, int p, int r, int i)
        {
            if (p == r)
                return a[p];
            int q = Partition(a, p, r);
            int k = q - p + 1;
            if (i == k)
                return a[q];
            if (i < k)
                return FindX_Ex(a, p, q - 1, i);
            else
                return FindX_Ex(a, q + 1, r, i - k);
        }

        public static int Partition(int[] a, int p, int r)
        {
            if (p > r)
                return -1;
            if (p == r)
                return p;
            int q = new Random().Next(p, r);
            a[q] ^= a[r];
            a[r] ^= a[q];
            a[q] ^= a[r];

            int k = a[r];

            return q;
        }
    }
}