using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IContract
{
    /// <summary>
    /// 运算符定义
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// 运算符名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 操作项
        /// </summary>
        int NumberOperands { get; }
    }
}
