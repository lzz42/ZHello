using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ZHello.Common
{
    //https://blog.csdn.net/laikaikai/article/details/80281283
    /* 进程注入
     一.DLL注入
        1.CreateToolhelp32Snapshot - 获取所有哦进程快照
        2.Process32First Process32Next - 遍历快照
        3.OpenProcess - 得到进程句柄
        4.GetProcAddress - 获取进程地址
        5.VirtualAllocEx - 得到写入路径的内存空间
        6.WriteProcessMemory - 在分配的内存中写入DLL
        7.CreateRemoteThread、NtCreateThreadEx、RtlCreateUserThread - 创建远程线程 执行代码
        8.DLL代码中需监听 DLL加载事件执行
    二.PE注入（可执行文件注入）
    三.PROCESS HOLLOWING
    四.线程劫持注入 THREAD EXECUTION HIJACKING
    五.通过SETWINDOWSHOOKEX进行注入
    六.通过修改注册的方法来注入
    七.APC注入和AtomBombing内存注入
    八.通过SETWINDOWLONG提供EXTRA WINDOW MEMORY INJECTION（EWMI）
    九.使用SHIMS进行注入
    十.Hook导入表（IAT hooking）或 INLINE HOOKING

     */

    public class InjectionHelper
    {
        /* 注入C#代码
         * 1.获取直接进程句柄
         * 2.判断该进程是否加载并运行CLR
         * 3.若没有运行CLR则加载并运行CLR
         * 4.运行CLR，创建应用程序域
         * 5.加载DLL，执行代码
             */

        public static bool Inject(int pid,MethodBase method)
        {
            var proc = Process.GetProcessById(pid);
            return false;
        }
    }

    #region WIN-API

    public static class WinAPI
    {
        //https://docs.microsoft.com/zh-cn/windows/desktop/api/processthreadsapi/nf-processthreadsapi-createremotethread
        //        HANDLE CreateRemoteThread(
        //  HANDLE hProcess,
        //  LPSECURITY_ATTRIBUTES lpThreadAttributes,
        //  SIZE_T dwStackSize,
        //  LPTHREAD_START_ROUTINE lpStartAddress,
        //  LPVOID lpParameter,
        //  DWORD dwCreationFlags,
        //  LPDWORD lpThreadId
        //);
        /// <summary>
        /// 在另一个指定进程的虚拟地址空间内容创建一个线程
        /// </summary>
        /// <param name="hProcess"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern int CreateRemoteThread(int hProcess, SECURITY_ATTRIBUTES lpThreadAttributes
            , uint dwStackSize, int lpStartAddress, IntPtr lpParameter, int dwCreationFlags, long lpThreadId);

        /// <summary>
        /// 在另一个指定进程的虚拟地址空间内容创建一个线程
        /// </summary>
        /// <param name="hProcess"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern int CreateRemoteThreadEx(int hProcess, SECURITY_ATTRIBUTES lpThreadAttributes
            , uint dwStackSize, int lpStartAddress, IntPtr lpParameter, int dwCreationFlags, long lpThreadId);



        /*
         typedef struct _SECURITY_ATTRIBUTES {
  DWORD  nLength;
  LPVOID lpSecurityDescriptor;
  BOOL   bInheritHandle;
} SECURITY_ATTRIBUTES, *PSECURITY_ATTRIBUTES, *LPSECURITY_ATTRIBUTES;*/

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        /*
         1.CreateToolhelp32Snapshot - 获取所有哦进程快照
         2.Process32First Process32Next - 遍历快照
         3.OpenProcess - 得到进程句柄
         4.GetProcAddress - 获取进程地址
         5.VirtualAllocEx - 得到写入路径的内存空间
         6.WriteProcessMemory - 在分配的内存中写入DLL
         7.CreateRemoteThread、NtCreateThreadEx、RtlCreateUserThread - 创建远程线程 执行代码
         8.DLL代码中需监听 DLL加载事件执行
         */
    }

    #endregion WIN-API
}