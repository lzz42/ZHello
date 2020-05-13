using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZHello.Algorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHello.Test.Algorithm
{
    [TestClass]
    public class Peterson_Algorithm_Test
    {
        [TestMethod]
        public void Peterson_Two_Test()
        {
            var al = new Alg_Peterson();
            var t1 = new Task(() =>
            {
                Thread.Sleep(271);
                al.Visit_Safe(0);
            });
            var t2 = new Task(() =>
            {
                Thread.Sleep(170);
                al.Visit_Safe(1);
            });
            t1.Start();
            t2.Start();
            new Form().ShowDialog();
        }
    }
}
