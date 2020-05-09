using System;

namespace ZHello.MQ
{
    /// <summary>
    /// 发布服务段
    /// </summary>
    public interface IPubServer : ISocket
    {
        /// <summary>
        /// 发布数据
        /// </summary>
        /// <param name="str"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        bool Publish(string str, out string error);

        /// <summary>
        /// 绑定额外的发布地址
        /// </summary>
        /// <param name="addr"></param>
        void BindExtraAddress(string addr);

        /// <summary>
        /// 取消绑定的发布地址
        /// </summary>
        /// <param name="addr"></param>
        void UnBindPubAddress(string addr);
    }

    /// <summary>
    /// 订阅客户端
    /// </summary>
    public interface ISubClient : ISocket
    {
        /// <summary>
        /// 数据到达事件
        /// </summary>
        event EventHandler<DataArgs> OnData;

        /// <summary>
        /// 添加额外的数据订阅地址
        /// </summary>
        /// <param name="addr"></param>
        void AddExtraAddress(string addr);

        /// <summary>
        /// 移除已添加的数据订阅地址
        /// </summary>
        /// <param name="addr"></param>
        void RemoveSubAddress(string addr);

        /// <summary>
        /// 取消订阅特点的数据头部
        /// </summary>
        /// <param name="prefix"></param>
        void UnSubscribe(string prefix);

        /// <summary>
        /// 取消所有的订阅数据
        /// </summary>
        void UnSubscribeAll();

        /// <summary>
        /// 订阅指定数据头的数据
        /// </summary>
        /// <param name="prefix"></param>
        void Subscribe(string prefix);

        /// <summary>
        /// 订阅所有数据
        /// </summary>
        void SubcribeAll();
    }

    /// <summary>
    ///
    /// </summary>
    public class DataArgs : EventArgs
    {
        public string Data { get; private set; }

        public DataArgs(string data)
        {
            Data = data;
        }
    }
}