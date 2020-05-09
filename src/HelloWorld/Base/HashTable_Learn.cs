using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Base
{
    public class HashTable_Learn
    {
        public void Main()
        {
            var table = new Hashtable();
            int a = 99;
            int b = 88;

            a = a ^ b;
            b = a ^ b;
            a = a ^ b;

        }
    }
}
