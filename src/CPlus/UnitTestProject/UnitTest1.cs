using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            HelloWorld_Introp.Main();
        }
    
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            HelloWorld_Introp.Main();
        }
    }

}
