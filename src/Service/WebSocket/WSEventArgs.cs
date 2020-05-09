using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketTest
{
    public class WSEventArgs : EventArgs
    {
        public object Data { get; set; }
        public string WSUrl { get; set; }
        public object Tag { get; set; }
        public WSEventArgs(object data, string wsUrl)
        {
            Data = data;
            WSUrl = wsUrl;
        }
    }

    /// <summary>
    /// websocket错误消息事件参数
    /// </summary>
    public class WSErrorEventArgs : WSEventArgs
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public WSErrorEventArgs(object data, string wsUrl) : base(data, wsUrl)
        {

        }
    }

    /// <summary>
    /// websocket普通消息事件参数
    /// </summary>
    public class WSMsgEventArgs : WSEventArgs
    {
        public bool IsBinary { get; set; }
        public bool IsPing { get; set; }
        public bool IsText { get; set; }
        public byte[] RawData { get; set; }

        public WSMsgEventArgs(object data, string wsUrl) : base(data, wsUrl)
        {

        }
    }

    /// <summary>
    /// websocket关闭消息事件参数
    /// </summary>
    public class WSCloseEventArgs : WSEventArgs
    {
        public ushort Code { get; set; }
        public string Reason { get; set; }
        public bool WasClean { get; set; }

        public WSCloseEventArgs(object data, string wsUrl) : base(data, wsUrl)
        {

        }
    }
}
