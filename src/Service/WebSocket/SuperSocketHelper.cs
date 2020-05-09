using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using SuperSocket.SocketBase;

namespace WebSocketTest
{
    /// <summary>
    /// 
    /// http://docs.supersocket.net/v1-6/zh-CN/A-Telnet-Example  
    /// </summary>
    public class SuperSocketHelper : IWebSocket
    {
        private AppServer mServer;

        public string Url { get; set; }

        public EventHandler<WSEventArgs> OnWSOpen { get; set; }
        public EventHandler<WSCloseEventArgs> OnWSClose { get; set; }
        public EventHandler<WSErrorEventArgs> OnWSError { get; set; }
        public EventHandler<WSMsgEventArgs> OnWSMsg { get; set; }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public void SendMsg(object msg)
        {
            throw new NotImplementedException();
        }

        public void SartServer(int port)
        {
            mServer = new AppServer();
            if (mServer.Setup(port))
            {
                mServer.Start();
            }
        }

        public void StopServer()
        {
            if (mServer != null)
            {
                mServer.Stop();
            }
        }
    }
}
