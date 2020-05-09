using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;
using static ZHello.DesignPattern.CircuitBreakerPattern;
using static ZHello.IPC.MQTest.ZeroMQTest;

namespace ZHello.IPC.MQTest
{
    public class ZeroMQTestMain
    {
        private static void MainTest(string[] args)
        {
            string arg = "";
            if (args.Length > 0)
            {
                arg = args[0];
            }
            Console.WriteLine("ZeroMQ VersionInfo:" + GetZeroMQVersion());
            Console.WriteLine("选择类型:server?client?publisher?subscriber?");
            string u = "tcp://127.0.0.1:8989";
            string us = "tcp://127.0.0.1:9000";
            bool b = true;
            bool t = true;
            do
            {
                string res;
                if (t)
                {
                    res = arg.ToLower();
                    t = false;
                    Console.WriteLine(res);
                }
                else
                {
                    res = Console.ReadLine().ToLower();
                }
                if (res.Contains(" "))
                {
                    var tt = res.Split(' ');
                    if (tt[0] == "c" && tt.Length > 1)
                    {
                        StartNewProcess(tt[1]);
                    }
                    continue;
                }
                switch (res)
                {
                    case "server":
                        DoServer(u);
                        break;

                    case "client":
                        DoClient(u);
                        break;

                    case "publisher":
                        DoPublisher(us);
                        break;

                    case "subscriber":
                        DoSubscriber(us);
                        break;

                    case "servercb":
                        DoServer_CircuitBreaker(u);
                        break;

                    case "clientcb":
                        DoClient_CircuitBreaker(u);
                        break;

                    case "v":
                        DoVentilator();
                        break;

                    case "t":
                        DoTaskWorker();
                        break;

                    case "s":
                        DoSink();
                        break;

                    case "exit":
                        b = false;
                        break;

                    case "pipe":
                        StartNewProcess("v");
                        StartNewProcess("t");
                        StartNewProcess("s");
                        break;

                    default:
                        break;
                }
            } while (b);
        }

        private static void DoPublisher(string u)
        {
            var socket = Publish_Subscribe.CreatePublisher(u);
            var ran = new Random();
            do
            {
                var rad = ran.Next(100, 500);
                Thread.Sleep(rad / 2);
                Publish_Subscribe.PublishMsg(socket, rad.ToString());
            } while (true);
        }

        private static void DoSubscriber(string u)
        {
            var socket = Publish_Subscribe.CreateSubscribe(u);
            string msg;
            var ran = new Random();
            do
            {
                var rad = ran.Next(100, 500);
                Thread.Sleep(rad / 2);
                Publish_Subscribe.UpdateMsg(socket, out msg);
            } while (true);
        }

        private static void DoServer(string u)
        {
            var socket = Request_Reply.CreateServer(u);
            Request_Reply.Listen(socket);
        }

        private static void DoClient(string u)
        {
            var socket = Request_Reply.CreateClient(u);
            Request_Reply.SendAndWaitReply(socket);
        }

        private static void DoServer_CircuitBreaker(string u)
        {
            var socket = Request_Reply.CreateServer(u);
            var breaker = new CircuitBreaker(() =>
            {
                do
                {
                    Request_Reply.ReceiveOnece(socket);
                } while (true);
            }, null);

            breaker.Process();
        }

        private static void DoClient_CircuitBreaker(string u)
        {
            var socket = Request_Reply.CreateClient(u);
            var breaker = new CircuitBreaker(() =>
            {
                do
                {
                    Request_Reply.SendMsg(socket, "ok");
                } while (true);
            }, null);
            breaker.Process();
        }

        private static string u1 = "tcp://127.0.0.1:10001";
        private static string u2 = "tcp://127.0.0.1:10002";

        private static void DoVentilator()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
            var v = new Parallel_Pipeline.Ventilator(u1, u2);
            do
            {
                Console.WriteLine("Input Number:");
                uint t = 1;
                uint.TryParse(Console.ReadLine(), out t);
                t = t > 100 ? 100 : t;
                v.ProduceTask(t);
            } while (true);
        }

