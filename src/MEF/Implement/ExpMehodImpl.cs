using IContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implement
{
    [Export("Implement.")]
    public class ExpMehodImpl
    {
        public string GetStr()
        {
            return "Export Function GetStr() ";
        }

        [Export("Add", typeof(Func<int, int, int>))]
        [ExportMetadata("n1", "v1")]
        public int Add(int a, int b)
        {
            return a + b;
        }

        [Export("RefeObj", typeof(Action<string, string>))]
        [ExportMetadata("n2", "v2")]
        public void RefeObj(string s1, string s2)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("RefeStr:{0},{1}", s1, s2));
        }

        [MetaData("Subtract", typeof(Func<double, double, double>), VData = "VX")]
        public double Subtract(double d1, double d2)
        {
            return d1 - d2;
        }
    }
}
