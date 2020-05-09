using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler.Crawler
{
    public class WebCrawler2 : IDisposable
    {
        public string BaseUrl { get; set; }
        public string TempUrl { get; protected set; }
        public string Name { get; protected set; }
        public AnalyzePageFn MFunction { get; protected set; }
        public AnalyzeUrlsFn MUlrsFn { get; protected set; }
        public EventHandler OnDone { get; set; }
        private Random random = new Random(7);
        private HttpWebRequest req;
        private HttpWebResponse resp;
        private Thread _thead;
        private ManualResetEvent _se = new ManualResetEvent(false);
        private List<string> _urls = new List<string>();
        public WebCrawler2(CrawlerConfig config)
        {
            BaseUrl = TempUrl = config.Url;
            MFunction = config.PageFn;
            Name = config.Name;
            Init();
        }

        private void Init()
        {
            DataStore.Init(Name);
            _thead = new Thread(Work) { IsBackground = true, Name = string.Format("{0}_{1}", this.GetType().Name, BaseUrl) };
            _thead.Start();
        }
        private void Work()
        {
            while (true)
            {
                if (_se.WaitOne(-1))
                {
                    try
                    {
                        if (!DoWork())
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    break;
                }
                var time = 300 + random.Next(100, 300);
                Thread.Sleep(time);
            }
            OnDone?.Invoke(null, null);
        }

        private bool DoWork()
        {
            Stream stream = null;
            string charset = null;
            bool isgzip = false;
            SendRequest(TempUrl, out stream, out charset, out isgzip);
            if (stream == null)
            {
                return false;
            }
            string html = null;
            ProcessStream(stream, isgzip, charset, out html);
            if (string.IsNullOrEmpty(html))
            {
                return false;
            }
            string tempurl, data;
            MFunction.Invoke(html, out tempurl, out data);
            DataStore.SaveFile(data);
            if (tempurl == null)
            {
                return false;
            }
            TempUrl = tempurl;
            return true;
        }

        private void SendRequest(string url, out Stream stream, out string charset, out bool isgzip)
        {
            charset = null;
            isgzip = false;
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
                charset = resp.CharacterSet != "" ? resp.CharacterSet : "gb2312";
                var ce = resp.GetResponseHeader("Content-Encoding");
                isgzip = !string.IsNullOrEmpty(ce) && ce.ToLower() == "gzip";
                stream = resp.GetResponseStream();
            }
            catch (Exception ex)
            {
                //TODO:请求url异常
            }
        }

        private void ProcessStream(Stream stream, bool isgzip, string charset, out string html)
        {
            html = null;
            if (stream == null)
            {
                return;
            }
            if (isgzip)
            {
                Stream st;
                EncodeAndCompress.DecompressStream(stream, out st);
                byte[] buff = new byte[st.Length];
                st.Read(buff, 0, buff.Length);
                html = Encoding.GetEncoding(charset).GetString(buff);
                st.Close();
            }
            else
            {
                var reader = new StreamReader(stream, true);
                html = reader.ReadToEnd();
                reader.Close();
            }
        }

        public void Run()
        {
            _se.Set();
        }

        public void Stop()
        {
            _se.Reset();
        }

        public void Dispose()
        {
            _se.Set();
            _se.Close();
        }
    }
}
