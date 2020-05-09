using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ZHello.IPC
{   

    /// <summary>
    /// 进程间通信 inter-process communication
    /// </summary>
    public class IPC_C
    {

        public static void Main(string[] args)
        {
            var nps =  IPCC.Build(eIPC.Client, typeof(NamedPipe));
            var p = new ProcessStartInfo();
            p.WorkingDirectory = "D:/";
            p.FileName = "1.ex3 e";
            p.Arguments = "ServerID";

        }
        public interface IPC: IDisposable
        {
            void Init(eIPC eIPC);
            void SendMsg(byte[] msg);
            string ReceiveMsg();
        }

        public enum eIPC
        {
            Client,
            Server,
        }

        public class IPCC
        {
            public readonly static string AssmGuid = "";
            static IPCC()
            {
                var tt = Assembly.GetExecutingAssembly();
                var g = tt.GetCustomAttribute(typeof(GuidAttribute));
                AssmGuid = ((GuidAttribute)g).Value;
            }

            public static IPC Build(eIPC etype,Type type)
            {
                if (type == null)
                    return null;
                if (type.GetInterface(typeof(IPC).Name) == null)
                    return null;
                IPC obj = (IPC)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, false, BindingFlags.CreateInstance, null,
                    null, null, null);
                obj.Init(etype);
                return obj;
            }
        }

        /*IPC
         * 进程间通信分为LPC RPC
         * LPC本地进程通信：管道，共享内存，消息队列，信号量，信号，剪切板，邮槽
         * RPC远程进程通信：TCP/UDP，stream
         * 
         * 
         */

        /*管道 pipe
         分两种：
         匿名管道：
         命名管道:

        C#:System.IO.Pipes
        本质对Windows API的封装
        管道 
        命名管道 ：双工通信，可以跨网络
        匿名管道 ：半双工通信，即只有一端可以写另一端可以读，只能在同一机器上使用，不能跨网络
            
            

             */
        #region MQ-message queue 
        public class ZeroMq
        {

        }

        public class RabbitMq
        {

        }

        public class RocketMq
        {

        }
       
        #endregion
        #region Pipe

        public class AnonymousPipe:IPC
        {
#pragma warning disable CS3026 // 符合 CLS 的字段不能是可变字段
            public volatile AnonymousPipeClientStream MAnClientPipe;
            public volatile AnonymousPipeServerStream MAnServerPipe;
#pragma warning restore CS3026 // 符合 CLS 的字段不能是可变字段
            private PipeStream apipe = null;
            public void Init(eIPC eIPC)
            {
            }

            public void Dispose()
            {
                apipe?.Dispose();
            }

            public string ReceiveMsg()
            {
                throw new NotImplementedException();
            }

            public void SendMsg(string msg)
            {
                throw new NotImplementedException();
            }

            public void SendMsg(byte[] msg)
            {
                throw new NotImplementedException();
            }
        }

        public class NamedPipe:IPC
        {
            private readonly static string MServerName = "";
            private readonly static string MClientName = "";
            private PipeStream npine = null;
            private string  _mcname = "";
            private string _msname = "";
            private Mutex _mutexc;
            private Mutex _mutexs;
            private bool _ready = false;
            static NamedPipe()
            {
                MServerName = IPCC.AssmGuid + "~sn";
                MClientName = IPCC.AssmGuid + "~cn";
            }

            public NamedPipe()
            {
                _mcname = IPCC.AssmGuid + "~mcn";
                _msname = IPCC.AssmGuid + "~msn";
            }

            public void Init(eIPC eIPC)
            {
                switch (eIPC)
                {
                    case eIPC.Client:
                        {
                            bool _cn = false;
                            _mutexc = new Mutex(true, _mcname, out _cn);
                            if (_cn)
                            {
                                npine = new NamedPipeClientStream(MServerName,
                                    MClientName, PipeDirection.InOut,
                                    PipeOptions.Asynchronous,
                                    TokenImpersonationLevel.None);
                                _ready = true;
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        break;
                    case eIPC.Server:
                        {
                            bool cn = false;
                            _mutexs = new Mutex(true, _msname, out cn);
                            if (cn)
                            {
                                npine = new NamedPipeServerStream(MServerName,PipeDirection.InOut, 1, PipeTransmissionMode.Message,PipeOptions.Asynchronous);
                                _ready = true;
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        break;
                    default:
                        break;
                }

            }

            public void Dispose()
            {
                _ready = false;
                npine?.Dispose();
                _mutexc?.Close();
                _mutexs?.Close();
            }

            public void SendMsg(byte[] msg)
            {
                try
                {
                    if (_ready&&!npine.IsConnected)
                    {
                        ((NamedPipeClientStream)npine).Connect(500);
                    }
                    var p = new StreamWriter(npine);
                    p.WriteLine(Encoding.UTF8.GetString(msg));
                    p.Flush();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            public string ReceiveMsg()
            {
                try
                {
                    if (_ready&&!npine.IsConnected)
                    {
                        ((NamedPipeServerStream)npine).WaitForConnection();
                    }
                    return new StreamReader(npine).ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "error";
                }
            }


        }

        #endregion

        #region windows消息

        /// <summary>
        /// Windows消息
        /// </summary>
        public class Wm_CopyData: IPC
        {
            public void Init(eIPC eIPC)
            {
                throw new NotImplementedException();
            }
            [DllImport("User32.dll", EntryPoint = "SendMessage")]
            private static extern int SendMessage(
                     int hWnd,                    // handle to destination window
                     int Msg,                     // message
                     int wParam,                  // first message parameter
                     ref CopyDataStruct lParam    // second message parameter
                     );
            [DllImport("User32.dll", EntryPoint = "FindWindow")]
            private static extern int FindWindow(string lpClassName, string lpWindowName);

            [StructLayout(LayoutKind.Sequential)]
            public struct CopyDataStruct
            {
                public IntPtr dwData;
                public int cbData;
                public IntPtr lpData;
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public void SendMsg(string msg)
            {
                throw new NotImplementedException();
            }

            public void SendMsg(byte[] msg)
            {
                throw new NotImplementedException();
            }

            public string ReceiveMsg()
            {
                throw new NotImplementedException();
            }

        }

        #endregion

        #region 内存映射

        /// <summary>
        /// 内存映射
        /// </summary>
        public class MemoryMappedFile:IPC
        {
            public void Init(eIPC eIPC)
            {
                throw new NotImplementedException();
            }
            public void Dispose()
            {
                throw new NotImplementedException();
            }
            public void SendMsg(string msg)
            {

            }
            public void SendMsg(byte[] msg)
            {
                throw new NotImplementedException();
            }

            public string ReceiveMsg()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region 共享内存

#pragma warning disable CS0657 // 不是此声明的有效特性位置
        [assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
#pragma warning restore CS0657 // 不是此声明的有效特性位置
                              // This class includes several Win32 interop definitions.
        internal class Win32
        {
            public static readonly IntPtr InvalidHandleValue = new IntPtr(-1);
            public const UInt32 FILE_MAP_WRITE = 2;
            public const UInt32 PAGE_READWRITE = 0x04;

            [DllImport("Kernel32", CharSet = CharSet.Unicode)]
            public static extern IntPtr CreateFileMapping(IntPtr hFile,
                IntPtr pAttributes, UInt32 flProtect,
                UInt32 dwMaximumSizeHigh, UInt32 dwMaximumSizeLow, String pName);

            [DllImport("Kernel32", CharSet = CharSet.Unicode)]
            public static extern IntPtr OpenFileMapping(UInt32 dwDesiredAccess,
                Boolean bInheritHandle, String name);

            [DllImport("Kernel32", CharSet = CharSet.Unicode)]
            public static extern Boolean CloseHandle(IntPtr handle);

            [DllImport("Kernel32", CharSet = CharSet.Unicode)]
            public static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject,
                UInt32 dwDesiredAccess,
                UInt32 dwFileOffsetHigh, UInt32 dwFileOffsetLow,
                IntPtr dwNumberOfBytesToMap);

            [DllImport("Kernel32", CharSet = CharSet.Unicode)]
            public static extern Boolean UnmapViewOfFile(IntPtr address);

            [DllImport("Kernel32", CharSet = CharSet.Unicode)]
            public static extern Boolean DuplicateHandle(IntPtr hSourceProcessHandle,
                IntPtr hSourceHandle,
                IntPtr hTargetProcessHandle, ref IntPtr lpTargetHandle,
                UInt32 dwDesiredAccess, Boolean bInheritHandle, UInt32 dwOptions);
            public const UInt32 DUPLICATE_CLOSE_SOURCE = 0x00000001;
            public const UInt32 DUPLICATE_SAME_ACCESS = 0x00000002;

            [DllImport("Kernel32", CharSet = CharSet.Unicode)]
            public static extern IntPtr GetCurrentProcess();
        }

        // This class wraps memory that can be simultaneously 
        // shared by multiple AppDomains and Processes.
        [Serializable]
        public sealed class SharedMemory : ISerializable, IDisposable
        {
            // The handle and string that identify 
            // the Windows file-mapping object.
            private IntPtr m_hFileMap = IntPtr.Zero;
            private String m_name;

            // The address of the memory-mapped file-mapping object.
            private IntPtr m_address;

            public unsafe Byte* Address
            {
                get { return (Byte*)m_address; }
            }

            // The constructors.
            public SharedMemory(Int32 size) : this(size, null) { }

            public SharedMemory(Int32 size, String name)
            {
                m_hFileMap = Win32.CreateFileMapping(Win32.InvalidHandleValue,
                    IntPtr.Zero, Win32.PAGE_READWRITE,
                    0, unchecked((UInt32)size), name);
                if (m_hFileMap == IntPtr.Zero)
                    throw new Exception("Could not create memory-mapped file.");
                m_name = name;
                m_address = Win32.MapViewOfFile(m_hFileMap, Win32.FILE_MAP_WRITE,
                    0, 0, IntPtr.Zero);
            }

            // The cleanup methods.
            public void Dispose()
            {
                GC.SuppressFinalize(this);
                Dispose(true);
            }

            private void Dispose(Boolean disposing)
            {
                Win32.UnmapViewOfFile(m_address);
                Win32.CloseHandle(m_hFileMap);
                m_address = IntPtr.Zero;
                m_hFileMap = IntPtr.Zero;
            }

            ~SharedMemory()
            {
                Dispose(false);
            }

            // Private helper methods.
            private static Boolean AllFlagsSet(Int32 flags, Int32 flagsToTest)
            {
                return (flags & flagsToTest) == flagsToTest;
            }

            private static Boolean AnyFlagsSet(Int32 flags, Int32 flagsToTest)
            {
                return (flags & flagsToTest) != 0;
            }


            // The security attribute demands that code that calls  
            // this method have permission to perform serialization.
            [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                // The context's State member indicates
                // where the object will be deserialized.

                // A SharedMemory object cannot be serialized 
                // to any of the following destinations.
                const StreamingContextStates InvalidDestinations =
                          StreamingContextStates.CrossMachine |
                          StreamingContextStates.File |
                          StreamingContextStates.Other |
                          StreamingContextStates.Persistence |
                          StreamingContextStates.Remoting;
                if (AnyFlagsSet((Int32)context.State, (Int32)InvalidDestinations))
                    throw new SerializationException("The SharedMemory object " +
                        "cannot be serialized to any of the following streaming contexts: " +
                        InvalidDestinations);

                const StreamingContextStates DeserializableByHandle =
                          StreamingContextStates.Clone |
                          // The same process.
                          StreamingContextStates.CrossAppDomain;
                if (AnyFlagsSet((Int32)context.State, (Int32)DeserializableByHandle))
                    info.AddValue("hFileMap", m_hFileMap);

                const StreamingContextStates DeserializableByName =
                          // The same computer.
                          StreamingContextStates.CrossProcess;
                if (AnyFlagsSet((Int32)context.State, (Int32)DeserializableByName))
                {
                    if (m_name == null)
                        throw new SerializationException("The SharedMemory object " +
                            "cannot be serialized CrossProcess because it was not constructed " +
                            "with a String name.");
                    info.AddValue("name", m_name);
                }
            }


            // The security attribute demands that code that calls  
            // this method have permission to perform serialization.
            [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
            private SharedMemory(SerializationInfo info, StreamingContext context)
            {
                // The context's State member indicates 
                // where the object was serialized from.

                const StreamingContextStates InvalidSources =
                          StreamingContextStates.CrossMachine |
                          StreamingContextStates.File |
                          StreamingContextStates.Other |
                          StreamingContextStates.Persistence |
                          StreamingContextStates.Remoting;
                if (AnyFlagsSet((Int32)context.State, (Int32)InvalidSources))
                    throw new SerializationException("The SharedMemory object " +
                        "cannot be deserialized from any of the following stream contexts: " +
                        InvalidSources);

                const StreamingContextStates SerializedByHandle =
                          StreamingContextStates.Clone |
                          // The same process.
                          StreamingContextStates.CrossAppDomain;
                if (AnyFlagsSet((Int32)context.State, (Int32)SerializedByHandle))
                {
                    try
                    {
                        Win32.DuplicateHandle(Win32.GetCurrentProcess(),
                            (IntPtr)info.GetValue("hFileMap", typeof(IntPtr)),
                            Win32.GetCurrentProcess(), ref m_hFileMap, 0, false,
                            Win32.DUPLICATE_SAME_ACCESS);
                    }
                    catch (SerializationException)
                    {
                        throw new SerializationException("A SharedMemory was not serialized " +
                            "using any of the following streaming contexts: " +
                            SerializedByHandle);
                    }
                }

                const StreamingContextStates SerializedByName =
                          // The same computer.
                          StreamingContextStates.CrossProcess;
                if (AnyFlagsSet((Int32)context.State, (Int32)SerializedByName))
                {
                    try
                    {
                        m_name = info.GetString("name");
                    }
                    catch (SerializationException)
                    {
                        throw new SerializationException("A SharedMemory object was not " +
                            "serialized using any of the following streaming contexts: " +
                            SerializedByName);
                    }
                    m_hFileMap = Win32.OpenFileMapping(Win32.FILE_MAP_WRITE, false, m_name);
                }
                if (m_hFileMap != IntPtr.Zero)
                {
                    m_address = Win32.MapViewOfFile(m_hFileMap, Win32.FILE_MAP_WRITE,
                        0, 0, IntPtr.Zero);
                }
                else
                {
                    throw new SerializationException("A SharedMemory object could not " +
                        "be deserialized.");
                }
            }
        }

        #endregion

        #region Remoting

        #endregion
    }
}
