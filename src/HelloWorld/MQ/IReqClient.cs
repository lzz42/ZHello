using System;

namespace ZHello.MQ
{
    /// <summary>
    /// 请求应答模型 客户端
    /// </summary>
    public interface IReqClient : ISocket
    {
        /// <summary>
        /// 发送请求并等待应答回复
        /// </summary>
        /// <param name="send"></param>
        /// <param name="recv"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        bool SendAndRecv(string send, out string recv, out string error);
    }

    /// <summary>
    /// 请求应答模型 服务端
    /// </summary>
    public interface IRepServer : ISocket
    {
        /// <summary>
        /// 处理收到的请求并进行回复
        /// </summary>
        Func<string, string> Respose { get; set; }
    }
}