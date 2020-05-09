using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ZHello.Common
{
    /// <summary>
    /// 托盘图标
    /// </summary>
    public class Tray : IDisposable
    {
        private NotifyIcon Notify = new NotifyIcon();

        private ContextMenu Menu { get; set; }
        private Dictionary<string, Action> MenuItemActions { get; set; }

        public Tray()
        {
            Notify = new NotifyIcon()
            {
                Text = "托盘显示",
                Icon = SystemIcons.Error,
                Visible = true,
            };
            Menu = new ContextMenu();
            Notify.ContextMenu = Menu;

            MenuItemActions = new Dictionary<string, Action>();
        }

        public void AddMenu(string text, Action click)
        {
            if (string.IsNullOrEmpty(text))
                return;
            if (MenuItemActions.ContainsKey(text))
                return;
            MenuItemActions.Add(text, click);
            var item = new MenuItem()
            {
                Text = text,
            };
            item.Click += Item_Click;
        }

        private void Item_Click(object sender, EventArgs e)
        {
            if (!(sender is MenuItem))
            {
                return;
            }
            var item = sender as MenuItem;
            if (string.IsNullOrEmpty(item.Text))
                return;
            if (!MenuItemActions.ContainsKey(item.Text))
            {
                return;
            }
            var act = MenuItemActions[item.Text];
            if (act == null)
            {
                return;
            }
            try
            {
                act.Invoke();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ShowBallon(int time, string title, string content)
        {
            Notify.ShowBalloonTip(time, title, content, ToolTipIcon.Warning);
        }

        public void Show()
        {
            Notify.Visible = true;
        }

        public void Hide()
        {
            Notify.Visible = false;
        }

        public void Dispose()
        {
            Notify.Visible = false;
            Notify.Dispose();
        }
    }

    public static class TrayHelper
    {
        #region 禁用关闭按钮

        /// <summary>
        /// 查找窗体句柄
        /// 根据指定的类名和窗口名仅查找顶级的窗体，不区分大小写
        /// </summary>
        /// <param name="lpClassName">类名或者为空</param>
        /// <param name="lpWindowName">窗体标题，若标题为null，则匹配所有窗体</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="bRevert"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        /// <summary>
        /// 删除菜单项
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="uPosition"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        private static extern IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="hwind"></param>
        /// <param name="cmdShow"></param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "ShowWindow")]
        public static extern bool ShowWindow(IntPtr hwind, int cmdShow);

        /// <summary>
        /// 设置窗口置前
        /// </summary>
        /// <param name="hwind"></param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hwind);

        public static void GetSysIcon(out Icon icon)
        {
            icon = SystemIcons.Question;
        }

        ///<summary>
        /// 禁用关闭按钮
        ///</summary>
        ///<param name="consoleName">控制台名字</param>
        public static void DisableCloseButton(string title)
        {
            Thread.Sleep(20);
            IntPtr windowHandle = FindWindow(null, title);
            IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
        }

        public static bool IsExistsConsole(string title)
        {
            IntPtr windowHandle = FindWindow(null, title);
            if (windowHandle.Equals(IntPtr.Zero))
            {
                return false;
            }
            return true;
        }

        private static void OpWindow(string title, int cmdshow)
        {
            IntPtr ParenthWnd = new IntPtr(0);
            IntPtr et = new IntPtr(0);
            ParenthWnd = FindWindow(null, title);
            ShowWindow(ParenthWnd, cmdshow);
        }

        public static void Hidden(string title)
        {
            OpWindow(title, 0);
        }

        public static void Show(string title)
        {
            OpWindow(title, 9);
        }

        #endregion 禁用关闭按钮
    }

    /// <summary>
    /// 播放声音单元
    /// </summary>
    public static class PlaySoundUtility
    {
        /// <summary>
        /// 播放标识
        /// </summary>
        [Flags]
        private enum PlaySoundFlags : int
        {
            /// <summary>
            /// 同步播放
            /// </summary>
            SND_SYNC = 0x0000,    /* play synchronously (default) */ //同步

            /// <summary>
            /// 异步播放
            /// </summary>
            SND_ASYNC = 0x0001,    /* play asynchronously */ //异步

            SND_NODEFAULT = 0x0002,    /* silence (!default) if sound not found */
            SND_MEMORY = 0x0004,    /* pszSound points to a memory file */
            SND_LOOP = 0x0008,    /* loop the sound until next sndPlaySound */
            SND_NOSTOP = 0x0010,    /* don't stop any currently playing sound */
            SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
            SND_ALIAS = 0x00010000, /* name is a registry alias */
            SND_ALIAS_ID = 0x00110000, /* alias is a predefined ID */
            SND_FILENAME = 0x00020000, /* name is file name */
            SND_RESOURCE = 0x00040004    /* name is resource name or atom */
        }

        /// <summary>
        /// 播放声音WAV文件
        /// </summary>
        /// <param name="szSound"></param>
        /// <param name="hMod"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("winmm.dll")]
        private static extern bool PlaySound(string szSound, IntPtr hMod, PlaySoundFlags flags);

        /// <summary>
        /// 播放WAV文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool PlaySound(string fileName)
        {
            if (System.IO.File.Exists(fileName) && System.IO.Path.GetExtension(fileName).ToLower().Equals(".wav"))
            {
                return PlaySound(fileName, IntPtr.Zero, PlaySoundFlags.SND_ASYNC);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 播放WAV文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool PlayWavSound(string fileName)
        {
            if (System.IO.File.Exists(fileName) && System.IO.Path.GetExtension(fileName).ToLower().Equals(".wav"))
            {
                try
                {
                    System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(fileName);
                    sndPlayer.Play();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}