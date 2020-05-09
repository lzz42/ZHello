using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace WebSocketTest
{
    public class WebSocketSharpHelper : IWebSocket
    {
        private WebSocket socket;
        private Logger logger;
        public string Url { get; set; }

        public EventHandler<WSEventArgs> OnWSOpen { get ; set; }
        public EventHandler<WSCloseEventArgs> OnWSClose { get; set; }
        public EventHandler<WSErrorEventArgs> OnWSError { get; set; }
        public EventHandler<WSMsgEventArgs> OnWSMsg { get; set; }

        private void Init(string url)
        {
            Url = url;
            socket = new WebSocket(Url);
            socket.WaitTime = new TimeSpan(1, 1, 1);
            logger = socket.Log;
            socket.OnOpen += Socket_OnOpen;
            socket.OnClose += Socket_OnClose;
            socket.OnMessage += Socket_OnMessage;
            socket.OnError += Socket_OnError;
        }

        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            if (OnWSError != null)
            {
                OnWSError.Invoke(sender, new WSErrorEventArgs(e.Message, Url) { Exception = e.Exception, Tag = e });
            }
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            if (OnWSMsg != null)
            {
                OnWSMsg.Invoke(sender, new WSMsgEventArgs(e.Data, Url) { RawData = e.RawData, Tag = e });
            }
        }

        private void Socket_OnOpen(object sender, EventArgs e)
        {
            if (OnWSOpen != null)
            {
                OnWSOpen.Invoke(sender, new WSEventArgs(e.ToString(), Url) { Tag = e });
            }
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            if (OnWSClose != null)
            {
                OnWSClose.Invoke(sender, new WSCloseEventArgs(e.Reason, Url) { Code = e.Code, Reason = e.Reason, WasClean = e.WasClean, Tag = e });
            }
        }

        public WebSocketSharpHelper(string url)
        {
            Init(url);
        }

        public void Close()
        {
            if (socket != null)
            {
                socket.Close();
            }
        }

        public void Open()
        {
            if (socket != null)
            {
                socket.Connect();
            }
        }

        public void SendMsg(object msg)
        {
            if (socket != null && msg != null)
            {
                socket.Send((string)msg);
            }
        }
    }
}
