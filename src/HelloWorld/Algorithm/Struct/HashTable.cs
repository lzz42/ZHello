﻿using System;

namespace ZHello.Algorithm.Struct
{
    /// <summary>
    /// 11.1 直接寻址表
    /// 关键字k的范围比较小，次数直接使用关键字作为下标
    /// </summary>
    public class DirectAddressTable<K>
    {
        /// <summary>
        /// 全域U
        /// </summary>
        private K[] U { get; set; }

        private K this[int i]
        {
            get { return U[i]; }
            set { U[i] = value; }
        }

        public K Search(int k)
        {
            return U[k];
        }

        public void Insert(K k, int x)
        {
            U[x] = k;
        }

        public void Delete(int x)
        {
            U[x] = default(K);
        }
    }

    public class HashTable
    {
        public static int DivideHash(int k, int m)
        {
            return k % m;
        }

        public static int MultiplyHash(int k, int m)
        {
            //A=(sqrt(5)-2)/2 = 0.618 033 988 7
            float A = 0.618f;
            var ret = k * m - Math.Floor(k * A);
            return (int)Math.Floor(m * ret);
        }
    }
}