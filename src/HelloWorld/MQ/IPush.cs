using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZHello.MQ
{
    public interface IPush: ISocket
    {
        void Push(string data);
    }
    public interface IPull : ISocket
    {
        void Pull(out string data);
    }
}
