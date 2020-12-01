using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Algorithm.Sort
{
    public class HeapSort : Sort, ISort
    {
        private class Heap
        {
            public Heap(int[] a)
            {
                HeapSize = 1;
                Data = a;
            }

            public int[] Data { get; set; }
            public int this[int i]
            {
                get { return Data[i]; }
                set { Data[i] = value; }
            }

            public int HeapSize { get; set; }

            public int Parent(int i)
            {
                return Data[(int)(Math.Floor(i / 2f))];
            }

            public int Left(int i)
            {
                return Data[2 * i];
            }

            public int Right(int i)
            {
                return Data[2 * i + 1];
            }
        }

        /// <summary>
        /// 维护最大堆性质 O(h)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="i"></param>
        private void Max_Heapify(Heap a, int i)
        {
            int l = a.Left(i);
            int r = a.Right(i);
            int largest = 0;
            if (l <= a.HeapSize && a[l] > a[i])
            {
                largest = l;
            }
            else
            {
                largest = i;
            }
            if (r <= a.HeapSize && a[r] > a[largest])
            {
                largest = r;
            }
            if (largest != i)
            {
                a[i] = a[i] ^ a[largest];
                a[largest] = a[i] ^ a[largest];
                a[i] = a[i] ^ a[largest];
                //a.HeapSize++;
                Max_Heapify(a, largest);
            }
        }

        /// <summary>
        /// 构建最大堆O(nlgn)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="h"></param>
        private void Build_Max_Heap(int[] a, out Heap h)
        {
            h = new Heap(a);
            h.HeapSize = a.Length - 1;
            int mSize = (int)(Math.Floor(h.HeapSize / 2f));
            for (int i = mSize; i >= 0; i--)
            {
                Max_Heapify(h, i);
            }
        }


        public void Sort(int[] a)
        {
            Heap h;
            Build_Max_Heap(a, out h);
            for (int i = a.Length - 1; i >= 1; i--)
            {
                h[0] = h[0] ^ h[i];
                h[i] = h[0] ^ h[i];
                h[0] = h[0] ^ h[i];
                h.HeapSize--;
                Max_Heapify(h, 0);
            }
            a = h.Data;
        }
    }
}
