using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHello.Test.Algorithm
{
    [TestClass]
    public class Alg_Peterson
    {
        [TestMethod]
        public void Peterson_Two_Test()
        {
            var al = new ZHello.Algorithm.Alg_Peterson();
            var t1 = new Task(() =>
            {
                Thread.Sleep(271);
                al.Work_Safe(0);
            });
            var t2 = new Task(() =>
            {
                Thread.Sleep(170);
                al.Work_Safe(1);
            });
            t1.Start();
            t2.Start();
            new Form().ShowDialog();
        }
    }
}