        private static void DoTaskWorker()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
            var t = new Parallel_Pipeline.TaskWorker(u1, u2);
            t.DoWork();
        }

        private static void DoSink()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
            var s = new Parallel_Pipeline.Sinker(u1);
            s.WaitResult();
        }

        private static void StartNewProcess(string arg)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var name = AppDomain.CurrentDomain.FriendlyName;
            var p = new ProcessStartInfo();
            p.FileName = Path.Combine(path, name);
            p.Verb = "runas";
            p.Arguments = arg;
            p.CreateNoWindow = false;
            //p.RedirectStandardInput = true;
            p.UseShellExecute = false;
            p.WindowStyle = ProcessWindowStyle.Normal;
            p.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Process.Start(p);
            //s.StandardInput.Write("ssss");
        }
    }

    public class ZeroMQTest
    {
        /*
         * Hadoop Zookeeper
         * AMQP
         *
         请求回复模式
         lockstep
         客户端必须zmq_send()然后zmq_recv()
         服务端必须zmq_recv()然后zmq_send()
         服务器重启后客户端不会自动重新连接(一定几率)

        ZeroMQ 字符串结尾不含终止符
        ZContext对象在进程开始和终止时进行创建和释放
        退出清理
        三种对象context socket message

        */

        /// <summary>
        /// 获取当前ZeroMQ版本号
        /// </summary>
        /// <returns></returns>
        public static string GetZeroMQVersion()
        {
            int major, minor, patch;
            ZeroMQ.lib.zmq.version(out major, out minor, out patch);
            return string.Format("{0}.{1}.{2}", major, minor, patch);
        }

        /// <summary>
        /// 请求回复模式
        /// </summary>
        public class Request_Reply
        {
            private static ZError error;

            public static ZSocket CreateServer(string url)
            {
                var context = new ZContext();
                var zsocker = new ZSocket(context, ZSocketType.REP);
                zsocker.Bind(url);
                return zsocker;
            }

            /// <summary>
            /// 同一个服务端可以帮到多个IP
            /// </summary>
            /// <param name="url"></param>
            /// <returns></returns>
            public static ZSocket CreateServer(string[] url)
            {
                var context = new ZContext();
                var zsocker = new ZSocket(context, ZSocketType.REP);
                for (int i = 0; i < url.Length; i++)
                {
                    zsocker.Bind(url[i]);
                }
                return zsocker;
            }

            public static ZSocket CreateClient(string url)
            {
                var context = new ZContext();
                var zsocker = new ZSocket(context, ZSocketType.REQ);
                zsocker.Connect(url);
                return zsocker;
            }

            public static void ReceiveOnece(ZSocket socket)
            {
                ZError error = null;
                using (var frame = socket.ReceiveFrame(out error))
                {
                    if (error != null)
                    {
                        throw new Exception(error.Text);
                    }
                    Console.WriteLine("Server::Get-request::{0}", frame.ReadString());
                    Thread.Sleep(new Random().Next(100, 300));
                    var msg = new Random().Next(1000, 9999).ToString();
                    socket.SendFrame(new ZFrame("NONO::" + msg));
                }
            }

            public static void Listen(ZSocket socket)
            {
                Console.WriteLine("Start Listen Msg ");
                while (true)
                {
                    using (var frame = socket.ReceiveFrame())
                    {
                        Console.WriteLine("Re:{0}", frame.ReadString());
                        Thread.Sleep(300);
                        var msg = Environment.TickCount;
                        socket.SendFrame(new ZFrame("I'MServer:" + msg));
                    }
                }
            }

            public static void SendMsg(ZSocket socket, string msg)
            {
                ZError error = null;
                socket.SendFrame(new ZFrame("OOOO:" + msg), ZSocketFlags.DontWait, out error);
                using (var reply = socket.ReceiveFrame(ZSocketFlags.DontWait, out error))
                {
                    Console.WriteLine("Client::get reply:" + reply?.ReadString());
                    Thread.Sleep(new Random().Next(500, 900));
                }
            }

            public static void SendAndWaitReply(ZSocket socket)
            {
                Console.WriteLine("Start Send Msg ");
                while (true)
                {
                    socket.SendFrame(new ZFrame("ImClient:" + Environment.TickCount));
                    using (var reply = socket.ReceiveFrame())
                    {
                        Console.WriteLine("receive reply:" + reply.ReadString());
                        Thread.Sleep(500);
                    }
                }
            }
        }

        /// <summary>
        /// 发布者订阅者模式
        /// </summary>
        public class Publish_Subscribe
        {
            public static ZSocket CreatePublisher(string url)
            {
                var context = new ZContext();
                var socket = new ZSocket(context, ZSocketType.PUB);
                socket.Bind(url);
                return socket;
            }

            public static ZSocket CreateSubscribe(string url)
            {
                var context = new ZContext();
                var socket = new ZSocket(context, ZSocketType.SUB);
                socket.Connect(url);
                return socket;
            }

            public static void PublishMsg(ZSocket socket, string msg)
            {
                using (var sendframe = new ZFrame(msg))
                {
                    socket.Send(sendframe);
                    Console.WriteLine("Publish Msg:" + msg);
                }
            }

            public static void UpdateMsg(ZSocket socket, out string msg)
            {
                socket.Subscribe("0000");
                using (var frame = socket.ReceiveFrame())
                {
                    msg = frame.ReadString();
                    Console.WriteLine("Update Msg:" + msg);
                }
            }
        }

        public class Parallel_Pipeline
        {
            public class Ventilator : IDisposable
            {
                private ZContext context;
                private ZSocket sender;
                private ZSocket sink;
                private string CUrl;
                private string VUrl;
                public int TaskNum = 10;

                public Ventilator(string s, string c)
                {
                    CUrl = s;
                    VUrl = c;
                    Init();
                }

                public void Init()
                {
                    context = new ZContext();
                    sender = new ZSocket(context, ZSocketType.PUSH);
                    sink = new ZSocket(context, ZSocketType.PUSH);
                    sender.Bind(VUrl);
                    sink.Connect(CUrl);
                }

                public void ProduceTask(uint num = 10)
                {
                    num = num > 100 ? 100 : num;
                    num = num <= 0 ? 1 : num;
                    //发出任务分配信号
                    var sig = BitConverter.GetBytes(num);
                    sink.Send(sig, 0, sig.Length);
                    //发出任务信息
                    for (int i = 0; i < num; i++)
                    {
                        var action = BitConverter.GetBytes(i);
                        sender.Send(action, 0, action.Length);
                    }
                }

                public void Dispose()
                {
                    sender.Close();
                    sink.Close();
                    sender.Dispose();
                    sink.Dispose();
                    context.Shutdown();
                    context.Dispose();
                }
            }

            public class TaskWorker : IDisposable
            {
                private bool loop = true;
                private ZContext context;
                private ZSocket receiver;
                private ZSocket sink;
                public string SUrl;
                public string TUrl;

                public TaskWorker(string s, string t)
                {
                    SUrl = s;
                    TUrl = t;
                    Init();
                }

                public void Init()
                {
                    context = new ZContext();
                    receiver = new ZSocket(context, ZSocketType.PULL);
                    sink = new ZSocket(context, ZSocketType.PUSH);
                    receiver.Connect(TUrl);
                    sink.Connect(SUrl);
                }

                public void DoWork()
                {
                    while (loop)
                    {
                        var res = new byte[4];
                        //获取任务数字
                        receiver.ReceiveBytes(res, 0, res.Length);
                        Task.Run(() =>
                        {
                            var num = BitConverter.ToInt32(res, 0);
                            Console.WriteLine("Get Task::{0}", num);
                            //TODO:do work
                            Thread.Sleep(new Random().Next(100, 800));
                            //通知sink该任务完成
                            sink.Send(res, 0, res.Length);
                        });
                    }
                }

                public void Dispose()
                {
                    loop = false;
                    receiver.Close();
                    sink.Close();
                    receiver.Dispose();
                    sink.Dispose();
                    context.Shutdown();
                    context.Dispose();
                }
            }

            public class Sinker : IDisposable
            {
                private ZContext context;
                private ZSocket sink;
                private bool loop = true;
                private string SUrl;

                public Sinker(string c)
                {
                    SUrl = c;
                    Init();
                }

                public void Init()
                {
                    context = new ZContext();
                    sink = new ZSocket(context, ZSocketType.PULL);
                    sink.Bind(SUrl);
                }

                public void WaitResult()
                {
                    while (loop)
                    {
                        var res = new byte[4];
                        sink.ReceiveBytes(res, 0, res.Length);
                        var r = BitConverter.ToInt32(res, 0);
                        if (r < 0)
                        {
                            Console.WriteLine("New Task:{0}", -r);
                        }
                        else
                        {
                            Console.WriteLine("Task:{0} Done", r);
                        }
                    }
                }

                public void Dispose()
                {
                    loop = false;
                    sink.Close();
                    sink.Dispose();
                    context.Shutdown();
                    context.Dispose();
                }
            }

            private Ventilator ventilator;
            private TaskWorker taskWorker;
            private Sinker sinker;
            private IList<TaskWorker> taskWorkers = new List<TaskWorker>();
            public string SinkerUrl = "tcp://127.0.0.1:10001";
            public string VentiltorUrl = "tcp://127.0.0.1:10002";

            public Parallel_Pipeline(string surl, string vurl)
            {
                if (!string.IsNullOrEmpty(surl))
                {
                    SinkerUrl = surl;
                }
                if (!string.IsNullOrEmpty(vurl))
                {
                    VentiltorUrl = vurl;
                }
            }

            public void Init()
            {
                ventilator = new Ventilator(SinkerUrl, VentiltorUrl);
                taskWorker = new TaskWorker(SinkerUrl, VentiltorUrl);
                sinker = new Sinker(SinkerUrl);
            }
        }
    }
}