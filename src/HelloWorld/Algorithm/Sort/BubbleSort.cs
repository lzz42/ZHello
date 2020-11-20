using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Algorithm.Sort
{
    public class BubbleSort:Sort,ISort
    {
        /// <summary>
        /// 冒泡排序算法时间复杂度n^2
        /// </summary>
        /// <param name="a"></param>
        public void Sort(int[] a)
        {
            //遍历所有待排序项
            for (int i = 0; i < a.Length; i++)
            {
                //若每一个都比前一个小 则结束排序循环
                var k = false;
                //比较前【0-N-i】项依次比较相邻的两项 将最大值放入结尾
                for (int j = 0; j < a.Length - 1 - i; j++)
                {
                    if (a[j] > a[j + 1])
                    {
                        k = true;
                        a[j] = a[j] ^ a[j + 1];
                        a[j + 1] = a[j] ^ a[j + 1];
                        a[j] = a[j] ^ a[j + 1];
                    }
                }
                //若没有调整排序的 则说明该序列已完全排序好 直接退出即可
                if (!k)
                {
                    break;
                }
            }
        }

    }
}
