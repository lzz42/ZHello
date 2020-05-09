using System;
using System.Reflection;
using System.Runtime.Remoting;

namespace ZHello.CLR
{
    public class AppDomainHelper
    {
        [LoaderOptimization(5)]
        public static bool CreateDomain(string name, string dir)
        {
            var applicationIdentity = new ApplicationIdentity("AppFullName");
            var activationContext = ActivationContext.CreatePartialActivationContext(applicationIdentity, null);
            var setup = new AppDomainSetup(activationContext);
            //
            setup.ApplicationName = "AppName";
            setup.ApplicationBase = dir;
            setup.DynamicBase = dir;
            setup.PrivateBinPath = dir;
            setup.DisallowApplicationBaseProbing = true;
            var domain = AppDomain.CreateDomain(name, null, setup, null);

            return false;
        }

        public static bool LoadAssembly(AppDomain domain, string dll)
        {
            var name = AssemblyName.GetAssemblyName(dll);
            domain.Load(name);

            return false;
        }

        public static void Calling()
        {
            var domain = AppDomain.CreateDomain("A1 #1", null);
            //load a assembly
            MarshalByRefType mbrt = null;

            //得到代理引用 按引用封送
            mbrt = (MarshalByRefType)domain.CreateInstanceAndUnwrap("assembly.FullName", "MarshalByRefType");
            Console.WriteLine("Is TransparentProxy:{0}", RemotingServices.IsTransparentProxy(mbrt));

            //得到按引用封送的结构对象
            //var mbvst = mbrt.GetMarshalByValStructType();
            //得到按值封送的类对象
            //var mbvt = mbrt.GetMarshalByValType();

            //无法得到不可封送类型
            //var nmt = mbrt.GetNonMarshalType();

            AppDomainManager mgr = new AppDomainManager();
        }
    }

    public class DomainMgr
    {
        private static AppDomainManager mDomainManager;

        public static AppDomainSetup CreateMyAppDomainSetup()
        {
            var setup = new AppDomainSetup();
            setup.AppDomainManagerAssembly = "MyAppDomainAssembly Strong Name string";
            setup.AppDomainManagerType = "MyAppDomainManager Full Name";
            return setup;
        }
    }

    public class MyAppDomainManager : AppDomainManager
    {
    }

    #region Marhal 封送类

    public class MarshalByRefType : MarshalByRefObject
    {
        /// <summary>
        /// 修改对象的租期时间
        /// </summary>
        /// <returns></returns>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand, Flags = System.Security.Permissions.SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            System.Runtime.Remoting.Lifetime.ILease lease = (System.Runtime.Remoting.Lifetime.ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == System.Runtime.Remoting.Lifetime.LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromMinutes(1);
                lease.SponsorshipTimeout = TimeSpan.FromMinutes(2);
                lease.RenewOnCallTime = TimeSpan.FromSeconds(2);
            }
            return lease;
        }

        public string GetDateTimeStr()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }
    }

    [Serializable]
    public class MarshalByValType
    {
    }

    [Serializable]
    public class MarshalByValStructType
    {
        public string GetDateTimeStr()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }
    }

    public class NonMarshalType
    {
        public string GetDateTimeStr()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }
    }

    public class MarshalByRefTypeEx : MarshalByRefObject
    {
        public MarshalByValType GetMarshalByValType()
        {
            return new MarshalByValType();
        }

        public MarshalByValStructType GetMarshalByValStructType()
        {
            return new MarshalByValStructType();
        }

        public NonMarshalType GetNonMarshalType()
        {
            return new NonMarshalType();
        }
    }

    #endregion Marhal 封送类
}