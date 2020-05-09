using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace ZHello.Common
{
    public static class UseUnmanagedLibrary
    {

        public enum PlatformKind : int
        {
            __Internal = 0,
            Posix,
            Win32,
        }

        public enum PlatformName : int
        {
            __Internal = 0,
            Posix,
            Windows,
            MacOSX,
        }

        public static void GetPlatform()
        {
            ImageFileMachine Architecture;
            string Compiler;
            PlatformKind Kind;
            PlatformName Name = PlatformName.Windows;
            PortableExecutableKinds peKinds;
            typeof(object).Module.GetPEKind(out peKinds, out Architecture);
            // Version osVersion;
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32Windows: // Win9x supported?
                case PlatformID.Win32S: // Win16 NTVDM on Win x86?
                case PlatformID.Win32NT: // Windows NT
                    Kind = PlatformKind.Win32;
                    Name = PlatformName.Windows;
                    /* osVersion = Environment.OSVersion.Version;
					if (osVersion.Major <= 4) {
						// WinNT 4
					} else if (osVersion.Major <= 5) {
						// Win2000, WinXP
					} else if (osVersion.Major <= 6) {
						// WinVista, Win7, Win8.x
						if (osVersion.Major == 0) {
						}
					} else {
						// info: technet .. msdn .. microsoft research
					} */
                    break;
                case PlatformID.WinCE:
                    // case PlatformID.Xbox:
                    Kind = PlatformKind.Win32;
                    Name = PlatformName.Windows;
                    break;

                case PlatformID.Unix:
                    // note: current Mono versions still indicate Unix for Mac OS X
                    Kind = PlatformKind.Posix;
                    Name = PlatformName.Posix;
                    break;

                case PlatformID.MacOSX:
                    Kind = PlatformKind.Posix;
                    Name = PlatformName.MacOSX;
                    break;

                default:
                    if ((int)Environment.OSVersion.Platform == 128)
                    {
                        // Mono formerly used 128 for MacOSX
                        Kind = PlatformKind.Posix;
                        Name = PlatformName.MacOSX;
                    }
                    break;
            }
            // TODO: Detect and distinguish available Compilers and Runtimes
            /* switch (Kind) {
			case PlatformKind.Windows:
				LibraryFileNameFormat = Platform.Windows.LibraryFileNameFormat;
				OpenPtr = Platform.Windows.OpenPtr;
				LoadProcedure = Platform.Windows.LoadProcedure;
				ReleasePtr = Platform.Windows.ReleasePtr;
				GetLastLibraryError = Platform.Windows.GetLastLibraryError;
				break;

			case PlatformKind.Posix:
				LibraryFileNameFormat = Platform.Posix.LibraryFileNameFormat;
				OpenPtr = Platform.Posix.OpenPtr;
				LoadProcedure = Platform.Posix.LoadProcedure;
				ReleasePtr = Platform.Posix.ReleasePtr;
				GetLastLibraryError = Platform.Posix.GetLastLibraryError;
				break;

			case PlatformKind.Unknown:
			default:
				throw new PlatformNotSupportedException ();
			} */
            var IsMono = Type.GetType("Mono.Runtime") != null;
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var IsMonoTouch = assemblies.Any(a => a.GetName().Name.Equals("MonoTouch", StringComparison.InvariantCultureIgnoreCase));
            var IsMonoMac = assemblies.Any(a => a.GetName().Name.Equals("MonoMac", StringComparison.InvariantCultureIgnoreCase));
            var IsXamarinIOS = assemblies.Any(a => a.GetName().Name.Equals("Xamarin.iOS", StringComparison.InvariantCultureIgnoreCase));
            var IsXamarinAndroid = assemblies.Any(a => a.GetName().Name.Equals("Xamarin.Android", StringComparison.InvariantCultureIgnoreCase));
            if (IsMonoMac)
            {
                Kind = PlatformKind.Posix;
                Name = PlatformName.MacOSX;
            }
            if (Name == PlatformName.Posix && File.Exists("/System/Library/CoreServices/SystemVersion.plist"))
            {
                Name = PlatformName.MacOSX;
            }
            if (IsXamarinIOS || IsMonoTouch)
            {
                // Kind = PlatformKind.__Internal;
                // Name = PlatformName.__Internal;
                //Is__Internal = true;
            }
        }

        #region Library

        private const string LibraryName = "kernel32";

        [DllImport(LibraryName, CharSet = CharSet.Auto, BestFitMapping = false, SetLastError = true)]
        private static extern SafeLibraryHandle LoadLibrary(string fileName);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport(LibraryName, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr moduleHandle);

        [DllImport(LibraryName)]
        private static extern IntPtr GetProcAddress(SafeLibraryHandle moduleHandle, string procname);

        #endregion Library

        /// <summary>
        /// Safe handle for unmanaged libraries. See http://msdn.microsoft.com/msdnmag/issues/05/10/Reliability/ for more about safe handles.
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public sealed class SafeLibraryHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
        {
            private SafeLibraryHandle()
                : base(true)
            { }

            protected override bool ReleaseHandle()
            {
                return FreeLibrary(handle);
            }
        }

        public static bool IsNullOrInvalid(this SafeLibraryHandle handle)
        {
            return handle == null || handle.IsInvalid;
        }

        /// <summary>
        /// Utility class to wrap an unmanaged shared lib and be responsible for freeing it.
        /// </summary>
        /// <remarks>
        /// This is a managed wrapper over the native LoadLibrary, GetProcAddress, and FreeLibrary calls on Windows
        /// and dlopen, dlsym, and dlclose on Posix environments.
        /// </remarks>
        public sealed class UnmanagedLibrary : IDisposable
        {
            private readonly string TraceLabel;

            private readonly SafeLibraryHandle _handle;

            internal UnmanagedLibrary(string libraryName, SafeLibraryHandle libraryHandle)
            {
                if (string.IsNullOrWhiteSpace(libraryName))
                {
                    throw new ArgumentException("A valid library name is expected.", "libraryName");
                }
                if (libraryHandle.IsNullOrInvalid())
                {
                    throw new ArgumentNullException("libraryHandle");
                }

                TraceLabel = string.Format("UnmanagedLibrary[{0}]", libraryName);

                _handle = libraryHandle;
            }

            /// <summary>
            /// Dynamically look up a function in the dll via kernel32!GetProcAddress or libdl!dlsym.
            /// </summary>
            /// <typeparam name="TDelegate">Delegate type to load</typeparam>
            /// <param name="functionName">Raw name of the function in the export table.</param>
            /// <returns>A delegate to the unmanaged function.</returns>
            /// <exception cref="MissingMethodException">Thrown if the given function name is not found in the library.</exception>
            /// <remarks>
            /// GetProcAddress results are valid as long as the dll is not yet unloaded. This
            /// is very very dangerous to use since you need to ensure that the dll is not unloaded
            /// until after you're done with any objects implemented by the dll. For example, if you
            /// get a delegate that then gets an IUnknown implemented by this dll,
            /// you can not dispose this library until that IUnknown is collected. Else, you may free
            /// the library and then the CLR may call release on that IUnknown and it will crash.
            /// </remarks>
            public TDelegate GetUnmanagedFunction<TDelegate>(string functionName) where TDelegate : class
            {
                IntPtr p = GetProcAddress(_handle, functionName);

                if (p == IntPtr.Zero)
                {
                    throw new MissingMethodException("Unable to find function '" + functionName + "' in dynamically loaded library.");
                }

                // Ideally, we'd just make the constraint on TDelegate be
                // System.Delegate, but compiler error CS0702 (constrained can't be System.Delegate)
                // prevents that. So we make the constraint system.object and do the cast from object-->TDelegate.
                return (TDelegate)(object)Marshal.GetDelegateForFunctionPointer(p, typeof(TDelegate));
            }

            public void Dispose()
            {
                if (_handle != null && !_handle.IsClosed)
                {
                    _handle.Close();
                }
            }
        }
    }
}