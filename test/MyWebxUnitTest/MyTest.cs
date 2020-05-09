using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebxUnitTest
{
    public class MyTest
    {
        public bool IsNum(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            if(int.TryParse(value,out  int v))
            {
                return false;
            }
            return true;
        }
    }
}
