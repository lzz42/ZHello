using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IContract
{

    /*定义约定*/
    /// <summary>
    /// 公用约定
    /// </summary>

    /// <summary>
    /// 定义计算器接口
    /// </summary>
    public interface ICal
    {
        /// <summary>
        /// 获取运算符
        /// </summary>
        /// <returns></returns>
        IList<IOperator> GetOperations();

        /// <summary>
        /// 进行运算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        int Opeartion(IOperator op, int[] paras);
    }



}
