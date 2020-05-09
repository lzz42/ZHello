#pragma warning disable CS3003,CS3002,CS3001

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Cryptography;
using System.Text;

//using System.IO;
//using System.Security;

namespace ZHello.Base
{
    /// <summary>
    /// 计算Hash md5 sha crc32值的类
    /// </summary>
    public static class Hash_Md5_Sha_Crc32
    {
        #region Expand Function

        /// <summary>
        /// 读取字符串第n个字符并转化为ASCII
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte CharAt(this string str, int index)
        {
            Contract.Requires(index >= 0 && str != null && index < str.Length);
            char c = '\0';
            try
            {
                if (index < str.ToCharArray().Length)
                {
                    c = str.ToCharArray()[index];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            byte b = c.ToByte_ASCII();
            return b;
        }

        public static byte ToByte_ASCII(this char c)
        {
            return Encoding.ASCII.GetBytes(c.ToString())[0];
        }

        public static byte ToByte_Unicode(this char c)
        {
            return Encoding.Unicode.GetBytes(c.ToString())[0];
        }

        public static byte ToByte_UTF8(this char c)
        {
            return Encoding.UTF8.GetBytes(c.ToString())[0];
        }

        /// <summary>
        /// 获取字符串的MD5值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="salt">加盐值</param>
        /// <returns></returns>
        public static string MD5(this string str, string salt = "")
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

        #endregion Expand Function
    }

    /// <summary>
    /// 通用Hash值计算
    /// </summary>
    public static class GeneralHashAlgorithm
    {
        /*RSHash*/

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long RSHash(string str)
        {
            int b = 378551;
            int a = 63689;
            long hash = 0;
            for (int i = 0; i < str.Length; i++)
            {
                hash = hash * a + str.CharAt(i);
                a = a * b;
            }
            return hash;
        }

        /*JSHash*/

        public static long JSHash(string str)
        {
            long hash = 1315423911;
            for (int i = 0; i < str.Length; i++)
                hash ^= ((hash << 5) + str.CharAt(i) + (hash >> 2));
            return hash;
        }

        /*PJWHash*/

        public static long PJWHash(string str)
        {
            int BitsInUnsignedInt = (int)(4 * 8);
            int ThreeQuarters = (int)Math.Ceiling((decimal)((BitsInUnsignedInt * 3) / 4)) + 1;
            int OneEighth = (int)Math.Ceiling((decimal)(BitsInUnsignedInt / 8)) + 1;
            var HighBits = (0xFFFFFFFF) << (BitsInUnsignedInt - OneEighth);
            long hash = 0;
            long test = 0;
            for (int i = 0; i < str.Length; i++)
            {
                hash = (hash << OneEighth) + str.CharAt(i);
                if ((test = hash & HighBits) != 0)
                    hash = ((hash ^ (test >> ThreeQuarters)) & (~HighBits));
            }
            return hash;
        }

        /*ELFHash*/

        public static long ELFHash(string str)
        {
            long hash = 0;
            long x = 0;
            for (int i = 0; i < str.Length; i++)
            {
                hash = (hash << 4) + str.CharAt(i);
                if ((x = hash & 0xF0000000L) != 0)
                    hash ^= (x >> 24);
                hash &= ~x;
            }
            return hash;
        }

        /*BKDRHash*/

        public static long BKDRHash(string str)
        {
            long seed = 131;//31131131313131131313etc..
            long hash = 0;
            for (int i = 0; i < str.Length; i++)
                hash = (hash * seed) + str.CharAt(i);
            return hash;
        }

        /*SDBMHash*/

        public static long SDBMHash(string str)
        {
            long hash = 0;
            for (int i = 0; i < str.Length; i++)
                hash = str.CharAt(i) + (hash << 6) + (hash << 16) - hash;
            return hash;
        }

        /*DJBHash*/

        public static long DJBHash(string str)
        {
            long hash = 5381;
            for (int i = 0; i < str.Length; i++)
                hash = ((hash << 5) + hash) + str.CharAt(i);
            return hash;
        }

        /*DEKHash*/

        public static long DEKHash(string str)
        {
            long hash = str.Length;
            for (int i = 0; i < str.Length; i++)
                hash = ((hash << 5) ^ (hash >> 27)) ^ str.CharAt(i);
            return hash;
        }

        /*BPHash*/

        public static long BPHash(string str)
        {
            long hash = 0;
            for (int i = 0; i < str.Length; i++)
                hash = hash << 7 ^ str.CharAt(i);
            return hash;
        }

        /*FNVHash*/

        public static long FNVHash(string str)
        {
            long fnv_prime = 0x811C9DC5;
            long hash = 0;
            for (int i = 0; i < str.Length; i++)
            {
                hash *= fnv_prime;
                hash ^= str.CharAt(i);
            }
            return hash;
        }

        /*APHash*/

        public static long APHash(string str)
        {
            long hash = 0xAAAAAAAA;
            for (int i = 0; i < str.Length; i++)
            {
                if ((i & 1) == 0)
                    hash ^= ((hash << 7) ^ str.CharAt(i) ^ (hash >> 3));
                else
                    hash ^= (~((hash << 11) ^ str.CharAt(i) ^ (hash >> 5)));
            }
            return hash;
        }
    }

    /// <summary>
    /// Md5计算
    /// </summary>
    public static class MD5Lib
    {
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

        /// <summary>
        /// 计算32位MD5码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string Hash_MD5_32(string word, bool toUpper = true)
        {
            try
            {
                MD5CryptoServiceProvider MD5CSP
                     = new MD5CryptoServiceProvider();
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
        public static string Hash_MD5_16(string word, bool toUpper = true)
        {
            try
            {
                string sHash = Hash_MD5_32(word).Substring(8, 16);
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
        public static string Hash_2_MD5_32(string word, bool toUpper = true)
        {
            try
            {
                MD5CryptoServiceProvider MD5CSP
                    = new MD5CryptoServiceProvider();

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
        public static string Hash_2_MD5_16(string word, bool toUpper = true)
        {
            try
            {
                MD5CryptoServiceProvider MD5CSP
                        = new MD5CryptoServiceProvider();

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
    }

    /// <summary>
    /// Sha计算
    /// </summary>
    public static class SHALib
    {
        /// <summary>
        /// 计算SHA-1码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string Hash_SHA_1(string word, bool toUpper = true)
        {
            try
            {
                SHA1CryptoServiceProvider SHA1CSP
                    = new SHA1CryptoServiceProvider();

                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = SHA1CSP.ComputeHash(bytValue);
                SHA1CSP.Clear();

                //根据计算得到的Hash码翻译为SHA-1码
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
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 计算SHA-256码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string Hash_SHA_256(string word, bool toUpper = true)
        {
            try
            {
                SHA256CryptoServiceProvider SHA256CSP
                    = new SHA256CryptoServiceProvider();

                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = SHA256CSP.ComputeHash(bytValue);
                SHA256CSP.Clear();

                //根据计算得到的Hash码翻译为SHA-1码
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
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 计算SHA-384码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string Hash_SHA_384(string word, bool toUpper = true)
        {
            try
            {
                SHA384CryptoServiceProvider SHA384CSP = new SHA384CryptoServiceProvider();
                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = SHA384CSP.ComputeHash(bytValue);
                SHA384CSP.Clear();
                //根据计算得到的Hash码翻译为SHA-1码
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
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 计算SHA-512码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string Hash_SHA_512(string word, bool toUpper = true)
        {
            try
            {
                SHA512CryptoServiceProvider SHA512CSP = new SHA512CryptoServiceProvider();
                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = SHA512CSP.ComputeHash(bytValue);
                SHA512CSP.Clear();

                //根据计算得到的Hash码翻译为SHA-1码
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
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 计算文件的 SHA256 值
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <returns>System.String.</returns>
        public static string SHA256File(FileStream fileStream)
        {
            SHA256 mySHA256 = SHA256Managed.Create();

            byte[] hashValue;

            // Create a fileStream for the file.
            //FileStream fileStream = fInfo.Open(FileMode.Open);
            // Be sure it's positioned to the beginning of the stream.
            fileStream.Position = 0;
            // Compute the hash of the fileStream.
            hashValue = mySHA256.ComputeHash(fileStream);

            // Close the file.
            fileStream.Close();
            // Write the hash value to the Console.
            return FileMd5ShaCrc32.PrintByteArray(hashValue);
        }
    }

    /// <summary>
    /// 计算文件的Md5、Sha、Crc32
    /// </summary>
    public static class FileMd5ShaCrc32
    {
        /// <summary>
        /// 计算文件的 MD5 值
        /// </summary>
        /// <param name="fileName">要计算 MD5 值的文件名和路径</param>
        /// <returns>MD5 值16进制字符串</returns>
        public static string MD5File(string fileName)
        {
            return HashFile(fileName, "md5");
        }

        /// <summary>
        /// 计算文件的 sha1 值
        /// </summary>
        /// <param name="fileName">要计算 sha1 值的文件名和路径</param>
        /// <returns>sha1 值16进制字符串</returns>
        public static string SHA1File(string fileName)
        {
            return HashFile(fileName, "sha1");
        }

        /// <summary>
        /// 计算文件的哈希值
        /// </summary>
        /// <param name="fileName">要计算哈希值的文件名和路径</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值16进制字符串</returns>
        private static string HashFile(string fileName, string algName)
        {
            if (!File.Exists(fileName))
                return string.Empty;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] hashBytes = HashData(fs, algName);
            fs.Close();
            return ByteArrayToHexString(hashBytes);
        }

        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="stream">要计算哈希值的 Stream</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值字节数组</returns>
        private static byte[] HashData(Stream stream, string algName)
        {
            HashAlgorithm algorithm;
            if (algName == null)
            {
                throw new ArgumentNullException("algName 不能为 null");
            }
            if (string.Compare(algName, "sha1", true) == 0)
            {
                algorithm = SHA1.Create();
            }
            else
            {
                if (string.Compare(algName, "md5", true) != 0)
                {
                    throw new Exception("algName 只能使用 sha1 或 md5");
                }
                algorithm = MD5.Create();
            }
            return algorithm.ComputeHash(stream);
        }

        /// <summary>
        /// 字节数组转换为16进制表示的字符串
        /// </summary>
        public static string ByteArrayToHexString(byte[] buf)
        {
            return BitConverter.ToString(buf).Replace("-", "");
        }

        public static string PrintByteArray(byte[] array)
        {
            StringBuilder sb = new StringBuilder();
            int i;
            for (i = 0; i < array.Length; i++)
            {
                sb.Append(String.Format("{0:X2}", array[i]));
            }
            return sb.ToString();
        }

        /// <summary>
        ///  计算指定文件的CRC32值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static String Crc32File(String fileName)
        {
            String hashCRC32 = String.Empty;
            //检查文件是否存在，如果文件存在则进行计算，否则返回空值
            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    //计算文件的CSC32值
                    Crc32 calculator = new Crc32();
                    Byte[] buffer = calculator.ComputeHash(fs);
                    calculator.Clear();
                    //将字节数组转换成十六进制的字符串形式
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("X2"));
                    }
                    hashCRC32 = stringBuilder.ToString();
                }//关闭文件流
            }
            return hashCRC32;
        }
    }

    /// <summary>
    /// 提供 CRC32 算法的实现
    ///
    /// </summary>
    public class Crc32 : HashAlgorithm
    {
        public const uint DefaultPolynomial = 0xedb88320;
        public const uint DefaultSeed = 0xffffffff;
        private uint hash;
        private uint seed;
        private uint[] table;
        private static uint[] defaultTable;

        public Crc32()
        {
            table = InitializeTable(DefaultPolynomial);
            seed = DefaultSeed;
            Initialize();
        }

        public Crc32(uint polynomial, uint seed)
        {
            table = InitializeTable(polynomial);
            this.seed = seed;
            Initialize();
        }

        public override void Initialize()
        {
            hash = seed;
        }

        protected override void HashCore(byte[] buffer, int start, int length)
        {
            hash = CalculateHash(table, hash, buffer, start, length);
        }

        protected override byte[] HashFinal()
        {
            byte[] hashBuffer = UInt32ToBigEndianBytes(~hash);
            this.HashValue = hashBuffer;
            return hashBuffer;
        }

        public static UInt32 Compute(byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(DefaultPolynomial), DefaultSeed, buffer, 0, buffer.Length);
        }

        public static UInt32 Compute(UInt32 seed, byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(DefaultPolynomial), seed, buffer, 0, buffer.Length);
        }

        public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
        }

        private static UInt32[] InitializeTable(UInt32 polynomial)
        {
            if (polynomial == DefaultPolynomial && defaultTable != null)
            {
                return defaultTable;
            }
            UInt32[] createTable = new UInt32[256];
            for (int i = 0; i < 256; i++)
            {
                UInt32 entry = (UInt32)i;
                for (int j = 0; j < 8; j++)
                {
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry = entry >> 1;
                }
                createTable[i] = entry;
            }
            if (polynomial == DefaultPolynomial)
            {
                defaultTable = createTable;
            }
            return createTable;
        }

        private static UInt32 CalculateHash(UInt32[] table, UInt32 seed, byte[] buffer, int start, int size)
        {
            UInt32 crc = seed;
            for (int i = start; i < size; i++)
            {
                unchecked
                {
                    crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
                }
            }
            return crc;
        }

        private byte[] UInt32ToBigEndianBytes(UInt32 x)
        {
            return new byte[] { (byte)((x >> 24) & 0xff), (byte)((x >> 16) & 0xff), (byte)((x >> 8) & 0xff), (byte)(x & 0xff) };
        }
    }
}