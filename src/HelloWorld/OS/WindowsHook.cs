using System;
using System.Runtime.InteropServices;

namespace ZHello.OS
{
    public class WindowsHook
    {
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        private const int WH_MOUSE_LL = 14;
        private const int WH_KEYBOARD_Local = 2;
        private const int WH_KEYBOARD_Global = 13;
        private const int WM_KEYDOWN = 0x100;//KEYDOWN
        private const int WM_KEYUP = 0x101;//KEYUP
        private const int WM_SYSKEYDOWN = 0x104;//SYSKEYDOWN
        private const int WM_SYSKEYUP = 0x105;//SYSKEYUP

        /// <summary>
        /// 键盘结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode;  //定一个虚拟键码。该代码必须有一个价值的范围1至254
            public int scanCode; // 指定的硬件扫描码的关键
            public int flags;  // 键标志
            public int time; // 指定的时间戳记的这个讯息
            public int dwExtraInfo; // 指定额外信息相关的信息
        }

        /// <summary>
        /// 安装钩子
        /// </summary>
        /// <param name="idHook">钩子类型，即确定钩子监听何种消息</param>
        /// <param name="lpfn">lpfn 钩子子程的地址指针,如果dwThreadId参数为0 或是一个由别的进程创建的线程的标识，
        /// lpfn必须指向DLL中的钩子子程。
        /// 除此以外，lpfn可以指向当前进程的一段钩子子程代码。
        /// 钩子函数的入口地址，当钩子钩到任何消息后便调用这个函数。</param>
        /// <param name="hInstance">hInstance应用程序实例的句柄。标识包含lpfn所指的子程的DLL。如果threadId 标识当前进程创建的一个线程，而且子
        /// 程代码位于当前进程，hInstance必须为NULL。可以很简单的设定其为本应用程序的实例句柄。</param>
        /// <param name="threadId">threaded 与安装的钩子子程相关联的线程的标识符
        /// 如果为0，钩子子程与所有的线程关联，即为全局钩子</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetWindowsHookEx", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        /// <summary>
        /// 卸载钩子
        /// </summary>
        /// <param name="idHook"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "UnhookWindowsHookEx", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// 调用下一个钩子
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "CallNextHookEx", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        /// <summary>
        /// 获取当前线程ID
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern int GetCurrentThreadId();

        /// <summary>
        /// 使用WINDOWS API函数代替获取当前实例的函数,防止钩子失效
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        /// <summary>
        /// ToAscii职能的转换指定的虚拟键码和键盘状态的相应字符或字符
        /// </summary>
        /// <param name="uVirtKey"></param>
        /// <param name="uScanCode"></param>
        /// <param name="lpbKeyState"></param>
        /// <param name="lpwTransKey"></param>
        /// <param name="fuState"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, //[in] 指定虚拟关键代码进行翻译。
                                         int uScanCode, // [in] 指定的硬件扫描码的关键须翻译成英文。高阶位的这个值设定的关键，如果是（不压）
                                         byte[] lpbKeyState, // [in] 指针，以256字节数组，包含当前键盘的状态。每个元素（字节）的数组包含状态的一个关键。如果高阶位的字节是一套，关键是下跌（按下）。在低比特，如果设置表明，关键是对切换。在此功能，只有肘位的CAPS LOCK键是相关的。在切换状态的NUM个锁和滚动锁定键被忽略。
                                         byte[] lpwTransKey, // [out] 指针的缓冲区收到翻译字符或字符。
                                         int fuState); // [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise.

        /// <summary>
        /// 获取按键的状态
        /// </summary>
        /// <param name="pbKeyState"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="lpfn"></param>
        /// <param name="hInstance"></param>
        /// <param name="threadId"></param>
        /// <returns>返回值为0，安装失败</returns>

        public static int InstallHook(HookType type, HookProc lpfn, IntPtr hInstance, int threadId)
        {
            return SetWindowsHookEx((int)type, lpfn, hInstance, threadId);
        }

        public static int InstallHook(HookType type, int pid)
        {
            //return SetWindowsHookEx((int)type, lpfn, hInstance, 0);
            return 0;
        }

        public enum HookType
        {
            Local_Keyboard = 2,
            Global_Keyboard = 13,
            Local_Mouse = 7,
            Global_Mouse = 14,
        }
    }
}