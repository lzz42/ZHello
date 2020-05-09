/*
 * GitHub :https://github.com/sshnet/SSH.NET
 * 
 * SSH：
 * Secure Shell的缩写。通过使用SSH，可以把所有传输的数据进行加密，而且能够防止DNS欺骗和IP欺骗。使用SSH，还可以将传输的数据压缩，所以可以加快传输的速度。SSH可以为FTP提供一个安全的“通道”。
 * 建立在应用层和传输层基础上的安全协议，它主要由以下三部分组成，共同实现SSH的安全保密机制。
        传输层协议，它提供诸如认证、信任和完整性检验等安全措施，此外它还可以任意地提供数据压缩功能。通常情况下，这些传输层协议都建立在面向连接的TCP数据流之上。
        用户认证协议层，用来实现服务器的跟客户端用户之间的身份认证，它运行在传输层协议之上。
        连接协议层，分配多个加密通道至一些逻辑通道上，它运行在用户认证层协议之上。
   当安全的传输层连接建立之后，客户端将发送一个服务请求。当用户认证层连接建立之后将发送第二个服务请求。这就允许新定义的协议可以和以前的协议共存。连接协议提供可用作多种目的通道，为设置安全交互Shell会话和传输任意的TCP/IP端口和X11连接提供标准方法。
   
   SSH提供两种级别的安全验证：SSH1和SSH2。
    SSH1（基于口令的安全验证），只要你知道自己的帐号和口令，就可以登录到远程主机，并且所有传输的数据都会被加密。但是，这种验证方式不能保证你正在连接的服务器就是你想连接的服务器。可能会有别的服务器在冒充真正的服务器，也就是受到"中间人"这种攻击方式的攻击。
    SSH2（基于密匙的安全验证），需要依靠密匙，也就是你必须为自己创建一对密匙，并把公有密匙放在需要访问的服务器上。如果你要连接到SSH服务器上，客户端软件就 会向服务器发出请求，请求用你的密匙进行安全验证。服务器收到请求之后，先在你在该服务器的用户根目录下寻找你的公有密匙，然后把它和你发送过来的公有密 匙进行比较。如果两个密匙一致，服务器就用公有密匙加密"质询"（challenge）并把它发送给客户端软件。客户端软件收到"质询"之后就可以用你的 私人密匙解密再把它发送给服务器。

    比较：SSH1相比，SSH2不需要在网络上传送用户口令。另外，SSH2不仅加密所有传送的数据，而"中间人"这种攻击方式也是不可能的（因为他没有你的私人密匙）。但是整个登录的过程可能慢一些。

SSH最常见的应用就是，用它来取代传统的Telnet、FTP等网络应用程序，通过SSH登录到远方机器执行你想进行的工作与命令。在不安全的网路通讯环境中，它提供了很强的验证（authentication）机制与非常安全的通讯环境。
 * SFTP：使用SSH协议进行FTP传输的协议，安全文件传输协议
 * 
 * SFTP服务端：
 * Windows 安装SFTP
 * 1.下载freesshd ：http://www.freesshd.com/?ctt=download
 * 2. freeSSHd.exe
 * 3.安装
 * 4.
 * 
 * 
 * SFTP客户端：
 * Filezillia
 * https://filezilla-project.org/download.php?type=client
 * 
 * 参考资料：
 * https://www.cnblogs.com/happyday56/p/5664693.html
 * 
 * 
 */

using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Net;
using Renci.SshNet;

 namespace HelloWorld.IPC
{
    public class RenciSshNetHelper
    {
        /// <summary>
        /// SSH1 - 用户名密码认证
        /// 使用密码和公钥认证建立SFTP连接
        /// </summary>
        /// <param name="host"></param>
        /// <param name="userName"></param>
        /// <param name="name"></param>
        /// <param name="psw"></param>
        /// <param name="keyName"></param>
        /// <param name="client"></param>
        public static void EstablishConnection(string host, string userName, string name, string psw, string keyName, out SftpClient client)
        {
            var connectionInfo = new ConnectionInfo("sftp.foo.com",
                                        "guest",
                                        new PasswordAuthenticationMethod("guest", "pwd"),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
            client = new SftpClient(connectionInfo);
            client.Connect();
        }

        /// <summary>
        /// SSH2 - 公钥私钥认证
        /// 使用用户名和密码建立SSH连接，并验证服务端指纹
        /// </summary>
        public static void VerifyHostIdentify()
        {
            byte[] expectedFingerPrint = new byte[] {
                                            0x66, 0x31, 0xaf, 0x00, 0x54, 0xb9, 0x87, 0x31,
                                            0xff, 0x58, 0x1c, 0x31, 0xb1, 0xa2, 0x4c, 0x6b
                                        };
            using (var client = new SshClient("sftp.foo.com", "guest", "pwd"))
            {
                client.HostKeyReceived += (sender, e) =>
                {
                    if (expectedFingerPrint.Length == e.FingerPrint.Length)
                    {
                        for (var i = 0; i < expectedFingerPrint.Length; i++)
                        {
                            if (expectedFingerPrint[i] != e.FingerPrint[i])
                            {
                                e.CanTrust = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        e.CanTrust = false;
                    }
                };
                client.Connect();
            }
        }
    }
}