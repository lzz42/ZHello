namespace ZHello.Algorithm
{
    #region 算法导论 基础知识

    /* 基础知识
     * 时间复杂度：
     * 空间复杂度：
     *
     *插入排序
     * 特点：
     * 1.原地排序 sorted in place
     * 循环不变式 loop invariant
     * 初始化：循环的第一轮迭代开始前，正确
     * 保持：循环的某一次迭代开始前，正确，下一次迭代开始前也正确
     * 终止：循环终止时，循环不变式提供有用的性质
     *
     分析算法
     RAM模型
     1.常见指令：算术指令（加、减、乘、除、取余、向上取整、向下取整），数据移动指令（装入、存储、复制），控制指令（条件与无条件转移、子程序调用与返回）
     2.每条指令的运行时间为常量
     3.没有并发执行的指令
     4.
     *
     */

    /*2.1 习题
     * 2.1-2：
     * begin:
     * for i=2 to A.length
     *     key = A[i]
     *     j = i - 1
     *     while A[j]<key
     *         A[j+1]=A[j]
     *         j = j - 1
     *         if j<1
     *             break
     *     A[j+1] = key
     * end
     *
     * 2.1-3:
     * begin:
     * for i=1 to A.length
     *     if v = A[i]
     *         return i
     * return NIL
     * end
     * 循环不变式：
     * 初始化：
     * 保持：
     * 终止：
     *
     * 2.1-4：
     * 形式化描述：
     * 输入：A，B都为长度为n的存储0/1的数组，
     * 输出：长度为（n+1）数组C，数组C为数组A+数组B对应项以及可能存在的前一项进位
     * begin：
     * k = 0
     * for i = 1 to n
     *     C[i] = A[i] + B[i] + k
     *     if C[i] > 1
     *         k = 1
     *         C[i] = 0
     *     else
     *         k = 0
     * C[n+1] = k
     * end
     */

    #endregion 算法导论 基础知识

    /// <summary>
    /// 排序算法
    /// 选择排序 select sort
    /// 插入排序 insert sort
    /// 冒泡排序 bobble sort
    /// </summary>
    public class Sort_Algorithm
    {
        /// <summary>
        /// 变量交换算法
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void Swap(int a, int b)
        {
            //最常见的形式也是最广泛的形式 采用第三个中间变量缓存引用或者值
            var t = a;
            a = b;
            b = t;
            //or 不适用第三个变量 交换两个变量的值 但有数值超出风险
            a = a + b;
            a = a - b;
            a = a - b;
            //or 仅适用于数值类型
            a = a ^ b;
            b = a ^ b;
            a = a ^ b;
        }

        /// <summary>
        /// 冒泡排序算法
        /// </summary>
        /// <param name="collection"></param>
        public static void Bubble_Sort(int[] collection)
        {
            System.Diagnostics.Contracts.Contract.Requires(collection != null && collection.Length > 0);
            //遍历所有待排序项
            for (int i = 0; i < collection.Length; i++)
            {
                //若每一个都比前一个小 则结束排序循环
                var k = false;
                //比较前【0-N-i】项依次比较相邻的两项 将最大值放入结尾
                for (int j = 0; j < collection.Length - 1 - i; j++)
                {
                    if (collection[j] > collection[j + 1])
                    {
                        k = true;
                        collection[j] = collection[j] ^ collection[j + 1];
                        collection[j + 1] = collection[j] ^ collection[j + 1];
                        collection[j] = collection[j] ^ collection[j + 1];
                    }
                }
                //若没有调整排序的 则说明该序列已完全排序好 直接退出即可
                if (!k)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 插入排序算法
        /// </summary>
        /// <param name="collection"></param>
        public static void Insertion_Sort(int[] collection)
        {
            System.Diagnostics.Contracts.Contract.Requires(collection != null && collection.Length > 0);
            //从第二项开始查找 因为第一项已经排好序
            for (int i = 1; i < collection.Length; i++)
            {
                //拿出要插入的项
                var key = collection[i];
                var j = i - 1;
                //依次倒序比较前面排好序的项 找到待插入项位置
                while (collection[j] > key)
                {
                    //若前一项大于 待插入项 则该项前移
                    collection[j + 1] = collection[j];
                    j--;
                    //若到达第一项则 停止比较
                    if (j < 0)
                    {
                        break;
                    }
                }
                //将待插入项放入 找到的位置
                collection[j + 1] = key;
            }
        }

        /// <summary>
        /// 直接插入排序
        /// </summary>
        /// <param name="source">待排序队列</param>
        public static void DirectInsertSort(int[] source)
        {
            System.Diagnostics.Contracts.Contract.Requires(source != null && source.Length > 0);
            for (int i = 1; i < source.Length; i++)
            {
                if (source[i] < source[i - 1])
                {
                    var temp = source[i];
                    int j = 0;
                    for (j = i - 1; j >= 0 && temp < source[j]; --j)
                    {
                        source[j + 1] = source[j];
                    }
                    source[j + 1] = temp;
                }
            }
        }

        /// <summary>
        /// 简单选择排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        public static void SimpleSelectSort(int[] source)
        {
            System.Diagnostics.Contracts.Contract.Requires(source != null && source.Length > 0);
            var temp = source[0];
            var t = 0;
            for (int i = 0; i < source.Length; i++)
            {
                t = i;
                for (int j = i + 1; j < source.Length; j++)
                {
                    if (source[j + 1] < source[j])
                    {
                        t = j;
                    }
                }
                temp = source[i];
                source[i] = source[t];
                source[t] = temp;
            }
        }
    }
}