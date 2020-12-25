using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Insert(K k,int x)
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
        public static int DevideHash(int k,int m)
        {
            var ret = 0;
            ret = k % m;
            return ret;
        }

        public static int MuiltiHash(int k,int m)
        {
            //A=(sqrt(5)-2)/2 =0.618 033 988 7
            float A = 0.5f;
            var ret = 0;
            ret = (int)Math.Floor(m * (k * m - Math.Floor(k * A)));
            return ret;
        }
    }
}
