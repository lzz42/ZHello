using System;
using System.Diagnostics;
using System.Threading;
using ZeroMQ;

namespace ZHello.MQ.ZeroMQ
{
    /// <summary>
    ///
    /// </summary>
    internal class ZeroReqClient : IReqClient
    {
        private ZSocket Socket { get; set; }

        public ZeroReqClient(string addr)
        {
            ZSocket socket;
            if (ZeroHelper.CreateReqClient(addr, out socket))
            {
                Socket = socket;
            }
        }

        public void Dispose()
        {
            Socket.Dispose();
        }

        public bool SendAndRecv(string send, out string recv, out string error)
        {
            if (send == null)
            {
                error = "不能发送NULL";
                recv = null;
                return false;
            }
            var b = false;
            error = null;
            recv = null;
            ZError zError;
            if (Socket.SendOnce(send, out zError))
            {
                do
                {
                    recv = Socket.RecvOnce(out zError);
                    Thread.Sleep(5);
                } while (recv == null);
                if (zError == null)
                {
                    b = true;
                }
            }
            if (zError != null)
            {
                error = zError.Text;
            }
            return b;
        }
    }

    /// <summary>
    ///
    /// </summary>
    internal class ZeroRepServer : IRepServer
    {
        private ZSocket Socket { get; set; }
        private Timer Timer { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Func<string, string> Respose { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="addr"></param>
        public ZeroRepServer(string addr)
        {
            ZSocket socket;
            if (ZeroHelper.CreateReqServer(addr, out socket))
            {
                Socket = socket;
                Timer = new Timer(TimerWrok);
                Timer.Change(20, 20);
            }
        }

        private void TimerWrok(object obj)
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            ZError error;
            var str = Socket.RecvOnce(out error);
            if (str != null)
            {
                string send = "NULL";
                if (Respose != null)
                {
                    send = Respose.Invoke(str);
                }
                if (send != null && !Socket.SendOnce(send, out error))
                {
                    Trace.WriteLine("Rep Server Send Error:" + error.Text);
                }
            }
            Timer.Change(20, 20);
        }

        public void Dispose()
        {
            Timer.Dispose();
            Socket.Dispose();
        }
    }
}