using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace ZHello.Algorithm.Lib
{
    /// <summary>
    /// 通用Hash值计算
    /// </summary>
    public static class HashLib
    {
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
            byte b = Encoding.ASCII.GetBytes(c.ToString())[0];
            return b;
        }

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
}