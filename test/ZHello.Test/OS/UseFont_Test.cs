using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZHello.OS;

namespace ZHello.Test.OS
{
    [TestClass]
    public class UseFont_Test
    {
        [TestMethod]
        public void ExistFont()
        {
            var f1 = "微软雅黑";
            var f2 = "微软雅黑hhh";
            var r1 = UseFont.ExistFont(f1);
            Assert.IsTrue(r1);
            var r2 = UseFont.ExistFont(f2);
            Assert.IsFalse(r2);
        }

        [TestMethod]
        public void InstallFont()
        {
            var file = "娃娃体W5-GB.ttc";
            var name = "娃娃体W5-GB";
            Assert.IsFalse(UseFont.ExistFont(name));
            Assert.IsTrue(UseFont.InstallFont(file, name));
            Assert.IsTrue(UseFont.ExistFont(name));
            Assert.IsTrue(UseFont.UninstallFont(name, file));
        }

        [TestMethod]
        public void UninstallFont()
        {
            var file = "娃娃体W5-GB.ttc";
            var name = "娃娃体W5-GB";
            Assert.IsTrue(UseFont.ExistFont(name));
            Assert.IsTrue(UseFont.UninstallFont(name, file));
            Assert.IsFalse(UseFont.ExistFont(name));
            //Assert.IsTrue(UseFont.InstallFont(file, name));
        }

    }
}
