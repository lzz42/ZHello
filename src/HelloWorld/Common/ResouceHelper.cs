using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ZHello.Common
{
    public class ResouceHelper
    {
        public static void RealseResouceToTempPathFlie(string key, string sdir, string name, bool isCorver = false)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(name))
            {
                return;
            }
            var dir = Path.Combine(Path.GetTempPath(), sdir);
            var fn = Path.Combine(dir, name);
            if (File.Exists(fn) && !isCorver)
            {
                return;
            }
            try
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(key);
                if (stream != null)
                {
                    if (isCorver && File.Exists(fn))
                    {
                        File.Delete(fn);
                    }
                    if (!Directory.Exists(Path.GetDirectoryName(fn)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fn));
                    }
                    using (var sf = File.Create(fn))
                    {
                        stream.CopyTo(sf);
                    }
                    stream.Dispose();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine(ex.Source);
            }
        }

        public static void RealseResouceToTempPathFlie(string key, string name, bool isCorver = false)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(name))
            {
                return;
            }
            var dir = Path.GetTempPath();
            var fn = Path.Combine(dir, name);
            if (File.Exists(fn) && !isCorver)
            {
                return;
            }
            try
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(key);
                if (stream != null)
                {
                    if (isCorver && File.Exists(fn))
                    {
                        File.Delete(fn);
                    }
                    using (var sf = File.Create(fn))
                    {
                        stream.CopyTo(sf);
                    }
                    stream.Dispose();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine(ex.Source);
            }
        }
    }
}