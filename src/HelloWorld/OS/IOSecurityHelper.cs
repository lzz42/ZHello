using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;

namespace ZHello.OS
{
    public class IOSecurityHelper
    {
        /// <summary>
        /// 创建具有完全访问权限的文件夹
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static void CreateFullControlFolder(string dirPath)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath).Attributes = FileAttributes.Normal;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Create Folder Error:{0}", ex.Message);
            }
        }

        /// <summary>
        ///为文件夹添加users，everyone用户组的完全控制权限
        /// </summary>
        /// <param name="dirPath"></param>
        public static void AddSecurityControllFolder(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                return;
            }
            //获取文件夹信息
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            //获得该文件夹的所有访问权限
            System.Security.AccessControl.DirectorySecurity dirSecurity = dir.GetAccessControl(AccessControlSections.All);
            //设定文件ACL继承
            //InheritanceFlags inherits = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            ////添加ereryone用户组的访问权限规则 完全控制权限
            //FileSystemAccessRule everyoneFileSystemAccessRule = new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            ////添加Users用户组的访问权限规则 完全控制权限
            //FileSystemAccessRule usersFileSystemAccessRule = new FileSystemAccessRule("Users", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            //bool isModified = false;
            //dirSecurity.ModifyAccessRule(AccessControlModification.Add, everyoneFileSystemAccessRule, out isModified); 
            //dirSecurity.ModifyAccessRule(AccessControlModification.Add, usersFileSystemAccessRule, out isModified);
            //设置访问权限
            dir.SetAccessControl(dirSecurity);
        }

        public static bool HasRWAccessControl(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                return false;
            }
            bool hasR = false;
            bool hasW = false;
            var acc = Directory.GetAccessControl(dirPath,AccessControlSections.All);
            var collection = acc.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
            foreach (var rule in collection)
	        {
                if (rule is FileSystemAccessRule)
                {
                    if ((rule as FileSystemAccessRule).FileSystemRights == FileSystemRights.Read)
                    {
                        hasR = true;
                    }
                    else if ((rule as FileSystemAccessRule).FileSystemRights == FileSystemRights.Write)
                    {
                        hasW = true;
                    }
                }
	        }
            return hasR && hasW;
        }
    }

    /*  Chapter 11 文件系统实现

    1.文件系统结构
        
        应用程序 
        ---> 高层文件高级操作
        逻辑文件系统 
        ---> 管理元数据
        文件组织系统
        ---> 将文件逻辑块转换为物理地址
        基本文件系统
        --->对磁盘物理块进行读写操作
        IO控制
        --->底层设备驱动+中断以及通信
        设备

        文件系统的两个设计问题：
            1.如何定义文件系统对用户的接口 - 定义文件、文件属性、文件操作
            2.创建数据结构和算法将逻辑文件系统映射到物理外存设备
        分层设计： 每层利用较低层的功能创建新功能为更高称服务
            设备-》I/O控制-》基本文件系统-》文件组织系统-》逻辑文件系统-》应用程序

        I/O控制为最底层 ： 设备驱动程序 + 中断处理程序 =》内存与磁盘间通信
        基本文件系统 ： 向设备驱动程序发送命令对磁盘上的物理块进行读写（使用实际物理块地址：驱动器 柱面cylinder、磁道 track 扇区sector）
        文件组织模块 file-organization module ： 联系逻辑块与物理块，将逻辑块地址转换成基本文件系统使用的物理块地址
        逻辑文件系统 管理元数据 - 包括文件系统所有的结构数据 通过文件控制块维护文件结构 ；负责保护和安全
            文件控制块 file control block FCB 包含文件信息

        Unix 使用Unix文件系统 UFS - 基于伯克利快速文件系统FFS
        Linux 标准文件系统 可扩展文件系统 extended file system  - ext2 ext3

    2.文件系统实现
        引导控制块 boot control block  - 系统从该卷引导操作系统所需的信息：UFS 称为引导快 NTFS 称为 分区引导扇区
        卷控制块 volume control block - 包括卷或分区的详细信息 ：分区块数 块大小 空闲块和FCB的数量和指针等，UFS - 超级块，NTFS - 存储在主控文件表中 Master File Table
        每个文件系统的目录结构用来组织文件：UFS：目录结构包含文件名和索引节点，NTFS：主控文件表
        每个文件的FCB包含文件的很多详细信息，UFS：称为索引节点-inode，NTFS：该信息存储在主控文件表中
        内存信息关于文件系统部分：
            a.一个安装表：包括所有的安装卷信息
            b.一个目录缓存表：保存最近访问的目录信息
            c.系统范围内打开的文件表：每个打开文件的FCB和其他信息（打开指定文件的进程数量）
            d.单个进程的打开文件表：一个指向系统打开文件表的指针和其他信息（文件当前位置指针，文件打开模式）
        创建新文件：
            1.应用程序调用逻辑文件系统；
            2.逻辑系统分配FCB；
            3.目录信息读入内存；
            4.新文件名更新目录和FCB；
            5.结果写回磁盘；
        打开文件：
            1.调用open()将文件名传递给文件系统；
            2.系统调用open()搜索系统范围打开文件表：确定该文件是否被其他进程使用；
            3.被使用：在单个进程打开文件表创建一项，并指向系统范围打开文件表
            4.未被使用：根据给定文件名搜索目录结构，找到文件后，其FCB复制到系统范围打开的文件表，并存储进程信息
            5.单个进程打开文件表新增一个条目
            6.返回一个指向单个进程的打开文件表中合适条目的指针，之后的所有操作都通过该指针进行：
            访问打开文件表的索引：Unix——文件描述符，windows——文件句柄
        关闭文件：
            1.单个进程打开文件表删除一个条目；
            2.系统范围打开文件表相应条目打开数递减
            3.若所有用户都关闭一个文件时，更新的文件元数据复制到磁盘的目录结构，系统范围打开文件表的相应条目将删除
        文件系统的缓存
              

    分区与安装
        分区： 
        生分区(raw)：没有文件系统
        熟分区(cooked)：有文件系统
        引导信息：通常为一组有序块，并作为镜像文件读入内存，该镜像文件按照预先指定的位置开始执行
        根分区(root partition）:包括操作系统内核或其他系统文件，在引导时装入内存
    虚拟文件系统
        解决问题：
        1.操作系统整合多个文件系统为一个目录结构
        2.访问文件系统空间时，在文件系统间无缝移动
        数据结构 子程序 分开基本系统调用功能和实现细节
        三个层次
            1。文件系统接口 open()/read()/write()/close()/文件描述符
            2。虚拟文件系统VFS
                定义VFS接口，将文件系统通用操作与具体实现分开
                提供网络唯一标识文件机制（vnode文件表示结构）
            3。实现文件系统类型或远程文件系统协议
        Linux VFS结构
            索引节点对象 iNode object 
            文件对象 File object
            超级块对象 superblock object
            目录条目对象 dentry object
    保护
        访问控制-每个文件和目录都增加一个访问控制列表ACL-Access Control list
    

     */
}
