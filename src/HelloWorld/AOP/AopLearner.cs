using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Interception.PolicyInjection.Pipeline;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Unity.Interception.PolicyInjection.Policies;

namespace HelloWorld.AOP
{
    public class TCP_IP
    {
        public int LayerCount = 4;
        public string ApplicationLayer { get; set; }
        public string TransportLayer { get; set; }
        public string NetLayer { get; set; }
        public string LinkLayer { get; set; }

    }

    public class DescriptionHandler : ICallHandler
    {
        public string Text { get; set; }
        public int Order { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            Trace.TraceInformation(Text);
            //打印参数
            var sb = new StringBuilder();
            var paras = input.Inputs;
            for (int i = 0; i < paras.Count; i++)
            {
                sb.AppendLine("Type:" + paras[i].GetType().Name + ",Value:" + paras[i].ToString());
            }
            Trace.TraceInformation(sb.ToString());
            //Before Method Excute 
            Trace.TraceInformation("Before Method Excute");
            //Excute Method
            var msg = getNext()(input,getNext);
            //After Method Excuted 
            Trace.TraceInformation("After Method Excuted");
            return msg;
        }
    }

    public class DescriptionContainer : IUnityContainer
    {
        public IEnumerable<IContainerRegistration> Registrations => throw new NotImplementedException();

        public IUnityContainer Parent => throw new NotImplementedException();

        public object BuildUp(Type type, object existing, string name, params ResolverOverride[] overrides)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer CreateChildContainer()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type type, string name)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterFactory(Type type, string name, Func<IUnityContainer, Type, string, object> factory, IFactoryLifetimeManager lifetimeManager)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterInstance(Type type, string name, object instance, IInstanceLifetimeManager lifetimeManager)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType(Type registeredType, Type mappedToType, string name, ITypeLifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type type, string name, params ResolverOverride[] overrides)
        {
            throw new NotImplementedException();
        }
    }

    public class DescriptionAttribute : HandlerAttribute
    {
        public string Text { get; set; }
        public int Order { get; set; }


        public override ICallHandler CreateHandler(Unity.IUnityContainer container)
        {
            return new DescriptionHandler() { Text = Text, Order = Order };
        }
    }

    public interface IUserTCP_IP
    {
        void SendMsg(TCP_IP ip, string msg, int len);
        void ReceiveMsg(TCP_IP ip, string msg,out int len);
    }

    public class MyPC : MarshalByRefObject, IUserTCP_IP
    {
        private static MyPC _PC = null;
        public static MyPC GetInstance()
        {
            if (_PC == null)
            {
                _PC = PolicyInjection.Create<MyPC>();
            }
            return _PC;
        }
        public MyPC()
        {

        }

        [Description(Text = "ReceiveMsg")]
        public void ReceiveMsg(TCP_IP ip, string msg, out int len)
        {
            len = (int)(Math.E * 1000);
            Trace.TraceInformation("Receive Msg:" + msg + ",Len:" + len.ToString());
        }

        [Description(Text = "SendMsg")]
        public void SendMsg(TCP_IP ip, string msg, int len)
        {
            Trace.TraceInformation("SendMsg Msg:" + msg + ",Len:" + len.ToString());
        }
    }

    [TestClass]
    public class AopLearner
    {
        [TestMethod]
        public void Main()
        {
            var ip1 = new TCP_IP()
            {
                LayerCount = 4,
                ApplicationLayer = "http,smtp,rtp",
                TransportLayer = "tcp,udp",
                NetLayer = "none",
                LinkLayer = "none",
            };
            var pc = MyPC.GetInstance();
            pc.SendMsg(ip1, "hello", 200);
            int len;
            pc.ReceiveMsg(ip1, "world", out len);
        }
    }
}
