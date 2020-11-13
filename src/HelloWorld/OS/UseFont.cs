using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ZHello.OS
{
    public class UseFont
    {
        /* 参考
         https://www.cnblogs.com/arxive/p/7795232.html
         https://docs.microsoft.com/en-us/windows/win32/gdi/font-installation-and-deletion
         */

        /// <summary>
        ///
        /// 经测试只能获取当前进程启动时的字体库 后续变更后获取不到变化后的字体库
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public static bool ExistFont(string fontName)
        {
            InstalledFontCollection collection = new InstalledFontCollection();
            var fonts = collection.Families;
            var ret = false;
            for (int i = 0; i < fonts.Length; i++)
            {
                if (string.Equals(fontName, fonts[i].Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    ret = true;
                    break;
                }
            }
            collection.Dispose();
            return ret;
        }

        public static bool InstallFont(string fontFile, string fontName)
        {
            //系统FONT目录
            string _sTargetFontPath = string.Format(@"{0}\fonts\{1}", System.Environment.GetEnvironmentVariable("WINDIR"), fontFile);
            //需要安装的FONT目录
            var _sResourceFontPath = string.Format(@"{0}\OS\{1}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fontFile);
            int Res;
            const int WM_FONTCHANGE = 0x001D;
            const int HWND_BROADCAST = 0xffff;
            try
            {
                if (!File.Exists(_sTargetFontPath) && File.Exists(_sResourceFontPath))
                {
                    int _nRet;
                    //复制文件
                    File.Copy(_sResourceFontPath, _sTargetFontPath);
                    //添加字体资源
                    _nRet = AddFontResource(_sTargetFontPath);
                    //发送windows消息通知字体资源改变  AddFontResource/RemoveFontResource
                    //通知所有的顶级窗体，需要发送的句柄为HWND_BROADCAST0(0xffff)
                    Res = SendMessage(HWND_BROADCAST, WM_FONTCHANGE, 0, 0);
                    //修改或者更新Win.ini文件
                    _nRet = WriteProfileString("fonts", fontName + "(TrueType)", fontFile);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InstallPrivateFont(string fontFile, string fontName)
        {
            var ret = false;
            if (string.IsNullOrEmpty(fontFile) || string.IsNullOrEmpty(fontName))
                return ret;
            if (!File.Exists(fontFile))
                return ret;
            PrivateFontCollection collection = new PrivateFontCollection();
            collection.AddFontFile(fontFile);
            collection.Dispose();
            ret = true;
            return ret;
        }

        public static bool UninstallFont(string fontName, string fontFile)
        {
            if (!ExistFont(fontName))
            {
                return true;
            }
            //系统FONT目录
            string _sTargetFontPath = string.Format(@"{0}\fonts\{1}", System.Environment.GetEnvironmentVariable("WINDIR"), fontFile);
            int Res;
            const int WM_FONTCHANGE = 0x001D;
            const int HWND_BROADCAST = 0xffff;
            try
            {
                if (File.Exists(_sTargetFontPath))
                {
                    int _nRet;
                    //删除文件
                    File.Delete(_sTargetFontPath);
                    //添加字体资源
                    _nRet = RemoveFontResource(_sTargetFontPath);
                    //发送windows消息通知字体资源改变  AddFontResource/RemoveFontResource
                    //通知所有的顶级窗体，需要发送的句柄为HWND_BROADCAST0(0xffff)
                    Res = SendMessage(HWND_BROADCAST, WM_FONTCHANGE, 0, 0);
                    //修改或者更新Win.ini文件
                    //_nRet = WriteProfileString("fonts", fontName + "(TrueType)", fontFile);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Font GetFontFromEmbedResource(string key)
        {
            var res = Assembly.GetExecutingAssembly().GetManifestResourceStream(key);
            if (res == null)
                return null;
            try
            {
                byte[] buff = new byte[res.Length];
                res.Read(buff, 0, (int)res.Length);
                res.Dispose();
                PrivateFontCollection pfc = new PrivateFontCollection();
                IntPtr MeAdd = Marshal.AllocHGlobal(buff.Length);
                Marshal.Copy(buff, 0, MeAdd, buff.Length);
                pfc.AddMemoryFont(MeAdd, buff.Length);
                return new Font(pfc.Families[0], 15, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int WriteProfileString(string lpszSection, string lpszKeyName, string lpszString);

        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("gdi32")]
        private static extern int AddFontResource(string lpFileName);

        [DllImport("gdi32")]
        private static extern int RemoveFontResource(string lpFileName);
    }
}