using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

/*  进程和线程
 * 进程---执行中的代码，活动实体
 * 进程包括：
 *  文本段：代码段
 *  当前活动：程序计数器的值，处理器寄存器的内容
 *  进程堆栈段：临时数据
 *  数据段：全局变量
 *  堆：进程运行期间动态分配的内存
 *  地址与数据分布：
 *  0————————————>>>————————————Max
 *  文本--->数据--->堆---><---栈
 * 进程状态
 *      新的：进程正在被创建
 *      运行：指令正在被执行
 *      等待：等待某个事件发生
 *      就绪：进程等待分配处理器
 *      终止：进程完成执行
 * 一次只有一个进程可以在一个处理器上运行
 * 进程控制块 PCB process control block
 *  进程状态：
 *  程序计数器：进程要执行的下个指令的地址，中断时需要保存
 *  CPU寄存器：中断时需要保存
 *  CPU调度信息：进程优先级，调度队列指针，其他调度参数
 *  内存管理信息：操作系统所使用的内存系统,基址，界限寄存器的值，页表，段表
 *  记账信息：CPU时间，实际使用时间，时间界限，记账数据，作业，进程数量
 *  I/O状态信息：IO设备表，打开的文件表
 *
 * 进程调用
 *
 * 调度队列
 *    作业队列：系统中所有进程
 *    就绪队列：
 *    设备队列：
 *  队列图
 *  调度程序 scheduler
 *
 *  进程创建——》缓冲池（磁盘）——》长期（作业）调度程序，从缓冲池选择进程——》加载到内存——》短期（CPU）调度程序——》选择进程执行
 * 多道程序设计程度：内存中的进程数量——》长期调度程序
 * 长期调度程序：
 *    进程分类：
 *    IO为主：执行IO比执行计算花费时间多
 *    CPU为主：更多时间执行计算
 *  中期调度程序：将进程从内存中移出
 *  上下文切换
 *      进程上下文使用进程的PCB表示：CPU寄存器值，进程状态，内存管理信息等；
 *      状态保存 state save，状态恢复 state restore
 *      上下文切换context switch：将CPU切换到另一个进程需要保存当前进程的状态（保存到PCB），并恢复另一个进程的状态；
 *  进程操作
 *      进程创建
 *      系统调用创建
 *          pid：进程唯一标识（整型）
 *      新进程所需资源：1.操作系统获取；2.父进程获取；
 *      初始化数据：由父进程提供；
 *      执行可能：
 *      1.父进程与子进程并发执行；
 *      2.父进程等待子进程执行完；
 *      地址空间：
 *      1.子进程是父进程的复制；
 *      2.子进程加载新的程序；
 *      进程终止
 *          级联终止
 *
 *
 *  进程间通信IPC
 *      两种基本模式：
 *      共享内存：速度快
 *      消息传递：较少量数据，需要系统调用实现因此耗时间多
 *      共享内存系统：
 *          1.建立内存共享区域；
 *          2.进程需要取消跨进程地址空间访问限制；
 *      消息传递
 *          命名：直接通信、间接通信
 *          同步：阻塞，非阻塞
 *          缓冲：零缓冲，有限缓冲，无限缓冲
 *  Client、Server
 *  Socket
 *      通信端点
 *      Server：监听指定端口等待客户机请求，收到请求，建立连接
 *      Client：通过指定的端口发送连接请求，
 *  RPC——远程过程调用
 *      允许客户机调用位于远程主机上的过程
 *      客户端提供存根（stub）：存根位于服务器的端口，并编组参数
 *      问题处理：
 *          1.处理数据表示差异：大尾端和小尾端问题——》外部数据表示XDR，即数据的机器无关表示；
 *          2.调用的语义问题：最多调用一次——》每个消息附加时间戳；刚好调用一次——》服务器发出ACK消息，客户端重复发生RPC直到收到ACK消息；
 *          3.通信问题，即确定客户端和服务端的端口绑定：
 *          a.固定端口地址；
 *          b.通过集合点机制（matchmaker）动态进行
 *              Client——mathchmaker获取本地端口——》|发送消息到S，C端口号|——》server收到消息——matchmaker找到端口号——》|发送消息到C端口号,回复S端口号|
 *              ——》C收到消息S端口——》|发送RPC消息到S端口号，C端口号|——》S收到RPC，调用——》|发送结果消息到C端口，S端口|——》C收到来自S的RPC结果
 *  RMI——远程方法调用
 *
 */

namespace ZHello.Async
{
    public class WinProcess
    {
        public void TestMethod()
        {
            PrintThreadInfo("main after");
            var s = GetString();
            s.Wait();
            PrintThreadInfo("main before");
            Console.WriteLine("res:{0}", s.Result);
            Console.ReadLine();
        }

        public static async Task<string> GetString()
        {
            return await new Task<string>(s =>
            {
                Thread.Sleep(600);
                return "ok";
            }, null);
        }

        public static void TestProcess()
        {
            Console.WriteLine(string.Format("Main :{0}", Environment.Is64BitProcess ? "x64" : "x86"));
            var procinfo = new ProcessStartInfo();
            procinfo.FileName = @"C:\Windows\System32\notepad.exe";
            //procinfo.FileName = @"Crawler.exe";
            //procinfo.FileName = @"C:\Program Files\Microsoft VS Code\Code.exe";
            procinfo.WorkingDirectory = @"";
            procinfo.Verb = "runas";

            var res = Process.Start(procinfo);
            bool r = false;
            if (IsWow64Process(res.Handle, out r))
            {
                Console.WriteLine(string.Format("Called:{0}", r ? "x86" : "x64"));
            }
        }

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "IsWow64Process")]
        public static extern bool IsWow64Process(IntPtr handle, out bool result);

        public static Dictionary<string, string> dic = new Dictionary<string, string>();

        public static void PrintThreadInfo(string msg = null)
        {
            Console.WriteLine("{0},Current Thread is：{1}", msg, Thread.CurrentThread.ManagedThreadId);
        }

        public static void Work()
        {
            Console.WriteLine("working begin");
            PrintThreadInfo("work");
            Thread.Sleep(300);
            Console.WriteLine("working done");
        }

        public static void Done()
        {
            PrintThreadInfo("has done");
            Console.WriteLine("has done");
        }
    }
}