using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHello.Test.Algorithm
{
    [TestClass]
    public class Alg_Dekker
    {
        [TestMethod]
        public void Dekker_Two()
        {
            ZHello.Algorithm.Alg_Dekker.Run_Dekker_Two();
            var form = new Form();
            form.ShowDialog();
        }
    }
}