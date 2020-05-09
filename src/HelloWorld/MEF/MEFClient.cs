using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ZHello.MEF
{
    /// <summary>
    /// 入口部件类
    /// </summary>
    public class MyClass
    {
        //入口标识
        [ImportMany(typeof(ICal))]
        public IEnumerable<ICal> Cals { get; set; }
    }

    /// <summary>
    /// 入口部件类
    /// </summary>
    public class MyCal
    {
        [Import]
        public ICalShow Show { get; set; }
    }

    public class MCal2
    {
        [Import("MEFSimple.DefinC")]
        public dynamic MDefinC { get; set; }

        [ImportMany("Add", typeof(Func<int, int, int>))]
        public Func<int, int, int>[] Add { get; set; }

        [ImportMany("Add", typeof(Func<int, int, int>))]
        public Lazy<Func<int, int, int>, IDictionary<string, object>>[] AddEx { get; set; }

        [Import("RefeObj", typeof(Action<string, string>))]
        public Action<string, string> RefeObj { get; set; }

        [ImportMany("Subtract", typeof(Func<double, double, double>))]
        public Lazy<Func<double, double, double>, IMetaDataEx>[] Subtract { get; set; }
    }

    public class MImC : IPartImportsSatisfiedNotification
    {
        [Import("MEFAchieve.DefinC")]
        public dynamic MDefinC { get; set; }

        [Import("MEFAchieve.DefinC")]
        public Lazy<dynamic> MLDefinC { get; set; } //惰性加载部件

        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<Lazy<ICalShow, IMetaDataEx>> CalShow { get; set; }

        [ImportMany("Add", typeof(Func<int, int, int>))]
        public Func<int, int, int>[] Add { get; set; }

        [ImportMany("Add", typeof(Func<int, int, int>))]
        public Lazy<Func<int, int, int>, IDictionary<string, object>>[] AddEx { get; set; }

        [Import("RefeObj", typeof(Action<string, string>))]
        public Action<string, string> RefeObj { get; set; }

        [ImportMany("Subtract", typeof(Func<double, double, double>))]
        public Lazy<Func<double, double, double>, IMetaDataEx>[] Subtract { get; set; } //自定义导出特性

        /// <summary>
        /// 加载完成事件
        /// </summary>
        public void OnImportsSatisfied()
        {
        }
    }
}