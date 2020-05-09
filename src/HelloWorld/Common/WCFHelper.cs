using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using ZHello.Common.Icl;

namespace ZHello.Common
{
    /*服务创建*/

    /// <summary>
    /// 定义接口-契约
    /// </summary>
    namespace Icl
    {
        //服务契约

        // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
        [ServiceContract(Name = "ServiceA", Namespace = "http://127.0.0.1:11223")]
        public interface IServiceA
        {
            [OperationContract]
            string GetDataA(int value);

            [OperationContract]
            CompositeType GetDataUsingDataContractA(CompositeType composite);

            // TODO: 在此添加您的服务操作
        }

        [ServiceContract(Name = "ServiceB", Namespace = "http://127.0.0.1:11224", CallbackContract = typeof(ICallBackA))]
        public interface IServiceB
        {
            [OperationContract]
            [FaultContract(typeof(FaultException))]
            string GetDataB(int value);

            [OperationContract]
            CompositeType GetDataUsingDataContractB(CompositeType composite);
        }

        [ServiceContract(Name = "ServiceC", Namespace = "http://127.0.0.1:11225")]
        public interface IServiceC
        {
            [OperationContract(IsOneWay = true)]
            void SetDataC(int value);

            [OperationContract]
            Tuple<string, int> GetStr(string name, int a, int b);
        }

        [ServiceContract(Name = "ICallBackA")]
        public interface ICallBackA
        {
            [OperationContract]
            int Call(int t);
        }

        //数据契约

        // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
        [DataContract]
        public class CompositeType : INotifyPropertyChanged
        {
            private bool boolValue = true;
            private string stringValue = "Hello ";

            [DataMember]
            public bool BoolValue
            {
                get { return boolValue; }
                set { SetProperty(ref boolValue, value); }
            }

            [DataMember]
            public string StringValue
            {
                get { return stringValue; }
                set { SetProperty(ref stringValue, value); }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnNotifyPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            protected virtual void SetProperty<T>(ref T item, T value, [CallerMemberName] string propertyName = null)
            {
                if (!EqualityComparer<T>.Default.Equals(item, value))
                {
                    item = value;
                    OnNotifyPropertyChanged(propertyName);
                }
            }
        }

        //错误契约

        // [FaultContract(detailType:typeof(Msg))]
        [DataContract]
        public class FaultException
        {
            [DataMember]
            public string Msg { get; set; }
        }

        //消息契约

        [MessageContract]
        public class Msg
        {
            [MessageBodyMember(Name = "msgb", Namespace = "msgb ms")]
            public string MsgB { get; set; }

            [MessageHeader(Name = "msgh",
                Namespace = "msgh ms",
                MustUnderstand = true)]
            public string MsgH { get; set; }
        }
    }

    /// <summary>
    /// 定义实现
    /// </summary>
    namespace IclLibrary
    {
        [ServiceBehavior()]
        public class CLA : IServiceA
        {
            private string m = AppDomain.CurrentDomain.FriendlyName;

            public string GetDataA(int value)
            {
                return m + "_A_" + value;
            }

            public CompositeType GetDataUsingDataContractA(CompositeType composite)
            {
                return new CompositeType() { BoolValue = !composite.BoolValue, StringValue = m + "_A_" + composite.StringValue };
            }
        }

        [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
        public class CLB : IServiceB
        {
            private string m = AppDomain.CurrentDomain.FriendlyName;

            public string GetDataB(int value)
            {
                if (value > 50)
                {
                    Console.WriteLine("GetDataB call value:" + value);
                    //服务端回调 获取回调信道
                    var call = OperationContext.Current.GetCallbackChannel<Icl.ICallBackA>();
                    if (call != null)
                    {
                        var res = call.Call(value);
                        Console.WriteLine("value call back:" + value + ",rsult is :" + res);
                    }
                }
                return m + "_B_" + value;
            }

            public CompositeType GetDataUsingDataContractB(CompositeType composite)
            {
                return new CompositeType() { BoolValue = !composite.BoolValue, StringValue = m + "_B_" + composite.StringValue };
            }
        }

        [ServiceBehavior]
        public class CLC : IServiceC
        {
            private string m = AppDomain.CurrentDomain.FriendlyName;

            public void SetDataC(int value)
            {
                Console.WriteLine("receive value:" + value);
            }

            [WebGet(UriTemplate = "string?name={name}")]
            public string GetStr(string name)
            {
                return "";
            }

            [WebGet(UriTemplate = "Tuple<string,int>?name={name}&a={a}&b={b}")]
            public Tuple<string, int> GetStr(string name, int a, int b)
            {
                return Tuple.Create(name, a * b);
            }
        }
    }

