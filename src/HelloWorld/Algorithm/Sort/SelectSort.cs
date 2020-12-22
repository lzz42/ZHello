namespace ZHello.Algorithm.Sort
{
    internal class SelectSort : Sort, ISort
    {
        /// <summary>
        /// 简单选择排序
        /// 时间复杂度O(n^2)
        /// 从第二项开始遍历：选择当前项后的所有项中的最大值或者最小值与当前项进行互换
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        public void Sort(int[] a)
        {
            int temp, t = 0;
            for (int i = 0; i < a.Length; i++)
            {
                t = i;
                for (int j = i + 1; j < a.Length; j++)
                {
                    if (a[j] < a[t])
                    {
                        t = j;
                    }
                }
                temp = a[i];
                a[i] = a[t];
                a[t] = temp;
            }
        }
    }
}