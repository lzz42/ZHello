using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Crawler.Crawler
{
    internal abstract class Crawler : ICrawl
    {
        public string Url { get; protected set; }

        public EventHandler OnDone { get; set; }

        protected CrawlerConfig Config { get; set; }

        /// <summary>
        /// 分析目录页面
        /// </summary>
        public AnalyzeUrlsFn MUlrsFn { get; protected set; }

        /// <summary>
        /// 分析单个页面
        /// </summary>
        public AnalyzePageFn MFunction { get; protected set; }

        protected HttpWebRequest req;
        protected HttpWebResponse resp;
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        /// <param name="info"></param>
        public void SendRequest(string url, out Stream stream, out PageInfo info)
        {
            info = new PageInfo();
            stream = null;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0";
                req.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
                req.Headers.Add("Upgrade-Insecure-Requests", "1");
                req.Headers.Add("Cache-Control", "max-age=0");
                resp = (HttpWebResponse)req.GetResponse();
                info.CharSet = resp.CharacterSet != "" ? resp.CharacterSet : "gb2312";
                var ce = resp.GetResponseHeader("Content-Encoding");
                info.IsGzip = !string.IsNullOrEmpty(ce) && ce.ToLower() == "gzip";
                stream = resp.GetResponseStream();
            }
            catch (Exception ex)
            {
                //TODO:请求url异常
            }
        }
        /// <summary>
        /// 处理接收流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="info"></param>
        /// <param name="html"></param>
        public void ProcessStream(Stream stream, PageInfo info, out string html)
        {
            html = null;
            if (stream == null)
            {
                return;
            }
            if (info.IsGzip)
            {
                Stream st;
                EncodeAndCompress.DecompressStream(stream, out st);
                byte[] buff = new byte[st.Length];
                st.Read(buff, 0, buff.Length);
                html = Encoding.GetEncoding(info.CharSet).GetString(buff);
                st.Close();
            }
            else
            {
                var reader = new StreamReader(stream, true);
                html = reader.ReadToEnd();
                reader.Close();
            }
        }

        public abstract void ProcessData(string html);
        public abstract bool SendRequest(string url, out Stream stream);
        public abstract bool ProcessStream(Stream stream, out PageResult result);
        public abstract bool ProcessResult(IList<PageResult> result);

        public virtual void MainWork()
        {
            results = new List<PageResult>();
            if (Config.Url != null)
            {
                switch (Config.Mode)
                {
                    case CrawlerMode.AllIndex:
                        AllIndexModeWork();
                        break;
                    case CrawlerMode.OnePage:
                        OnePageModeWork();
                        break;
                    default:
                        break;
                }
                ProcessResult(results);
            }
        }

        protected List<PageResult> results { get; set; }

        public virtual void AllIndexModeWork()
        {
            //根据当前CPU数量 新建线程 
            if (Config.AllPageFunction != null)
            {
                var res = Config.AllPageFunction(Config.Url);
                if (res != null && res.Count > 0)
                {
                    foreach (var url in res)
                    {
                        var page = Config.GetPage(url);
                        var result = Config.GetPageData(page);
                        result.Url = url;
                        results.Add(result);
                    }
                }
            }
        }

        public virtual void OnePageModeWork()
        {
            if (Config.OnePageFunction != null)
            {
                var res = Config.OnePageFunction(Config.Url);
                if (res != null && res.Item1 != null)
                {
                    var result = Config.GetPageData(res.Item2);
                    result.Url = Config.Url;
                    results.Add(result);
                    var k = true;
                    string tempUrl = res.Item1;
                    while (k)
                    {
                        res = Config.OnePageFunction(tempUrl);
                        if (res != null && res.Item1 != null)
                        {
                            var result2 = Config.GetPageData(res.Item1);
                            result2.Url = res.Item1;
                            results.Add(result2);
                            tempUrl = res.Item1;
                        }
                        else
                        {
                            k = false;
                        }
                    }
                }
            }
        }

    }
}