    /*服务发布与使用*/

    /// <summary>
    /// 实现宿主
    /// </summary>
    namespace WcfServiceHost
    {
        internal class Program
        {
            private static List<ServiceHost> hosts = new List<ServiceHost>();

            private static void Main(string[] args)
            {
                var a = BuldHost(typeof(IclLibrary.CLA));
                hosts.Add(a);

                var b = BuldHost(typeof(IclLibrary.CLB));
                hosts.Add(b);

                var c = BuldHost(typeof(IclLibrary.CLC));
                hosts.Add(c);

                var d = BuldHostEx<IServiceB>(typeof(IclLibrary.CLB), @"http://127.0.0.1:10232");
                hosts.Add(d);

                Console.ReadKey();
                hosts.ForEach(h => h?.Close());
            }

            private static ServiceHost BuldHost(Type type)
            {
                try
                {
                    if (type != null)
                    {
                        var host = new ServiceHost(type);
                        host.Opened += delegate
                        {
                            Console.WriteLine("Service" + type.Name + "已经启动，按任意键终止服务！");
                        };
                        host.Open();
                        return host;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return null;
            }

            private static ServiceHost BuldHostEx<T>(Type type, string url)
            {
                try
                {
                    var _host = new ServiceHost(type);
                    var address = new Uri(url);
                    var binding = new WSDualHttpBinding();
                    var t = typeof(T);
                    //_host.AddServiceEndpoint(typeof(T),
                    //    new WSDualHttpBinding(),
                    //    new Uri(url)
                    //    );
                    _host.AddServiceEndpoint(t, binding, address);
                    //数据发布
                    if (_host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                    {
                        //服务元数据
                        var behavior = new ServiceMetadataBehavior();
                        behavior.HttpGetEnabled = true;
                        //元数据Url
                        behavior.HttpGetUrl = new Uri(url + "/metadata");
                        //将元数据信息添加到服务
                        _host.Description.Behaviors.Add(behavior);
                    }
                    _host.Faulted += (object sender, EventArgs e) => { };
                    _host.Opened += (sender, e) =>
                    {
                        var h = sender as ServiceHost;
                        Console.WriteLine("service has opened :" + h.BaseAddresses);
                    };
                    if (_host.State != CommunicationState.Opening)
                    {
                        try
                        {
                            _host.Open();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return null;
            }
        }
    }

    /*服务使用*/

    namespace TestWCF
    {
        internal class Program
        {
            //static void Main(string[] args)
            //{
            //    var a = new ServiceReference.ServiceAClient();
            //    a.Open();
            //    if (a.State == System.ServiceModel.CommunicationState.Opened)
            //    {
            //       var t1 = a.GetDataA(99);
            //       Console.WriteLine(t1);
            //       var t2 = a.GetDataUsingDataContractA(new ServiceReference1.CompositeType() { BoolValue = true, StringValue = "testA" });
            //       Console.WriteLine(t2.BoolValue + "::" + t2.StringValue);
            //    }
            //    var cb = new CallB();
            //    var b = new ServiceReference2.ServiceBClient(new System.ServiceModel.InstanceContext(cb));
            //    b.Open();
            //    if (b.State == System.ServiceModel.CommunicationState.Opened)
            //    {
            //       var t1 = b.GetDataB(69);
            //       Console.WriteLine(t1);
            //       var t2 = b.GetDataUsingDataContractB(new ServiceReference2.CompositeType() { BoolValue = true, StringValue = "testB" });
            //       Console.WriteLine(t2.BoolValue + "::" + t2.StringValue);
            //    }
            //    var c = new ServiceReference3.ServiceCClient();
            //    c.Open();
            //    if (c.State == System.ServiceModel.CommunicationState.Opened)
            //    {
            //       c.SetDataC(99);
            //    }
            //    Console.ReadLine();
            //    a.Close();
            //    b.Close();
            //    c.Close();
            //}

            /// <summary>
            /// 构建一般服务连接
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="binding"></param>
            /// <param name="address"></param>
            /// <returns></returns>
            private static Tuple<T, ChannelFactory<T>> BuildChannel<T>(Binding binding, EndpointAddress address)
            {
                var factory = new ChannelFactory<T>(binding, address);
                return Tuple.Create(factory.CreateChannel(), factory);
            }

            /// <summary>
            /// 构建双工通信的服务连接 双工通信提供回调URL 在binding中设置ClientAddress
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="context"></param>
            /// <param name="binding"></param>
            /// <param name="address"></param>
            /// <returns></returns>
            private static Tuple<T, DuplexChannelFactory<T>> BuildDuplexChannel<T>(InstanceContext context, Binding binding, EndpointAddress address)
            {
                var factory = new DuplexChannelFactory<T>(context, binding, address);
                return Tuple.Create(factory.CreateChannel(), factory);
            }

            /*
                    var callback = new EncoderCallProxy();
                    var cf = new DuplexChannelFactory<MLRealtimeEncoderLib.IMLRealtimeEncoderStatusMonitorService>(new InstanceContext(callback));
                    var binding = new WSDualHttpBinding();
                    binding.ClientBaseAddress = new Uri("http://127.0.0.1:11766/");
                    cf.Endpoint.Binding = binding;
                    cf.Endpoint.Address = new EndpointAddress($"http://127.0.0.1:11756/");
                    _configService = cf.CreateChannel();
                    ((IClientChannel)_configService).OperationTimeout = TimeSpan.FromMilliseconds(5000);
                    MLEncodersMgr.Instance.MEncoderService = _configService;
                    MLEncodersMgr.Instance.EncoderProxy = callback;

             */
        }
    }
}

/* WCF - Windows Communication Foundation
 *
    System.ServiceModel
    合并了ASP.NetWeb服务、.Net Remoting、消息队列、Enterprise Services

    WCF支持的通信协议

    SOAP 协议
    Sample Object Access Protocol
    可发送XML架构定义消息
    包含信封：标题、正文

    WSDL 协议
    Web Service Description Language

    REST

    JSON

    Address
    EndpointAddress

    Binding
    	Name
    	NameSpace
    	Binding Element
    		Encoding Binding Element :描述数据的编码方式；
    		Transport Binding Element ：描述数据的传输方式；
    		Protocol Binding Element :指定安全性 可靠性 上下文流设置

    消息交换模式
    	One-Way Calls
    	Request/Reply
    	Duplex

    步骤：
    1.定义契约（协定）：

    	四种主要协定：
    	a.服务协定：指明该类（结构）或方法可被远程调用
    		Service Contract 包含：ServiceContract（用于类与结构体）与OperationContract（用于方法）
    	b.数据协定：指明该类或者属性可以被远程序列化并传输
    		DataContract（用于类或者结构体）、MemberContract（用于属性或者字段）
    	c.异常协定：自定义异常消息格式
    		FaultContract
    	d.消息协定：自定义消息结构
    		指定消息头、消息体、是否对内容进行加密或签名；

    1.服务创建

    	定义协定 实现服务
    	1.1 定义协定 - 接口
    	1.2 定义实现 - 服务行为
    	1.3 定义绑定 - 数据传输

    2.寄宿发布服务

    	定义宿主 开启服务
    	配置服务config
    	2.1 寄宿到Windows服务；
    	2.2 寄宿到IIS；
    	2.3 寄宿到应用程序等；
    	3.4 服务发布（运行）

    3.客户端获取服务

    	客户端需要使用代理访问服务
    	创建代理三种方式：
    	3.1 VS添加服务引用  -  自动创建代理类
    	3.2 ServiceModel 元数据实用工具（Svcutil.exe） -  使用该工具创建代理类型
    	3.3 ChannelFactory类

    	Mark： WSDL文档 （由MEX端点创建，由服务配置）

    		使用元数据：
    		共享类型 ：
    			代理类派生在基类ClientBase<TChannel> 或者使用ChannelFactory<TChannel>
    			需要绑定和端点

    4.双工通信

    	必须指定在客户端实现的协议
    	并发与死锁
    	ConcurrencyMode
    	OperationContext类
    	OperationContext.Current返回与客户端中当前请求关联的OperationContext
    	服务端回调 获取回调信道

    	客户端:
    		1.启动与服务端连接：不能使用ChannelFactory 需使用 DuplexChannelFactory类
    			eg: var context =new InstanceContext(TCallBack);
    				var factory = new DuplexChannelFactory<TChannel>(context,Binding,Address);

    	路由
    		SOAP协议的高级特性
    		RouterService
     */