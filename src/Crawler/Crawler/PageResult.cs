using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Crawler
{
    public abstract class PageResult
    {
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
