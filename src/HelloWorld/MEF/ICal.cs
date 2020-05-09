using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ZHello.MEF
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
        IList<IOperation> GetOperations();

        /// <summary>
        /// 进行运算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        int Opeartion(IOperation op, int[] paras);
    }

    /// <summary>
    /// 运算符定义
    /// </summary>
    public interface IOperation
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

    /// <summary>
    /// 显示定义
    /// </summary>
    public interface ICalShow
    {
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        string Show(int a);

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        string Show(int a, int b);
    }

    /// <summary>
    /// 元数据定义
    /// </summary>
    public interface IMetaDataEx
    {
        string V { get; }//只能定义只读属性 才能用作MetaData代替
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class MetaDataExAttribute : ExportAttribute
    {
        public string V { get; set; }

        public MetaDataExAttribute(string name, Type type)
            : base(name, type)
        {
        }
    }
}