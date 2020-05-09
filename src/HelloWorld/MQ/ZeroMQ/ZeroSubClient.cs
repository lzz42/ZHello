using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ZeroMQ;

namespace ZHello.MQ.ZeroMQ
{
    internal class ZeroSubClient : ISubClient
    {
        public event EventHandler<DataArgs> OnData;

        private List<string> ConnectingAddrs { get; set; }
        private ZSocket Socket { get; set; }

        public ZeroSubClient(string addr)
        {
            ConnectingAddrs = new List<string>();
            ZSocket socket;
            if (ZeroHelper.CreateSubClient(addr, out socket))
            {
                ConnectingAddrs.Add(addr.Trim().ToLower());
                Socket = socket;
            }
            Timer = new Timer(TimerWrok, null, 20, 20);
        }

        public void Dispose()
        {
            Socket.Dispose();
            Timer.Dispose();
        }

        public void Subscribe(string prefix)
        {
            Socket.Subscribe(prefix);
        }

        public void SubcribeAll()
        {
            Socket.Subscribe("");
        }

        public void AddExtraAddress(string addr)
        {
            if (string.IsNullOrEmpty(addr))
                return;
            addr = addr.Trim().ToLower();
            if (ConnectingAddrs.Contains(addr))
            {
                return;
            }
            ZError error;
            if (!Socket.Connect(addr, out error))
            {
                Trace.WriteLine("添加额外的订阅地址失败：" + error.Text);
            }
            else
            {
                ConnectingAddrs.Add(addr);
            }
        }

        public void RemoveSubAddress(string addr)
        {
            if (string.IsNullOrEmpty(addr))
                return;
            addr = addr.Trim().ToLower();
            if (ConnectingAddrs.Contains(addr))
            {
                Socket.Disconnect(addr);
                ConnectingAddrs.Remove(addr);
            }
        }

        public void UnSubscribe(string prefix)
        {
            Socket.Unsubscribe(prefix);
        }

        public void UnSubscribeAll()
        {
            Socket.UnsubscribeAll();
        }

        #region Timer

        private Timer Timer { get; set; }

        private void TimerWrok(object obj)
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            ZError error;
            var str = Socket.RecvOnce(out error);
            if (str != null && OnData != null)
            {
                OnData.Invoke(null, new DataArgs(str));
            }
            Timer.Change(20, 20);
        }

        #endregion Timer
    }

    internal class ZeroPubServer : IPubServer
    {
        private ZSocket Socket { get; set; }

        private List<string> BindingAddrs { get; set; }

        public ZeroPubServer(string addr)
        {
            BindingAddrs = new List<string>();
            ZSocket socket;
            if (ZeroHelper.CreatePubServer(addr, out socket))
            {
                Socket = socket;
                BindingAddrs.Add(addr.Trim().ToLower());
                Socket.Send(new byte[0], 0, 0);
            }
        }

        public void Dispose()
        {
            Socket.Dispose();
        }

        public bool Publish(string str, out string error)
        {
            if (str == null)
            {
                error = "不能推送NULL";
                return false;
            }
            error = null;
            ZError zError;
            if (!Socket.SendOnce(str, out zError))
            {
                error = zError.Text;
                return false;
            }
            return true;
        }

        public void BindExtraAddress(string addr)
        {
            if (string.IsNullOrEmpty(addr))
                return;
            addr = addr.Trim().ToLower();
            if (BindingAddrs.Contains(addr))
            {
                return;
            }
            ZError error;
            if (Socket.Bind(addr, out error))
            {
                BindingAddrs.Add(addr);
                return;
            }
            Trace.WriteLine("绑定额外地址异常：" + error);
        }

        public void UnBindPubAddress(string addr)
        {
            if (string.IsNullOrEmpty(addr))
                return;
            addr = addr.Trim().ToLower();
            if (BindingAddrs.Contains(addr))
            {
                Socket.Unbind(addr);
                BindingAddrs.Remove(addr);
            }
        }
    }
}