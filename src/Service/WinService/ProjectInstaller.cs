using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace WService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        //
        // 摘要:
        //     在派生类中重写时，完成安装事务。
        //
        // 参数:
        //   savedState:
        //     System.Collections.IDictionary 包含在集合中所有安装程序都运行后的计算机的状态。
        //
        // 异常:
        //   T:System.ArgumentException:
        //     savedState 参数为 null。- 或 - 保存的状态 System.Collections.IDictionary 可能已损坏。
        //
        //   T:System.Configuration.Install.InstallException:
        //     在安装的 System.Configuration.Install.Installer.Commit(System.Collections.IDictionary)
        //     阶段发生异常。忽略该异常，安装继续进行。但是在安装完成后，应用程序可能无法正常工作。
        public override void Commit(IDictionary savedState)
        {
            
        }

        //
        // 摘要:
        //     在派生类中被重写时，执行安装。
        //
        // 参数:
        //   stateSaver:
        //     System.Collections.IDictionary 用于保存执行提交、回滚或卸载操作所需的信息。
        //
        // 异常:
        //   T:System.ArgumentException:
        //     stateSaver 参数为 null。
        //
        //   T:System.Exception:
        //     该集合的一个安装程序的 System.Configuration.Install.Installer.BeforeInstall 事件处理程序发生异常。-
        //     或 - 该集合的一个安装程序的 System.Configuration.Install.Installer.AfterInstall 事件处理程序发生异常。
        public override void Install(IDictionary stateSaver)
        {
            
        }

        protected override void OnBeforeRollback(IDictionary savedState)
        {
            //base.OnBeforeRollback(savedState);
        }


        //
        // 摘要:
        //     在派生类中重写时，还原计算机的安装前状态。
        //
        // 参数:
        //   savedState:
        //     System.Collections.IDictionary 包含计算机的安装前状态。
        //
        // 异常:
        //   T:System.ArgumentException:
        //     savedState 参数为 null。- 或 - 保存的状态 System.Collections.IDictionary 可能已损坏。
        //
        //   T:System.Configuration.Install.InstallException:
        //     在安装的 System.Configuration.Install.Installer.Rollback(System.Collections.IDictionary)
        //     阶段发生异常。忽略该异常，回滚继续进行。但是，回滚完成后计算机可能无法完全还原为其初始状态。
        public override void Rollback(IDictionary savedState)
        {
            
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            //base.OnBeforeUninstall(savedState);
        }

        //
        // 摘要:
        //     在派生类中重写时，移除安装。
        //
        // 参数:
        //   savedState:
        //     System.Collections.IDictionary 包含安装完成后计算机的状态。
        //
        // 异常:
        //   T:System.ArgumentException:
        //     保存的状态 System.Collections.IDictionary 可能已损坏。
        //
        //   T:System.Configuration.Install.InstallException:
        //     卸载时发生异常。忽略该异常，卸载继续进行。但是，卸载完成后应用程序可能未完全卸载。
        public virtual void Uninstall(IDictionary savedState)
        {
            
        }

    }
}
