using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace UnitTestProject
{
    [TestClass]
    public class TestMain
    {
        [TestMethod]
        public void TestITarget()
        {
            //创建mock对象 指定要模拟的接口
            var mo = new Mock<ITarget>(MockBehavior.Strict);
            //指定模拟方法
            mo.Setup(t => t.SomeMethod());

            //指定任何字符串时返回 true
            //mo.Setup(t => t.SomeMethod(It.IsAny<string>())).Returns(true);

            //根据传入的参数 确定返回值
            mo.Setup(t => t.SomeMethod(It.IsAny<string>())).Returns((string s) => s != null && (s.Length % 2 == 1));
            //指定参数为"no"时，返回false
            mo.Setup(t => t.SomeMethod("no")).Returns(true);
            //指定参数为""时，抛出异常ArgumentException
            mo.Setup(t => t.SomeMethod("")).Throws(new ArgumentException("Moq Exp"));

            int a = 1;
            mo.Setup(t => t.SomeMethod(out a)).Returns(false);
            int b, c, d;

            string str=It.IsAny<string>();
            mo.Setup(t => t.SomeMethod(ref str)).Returns(true);


            Assert.AreEqual(true, mo.Object.SomeMethod("no"));

            //string p1 = "121";
            //var tk = mo.Object.SomeMethod(ref p1);
            //Assert.AreEqual(true, tk);

            //Assert.AreEqual(true, mo.Object.SomeMethod(""));
            //Assert.ThrowsException<Exception>(()=>mo.Object.SomeMethod("ok"));
        }
    }
}
