#pragma warning disable CS3003,CS3002

using System;
using System.Runtime.InteropServices;

namespace ZHello.Async
{
    /*
     获取系统信息
        CPU信息
        系统架构

         */

    public class SystemInfo
    {    /// <summary>
         /// 处理器信息
         /// </summary>
        public static class ProcessorInfo
        {
            //x86 url:https://msdn.microsoft.com/en-us/library/ms724381(v=VS.85).aspx
            //x64 url:https://msdn.microsoft.com/en-us/library/ms724340(v=vs.85).aspx
            /*
    void WINAPI GetSystemInfo(
    _Out_ LPSYSTEM_INFO lpSystemInfo
    );

    void WINAPI GetNativeSystemInfo(
    _Out_ LPSYSTEM_INFO lpSystemInfo
    );

    typedef struct _SYSTEM_INFO {
      union {
        DWORD  dwOemId;
        struct {
          WORD wProcessorArchitecture;
          WORD wReserved;
        };
      };
      DWORD     dwPageSize;
      LPVOID    lpMinimumApplicationAddress;
      LPVOID    lpMaximumApplicationAddress;
      DWORD_PTR dwActiveProcessorMask;
      DWORD     dwNumberOfProcessors;
      DWORD     dwProcessorType;
      DWORD     dwAllocationGranularity;
      WORD      wProcessorLevel;
      WORD      wProcessorRevision;
    } SYSTEM_INFO;

             */

            #region 获取硬件信息 CPU

            public static uint GetCPUProcessorNumber()
            {
                System_Info info;
                if (Environment.Is64BitProcess)
                {
                    GetNativeSystemInfo(out info);
                }
                else
                {
                    GetSystemInfo(out info);
                }
                return info.dwNumberOfProcessors;
            }

            /*https://msdn.microsoft.com/en-us/library/ms724381(v=VS.85).aspx
             */

            /// <summary>
            /// x86下调用
            /// </summary>
            /// <param name="info"></param>
            [DllImport("Kernel32.dll")]
            private static extern void GetSystemInfo(out System_Info info);

            /*https://msdn.microsoft.com/en-us/library/ms724340(v=vs.85).aspx
             */

            /// <summary>
            /// wow64下，64位应用程序调用
            /// </summary>
            [DllImport("kernel32.dll")]
            private static extern void GetNativeSystemInfo(out System_Info info);

            /*https://msdn.microsoft.com/en-us/library/ms683194(v=vs.85).aspx
             */

            /// <summary>
            /// 获取逻辑处理器和相关硬件的信息
            /// </summary>
            /// <param name="Buffer"></param>
            /// <param name="ReturnLength"></param>
            [DllImport("kernel32.dll")]
            private static extern void GetLogicalProcessorInformation(out IntPtr Buffer, ref IntPtr ReturnLength);

            /*https://msdn.microsoft.com/en-us/library/dd405488(v=vs.85).aspx
             */

            [DllImport("kernel32.dll")]
            private static extern void GetLogicalProcessorInformationEx(LOGICAL_PROCESSOR_RELATIONSHIP lpr, out IntPtr ptr, ref IntPtr ReturnLength);

            [DllImport("Kernel32.dll")]
            public static extern UInt32 GetLastError();

            /*https://msdn.microsoft.com/en-us/library/ms724958(d=printer,v=vs.85).aspx
             */

            [StructLayout(LayoutKind.Sequential)]
            public struct System_Info
            {
                /// <summary>
                /// 已过时为保持兼容性留下的结构体
                /// </summary>
                //[Obsolete("已过时为保持兼容性留下的结构体")]
                //public OemId oemId;

                public UnionOem oemId;

                /// <summary>
                /// 页保护和提交的页大小和颗粒度
                /// </summary>
                public UInt32 dwPageSize;

                /// <summary>
                /// 应用程序或者DLL最小地址指针
                /// </summary>
                public IntPtr lpMinimumApplicationAddress;

                /// <summary>
                /// 应用程序或者DLL最大地址指针
                /// </summary>
                public IntPtr lpMaximumApplicationAddress;

                /// <summary>
                /// 系统内处理器配置的标记
                /// </summary>
                public IntPtr dwActiveProcessorMask;

                /// <summary>
                /// 处理器数量
                /// </summary>
                public UInt32 dwNumberOfProcessors;

                /// <summary>
                /// 处理器类型
                /// </summary>
                [Obsolete("已过时，为兼容性保留，")]
                public UInt32 dwProcessorType;

                /// <summary>
                /// 虚拟内存分配的颗粒度
                /// </summary>
                public UInt32 dwAllocationGranularity;

                /// <summary>
                /// 架构相关的处理器等级
                /// </summary>
                public UInt16 wProcessorLevel;

                /// <summary>
                /// 处理器修订号
                /// </summary>
                public UInt16 wProcessorRevision;
            }

            /// <summary>
            /// 已过时为保持兼容性留下的结构体
            /// </summary>
            [StructLayout(LayoutKind.Explicit)]
            public struct OemId
            {
                [FieldOffset(0)]
                private UInt32 dwOemId;

                /// <summary>
                /// 安装系统处理器架构
                /// </summary>
                [FieldOffset(0)]
                private UInt16 wProcessArchitecture;

