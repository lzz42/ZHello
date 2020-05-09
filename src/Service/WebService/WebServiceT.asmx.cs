using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Services;

namespace WebService
{
    /// <summary>
    /// WebServiceT 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class WebServiceT : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        private void PrintCurrentThreadPrincipal()
        {
            var principal = (WindowsPrincipal)System.Threading.Thread.CurrentPrincipal;
            var iden = (WindowsIdentity)principal.Identity;
            var sbuilder = new StringBuilder();
            sbuilder.AppendFormat("IdentityType:{0};Name:{2};'User'?:{3};", iden.ToString(), iden.Name, principal.IsInRole(WindowsBuiltInRole.User));
            sbuilder.AppendFormat("'Administrator'?:{0};", principal.IsInRole(WindowsBuiltInRole.Administrator));
            sbuilder.AppendFormat("Authenticated:{0};", iden.IsAuthenticated);
            sbuilder.AppendFormat("AuthType:{0};", iden.AuthenticationType);
            sbuilder.AppendFormat("Anonymous?{0};", iden.IsAnonymous);
            sbuilder.AppendFormat("Token:{0};", iden.Token);
            Trace.WriteLine(sbuilder.ToString());
        }

        [WebMethod]
        public bool ExistsProc(string proc)
        {
            return Methods.ExistsProc(proc);
        }

        [WebMethod]
        public bool KillProc(string proc)
        {
            return Methods.ForeStopProc(proc);
        }

        [WebMethod]
        public bool StartProc(string proc)
        {
            return Methods.StartProc(proc, null, false);
        }

    }

    public class Methods
    {
        public static bool ExistsProc(string proc)
        {
            Process[] procs = Process.GetProcesses(); //取得所有进程 
            for (int i = 0; i < procs.Length; i++)
            {
                if (procs[i].ProcessName == proc)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ForeStopProc(string proc)
        {
            try
            {
                Process[] procs = Process.GetProcesses(); //取得所有进程 
                for (int i = 0; i < procs.Length; i++)
                {
                    if (procs[i].ProcessName == proc)
                    {
                        procs[i].Kill();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public static Process StartProc(string proc, string args)
        {
            var info = new ProcessStartInfo();
            info.FileName = proc;
            info.Arguments = args;
            info.Verb = "runas";
            var p = Process.Start(info);
            return p;
        }

        public static bool StartProc(string proc, string args, bool needUAC)
        {
            try
            {
                var info = new ProcessStartInfo();
                info.FileName = proc;
                info.CreateNoWindow = false;
                info.UseShellExecute = true;
                info.WindowStyle = ProcessWindowStyle.Normal;
                if (args != null)
                    info.Arguments = args;
                if (needUAC)
                {
                    info.Verb = "runas";
                }
                Process.Start(info);
                return true;
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message);
            }
            return false;
        }

    }

}
