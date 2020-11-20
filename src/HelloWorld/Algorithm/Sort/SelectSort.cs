namespace ZHello.Algorithm.Sort
{
    internal class SelectSort : Sort, ISort
    {
        /// <summary>
        /// 简单选择排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        public void Sort(int[] a)
        {
            var temp = a[0];
            var t = 0;
            for (int i = 0; i < a.Length; i++)
            {
                t = i;
                for (int j = i + 1; j < a.Length-1; j++)
                {
                    if (a[j + 1] < a[j])
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