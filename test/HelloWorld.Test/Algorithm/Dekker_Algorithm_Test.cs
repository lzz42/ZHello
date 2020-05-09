using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HelloWorld.Algorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloWorld.Test.Algorithm
{
    [TestClass]
    public class Dekker_Algorithm_Test
    {
        [TestMethod]
        public void Dekker_Two_Test()
        {
            Alg_Dekker.Run_Dekker_Two();
            var form = new Form();
            form.ShowDialog();
        }
    }
}
