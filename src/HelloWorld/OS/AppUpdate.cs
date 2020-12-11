using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ZHello.OS
{
    /*
    参考资料:
    https://docs.microsoft.com/en-us/windows/win32/dlls/dynamic-link-library-updates
    https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-movefileexa
     */

    /// <summary>
    /// 升级程序
    /// </summary>
    public class AppUpdate
    {
        public static void UpdateThis()
        {
            var ass = Assembly.GetExecutingAssembly();
            var appDir = Path.GetDirectoryName(ass.Location);
            var oldVer = "1.0.0.0";
            string[] oldFiles = new string[] { "ZHello.exe" };
            string[] newFiles = new string[] { "D:\\ZHello.exe" };
            Update(appDir, oldVer, oldFiles, newFiles);
        }

        public static void Update(string appDir, string oldVersion, string[] oldFiles, string[] newFiles)
        {
            var tempDir = Path.Combine(appDir, oldVersion);
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            var list = new List<string>();
            //Move Old Files
            for (int i = 0; i < oldFiles.Length; i++)
            {
                var f1 = Path.Combine(appDir, oldFiles[i]);
                var f2 = Path.Combine(tempDir, oldFiles[i]);
                if (File.Exists(f1))
                {
                    if (!MoveFileEx(f1, f2, MoveFileFlags.MOVEFILE_REPLACE_EXISTING))
                    {
                        var error = GetLastError();
                        Trace.WriteLine("Update MoveFileEx Error:" + error + "\t File:" + oldFiles[i]);
                    }
                    else
                    {
                        list.Add(f2);
                    }
                }
            }
            //Copy New Files
            for (int i = 0; i < newFiles.Length; i++)
            {
                var f1 = newFiles[i];
                var f2 = Path.Combine(appDir, Path.GetFileName(f1));
                if (File.Exists(f1))
                {
                    File.Copy(f1, f2);
                }
            }
            //Delete Old Files
        }

        private enum MoveFileFlags
        {
            MOVEFILE_COPY_ALLOWED = 0x02,
            MOVEFILE_CREATE_HARDLINK = 0x10,
            MOVEFILE_DELAY_UNTIL_REBOOT = 0x04,
            MOVEFILE_FAIL_IF_NOT_TRACKABLE = 0x20,
            MOVEFILE_REPLACE_EXISTING = 0x01,
            MOVEFILE_WRITE_THROUGH = 0x08,
        }

        [DllImport("Kernel32.dll")]
        private static extern int GetLastError();

        /// <summary>
        ///
        /// https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-movefileexa
        /// </summary>
        /// <param name="lpExistFileName"></param>
        /// <param name="lpNewFileName"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        private static extern bool MoveFileEx(string lpExistFileName, string lpNewFileName, MoveFileFlags flags);
    }
}