using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class AnalyzeHtml
    {
        public static void GetUrlAndData(string html, out string nextUrl, out string data)
        {
            nextUrl = null;
            data = null;
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNode navNode = doc.GetElementbyId("zjneirong");
                HtmlNode xiayie = doc.GetElementbyId("A3");
                HtmlNode xiayie2 = doc.GetElementbyId("xiaye");
                HtmlNode neirongdiv = doc.GetElementbyId("neirongDiv");
                var div = FindHtmlNodeByClass(neirongdiv, "bookname");
                var h1 = FindHtmlNodeByName(div, "h1");
                if (h1 == null)
                    return;
                var title = h1.InnerText.Trim();
                var contnet = navNode.InnerText.Trim();
                var fhref = xiayie.GetAttributeValue("href", "f").ToLower();
                var ehref = xiayie2.GetAttributeValue("href", "e").ToLower();
                var sb = new StringBuilder();
                sb.AppendLine(title);
                sb.AppendLine(contnet);
                if (fhref == ehref)
                {
                    if (fhref != "http://www.bxwx3.org/txt/48595/")
                    {
                        nextUrl = fhref;
                    }
                }
                data = sb.ToString();
            }
            catch (Exception ex)
            {
                //TODO:解析HTML异常
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="nextUrl"></param>
        /// <param name="data"></param>
        public static void GetUrlAndData_biquge(string html, out string nextUrl, out string data)
        {
            nextUrl = null;
            data = null;
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNode wrapper = doc.GetElementbyId("wrapper");
                var bookname = FindHtmlNodeByClass(wrapper, "bookname");
                var titleNode = FindHtmlNodeByName(wrapper, "h1");
                var contentN = doc.GetElementbyId("content");
                var bottem1N = FindHtmlNodeByClass(wrapper, "bottem1");
                var bottemN = FindHtmlNodeByClass(wrapper, "bottem");
                var fnode = FindHtmlNodeByNameAndInnerText(bottem1N, "a", "下一章");
                var lnode = FindHtmlNodeByNameAndInnerText(bottemN, "a", "下一章");
                var title = titleNode.InnerText.Trim().Replace("&nbsp;", "");
                var contnet = contentN.InnerText.Trim().Replace("&nbsp;", "");
                var fhref = fnode.GetAttributeValue("href", "f").ToLower();
                var lhref = lnode.GetAttributeValue("href", "e").ToLower();
                var sb = new StringBuilder();
                sb.AppendLine(title);
                sb.AppendLine(contnet + "\n\r\n\r");
                if (fhref == lhref)
                {
                    if (fhref != "http://www.biquge.info/2_2309/")
                    {
                        nextUrl = fhref;
                    }
                }
                data = sb.ToString();
                Console.WriteLine(title);
            }
            catch (Exception ex)
            {
                //TODO:解析HTML异常
            }
        }

        public static void GetAllUrls(string html, out List<string> urls, out string bookName, out string author, out string summer)
        {
            urls = new List<string>();
            bookName = null;
            author = null;
            summer = null;
            if (string.IsNullOrEmpty(html))
                return;
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var wrapper = doc.GetElementbyId("wrapper");
                var mainInfo = doc.GetElementbyId("info");
                var info = doc.GetElementbyId("info");
                var intro = doc.GetElementbyId("intro");
                var list = doc.GetElementbyId("list");
                bookName = FindHtmlNodeByName(info, "h1")?.InnerText.Trim().Replace("&nbsp;", "");
                author = FindHtmlNodeByName(info, "p")?.InnerText.Trim().Replace("&nbsp;", "");
                summer = FindHtmlNodeByName(intro, "p")?.InnerText.Trim().Replace("&nbsp;", "");
                var dl = FindHtmlNodeByName(list, "dl");
                foreach (var dd in dl.ChildNodes)
                {
                    if (dd.Name.ToLower() == "dd")
                    {
                        var a = FindHtmlNodeByName(dd, "a");
                        var url = a.GetAttributeValue("href", "");
                        urls.Add(url);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:解析HTML异常
            }
        }

        /// <summary>
        /// 根据节点类名 查找结点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="clsstr"></param>
        /// <returns></returns>
        public static HtmlNode FindHtmlNodeByClass(HtmlNode node, string clsstr)
        {
            if (node != null)
            {
                var cls = node.GetAttributeValue("class", "");
                if (clsstr == cls)
                {
                    return node;
                }
                else
                {
                    if (node.ChildNodes != null)
                    {
                        var lis = node.ChildNodes.ToList();
                        for (int i = 0; i < lis.Count; i++)
                        {
                            var res = FindHtmlNodeByClass(lis[i], clsstr);
                            if (res != null)
                            {
                                return res;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据结点名称属性查找结点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HtmlNode FindHtmlNodeByName(HtmlNode node, string name)
        {
            if (node != null)
            {
                if (name == node.Name.ToLower())
                {
                    return node;
                }
                else
                {
                    if (node.ChildNodes != null)
                    {
                        var lis = node.ChildNodes.ToList();
                        for (int i = 0; i < lis.Count; i++)
                        {
                            var res = FindHtmlNodeByName(lis[i], name);
                            if (res != null)
                            {
                                return res;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据节点内容查找结点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static HtmlNode FindHtmlNodeByInnerText(HtmlNode node, string text)
        {
            if (node != null)
            {
                if (text == node.InnerText.Trim().ToLower())
                {
                    return node;
                }
                else
                {
                    if (node.ChildNodes != null)
                    {
                        var lis = node.ChildNodes.ToList();
                        for (int i = 0; i < lis.Count; i++)
                        {
                            var res = FindHtmlNodeByInnerText(lis[i], text);
                            if (res != null)
                            {
                                return res;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static HtmlNode FindHtmlNodeByNameAndInnerText(HtmlNode node, string name, string text)
        {
            if (node != null)
            {
                if (name == node.Name.ToLower() && text == node.InnerText.Trim().ToLower())
                {
                    return node;
                }
                else
                {
                    if (node.ChildNodes != null)
                    {
                        var lis = node.ChildNodes.ToList();
                        for (int i = 0; i < lis.Count; i++)
                        {
                            var res = FindHtmlNodeByNameAndInnerText(lis[i], name, text);
                            if (res != null)
                            {
                                return res;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
