using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class SxsHelper
    {
        public const string Dll = "DLL\\sxs.dll";

        [DllImport(Dll,BestFitMapping =true,CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode, EntryPoint = "CreateAssemblyByCache", ExactSpelling = true,PreserveSig = false,
            SetLastError = true, ThrowOnUnmappableChar = true)]
        public static extern int CreateAssemblyByCache(IntPtr ppAsmCache,int dwReserved);

        [DllImport(Dll, BestFitMapping = true, CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode, EntryPoint = "CreateAssemblyCacheItem", ExactSpelling = true, PreserveSig = false,
            SetLastError = true, ThrowOnUnmappableChar = true)]
        public static extern int CreateAssemblyCacheItem(int dwFlags, int pvReserved, out IntPtr ppAsmItem, IntPtr lppszAssemblyName);
    }
}
