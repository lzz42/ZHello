using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace ZHello.Common
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    internal class SerializationHelper
    {
        #region JSON序列化与反序列化

        /// <summary>
        /// JSON序列化类到字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static bool Serialize<T>(T t, out string jsonStr)
        {
            bool result = false;
            jsonStr = null;
            try
            {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
                using (MemoryStream ms = new MemoryStream())
                {
                    js.WriteObject(ms, t);
                    ms.Position = 0;
                    StreamReader reader = new StreamReader(ms);
                    jsonStr = reader.ReadToEnd();
                    reader.Close();
                    ms.Close();
                    if (!string.IsNullOrEmpty(jsonStr))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// JSON反序列化字符串到类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Deserialize<T>(string jsonStr, out T t)
        {
            bool result = false;
            t = default(T);
            if (string.IsNullOrEmpty(jsonStr))
            {
                return false;
            }
            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr)))
                {
                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
                    t = (T)js.ReadObject(ms);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// JSON序列化类到字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static bool Serialize<T>(T t, out Stream stream)
        {
            bool result = false;
            stream = null;
            try
            {
                if (t != null)
                {
                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
                    MemoryStream ms = new MemoryStream();
                    js.WriteObject(ms, t);
                    ms.Position = 0;
                    stream = ms;
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// JSON反序列化字符串到类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Deserialize<T>(Stream stream, out T t)
        {
            bool result = false;
            t = default(T);
            if (stream == null)
            {
                return false;
            }
            try
            {
                Stream ms = stream;
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
                t = (T)js.ReadObject(ms);
                result = true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
            return result;
        }

        #endregion JSON序列化与反序列化

        #region XML序列化与反序列化

        /// <summary>
        /// XML序列化类到字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static bool XmlSerialize<T>(T t, out string xmlStr)
        {
            bool result = false;
            xmlStr = null;
            try
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var ser = new XmlSerializer(typeof(T));
                using (MemoryStream ms = new MemoryStream())
                {
                    ser.Serialize(ms, t, ns);
                    ms.Position = 0;
                    StreamReader reader = new StreamReader(ms);
                    xmlStr = reader.ReadToEnd();
                    reader.Close();
                    ms.Close();
                    if (!string.IsNullOrEmpty(xmlStr))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// XML反序列化字符串到类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlStr"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool XmlDeserialize<T>(string xmlStr, out T t)
        {
            bool result = false;
            t = default(T);
            if (string.IsNullOrEmpty(xmlStr))
            {
                return false;
            }
            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr)))
                {
                    var ser = new XmlSerializer(typeof(T));
                    t = (T)ser.Deserialize(ms);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// XML序列化到文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool XmlSerializeToFile<T>(T t, string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return false;
            }
            if (!Directory.Exists(Path.GetDirectoryName(file)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file));
            }
            if (File.Exists(file))
                File.Delete(file);
            bool result = false;
            try
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var ser = new XmlSerializer(typeof(T));
                using (var ms = File.Create(file))
                {
                    var wt = new StreamWriter(ms, Encoding.UTF8);
                    ser.Serialize(wt, t, ns);
                    wt.Flush();
                    wt.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// 从文件XML反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool XmlDeserializeFromFile<T>(string file, out T t)
        {
            bool result = false;
            t = default(T);
            if (string.IsNullOrEmpty(file))
            {
                return false;
            }
            if (!File.Exists(file))
                return false;
            try
            {
                var xmlStr = File.ReadAllText(file, Encoding.UTF8);
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr)))
                {
                    var ser = new XmlSerializer(typeof(T));
                    t = (T)ser.Deserialize(ms);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
            return result;
        }

        #endregion XML序列化与反序列化
    }
}