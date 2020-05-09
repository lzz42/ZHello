using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAspWebAPI
{
    [ProtoBuf.ProtoContract]
    public class User
    {
        [ProtoBuf.ProtoMember(1)]
        public string Name { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public int Age { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public string Address { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public float Hight { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public bool IsAlive { get; set; }
    }
}
