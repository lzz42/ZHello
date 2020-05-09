using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Crawler
{
    /// <summary>
    /// 处理进度事件
    /// </summary>
    public class ProcessEventArgs : EventArgs
    {
        /// <summary>
        /// 处理状态
        /// </summary>
        public ProcessStatus Status { get; }

        /// <summary>
        /// 其他信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 附件对象
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 0-1处理进度
        /// </summary>
        public double ProgressRate { get; }

        public ProcessEventArgs(ProcessStatus status, double rate, string msg, object tag)
        {
            Status = status;
            ProgressRate = rate;
            Msg = msg;
            Tag = tag;
        }
        public ProcessEventArgs(ProcessStatus status, double rate)
            :this(status,rate,null,null)
        {

        }

    }

    /// <summary>
    /// 处理状态
    /// </summary>
    public enum ProcessStatus
    {
        /// <summary>
        /// 未开始
        /// </summary>
        UnStart,

        /// <summary>
        /// 处理中
        /// </summary>
        Processing,

        /// <summary>
        /// 已停止
        /// </summary>
        Stoped,

        /// <summary>
        /// 已完成
        /// </summary>
        Done,
    }
}
