using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;

namespace HelloWorld.Zip
{
    public class ZipHelper
    {
        /// <summary>
        /// ZIP:解压一个zip文件
        /// </summary>
        /// <param name="ZipFile">需要解压的Zip文件（绝对路径）</param>
        /// <param name="TargetDirectory">解压到的目录</param>
        /// <param name="Password">解压密码</param>
        /// <param name="OverWrite">是否覆盖已存在的文件</param>
        public static void UnZip(string ZipFile, string TargetDirectory, string Password = null, bool OverWrite = true)
        {
            if (!Directory.Exists(TargetDirectory))
            {
                throw new FileNotFoundException("指定的目录: " + TargetDirectory + " 不存在!");
            }
            if (!TargetDirectory.EndsWith("\\"))
            {
                TargetDirectory = TargetDirectory + "\\";
            }
            using (ZipInputStream zipfiles = new ZipInputStream(File.OpenRead(ZipFile)))
            {
                zipfiles.Password = Password;
                ZipEntry theEntry;
                while ((theEntry = zipfiles.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;
                    if (pathToZip != "")
                    {
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";
                    }
                    string fileName = Path.GetFileName(pathToZip);
                    Directory.CreateDirectory(TargetDirectory + directoryName);
                    if (fileName == "")
                    {
                        continue;
                    }
                    if ((File.Exists(TargetDirectory + directoryName + fileName) && OverWrite) || (!File.Exists(TargetDirectory + directoryName + fileName)))
                    {
                        using (FileStream streamWriter = File.Create(TargetDirectory + directoryName + fileName))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = zipfiles.Read(data, 0, data.Length);
                                if (size > 0)
                                    streamWriter.Write(data, 0, size);
                                else
                                    break;
                            }
                            streamWriter.Close();
                        }
                    }
                }
                zipfiles.Close();
            }
        }

        /// <summary>
        /// ZIP：压缩文件夹
        /// </summary>
        /// <param name="DirectoryToZip">需要压缩的文件夹（绝对路径）</param>
        /// <param name="ZipedPath">压缩后的文件路径（绝对路径）</param>
        /// <param name="ZipedFileName">压缩后的文件名称</param>
        /// <param name="IsEncrypt">是否加密</param>
        /// <param name="psw">加密密码</param>
        /// <param name="IgNoreFiles">忽略文件或者文件夹列表</param>
        public static void ZipDirectory(string DirectoryToZip, string ZipedPath, string ZipedFileName = "", bool IsEncrypt = false, string psw = null, List<string> IgNoreFiles = null)
        {
            if (!Directory.Exists(DirectoryToZip))
            {
                throw new FileNotFoundException("指定的目录: " + DirectoryToZip + " 不存在!");
            }
            string ZipFileName = null;
            if (string.IsNullOrEmpty(ZipedFileName))
            {
                ZipFileName = ZipedPath + "\\" + DirectoryToZip + ".zip";
            }
            else
            {
                ZipFileName = ZipedPath + "\\" + ZipedFileName + ".zip";
            }
            using (FileStream ZipFile = File.Create(ZipFileName))
            {
                using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                {
                    if (IsEncrypt && !string.IsNullOrEmpty(psw))
                    {
                        //压缩文件加密
                        s.Password = psw;
                    }
                    ZipSetp(DirectoryToZip, s, "", IgNoreFiles);
                }
            }
        }

        /// <summary>
        /// ZIP:压缩单个文件
        /// </summary>
        /// <param name="FileToZip">需要压缩的文件（绝对路径）</param>
        /// <param name="ZipedPath">压缩后的文件路径（绝对路径）</param>
        /// <param name="ZipedFileName">压缩后的文件名称（文件名，默认 同源文件同名）</param>
        /// <param name="CompressionLevel">压缩等级（0 无 - 9 最高，默认 5）</param>
        /// <param name="BlockSize">缓存大小（每次写入文件大小，默认 2048）</param>
        /// <param name="IsEncrypt">是否加密（默认 加密）</param>
        public static void ZipFile(string FileToZip, string ZipedPath, string ZipedFileName = null, int CompressionLevel = 5, int BlockSize = 2048, bool IsEncrypt = true, string psw = null)
        {
            if (!File.Exists(FileToZip))
            {
                throw new FileNotFoundException("要压缩的文件: " + FileToZip + " 不存在!");
            }
            string ZipFileName = ZipedPath + "\\" + Path.GetFileName(FileToZip) + ".zip";
            if (!string.IsNullOrEmpty(ZipedFileName))
            {
                ZipFileName = ZipedPath + "\\" + ZipedFileName + ".zip";
            }
            using (FileStream ZipFile = File.Create(ZipFileName))
            {
                using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                {
                    using (FileStream StreamToZip = new FileStream(FileToZip, FileMode.Open, FileAccess.Read))
                    {
                        string fileName = FileToZip.Substring(FileToZip.LastIndexOf("\\") + 1);
                        ZipEntry ZipEntry = new ZipEntry(fileName);
                        if (IsEncrypt && !string.IsNullOrEmpty(psw))
                        {
                            //压缩文件加密
                            ZipStream.Password = psw;
                        }
                        ZipStream.PutNextEntry(ZipEntry);
                        //设置压缩级别
                        ZipStream.SetLevel(CompressionLevel);
                        //缓存大小
                        byte[] buffer = new byte[BlockSize];
                        int sizeRead = 0;
                        try
                        {
                            do
                            {
                                sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                ZipStream.Write(buffer, 0, sizeRead);
                            }
                            while (sizeRead > 0);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        StreamToZip.Close();
                    }
                    ZipStream.Finish();
                    ZipStream.Close();
                }
                ZipFile.Close();
            }
        }

        /// <summary>
        /// 递归遍历目录
        /// </summary>
        private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath, List<string> IgNoreFiles = null)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();
            string[] filenames = Directory.GetFileSystemEntries(strDirectory);
            foreach (string file in filenames)
            {
                if (IgNoreFiles != null && IgNoreFiles.Count > 0 && IgNoreFiles.Contains(file + "\\"))
                {
                    continue;
                }
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    var builder = new StringBuilder(parentPath);
                    builder.Append(file.Substring(file.LastIndexOf("\\") + 1));
                    builder.Append("\\");
                    ZipSetp(file, s, builder.ToString());
                }
                else 
                {
                    //打开压缩文件
                    using (FileStream fs = File.OpenRead(file))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        var builder = new StringBuilder();
                        builder.Append(parentPath);
                        builder.Append(file.Substring(file.LastIndexOf("\\") + 1));
                        ZipEntry entry = new ZipEntry(builder.ToString());
                        entry.DateTime = DateTime.Now;
                        entry.Size = fs.Length;
                        fs.Close();
                        crc.Reset();
                        crc.Update(buffer);
                        entry.Crc = crc.Value;
                        s.PutNextEntry(entry);
                        s.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
    }
}