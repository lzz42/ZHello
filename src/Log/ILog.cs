using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public interface ILog
    {
        bool IsErrorLog { get; }

        void Info(Exception ex, string msg);
        void Debug(Exception ex, string msg);
        void Warn(Exception ex, string msg);
        void Error(Exception ex, string msg);
    }
}
