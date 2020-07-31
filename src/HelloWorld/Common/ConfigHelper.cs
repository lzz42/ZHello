using System;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Configuration;

namespace ZHello.Common
{
    #region Config文件

    /// <summary>
    /// 配置文件类
    /// </summary>
    public abstract class AbstConfig
    {
        protected Configuration conf;

        public abstract string ReadValue(string ID, string groupName);

        public abstract bool WriteValue(string ID, string value, string groupName);
    }

    /// <summary>
    /// 配置文件数据类
    /// </summary>
    public sealed class ConfigData : ConfigurationSection
    {
        [ConfigurationProperty("ID")]
        public string ID
        {
            get { return (string)this["ID"]; }
            set { this["ID"] = value; }
        }

        [ConfigurationProperty("Value")]
        public string Value
        {
            get { return (string)this["Value"]; }
            set { this["Value"] = value; }
        }
    }

    /// <summary>
    /// 读写web.cofig配置文件的类
    /// </summary>
    public sealed class BSDbConfig : AbstConfig
    {
        public BSDbConfig(string fileName)
        {
            fileName = fileName == "~" ? fileName : "~\\" + fileName;
            conf = WebConfigurationManager.OpenWebConfiguration(fileName);
        }

        public BSDbConfig() : this("~")
        {
        }

        public override string ReadValue(string ID, string groupName)
        {
            string res = null;
            if (conf.SectionGroups[groupName] != null)
            {
                try
                {
                    res = (conf.SectionGroups[groupName].Sections[ID] as ConfigData)?.Value;
                }
                catch (Exception)
                {
                }
            }
            return res;
        }

        public override bool WriteValue(string id, string value, string groupName)
        {
            bool res = true;
            try
            {
                if (conf.SectionGroups[groupName] == null)
                {
                    conf.SectionGroups.Add(groupName, new ConfigurationSectionGroup());
                }
                var data = conf.SectionGroups[groupName].Sections[id] as ConfigurationSection;
                var cfd = new ConfigData() { ID = id, Value = value };
                if (data == null)
                {
                    conf.SectionGroups[groupName].Sections.Add(id, cfd);
                }
                else
                {
                    conf.SectionGroups[groupName].Sections.Remove(id);
                    conf.SectionGroups[groupName].Sections.Add(id, cfd);
                }
                conf.Save();
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }
    }

    /// <summary>
    /// 读写app.config配置文件的类
    /// </summary>
    public sealed class CSDbConfig : AbstConfig
    {
        public CSDbConfig(string configFilePath, string fileName)
        {
            conf = ConfigurationManager.OpenExeConfiguration(System.IO.Path.Combine(configFilePath, fileName));
        }

        public CSDbConfig(string fileName) : this(Environment.CurrentDirectory, fileName)
        {
        }

        public override string ReadValue(string ID, string groupName)
        {
            string res = null;
            if (conf.SectionGroups[groupName] != null)
            {
                res = (conf.SectionGroups[groupName].Sections[ID] as ConfigData)?.Value;
            }
            return res;
        }

        public override bool WriteValue(string id, string value, string groupName)
        {
            bool res = true;
            try
            {
                if (conf.SectionGroups[groupName] == null)
                {
                    conf.SectionGroups.Add(groupName, new ConfigurationSectionGroup());
                }
                var data = conf.SectionGroups[groupName].Sections[id] as ConfigurationSection;
                var cfd = new ConfigData() { ID = id, Value = value };
                if (data == null)
                {
                    conf.SectionGroups[groupName].Sections.Add(id, cfd);
                }
                else
                {
                    conf.SectionGroups[groupName].Sections.Remove(id);
                    conf.SectionGroups[groupName].Sections.Add(id, cfd);
                }
                conf.Save();
            }
            catch
            {
                res = false;
            }
            return res;
        }
    }

    #endregion Config文件

    #region ini文件

    /// <summary>
    /// ini文件读写
    /// </summary>
    public class IniHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void ReadValue(string fileName, string section, string key, out string value)
        {
            value = string.Empty;
            if (File.Exists(fileName))
            {
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void WriteValue(string fileName, string section, string key, string value)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">关键字</param>
        /// <param name="val">关键字数值</param>
        /// <param name="filePath">文件全路径</param>
        /// <returns></returns>
        [DllImport("kernal32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /// <summary>
        ///
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">关键字</param>
        /// <param name="def">无法读取时的缺省值</param>
        /// <param name="retVal">读取数值</param>
        /// <param name="size">数值大小</param>
        /// <param name="filePath">文件全路径</param>
        /// <returns></returns>
        [DllImport("kernal32")]
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
    }

    #endregion ini文件

    #region xml文件

    /// <summary>
    /// xml文件读写
    /// </summary>
    public class XmlHelper
    {
    }

    #endregion xml文件
}