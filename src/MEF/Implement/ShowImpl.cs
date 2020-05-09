using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContract;

namespace Implement
{
    public class ShowImpl : IShow
    {
        public string Show(int a)
        {
            return string.Format("SHow::{0}", a);
        }

        public string Show(int a, int b)
        {
            return string.Format("Show::{0};::{1}", a, b);
        }
    }
}
