using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelloWorld.Common;
using HelloWorld.Base;
using System.Diagnostics;

namespace HelloWorld.Test.Common
{
    [TestClass]
    public class DbgHelper_Test
    {
        [TestMethod]
        public void Ma()
        {
            var obj = new CallInfo();
            var ty = obj.GetType();
            var mtoken = ty.MetadataToken;
            var pdb = DbgHelper.PDB;
            var addr = pdb.LoadModuleEx(ty.Assembly.CodeBase);
            var m1 = pdb.EnumModules();
            var m2 = pdb.EnumSymbols(addr);
            var m3 = pdb.EnumTypes(addr);
            var m4 = pdb.EnumSourceFiles(addr);
        }

        [TestMethod]
        public void Main2()
        {
            int a = 99;
            int b = 88;
            int c = a ^ b ^ a;
            Trace.WriteLine(string.Format("C {0}", c));
            Trace.WriteLine(string.Format("{0}\t {1}", a, b));
            a = a ^ b;
            b = a ^ b;
            a = a ^ b;
            Trace.WriteLine(string.Format("{0}\t {1}", a, b));
        }

    }
}
