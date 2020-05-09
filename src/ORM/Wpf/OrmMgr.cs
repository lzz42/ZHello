using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Orm
{
    public class OrmMgr
    {
       
    }

    public class SqlSugarHelper
    {
        private static SqlSugarClient _db;
        private static object _lock = new object();
        public static string MConnectionStr = "";
        public static void Init(string cstr)
        {
            ConnectionConfig conf = new ConnectionConfig();
            conf.DbType = DbType.Sqlite;
            conf.ConnectionString = cstr;
            conf.InitKeyType = InitKeyType.Attribute;
            _db = new SqlSugarClient(conf);
        }

        public static SqlSugarClient GetInstance()
        {
            lock (_lock)
            {
                if (_db != null)
                {
                    Init(MConnectionStr);
                }
            }
            return _db;
        }

    }

}
