using System;
using Xunit;

namespace MyWebxUnitTest
{
    public class UnitTest1
    {
        private MyTest test;
        public UnitTest1()
        {
            test = new MyTest();
        }

        [Fact]
        public void Test1()
        {
            Assert.True(test.IsNum("1"));
        }

        [Fact]
        public void Test2()
        {
            Assert.True(test.IsNum("11a"));
        }
        [Theory]
        [InlineData("-23222")]
        [InlineData("-23222x")]
        [InlineData("-23222kk")]
        public void Test3(string value)
        {
            Assert.True(test.IsNum(value));
        }
    }
}
