using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZHello.MQ;

namespace ZHello.Test.MQ
{
    [TestClass]
    public class ZeroMqTest
    {
        private static void Main(string[] args)
        {
#if C
            Console.WriteLine("Req Client");
            var t = new ZeroMqTest();
            t.RunReqClient();
            Console.ReadLine();
#else
            Console.WriteLine("Rep Server");
            var t1 = new ZeroMqTest();
            t1.RunRepServer();
            Console.ReadLine();
#endif
        }

        [TestMethod]
        public void RunReqClient()
        {
            //var c = new ZeroReqClient("tcp://127.0.0.1:18848");
            var c = MQFactory.MakeReqClient("tcp://127.0.0.1:18848");
            string recv = "";
            string error;
            int cc = 0;
            while (recv != "exit")
            {
                cc++;
                Thread.Sleep(100);
                string send = Environment.TickCount.ToString();
                if (cc <= 5)
                {
                    send += "_SS_";
                }
                c.SendAndRecv(send, out recv, out error);
                Console.WriteLine("请求：" + send);
                Console.WriteLine("收到：" + recv);
            }
            c.SendAndRecv("exit", out recv, out error);
            Console.WriteLine("RECV:" + recv);
            Console.WriteLine("ERROR:" + error);
            c.Dispose();
        }

        [TestMethod]
        public void RunRepServer()
        {
            var s = MQFactory.MakeRepServer("tcp://127.0.0.1:18848");
            int cc = 0;
            string exit = "";
            s.Respose = (r) =>
             {
                 cc++;
                 exit = r;
                 string re = "";
                 if (cc >= 10)
                 {
                     re = "exit";
                 }
                 else
                 {
                     int t = 0;
                     if (int.TryParse(r, out t))
                     {
                         if (t % 2 == 0)
                         {
                             re = string.Format("{0} is Even.", t);
                         }
                         else
                         {
                             re = string.Format("{0} is Odd.", t);
                         }
                     }
                     else
                     {
                         re = "Not a Number::" + r;
                     }
                 }
                 Console.WriteLine("次数：" + cc);
                 Console.WriteLine("收到：" + r);
                 Console.WriteLine("回复：" + re);
                 return re;
             };
            while (exit != "exit")
            {
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void RunSubClient()
        {
        }

        [TestMethod]
        public void RunPubServer()
        {
        }
    }
}