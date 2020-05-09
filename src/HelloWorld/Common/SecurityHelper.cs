using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace ZHello.Common
{
    public class SecurityHelper
    {
        public static void SecurityTest()
        {
            PermissionSet permissionSet = new PermissionSet(PermissionState.None);
            PermissionSet permissionSet2 = new PermissionSet(PermissionState.Unrestricted);
            Evidence evidence = new Evidence();
        }
    }
}