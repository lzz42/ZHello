using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZHello.Net;

namespace ZHello.Test.Net
{
    [TestClass]
    public class OpFtp_Test
    {
        private OpFtp Ftp;
        [TestInitialize]
        public void Init()
        {
            Ftp = OpFtp.CreateFtpServer("ftp://10.0.50.143:18820/", "", "ftpuser", "f");
        }

        [TestMethod]
        public void Upload()
        {
            var f1 = "D:\\ZHello.exe";
            var r1 = Ftp.Upload(f1);
            Assert.IsTrue(r1);
        }

        [TestMethod]

        public void Download()
        {
            var f21 = "D:\\ZHello.exe01.pdf";
            var f22 = "D:\\ZHello.exe02.wav";
            var f31 = "D:\\ZHello.exe03.pdf";
            var f32 = "D:\\ZHello.exe04.pdf";
            var ftp21 = "The Little Redis Book.pdf";
            var ftp22 = "人生最美初见时.wav";
            var ftp31 = "新建文件夹//django.pdf";
            var ftp32 = "新建文件夹//C++预处理命令参考.pdf";
            Assert.IsTrue(Ftp.Download(f21, ftp21));
            Assert.IsTrue(Ftp.Download(f22, ftp22));
            Assert.IsTrue(Ftp.Download(f31, ftp31));
            Assert.IsTrue(Ftp.Download(f32, ftp32));
        }

        [TestMethod]
        public void ListInfos()
        {
            var lists1 = new List<string>();
            var lists2 = new List<string>();
            Assert.IsTrue(Ftp.GetCurrentDirList(out lists1));
            Assert.IsTrue(Ftp.GetCurrentDirDetialList(out lists2));
        }

        [TestMethod]
        public void GetFileSize()
        {
            var ftp21 = "The Little Redis Book.pdf";
            var ftp22 = "人生最美初见时.wav";
            var ftp31 = "新建文件夹//django.pdf";
            var ftp32 = "新建文件夹//C++预处理命令参考.pdf";
            var ftp41 = "新建文件夹";

            long s1, s2, s3, s4,s5;
            Assert.IsTrue(Ftp.GetFileSize(ftp21, out s1));
            Assert.IsTrue(Ftp.GetFileSize(ftp22, out s2));
            Assert.IsTrue(Ftp.GetFileSize(ftp31, out s3));
            Assert.IsTrue(Ftp.GetFileSize(ftp32, out s4));
            //Assert.IsTrue(Ftp.GetFileSize(ftp41, out s5));

            Assert.IsTrue(Ftp.Exist(ftp21));
            Assert.IsTrue(Ftp.Exist(ftp41));
        }

    }
}
