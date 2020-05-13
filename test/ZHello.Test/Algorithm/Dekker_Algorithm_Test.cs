using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZHello.Algorithm;

namespace ZHello.Test.Algorithm
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