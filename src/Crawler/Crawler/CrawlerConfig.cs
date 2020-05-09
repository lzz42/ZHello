using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Crawler
{
    /// <summary>
    /// 爬虫配置类
    /// </summary>
    public struct CrawlerConfig
    {
        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 爬虫模式
        /// </summary>
        public CrawlerMode Mode { get; set; }

        public AnalyzePageFn PageFn { get; set; }
        public AnalyzeUrlsFn UrlsFn { get; set; }

        public Func<string,Tuple<string,string>> OnePageFunction { get; set; }

        public Func<string, List<string>> AllPageFunction { get; set; }
        
        /// <summary>
        /// 从url获取网页数据
        /// </summary>
        public Func<string, string> GetPage { get; set; }

        /// <summary>
        /// 从网页中提取信息
        /// 分析网页结构获取网页信息
        /// </summary>
        public Func<string, PageResult> GetPageData { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
