using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.UI;

namespace ZHello.Common
{
    public class WebHelper
    {
        #region GETPOST

        /*
 Http请求
 get post put delete 查 改 增 删

 get：用户获取信息
 参数通过URL传输，在URL后用？分割，参数间用&分割
 http://url?para1=value1&para2=value2

 post 更新数据

     */

        public static HttpWebResponse HttpGet(string url, IDictionary<string, string> paras, int timeout, string agent, CookieCollection cookies)
        {
            bool first = true;
            string data = "";
            if (paras != null && paras.Count > 0)
            {
                var buffer = new StringBuilder();
                foreach (var key in paras.Keys)
                {
                    if (!first)
                    {
                        buffer.AppendFormat("&{0}={1}", key, paras[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, paras[key]);
                        first = false;
                    }
                }
                data = buffer.ToString();
            }
            url += data == "" ? "" : ("?" + data);
            return HttpGet(url, timeout, agent, cookies);
        }

        public static HttpWebResponse HttpGet(string url, int timeout, string agnet, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) =>
                {
                    if (sslPolicyErrors == SslPolicyErrors.None)
                        return true;
                    Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
                    // Do not allow this client to communicate with unauthenticated servers.
                    return false;
                });
                request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            }
            request.Method = "GET";
            request.UserAgent = agnet;//设置代理
            request.Timeout = timeout;//设置超时
            request.Headers.Add("name", "value");//添加头
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream, Encoding.UTF8);
            var result = reader.ReadToEnd();
            stream.Close();
            reader.Close();
            Debug.WriteLine(result);
            return response;
        }

        public static HttpWebResponse HttpPost(string url, IDictionary<string, string> paras, int timeout, string agent, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) =>
                {
                    if (sslPolicyErrors == SslPolicyErrors.None)
                        return true;
                    Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
                    // Do not allow this client to communicate with unauthenticated servers.
                    return false;
                });
                request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "text/html";
            request.UserAgent = agent;//设置代理
            request.Timeout = timeout;//设置超时
            request.Headers.Add("name", "value");//添加头
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //发送post数据
            if (paras != null || paras.Count > 0)
            {
                StringBuilder buffer = new StringBuilder();
                bool first = true;
                foreach (var k in paras.Keys)
                {
                    if (!first)
                    {
                        buffer.AppendFormat("&{0}={1}", k, paras[k]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", k, paras[k]);
                        first = false;
                    }
                }
                byte[] data = Encoding.ASCII.GetBytes(buffer.ToString());
                using (var s = request.GetRequestStream())
                {
                    s.Write(data, 0, data.Length);
                }
            }
            string[] values = request.Headers.GetValues("Content-Type");
            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream, Encoding.UTF8);
            var result = reader.ReadToEnd();
            stream.Close();
            reader.Close();
            Debug.WriteLine(result);
            return response;
        }

        #endregion GETPOST

        #region Cookie

        /// <summary>
        /// 新建cookie对象
        /// </summary>
        /// <param name="iCookieName"></param>
        /// <param name="iCookieValue"></param>
        /// <returns></returns>
        public static HttpCookie CreateCookie(string iCookieName, string iCookieValue)
        {
            HttpCookie iCookie = new HttpCookie(iCookieName);
            iCookie.Value = iCookieValue;
            //
            DateTime dtNow = DateTime.Now;
            TimeSpan tsMinute = new TimeSpan(1, 1, 30, 0);
            iCookie.Expires = dtNow + tsMinute;
            //iCookie.Expires = DateTime.Now.AddMinutes(3);
            return iCookie;
        }

        /// <summary>
        /// 在Cookie中添加键值对
        /// </summary>
        /// <param name="iCookie"></param>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HttpCookie AddValue(HttpCookie iCookie, string keyName, string value)
        {
            if (iCookie == null)
            {
                return null;
            }
            else
            {
                iCookie[keyName] = value;
                iCookie.Expires = DateTime.Now.AddMinutes(3);
                return iCookie;
            }
        }

        #endregion Cookie

        #region Session

        /// <summary>
        /// 设置回话值
        /// </summary>
        /// <param name="iPage"></param>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        public static void SetSession(Page iPage, string keyName, object value)
        {
            iPage.Session[keyName] = value;
        }

        /// <summary>
        /// 获取回话内string值
        /// </summary>
        /// <param name="iPage"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string GetSession(Page iPage, string keyName)
        {
            if (iPage.Session[keyName] != null)
            {
                return iPage.Session[keyName].ToString();
            }
            return null;
        }

        /// <summary>
        /// 获取回话objcet值
        /// </summary>
        /// <param name="iPage"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static object GetSessionObject(Page iPage, string keyName)
        {
            return iPage.Session[keyName];
        }

        /// <summary>
        /// 销毁回话
        /// </summary>
        /// <param name="iPage"></param>
        public static void RuinSession(Page iPage)
        {
            iPage.Session.Abandon();
        }

        #endregion Session
    }
}