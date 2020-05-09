using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class EncodeAndCompress
    {
        public static void Convert(string istr, out string ostr)
        {
            ostr = null;
            //ASCII,UTF7,UTF8,UTF32,UNICODE
            byte[] bytes1 = Encoding.ASCII.GetBytes(istr);
            byte[] bytes2 = Encoding.UTF8.GetBytes(istr);
            byte[] bytes3 = Encoding.Unicode.GetBytes(istr);
            //
            string s1 = Encoding.ASCII.GetString(bytes1);
            string s2 = Encoding.UTF8.GetString(bytes2);
            string s3 = Encoding.Unicode.GetString(bytes3);
            //
            //GB2312,BIG5
            byte[] bytes4 = Encoding.GetEncoding("GB2312").GetBytes(istr);
            byte[] bytes5 = Encoding.GetEncoding("BIG5").GetBytes(istr);
            //
            string s4 = Encoding.GetEncoding("GB2312").GetString(bytes4);
            string s5 = Encoding.GetEncoding("BIG5").GetString(bytes5);
            string s11 = Encoding.ASCII.GetString(bytes4);
            string s21 = Encoding.UTF8.GetString(bytes4);
            string s31 = Encoding.Unicode.GetString(bytes4);
        }

        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        public static void CompressStream(Stream inStream, out Stream outStream)
        {
            outStream = new MemoryStream();
            zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outStream, zlib.zlibConst.Z_DEFAULT_COMPRESSION);
            try
            {
                CopyStream(inStream, outStream);
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        public static void DecompressStream(Stream inStream, out Stream outStream)
        {
            outStream = new MemoryStream();
            zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outStream);
            try
            {
                CopyStream(inStream, outStream);
                outZStream.finish();
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
    }
}
