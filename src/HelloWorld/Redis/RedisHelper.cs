#pragma warning disable CS3002,CS3001 // #warning 指令

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisHelper
{
    public class RedisHelper 
    {
        public static string ConnectionStr = @"127.0.0.1:6379,password="",keepAlive=180";
        private static volatile IConnectionMultiplexer MConnection;
        private static object _lock = new object();
        public static IConnectionMultiplexer GetConnection()
        {
            if (MConnection != null&& MConnection.IsConnected)
            {
                return MConnection;
            }
            lock (_lock)
            {
                if (MConnection != null && MConnection.IsConnected)
                {
                    return MConnection;
                }
                MConnection?.Dispose();
                ConfigurationOptions config = new ConfigurationOptions
                {
                    EndPoints =
                            {
                                { "redis0", 6379 }
                            },
                                CommandMap = CommandMap.Create(new HashSet<string>
                            { // EXCLUDE a few commands
                                "INFO", "CONFIG", "CLUSTER",
                                "PING", "ECHO", "CLIENT"
                            }, available: false),
                            KeepAlive = 180,
                            //DefaultVersion = new Version(2, 8, 8),
                            Password = ""
                };
                try
                {
                    MConnection = ConnectionMultiplexer.Connect(ConnectionStr);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return MConnection;
        }

        public static IDatabase GetDatabase(int? db=null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }

        public static void Set(IDatabase db,string key,object value)
        {
            if (db == null)
                return;
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
            db.StringSet(key, bytes, null);
        }

        public static void Get<T>(IDatabase db,string key,out T value)
        {
            if(db==null)
            {
                value = default(T);
                return;
            }
            var tempv = db.StringGet(key);
            if(!tempv.HasValue)
            {
                value = default(T);
                return;
            }
            try
            {
               var bytes = (byte[])tempv;
               var json = Encoding.UTF8.GetString(bytes);
                value = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static bool Haskey(IDatabase db,string key)
        {
            if (db == null)
                return false;
            return db.KeyExists(key);
        }

        public static void CeateDb()
        {

        }

    }

    public class RedisMain
    {
        public static void Main(string[] args)
        {
            if (5 * 256 == 5 << 8)
            {
                Console.WriteLine("yes");
            }
            var con = RedisHelper.GetConnection();
            var db = RedisHelper.GetDatabase();
            var key = "kkkk";
            RedisHelper.Set(db, key, "11111");
            string s;
            RedisHelper.Get(db, key, out s);
        }
    }
}
