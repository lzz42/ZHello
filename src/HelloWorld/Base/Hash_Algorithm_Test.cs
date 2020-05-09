using System;
using System.Collections.Generic;

namespace ZHello.Base
{
    /// <summary>
    /// 测试Hash算法
    /// </summary>
    internal class Hash_Algorithm_Test
    {
        private static Dictionary<string, Type> sdic = new Dictionary<string, Type>();
        private static Dictionary<string, RuntimeTypeHandle> shdic = new Dictionary<string, RuntimeTypeHandle>();
        private static Dictionary<long, Type> idic = new Dictionary<long, Type>();

        public static void ComparePerformance_Dictionary()
        {
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            Type[] types;
            var lis = new List<Type>();
            for (int i = 0; i < ass.Length; i++)
            {
                lis.AddRange(ass[i].GetExportedTypes());
            }
            types = lis.ToArray();

            var scc = 0d;
            var icc = 0d;
            int amount = 1024 * 100;
            for (int i = 0; i < amount; i++)
            {
                var guid = Guid.NewGuid();
                var t = typeof(Guid);
                try
                {
                    var temp = Environment.TickCount;
                    sdic.Add(guid.ToString(), t);
                    //shdic.Add(guid.ToString(), Type.GetTypeHandle(t));
                    scc += Environment.TickCount - temp;
                    //Console.WriteLine("line:{0}", i);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Scc" + ex.Message);
                }
                try
                {
                    var temp = Environment.TickCount;
                    idic.Add(HashAlgorithm(guid.ToString()), t);
                    icc += Environment.TickCount - temp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("idc" + ex.Message);
                }
            }
            GC.Collect(0);
            Console.WriteLine("string dic build Alltime:{0},Avgtime:{1}", scc, scc / types.Length);
            Console.WriteLine("long dic build Alltime:{0},Avgtime:{1}", icc, icc / types.Length);
            Console.WriteLine(string.Format("total:{0}", idic.Count));
            //随机读取检索
            var rand = new Random(7);
            var sc = 0d;
            var ic = 0d;
            int all = amount / 3;
            for (int i = 0; i < all; i++)
            {
                var n = rand.Next(0, types.Length - 1);
                var fnk = types[n].FullName;
                Type result, result2;

                var tt = Environment.TickCount;
                if (sdic.TryGetValue(fnk, out result))
                {
                }
                sc += Environment.TickCount - tt;
                tt = Environment.TickCount;
                if (idic.TryGetValue(HashAlgorithm(fnk), out result))
                {
                }
                ic += Environment.TickCount - tt;
            }
            Console.WriteLine("Test amount:{0}", all);
            Console.WriteLine("string total Time:{0},AvgTime:{1}", sc, sc / all);
            Console.WriteLine("long total Time:{0},AvgTime:{1}", ic, ic / all);
        }

        private static long HashAlgorithm(string s)
        {
            return GeneralHashAlgorithm.APHash(s);
        }
    }
}