using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Algorithm.Sort
{
    /*
    基数排序
        一种用在卡片排序机上的排序算法
    桶排序
        假设输入数据服从均匀分布，分布区间为[0,1) 
     */

    /// <summary>
    /// 计数排序 非比较的排序算法
    /// 时间复杂度O(n)
    /// 要求：输入数据为0~k区间内的整数
    /// 排序依据：根据待排序数字的实际值确定其在数组中的位置
    /// 算法导论 8.2 计数排序
    /// </summary>
    public class CountingSort : Sort, ISort
    {

        public CountingSort()
        {

        }

        /// <summary>
        /// 计算最大数字
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private int GetMax(int[] a)
        {
            if (a == null || a.Length == 0)
                return 0;
            int max = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] > max)
                    max = a[i];
            }
            return max;
        }

        /// <summary>
        /// 计数排序
        /// </summary>
        /// <param name="a"></param>
        public void Sort(int[] a)
        {
            var max = GetMax(a);
            if (max == 0)
                return;
            //构建基础0~k的数组
            var c = new int[max + 1];
            //统计待排序数组中每个数字的数量，即第i个数字有多少个c[i]
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] < 0)
                    throw new NotSupportedException("Not Support Nagatiave Number");
                c[a[i]]++;
            }
            //计算第i个元素的计数总和位置
            for (int i = 1; i < c.Length; i++)
            {
                c[i] =c[i] + c[i - 1];
            }
            //根据计数位置 重新填充备用数组
            //C中的第i项的值c[i]为元素i在数据B中的最大位置
            //遍历数组A,所有元素找到对应元素在C中的值即为在B中位置
            var b = new int[a.Length];
            //for (int i = a.Length - 1; i >= 0; i--)
            //{
            //    b[c[a[i]] - 1] = a[i];
            //    c[a[i]]--;
            //}
            for (int i = 0; i < a.Length; i++)
            {
                b[c[a[i]] - 1] = a[i];
                //此处必须减一操作，因为对于相同的值是递增的，即第一次访问的位置未相同元素的最大索引值
                c[a[i]]--;
            }
            //复制备用数组到输入数组
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = b[i];
            }
        }
    }
}
