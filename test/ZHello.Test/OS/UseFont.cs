using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHello.Test.OS
{
    [TestClass]
    public class UseFont
    {
        [TestMethod]
        public void ExistFont()
        {
            var f1 = "微软雅黑";
            var f2 = "微软雅黑hhh";
            var r1 = ZHello.OS.UseFont.ExistFont(f1);
            Assert.IsTrue(r1);
            var r2 = ZHello.OS.UseFont.ExistFont(f2);
            Assert.IsFalse(r2);
        }

        [TestMethod]
        public void InstallFont()
        {
            var file = "娃娃体W5-GB.ttc";
            var name = "娃娃体W5-GB";
            Assert.IsFalse(ZHello.OS.UseFont.ExistFont(name));
            Assert.IsTrue(ZHello.OS.UseFont.InstallFont(file, name));
            Assert.IsTrue(ZHello.OS.UseFont.ExistFont(name));
            Assert.IsTrue(ZHello.OS.UseFont.UninstallFont(name, file));
        }

        [TestMethod]
        public void UninstallFont()
        {
            var file = "娃娃体W5-GB.ttc";
            var name = "娃娃体W5-GB";
            Assert.IsTrue(ZHello.OS.UseFont.ExistFont(name));
            Assert.IsTrue(ZHello.OS.UseFont.UninstallFont(name, file));
            Assert.IsFalse(ZHello.OS.UseFont.ExistFont(name));
        }
    }
}