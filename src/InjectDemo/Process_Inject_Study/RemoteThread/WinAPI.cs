using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Process_Inject_Study.RemoteThread
{
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
            , uint dwStackSize,int lpStartAddress,IntPtr lpParameter,int dwCreationFlags,long lpThreadId);

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
}
