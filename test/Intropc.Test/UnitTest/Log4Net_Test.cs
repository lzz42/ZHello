using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log;
using Moq;

namespace UnitTestProject.UnitTest_Log
{
    //[TestClass]
    public class Log4Net_Test
    {
        //[TestMethod]
        public void MainTest()
        {
            var mo = new Mock<ILog>();
            mo.Setup(l => l.Debug(new Exception("Moq Test", null), null)).Verifiable();
        }
    }
}
