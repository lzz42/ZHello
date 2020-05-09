using ZHello.Common;
using ZHello.MQ.ZeroMQ;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ZHello.MQ
{
    /// <summary>
    ///
    /// </summary>
    public class MQFactory
    {
        private const string ZeroMQFullName = "ZeroMQ, Version=4.1.0.31, Culture=neutral, PublicKeyToken=4a9630883fd6c563";
        private const string SubDirGUID = "F9C0522C-26A2-49DA-8B5C-5C9D60BF8E6444";
        private const string ZeroMQName = "zeromq.dll";
        private const string Libzmq = "libzmq.dll";
        private static readonly string ZeroMQKey = "Zero.ZeroMQ.dll";
        private static readonly string x64libzmqKey = "Zero.amd64.libzmq.dll";
        private static readonly string x86libzmqKey = "Zero.i386.libzmq.dll";
        private static readonly string SubDir;
        private static readonly string DllDir;
        private static readonly string ResoucePrefix;

        static MQFactory()
        {
            SubDir = string.Format("{0}_{1}", typeof(MQFactory).Assembly.GetName().Name, SubDirGUID);
            ResoucePrefix = string.Format("{0}.{1}.", typeof(MQFactory).Assembly.GetName().Name, "Dlls");
            ZeroMQKey = ResoucePrefix + ZeroMQKey;
            x64libzmqKey = ResoucePrefix + x64libzmqKey;
            x86libzmqKey = ResoucePrefix + x86libzmqKey;
            if (Environment.Is64BitProcess)
            {
                DllDir = Path.Combine(Path.GetTempPath(), SubDir, "x64");
                ResouceHelper.RealseResouceToTempPathFlie(ZeroMQKey, DllDir, ZeroMQName);
                ResouceHelper.RealseResouceToTempPathFlie(x64libzmqKey, DllDir, Libzmq);
            }
            else
            {
                DllDir = Path.Combine(Path.GetTempPath(), SubDir, "x86");
                ResouceHelper.RealseResouceToTempPathFlie(ZeroMQKey, DllDir, ZeroMQName);
                ResouceHelper.RealseResouceToTempPathFlie(x86libzmqKey, DllDir, Libzmq);
            }
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            ZeroHelper.InitContext();
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                if (args.Name == ZeroMQFullName)
                {
                    return Assembly.LoadFile(Path.Combine(DllDir, ZeroMQName));
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine(ex.Source);
            }
            return null;
        }

        public static ISubClient MakeSubClient(string addr)
        {
            return new ZeroSubClient(addr);
        }
        public static IPubServer MakePubServer(string addr)
        {
            return new ZeroPubServer(addr);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public static IReqClient MakeReqClient(string addr)
        {
            return new ZeroReqClient(addr);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public static IRepServer MakeRepServer(string addr)
        {
            return new ZeroRepServer(addr);
        }
    }
}