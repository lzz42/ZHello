using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContract;

namespace Implement
{
    public class ByteOperator : IOperator
    {
        public string Name { get; set; }

        public int NumberOperands { get; set; }
    }
}
