using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Crawler
{
    internal interface ICrawl
    {
        void SendRequest(string url, out Stream stream, out PageInfo info);
        void ProcessStream(Stream stream, PageInfo info, out string html);
        void ProcessData(string html);

        bool SendRequest(string url, out Stream stream);
        bool ProcessStream(Stream stream, out PageResult result);
        bool ProcessResult(IList<PageResult> result);
    }
}
