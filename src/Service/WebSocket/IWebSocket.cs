using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketTest
{
    public interface IWebSocket
    {
        string Url { get; set; }
        void Open();
        void Close();
        void SendMsg(object msg);

        EventHandler<WSEventArgs> OnWSOpen { get; set; }

        EventHandler<WSCloseEventArgs> OnWSClose { get; set; }

        EventHandler<WSErrorEventArgs> OnWSError { get; set; }

        EventHandler<WSMsgEventArgs> OnWSMsg { get; set; }
    }
}
