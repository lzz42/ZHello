using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZHello.Algorithm.Lib
{
    /// <summary>
    /// Md5计算
    /// </summary>
    public static class MD5Lib
    {
        /// <summary>
        /// 获取字符串的MD5值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="salt">加盐值</param>
        /// <returns></returns>
        public static string MD5_UTF8(string str, string salt = "")
        {
            //MD5计算类
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] bytValue, bytHash;
                //将要计算的字符串转换为字节数组
                bytValue = Encoding.UTF8.GetBytes(salt + str);
                //计算结果同样是字节数组
                bytHash = md5.ComputeHash(bytValue);
                //将字节数组转换为字符串
                string sTemp = "";
                for (int i = 0; i < bytHash.Length; i++)
                {
                    sTemp += bytHash[i].ToString("x").PadLeft(2, '0');
                }
                return sTemp;
            }
        }

        /// <summary>
        /// 计算文件的 MD5 值
        /// </summary>
        /// <param name="fileName">要计算 MD5 值的文件名和路径</param>
        /// <returns>MD5 值16进制字符串</returns>
        public static string MD5File(string fileName)
        {
            string res = null;
            if (File.Exists(fileName))
            {
                using (var fs = File.OpenRead(fileName))
                {
                    using (var md5 = MD5.Create())
                    {
                        byte[] hashBytes = md5.ComputeHash(fs);
                        res = BitConverter.ToString(hashBytes).Replace("-", "");
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 计算32位MD5码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string MD5_32(string word, bool toUpper = true)
        {
            try
            {
                MD5CryptoServiceProvider MD5CSP = new MD5CryptoServiceProvider();
                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = MD5CSP.ComputeHash(bytValue);
                MD5CSP.Clear();
                //根据计算得到的Hash码翻译为MD5码
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                //根据大小写规则决定返回的字符串
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 计算16位MD5码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string MD5_16(string word, bool toUpper = true)
        {
            try
            {
                string sHash = MD5_32(word).Substring(8, 16);
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 计算32位2重MD5码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string MD5_32_2(string word, bool toUpper = true)
        {
            try
            {
                MD5CryptoServiceProvider MD5CSP = new MD5CryptoServiceProvider();
                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = MD5CSP.ComputeHash(bytValue);
                //根据计算得到的Hash码翻译为MD5码
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                bytValue = Encoding.UTF8.GetBytes(sHash);
                bytHash = MD5CSP.ComputeHash(bytValue);
                MD5CSP.Clear();
                sHash = "";
                //根据计算得到的Hash码翻译为MD5码
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }

                //根据大小写规则决定返回的字符串
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 计算16位2重MD5码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string MD5_16_2(string word, bool toUpper = true)
        {
            try
            {
                MD5CryptoServiceProvider MD5CSP = new MD5CryptoServiceProvider();
                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = MD5CSP.ComputeHash(bytValue);
                //根据计算得到的Hash码翻译为MD5码
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                sHash = sHash.Substring(8, 16);
                bytValue = Encoding.UTF8.GetBytes(sHash);
                bytHash = MD5CSP.ComputeHash(bytValue);
                MD5CSP.Clear();
                sHash = "";
                //根据计算得到的Hash码翻译为MD5码
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                sHash = sHash.Substring(8, 16);
                //根据大小写规则决定返回的字符串
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static string Md5Encrypt(string input)
        {
            //用来计算MD5值的对象
            using (MD5 md5Hash = MD5.Create())
            {
                //获取字符串对应的byte数组，计算MD5值
                byte[] md5Byts = md5Hash.ComputeHash(Encoding.Default.GetBytes(input));
                //创建一个新的Stringbuilder来收集的字节和创建一个字符串
                StringBuilder sb = new StringBuilder();
                //循环遍历每个字节的散列的数据和每一个十六进制格式字符串
                for (int i = 0; i < md5Byts.Length; i++)
                {
                    //"x"表示16进制，2表示保留两位，例：2——>02
                    sb.Append(md5Byts[i].ToString("x2"));
                }
                //返回十六进制字符串。
                return sb.ToString();
            }
        }
    }
}