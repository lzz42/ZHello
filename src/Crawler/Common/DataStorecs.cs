using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    /// <summary>
    /// 数据存储
    /// </summary>
    public class DataStore
    {
        private static string fn = "temp.txt";
        private static StreamWriter sw;

        public static void SaveFile(string str)
        {
            if (string.IsNullOrEmpty(str))
                return;
            try
            {
                sw.Write(str);
                sw.Flush();
            }
            catch (Exception ex)
            {
                //TODO:保存数据异常
            }
        }

        public static void Init(string name)
        {
            fn = name + ".txt";
            if (!File.Exists(fn))
            {
                File.Create(fn).Close();
            }
            sw = new StreamWriter(fn, true, Encoding.GetEncoding("utf-8"), 1024 * 1024 * 10);
        }

        public static void Dispose()
        {
            sw?.Flush();
            sw?.Close();
        }
    }
}
