using System;
using System.IO;
using System.Runtime.InteropServices;
using ZeroMQ;
using ZeroMQ.lib;

namespace ZHello.MQ.ZeroMQ
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ZeroHelper
    {
        /// <summary>
        /// 获取当前ZeroMQ版本号
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            int major, minor, patch;
            zmq.version(out major, out minor, out patch);
            return string.Format("{0}.{1}.{2}", major, minor, patch);
        }

        private static ZContext Context { get; set; }

        public static void InitContext()
        {
            Context = new ZContext()
            {
                IPv6Enabled = true,
                MaxSockets = 1024,
                ThreadPoolSize = 4
            };
        }

        public static void DestroyContext()
        {
            Context.Dispose();
        }


        #region Recv and Send

        /// <summary>
        /// 接收一次数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string RecvOnce(this ZSocket socket, out ZError error)
        {
            if (socket == null)
            {
                throw new System.ArgumentNullException(typeof(ZSocket).Name);
            }
            error = null;
            using (var frame = socket.ReceiveFrame(ZSocketFlags.None, out error))
            {
                if (error != null)
                {
                    return null;
                }
                if (frame != null)
                {
                    var str = frame.ReadString();
                    return str;
                }
            }
            return null;
        }

        /// <summary>
        /// 发送一次数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="str"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool SendOnce(this ZSocket socket, string str, out ZError error)
        {
            if (socket == null)
            {
                throw new System.ArgumentNullException(typeof(ZSocket).Name);
            }
            error = null;
            using (var frame = new ZFrame(str, System.Text.Encoding.UTF8))
            {
                return socket.SendFrame(frame, out error);
            }
        }

        #endregion Recv and Send

        #region 请求应答模式

        public static bool CreateReqClient(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.REQ);
            socket.Connect(url);
            return true;
        }

        public static bool CreateReqServer(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.REP);
            socket.Bind(url);
            return true;
        }

        public static bool CreateReqServer(string[] urls, out ZSocket socket)
        {
            if (urls is null)
            {
                throw new System.ArgumentNullException(nameof(urls));
            }
            socket = new ZSocket(Context, ZSocketType.REP);
            for (int i = 0; i < urls.Length; i++)
            {
                socket.Bind(urls[i]);
            }
            return true;
        }

        #endregion 请求应答模式

        #region 发布订阅模式

        public static bool CreateSubClient(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.SUB);
            socket.Connect(url);
            return true;
        }

        public static bool CreateXSubClient(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.XSUB);
            socket.Connect(url);
            return true;
        }

        public static bool CreatePubServer(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.PUB);
            socket.Bind(url);
            return true;
        }

        public static bool CreateXPubServer(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.XPUB);
            socket.Bind(url);
            return true;
        }

        #endregion 发布订阅模式

        #region 推送拉取模式

        public static bool CreatePushClient(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.PUSH);
            socket.Connect(url);
            return true;
        }

        public static bool CreatePullServer(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.PULL);
            socket.Bind(url);
            return true;
        }

        #endregion 推送拉取模式

        #region Dealer router

        public static bool CreateDelayerClient(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.DEALER);
            socket.Connect(url);
            return true;
        }

        public static bool CreateRouterServer(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.ROUTER);
            socket.Bind(url);
            return true;
        }

        #endregion Dealer router

        #region Other

        public static bool CreateNoneSocket(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.None);
            return true;
        }

        public static bool CreatePairSocket(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.PAIR);
            return true;
        }

        public static bool CreateStreamSocket(string url, out ZSocket socket)
        {
            socket = new ZSocket(Context, ZSocketType.STREAM);
            return true;
        }

        #endregion Other
    }
}