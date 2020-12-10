using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZHello.Base;

namespace ZHello.Test.Common
{
    [TestClass]
    public class DbgHelper
    {

        [TestMethod]
        public void Main2()
        {

            var obj = new CallInfo();
            var ty = obj.GetType();
            var mtoken = ty.MetadataToken;

            //var pdb = DbgHelper.PDB;
            //var addr = pdb.LoadModuleEx(ty.Assembly.CodeBase);
            //var m1 = pdb.EnumModules();
            //var m2 = pdb.EnumSymbols(addr);
            //var m3 = pdb.EnumTypes(addr);
            //var m4 = pdb.EnumSourceFiles(addr);


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