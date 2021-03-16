using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Net
{
    /*
    IIS搭建的FTP服务器在浏览器中的中文文件或者文件夹名为乱码问题：
    参考：https://www.cnblogs.com/zhaokunbokeyuan256/p/10001726.html
    在ftp目录下新建文件：web.config
    内容如下：
    <?xml version="1.0" encoding="UTF-8"?>
<configuration>
    <appSettings file="" />
    <system.web>
        <globalization requestEncoding="utf-8" responseEncoding="utf-8"/>
    </system.web>
</configuration>
     */
    public class OpFtp
    {
        private string FtpServerAddr { get; set; }
        private string FtpServerPath { get; set; }
        private string FtpServerUser { get; set; }
        private string FtpServerPsw { get; set; }

        private OpFtp()
        {

        }

        public static OpFtp CreateFtpServer(string ftpServerAddr, string ftpPath, string user, string psw)
        {
            OpFtp ftp = new OpFtp()
            {
                FtpServerAddr = ftpServerAddr,
                FtpServerPath = ftpPath,
                FtpServerUser = user,
                FtpServerPsw = psw,
            };
            return ftp;
        }

        public bool Upload(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException(file);
            var fileInfo = new FileInfo(file);
            var furl = FtpServerAddr + fileInfo.Name;
            FtpWebRequest reqFtp;
            reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(furl));
            reqFtp.Credentials = new NetworkCredential(FtpServerUser, FtpServerPsw);
            reqFtp.KeepAlive = false;
            reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
            reqFtp.UseBinary = true;
            reqFtp.ContentLength = fileInfo.Length;
            int buffLen = 2048;
            byte[] buff = new byte[buffLen];
            int contentLen;
            var fs = fileInfo.OpenRead();
            float total = 0, total2 = 0;
            float step = fileInfo.Length / 10;
            try
            {
                var stream = reqFtp.GetRequestStream();
                do
                {
                    contentLen = fs.Read(buff, 0, buffLen);
                    if (contentLen <= 0)
                        break;
                    total += contentLen;
                    if (total == fileInfo.Length
                        || total2 == 0
                        || total - total2 >= step)
                    {
                        total2 = total;
                        System.Diagnostics.Trace.WriteLine(string.Format("Local-->-->-->--FtpServer:\t{0}%", total / fileInfo.Length * 100));
                    }
                    stream.Write(buff, 0, contentLen);
                } while (contentLen != 0);
                stream.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool Download(string localFile, string ftpFile)
        {
            FtpWebRequest reqFtp;
            var fileStream = new FileStream(localFile, FileMode.Create);
            try
            {
                reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(FtpServerAddr + ftpFile));
                reqFtp.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(FtpServerUser, FtpServerPsw);
                var rep = (FtpWebResponse)reqFtp.GetResponse();
                var stream = rep.GetResponseStream();
                var repLen = rep.ContentLength;
                int buffSize = 2048;
                byte[] buff = new byte[buffSize];
                int readCount = 0;
                int total = 0, total2 = 0; ;
                float step = repLen / 10;
                do
                {
                    readCount = stream.Read(buff, 0, buffSize);
                    total += readCount;
                    if (repLen > 0
                        || total2 == 0
                        || total == repLen
                        || total - total2 >= step)
                    {
                        total2 = total;
                        if (repLen > 0)
                        {
                            System.Diagnostics.Trace.WriteLine(string.Format("FtpServer-->-->-->--Local:\t{0}%", total / repLen));
                        }
                    }
                    fileStream.Write(buff, 0, readCount);
                } while (readCount > 0);
                fileStream.Close();
                stream.Close();
                rep.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool Delete(string ftpFile)
        {
            var furi = new Uri(FtpServerAddr + ftpFile);
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(furi);
                req.Credentials = new NetworkCredential(FtpServerUser, FtpServerPsw);
                req.KeepAlive = false;
                req.Method = WebRequestMethods.Ftp.DeleteFile;
                string result;
                var rep = (FtpWebResponse)req.GetResponse();
                long size = rep.ContentLength;
                var stream = rep.GetResponseStream();
                var reader = new StreamReader(stream);
                result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
                rep.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool DeleteDir(string ftpDir)
        {
            var furi = new Uri(FtpServerAddr + ftpDir);
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(furi);
                req.Credentials = new NetworkCredential(FtpServerUser, FtpServerPsw);
                req.KeepAlive = false;
                req.Method = WebRequestMethods.Ftp.RemoveDirectory;
                string result;
                var rep = (FtpWebResponse)req.GetResponse();
                long size = rep.ContentLength;
                var stream = rep.GetResponseStream();
                var reader = new StreamReader(stream);
                result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
                rep.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool Exist(string ftpItem)
        {
            try
            {
                DateTime dt;
                if (GetStamp(ftpItem, out dt))
                    return true;
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return false;
        }

        public bool GetStamp(string ftpItem,out DateTime dt)
        {
            dt = DateTime.MinValue;
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(new Uri(FtpServerAddr + ftpItem));
                req.Credentials = new NetworkCredential(FtpServerUser, FtpServerPsw);
                req.KeepAlive = false;
                req.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                var rep = (FtpWebResponse)req.GetResponse();
                dt = rep.LastModified;

                //var stream = rep.GetResponseStream();
                //var reader = new StreamReader(stream);
                //string result = reader.ReadToEnd();
                //DateTime dt2;
                //if(DateTime.TryParse(result, out dt2))
                //{
                //    dt = dt2;
                //}
                //reader.Close();
                //stream.Close();

                rep.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool GetFileSize(string ftpFile, out long size)
        {
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(new Uri(FtpServerAddr + ftpFile));
                req.Credentials = new NetworkCredential(FtpServerUser, FtpServerPsw);
                req.KeepAlive = false;
                req.Method = WebRequestMethods.Ftp.GetFileSize;
                var rep = (FtpWebResponse)req.GetResponse();
                size = rep.ContentLength;
                //var stream = rep.GetResponseStream();
                //var reader = new StreamReader(stream);
                //string result = reader.ReadToEnd();
                //reader.Close();
                //stream.Close();
                rep.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool GetCurrentDirList(out List<string> infos)
        {
            infos = new List<string>();
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(new Uri(FtpServerAddr));
                req.Credentials = new NetworkCredential(FtpServerUser, FtpServerPsw);
                req.KeepAlive = false;
                req.Method = WebRequestMethods.Ftp.ListDirectory;
                string result;
                var rep = (FtpWebResponse)req.GetResponse();
                long size = rep.ContentLength;
                var stream = rep.GetResponseStream();
                var reader = new StreamReader(stream);
                do
                {
                    result = reader.ReadLine();
                    infos.Add(result);
                } while (result != null && !reader.EndOfStream);
                reader.Close();
                stream.Close();
                rep.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool GetCurrentDirDetialList(out List<string> infos)
        {
            infos = new List<string>();
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(new Uri(FtpServerAddr));
                req.Credentials = new NetworkCredential(FtpServerUser, FtpServerPsw);
                req.KeepAlive = false;
                req.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                string result;
                var rep = (FtpWebResponse)req.GetResponse();
                long size = rep.ContentLength;
                var stream = rep.GetResponseStream();
                var reader = new StreamReader(stream);
                do
                {
                    result = reader.ReadLine();
                    infos.Add(result);
                } while (result!=null && !reader.EndOfStream);
                reader.Close();
                stream.Close();
                rep.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
