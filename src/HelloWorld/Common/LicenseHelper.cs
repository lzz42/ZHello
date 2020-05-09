using System;
using System.Collections;
using System.ComponentModel;

namespace ZHello.Common
{
    //[LicenseProvider(typeof(LicFileLicenseProvider))]
    [LicenseProvider(typeof(MyLicenseProvider))]
    public class MyLicenseControl : IDisposable
    {
        private License _license = null;

        public MyLicenseControl()
        {
            _license = LicenseManager.Validate(typeof(MyLicenseControl), this);
        }

        public void Dispose()
        {
            _license.Dispose();
        }
    }

    public class MyLicenseProvider : LicenseProvider
    {
        private static Hashtable keys = new Hashtable();

        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            switch (context.UsageMode)
            {
                case LicenseUsageMode.Runtime:
                    {
                        System.Diagnostics.Trace.TraceWarning("在运行时执行License检查");
                    }
                    break;

                case LicenseUsageMode.Designtime:
                    {
                        System.Diagnostics.Trace.TraceWarning("在设计时或编译时执行License检查");
                    }
                    break;

                default:
                    break;
            }
            var savedKey = context.GetSavedLicenseKey(type, null);
            System.Diagnostics.Trace.TraceWarning("LicenseSavedKey::" + savedKey);
            if (string.IsNullOrEmpty(savedKey))
            {
                var key = "MyKey";
                context.SetSavedLicenseKey(type, key);
                savedKey = key;
            }
            return new MyLicense(savedKey);
        }
    }

    public class MyLicense : License
    {
        public override string LicenseKey { get; }

        public MyLicense(string key)
        {
            LicenseKey = key;
        }

        public override void Dispose()
        {
        }
    }

    /* 授予组件和控件许可权限
     1.在设计时进行验证；

     */
}