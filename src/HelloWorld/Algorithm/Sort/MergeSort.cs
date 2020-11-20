using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Algorithm.Sort
{
    public class MergeSort : Sort, ISort
    {
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
                L[i] = a[p+i];
            }
            for (i = 0; i < n2; i++)
            {
                R[i] = a[q+i];
            }
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
