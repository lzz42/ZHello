using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHello.Test.Cecil
{
    [TestClass]
    public class CSScript_Test
    {
        [TestMethod]
        public void Main()
        {
            var obj = new ZHello.Cecil.CS_Script();
            obj.Main();
        }
    }
}
