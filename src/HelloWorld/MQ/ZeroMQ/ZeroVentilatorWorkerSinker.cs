using System;
using System.Diagnostics;
using System.Threading;
using ZeroMQ;

namespace ZHello.MQ.ZeroMQ
{
    public class ZeroVentilatorWorkerSinker
    {
        public void Ventilator()
        {
            //
            // Task ventilator
            // Binds PUSH socket to tcp://127.0.0.1:5557
            // Sends batch of tasks to workers via that socket
            // Socket to send messages on and
            // Socket to send start of batch message on
            using (var context = new ZContext())
            using (var sender = new ZSocket(context, ZSocketType.PUSH))
            using (var sink = new ZSocket(context, ZSocketType.PUSH))
            {
                sender.Bind("tcp://*:5557");
                sink.Connect("tcp://127.0.0.1:5558");

                Console.WriteLine("Press ENTER when the workers are ready…");
                Console.ReadKey(true);
                Console.WriteLine("Sending tasks to workers…");

                // The first message is "0" and signals start of batch
                sink.Send(new byte[] { 0x00 }, 0, 1);

                // Initialize random number generator
                var rnd = new Random();

                // Send 100 tasks
                int i = 0;
                long total_msec = 0;    // Total expected cost in msecs
                for (; i < 100; ++i)
                {
                    // Random workload from 1 to 100msecs
                    int workload = rnd.Next(100) + 1;
                    total_msec += workload;
                    byte[] action = BitConverter.GetBytes(workload);

                    Console.WriteLine("{0}", workload);
                    sender.Send(action, 0, action.Length);
                }

                Console.WriteLine("Total expected cost: {0} ms", total_msec);
            }
        }

        public void Worker()
        {
            //
            // Task worker
            // Connects PULL socket to tcp://127.0.0.1:5557
            // Collects workloads from ventilator via that socket
            // Connects PUSH socket to tcp://127.0.0.1:5558
            // Sends results to sink via that socket
            // Socket to receive messages on and
            // Socket to send messages to
            using (var context = new ZContext())
            using (var receiver = new ZSocket(context, ZSocketType.PULL))
            using (var sink = new ZSocket(context, ZSocketType.PUSH))
            {
                receiver.Connect("tcp://127.0.0.1:5557");
                sink.Connect("tcp://127.0.0.1:5558");

                // Process tasks forever
                while (true)
                {
                    var replyBytes = new byte[4];
                    receiver.ReceiveBytes(replyBytes, 0, replyBytes.Length);
                    int workload = BitConverter.ToInt32(replyBytes, 0);
                    Console.WriteLine("{0}.", workload);    // Show progress
                    Thread.Sleep(workload);    // Do the work
                    sink.Send(new byte[0], 0, 0);    // Send results to sink
                }
            }
        }

        public void Sinker()
        {
            //
            // Task sink
            // Binds PULL socket to tcp://127.0.0.1:5558
            // Collects results from workers via that socket
            // Prepare our context and socket
            using (var context = new ZContext())
            using (var sink = new ZSocket(context, ZSocketType.PULL))
            {
                sink.Bind("tcp://*:5558");
                // Wait for start of batch
                sink.ReceiveFrame();
                // Start our clock now
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                // Process 100 confirmations
                for (int i = 0; i < 100; ++i)
                {
                    sink.ReceiveFrame();

                    if ((i / 10) * 10 == i)
                        Console.Write(":");
                    else
                        Console.Write(".");
                }
                // Calculate and report duration of batch
                stopwatch.Stop();
                Console.WriteLine("Total elapsed time: {0} ms", stopwatch.ElapsedMilliseconds);
            }
        }
    }
}