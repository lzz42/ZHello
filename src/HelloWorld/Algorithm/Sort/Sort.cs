using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Algorithm.Sort
{
    #region 算法导论 基础知识

    /* 基础知识
     * 时间复杂度 空间复杂度：
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
     2.1 习题
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

    public class Sort
    {
        /// <summary>
        /// 变量交换
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void Swap(ref int a, ref int b)
        {
            if (a == b)
                return;
            //最常见的形式也是最广泛的形式 采用第三个中间变量缓存
            //var t = a;
            //a = b;
            //b = t;
            //交换两个变量的值 但有数值超出风险
            //a = a + b;
            //a = a - b;
            //a = a - b;
            //or 仅适用于数值类型
            a = a ^ b;
            b = a ^ b;
            a = a ^ b;
        }
    }

    public interface ISort
    {
        public abstract void Sort(int[] a);
    }
}
