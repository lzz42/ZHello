using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHello.Test.Algorithm.Sort
{
    [TestClass]
    public class Sort
    {
        private int[] a0 { get; set; }

        private int[] a1 { get; set; }

        private int[] b0 { get; set; }

        private int[] b1 { get; set; }

        public static bool Equal(int[] a, int[] b)
        {
            if (a == null && b == null)
                return true;
            if (a != null && b != null)
            {
                if (a.Length == b.Length)
                {
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i] != b[i])
                            return false;
                    }
                    return true;
                }
            }
            return false;
        }

        public static void Trace(int[] a)
        {
            var builder = new StringBuilder();
            builder.Append("[");
            for (int i = 0; i < a.Length; i++)
            {
                builder.Append(string.Format(" {0}", a[i].ToString().PadLeft(4, ' ')));
            }
            builder.Append("]");
            //Console.WriteLine(builder.ToString());
            System.Diagnostics.Trace.WriteLine(builder.ToString());
        }

        [TestMethod]
        public void Swap()
        {
            int a1 = 1000;
            int a2 = 2000;

            int c1 = 1000;
            int c2 = 1000;

            int a10 = a1;
            int a20 = a2;

            ZHello.Algorithm.Sort.Sort.Swap(ref a1, ref a2);
            ZHello.Algorithm.Sort.Sort.Swap(ref c1, ref c2);
            Assert.IsTrue(a10 == a2);
            Assert.IsTrue(a20 == a1);
            Assert.IsTrue(c1 == 1000);
            Assert.IsTrue(c2 == 1000);
        }

        [TestInitialize]
        public void Init()
        {
            a0 = new int[11];
            a1 = new int[11];
            b0 = new int[12];
            b1 = new int[12];
            for (int i = 0; i < 11; i++)
            {
                a0[i] = i + 1;
                a1[i] = i + 1;
                b0[i] = i + 1;
                b1[i] = i + 1;
            }
            b0[11] = 1024;
            b1[11] = 1024;

            var r = new Random(7);
            for (int i = 0; i < 11; i++)
            {
                var rr = r.Next(1024, 2048);
                var cc = rr % 11;
                ZHello.Algorithm.Sort.Sort.Swap(ref a1[i], ref a1[cc]);
            }
            for (int i = 0; i < 12; i++)
            {
                var rr = r.Next(1024, 2048);
                var cc = rr % 12;
                ZHello.Algorithm.Sort.Sort.Swap(ref b1[i], ref b1[cc]);
            }
        }

        public void ISortTest(ZHello.Algorithm.Sort.ISort sort)
        {
            Trace(a1);
            sort.Sort(a1);
            Trace(a1);

            Trace(b1);
            sort.Sort(b1);
            Trace(b1);

            Assert.IsTrue(Equal(a0, a1));
            Assert.IsTrue(Equal(b0, b1));
        }

        [TestMethod]
        public void BubbleSort()
        {
            var sort = new ZHello.Algorithm.Sort.BubbleSort();
            ISortTest(sort);
        }

        [TestMethod]
        public void InsertSort()
        {
            var sort = new ZHello.Algorithm.Sort.InsertSort();
            ISortTest(sort);
        }

        [TestMethod]
        public void MergeSort()
        {
            var sort = new ZHello.Algorithm.Sort.MergeSort();
            ISortTest(sort);
        }

        [TestMethod]
        public void QuickSort()
        {
            var sort = new ZHello.Algorithm.Sort.QuickSort();
            ISortTest(sort);
        }

        [TestMethod]
        public void SelectSort()
        {
            var sort = new ZHello.Algorithm.Sort.SelectSort();
            ISortTest(sort);
        }

        [TestMethod]
        public void HeapSort()
        {
            var sort = new ZHello.Algorithm.Sort.HeapSort();
            ISortTest(sort);
        }
    }
}