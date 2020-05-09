using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Reflection;
//using Thrift;
//using Thrift.Protocol;

namespace ZHello.IPC
{
    public class ThriftHelper
    {
        
    }
}

/* Thirft
开源跨语言的RPC框架
通过一个中间语言IDL定义RPC的数据类型和接口，并写在.thrift文件中，通过特殊的编译器生成不同语言的代码；
该代码包括：目标语言的接口定义，方法，数据类型，RPC协议层，传输层实现；
协议栈结构


1.编写.thrift脚本；
2.使用thrift对脚本进行编译生成指定语言的代码文件，thrift.exe --gen csharp *.thrift；
3.引用生成的代码文件；
4.

 */