                /// <summary>
                /// 将来预留字段
                /// </summary>
                [FieldOffset(16)]
                private UInt16 wReserved;
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct UnionOem
            {
                [FieldOffset(0)]
                private UInt16 OemId;

                [FieldOffset(0)]
                private ProcessorArchitectureInfo ProcessorArchitecture;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct ProcessorArchitectureInfo
            {
                private UInt16 wProcessArchitecture;
                private UInt16 wReserved;
            }

            public enum ProcessArchitecture
            {
                /// <summary>
                /// x64 AMD or intel
                /// </summary>
                PROCESSOR_ARCHITECTURE_AMD64 = 9,

                /// <summary>
                /// ARM
                /// </summary>
                PROCESSOR_ARCHITECTURE_ARM = 5,

                /// <summary>
                /// ARM64
                /// </summary>
                PROCESSOR_ARCHITECTURE_ARM64 = 12,

                PROCESSOR_ARCHITECTURE_IA64 = 6,
                PROCESSOR_ARCHITECTURE_INTEL = 0,
                PROCESSOR_ARCHITECTURE_UNKNOWN = 0xffff,
            }

            public enum LOGICAL_PROCESSOR_RELATIONSHIP
            {
                RelationProcessorCore = 0,
                RelationNumaNode = 1,
                RelationCache = 2,
                RelationProcessorPackage = 3,
                RelationGroup = 4,
                RelationAll = 0xffff,
            }

            /*https://docs.microsoft.com/zh-cn/windows/desktop/api/winnt/ns-winnt-_system_logical_processor_information_ex
    typedef struct _SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX {
      LOGICAL_PROCESSOR_RELATIONSHIP Relationship;
      DWORD                          Size;
      union {
        PROCESSOR_RELATIONSHIP Processor;
        NUMA_NODE_RELATIONSHIP NumaNode;
        CACHE_RELATIONSHIP     Cache;
        GROUP_RELATIONSHIP     Group;
      } DUMMYUNIONNAME;
    } SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX, *PSYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX;
    */

            [StructLayout(LayoutKind.Sequential)]
            public struct SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX
            {
                private LOGICAL_PROCESSOR_RELATIONSHIP Relationship;
                private UInt16 Size;
                private DUMMYUNIONNAME dUMMYUNIONNAME;
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct DUMMYUNIONNAME
            {
                [FieldOffset(0)]
                private PROCESSOR_RELATIONSHIP pROCESSOR_RELATIONSHIP;

                [FieldOffset(0)]
                private NUMA_NODE_RELATIONSHIP nUMA_NODE_RELATIONSHIP;
            }

            /*https://docs.microsoft.com/zh-cn/windows/desktop/api/winnt/ns-winnt-_processor_relationship
             * typedef struct _PROCESSOR_RELATIONSHIP {
      BYTE           Flags;
      BYTE           EfficiencyClass;
      BYTE           Reserved[20];
      WORD           GroupCount;
      GROUP_AFFINITY GroupMask[ANYSIZE_ARRAY];
    } PROCESSOR_RELATIONSHIP, *PPROCESSOR_RELATIONSHIP;*/

            [StructLayout(LayoutKind.Sequential)]
            public struct PROCESSOR_RELATIONSHIP
            {
                private byte Flags;
                private byte EfficiencyClass;
                private byte Reserved_20;
                private Int32 GroupCount;
                private GROUP_AFFINITY[] GroupMask;
            }

            /*https://docs.microsoft.com/zh-cn/windows/desktop/api/winnt/ns-winnt-_numa_node_relationship
             * typedef struct _NUMA_NODE_RELATIONSHIP {
      DWORD          NodeNumber;
      BYTE           Reserved[20];
      GROUP_AFFINITY GroupMask;
    } NUMA_NODE_RELATIONSHIP, *PNUMA_NODE_RELATIONSHIP;*/

            [StructLayout(LayoutKind.Sequential)]
            public struct NUMA_NODE_RELATIONSHIP
            {
                private UInt16 NodeNumber;
                private byte Reserved_20;
                private GROUP_AFFINITY GroupMask;
            }

            /*https://docs.microsoft.com/zh-cn/windows/desktop/api/winnt/ns-winnt-_cache_relationship
             * typedef struct _CACHE_RELATIONSHIP {
      BYTE                 Level;
      BYTE                 Associativity;
      WORD                 LineSize;
      DWORD                CacheSize;
      PROCESSOR_CACHE_TYPE Type;
      BYTE                 Reserved[20];
      GROUP_AFFINITY       GroupMask;
    } CACHE_RELATIONSHIP, *PCACHE_RELATIONSHIP;*/

            public struct CACHE_RELATIONSHIP
            {
            }

            /*https://docs.microsoft.com/zh-cn/windows/desktop/api/winnt/ns-winnt-_group_relationship
             * typedef struct _GROUP_RELATIONSHIP {
      WORD                 MaximumGroupCount;
      WORD                 ActiveGroupCount;
      BYTE                 Reserved[20];
      PROCESSOR_GROUP_INFO GroupInfo[ANYSIZE_ARRAY];
    } GROUP_RELATIONSHIP, *PGROUP_RELATIONSHIP;*/

            public struct GROUP_RELATIONSHIP
            {
            }

            /*https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/content/miniport/ns-miniport-_group_affinity
             * typedef struct _GROUP_AFFINITY {
      KAFFINITY Mask;
      USHORT    Group;
      USHORT    Reserved[3];
    } GROUP_AFFINITY, *PGROUP_AFFINITY;*/

            public struct GROUP_AFFINITY
            {
                private IntPtr Mask;
                private UInt16 Group;
                private UInt16 Reserved_3;
            }

            #endregion 获取硬件信息 CPU
        }
    }
}