using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace ZHello.Common
{
    public class DbgHelper
    {
        #region cctor

        static DbgHelper()
        {
            PDB = PDB.CreatePDBHandler(Process.GetCurrentProcess().Handle);
        }

        #endregion cctor

        #region PDB

        internal static PDB PDB { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ass"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool LoadPDBFile(Assembly ass, string file = null)
        {
            if (ass == null)
                return false;
            if (file == null)
            {
                file = Path.Combine(Path.GetDirectoryName(ass.Location), Path.GetFileNameWithoutExtension(ass.CodeBase) + ".dll");
            }
            if (file == null || !File.Exists(file))
                return false;
            try
            {
                var addr = PDB.LoadModuleEx(file);
                if (addr > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public static bool GetType(Type type)
        {
            if (type == null)
                return false;
            var mn = type.Module.ScopeName;
            var mds = PDB.EnumModules();
            ModuleInfo info;
            if (mds == null)
            {
                return false;
            }
            foreach (var item in mds)
            {
                if (item.Name != mn)
                {
                    continue;
                }
                info = item;
                var sfs = PDB.EnumSourceFiles(info.Base);
                if (sfs == null)
                    continue;
                SymbolInfo sinfo = new SymbolInfo()
                {
                    Name = type.FullName,
                };
                return true;
            }
            return false;
        }

        #endregion PDB
    }

    internal sealed class PDB : IDisposable
    {
        private static int _instances;
        private IntPtr _hProcess;
        private bool _ownHandle;

        public void Dispose()
        {
            Win32.SymCleanup(_hProcess);
            if (_ownHandle)
                Win32.CloseHandle(_hProcess);
        }

        private PDB(IntPtr hProcess, bool ownHandle)
        {
            _hProcess = hProcess;
            _ownHandle = ownHandle;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static PDB CreatePDBHandler(int pid, string path = null)
        {
            var hd = new IntPtr(pid);
            if (Win32.SymInitialize(hd, path, true))
            {
                return new PDB(hd, true);
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static PDB CreatePDBHandler(IntPtr pid, string path = null)
        {
            var hd = pid;
            if (Win32.SymInitialize(hd, path, true))
            {
                return new PDB(hd, true);
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dll"></param>
        /// <param name="moduleName"></param>
        /// <param name="hFile"></param>
        /// <returns></returns>
        public ulong LoadModuleEx(string dll, string moduleName = null, IntPtr? hFile = null)
        {
            var address = Win32.SymLoadModuleEx(_hProcess, hFile ?? IntPtr.Zero, dll, moduleName, 0, 0, IntPtr.Zero, 0);
            var error = Marshal.GetLastWin32Error();
            if (address == 0 && error != 0)
                throw new Win32Exception(error);
            return address;
        }

        public SymbolInfo GetSymbolFromAddress(ulong address, out ulong displacement)
        {
            var info = new SymbolInfo();
            info.Init();
            Win32.SymFromAddr(_hProcess, address, out displacement, ref info).ThrowIfWin32Failed();
            return info;
        }

        public IList<ModuleInfo> EnumModules()
        {
            List<ModuleInfo> modules = new List<ModuleInfo>(8);
            Win32.SymEnumerateModules64(_hProcess, (name, dllBase, context) =>
            {
                modules.Add(new ModuleInfo { Name = name, Base = dllBase });
                return true;
            }, IntPtr.Zero).ThrowIfWin32Failed();
            return modules;
        }

        public ICollection<SymbolInfo> EnumSymbols(ulong baseAddress, string mask = "*!*")
        {
            var symbols = new List<SymbolInfo>(16);
            Win32.SymEnumSymbols(_hProcess, baseAddress, mask, (ref SymbolInfo symbol, uint size, IntPtr context) =>
            {
                symbols.Add(symbol);
                return true;
            }, IntPtr.Zero);
            return symbols;
        }

        public IList<SymbolInfo> EnumTypes(ulong baseAddress, string mask = "*")
        {
            var symbols = new List<SymbolInfo>(16);
            Win32.SymEnumTypesByName(_hProcess, baseAddress, mask, (ref SymbolInfo symbol, uint size, IntPtr context) =>
            {
                symbols.Add(symbol);
                return true;
            }, IntPtr.Zero);
            return symbols;
        }

        public unsafe IList<SourceFile> EnumSourceFiles(ulong baseAddress, string mask = "*")
        {
            var files = new List<SourceFile>(4);
            Win32.SymEnumSourceFiles(_hProcess, baseAddress, mask, (source, context) =>
            {
                files.Add(new SourceFile
                {
                    BaseAddress = source.ModuleBase,
                    //FileName = source.FileName,
                });
                return true;
            }, IntPtr.Zero);
            return files;
        }

        public bool GetSourceFile(ulong addr)
        {
            //string f1, f2;
            //int s1;
            IntPtr p1 = Marshal.AllocHGlobal(1024), p2 = Marshal.AllocHGlobal(1024);
            int s1;
            var r = Win32.SymGetSourceFile(_hProcess, addr, IntPtr.Zero, p1, p2, out s1);
            var error = Marshal.GetLastWin32Error();
            if (addr == 0 && error != 0)
                throw new Win32Exception(error);
            return r;
        }

        public void GetInfo(string name)
        {
            //SymbolInfo info = new SymbolInfo();
            Win32.SymEnumTypes(_hProcess, 268435456, EnumSymbolCallback, IntPtr.Zero);
            //var bn = Win32.SymFromName(_hProcess, name, ref info);
            //return bn;
        }

        private bool EnumSymbolCallback(ref SymbolInfo symbol, uint symbolSize, IntPtr context)
        {
            return false;
        }
    }

    #region Symbol

    public sealed class SymbolHandler : IDisposable
    {
        private static int _instances;
        private IntPtr _hProcess;
        private bool _ownHandle;

        private SymbolHandler(IntPtr hProcess, bool ownHandle)
        {
            _hProcess = hProcess;
            _ownHandle = ownHandle;
        }

        public static SymbolHandler CreateFromProcess(int pid, SymbolOptions options = SymbolOptions.None, string searchPath = null)
        {
            if (searchPath == null)
                searchPath = GetDefaultSearchPath();
            var handle = new IntPtr(pid);
            if (Win32.SymInitialize(handle, searchPath, true))
            {
                return new SymbolHandler(handle, false);
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public static SymbolHandler TryCreateFromProcess(int pid, SymbolOptions options = SymbolOptions.None, string searchPath = null)
        {
            if (searchPath == null)
                searchPath = GetDefaultSearchPath();
            var handle = new IntPtr(pid);
            if (Win32.SymInitialize(handle, searchPath, true))
            {
                return new SymbolHandler(handle, false);
            }
            return null;
        }

        public static SymbolHandler CreateFromHandle(IntPtr handle, SymbolOptions options = SymbolOptions.None, string searchPath = null)
        {
            if (searchPath == null)
                searchPath = GetDefaultSearchPath();
            Win32.SymSetOptions(options);
            Win32.SymInitialize(handle, searchPath, true).ThrowIfWin32Failed();
            return new SymbolHandler(handle, false);
        }

        private static string GetDefaultSearchPath()
        {
            var path = Environment.GetEnvironmentVariable("_NT_SYMBOL_PATH");
            if (path != null)
            {
                int index = path.IndexOf('*');
                if (index < 0)
                    return path;
                path = path.Substring(index + 1, path.IndexOf('*', index + 1) - index - 1);
                if (string.IsNullOrWhiteSpace(path))
                    path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            return path;
        }

        public static SymbolHandler Create(SymbolOptions options = SymbolOptions.CaseInsensitive | SymbolOptions.UndecorateNames, string searchPath = null)
        {
            if (Debugger.IsAttached)
                options |= SymbolOptions.Debug;
            if (searchPath == null)
                searchPath = GetDefaultSearchPath();
            Win32.SymSetOptions(options);
            var handle = new IntPtr(++_instances);
            Win32.SymInitialize(handle, searchPath, false).ThrowIfWin32Failed();
            return new SymbolHandler(handle, false);
        }

        public void Dispose()
        {
            Win32.SymCleanup(_hProcess);
            if (_ownHandle)
                Win32.CloseHandle(_hProcess);
        }

        public ulong LoadSymbolsForModule(string imageName, ulong dllBase = 0, string moduleName = null, IntPtr? hFile = null)
        {
            var address = Win32.SymLoadModuleEx(_hProcess, hFile ?? IntPtr.Zero, imageName, moduleName, dllBase, 0, IntPtr.Zero, 0);
            var error = Marshal.GetLastWin32Error();
            if (address == 0 && error != 0)
                throw new Win32Exception(error);
            return address;
        }

        public ulong TryLoadSymbolsForModule(string imageName, string moduleName = null, IntPtr? hFile = null)
        {
            var address = Win32.SymLoadModuleEx(_hProcess, hFile ?? IntPtr.Zero, imageName, moduleName, 0, 0, IntPtr.Zero, 0);
            return address;
        }

        public SymbolInfo GetSymbolFromAddress(ulong address, out ulong displacement)
        {
            var info = new SymbolInfo();
            info.Init();
            Win32.SymFromAddr(_hProcess, address, out displacement, ref info).ThrowIfWin32Failed();
            return info;
        }

        public bool TryGetSymbolFromAddress(ulong address, ref SymbolInfo symbol, out ulong displacement)
        {
            symbol.Init();
            return Win32.SymFromAddr(_hProcess, address, out displacement, ref symbol);
        }

        public IList<ModuleInfo> EnumModules()
        {
            List<ModuleInfo> modules = new List<ModuleInfo>(8);
            Win32.SymEnumerateModules64(_hProcess, (name, dllBase, context) =>
            {
                modules.Add(new ModuleInfo { Name = name, Base = dllBase });
                return true;
            }, IntPtr.Zero).ThrowIfWin32Failed();
            return modules;
        }

        public bool GetSymbolFromName(string name, ref SymbolInfo symbol)
        {
            return Win32.SymFromName(_hProcess, name, ref symbol);
        }

        public bool GetSymbolFromIndex(ulong dllBase, int index, ref SymbolInfo symbol)
        {
            return Win32.SymFromIndex(_hProcess, dllBase, index, ref symbol);
        }

        public ICollection<SymbolInfo> EnumSymbols(ulong baseAddress, string mask = "*!*")
        {
            var symbols = new List<SymbolInfo>(16);

            Win32.SymEnumSymbols(_hProcess, baseAddress, mask, (ref SymbolInfo symbol, uint size, IntPtr context) =>
            {
                symbols.Add(symbol);
                return true;
            }, IntPtr.Zero);
            return symbols;
        }

        public IList<SymbolInfo> EnumTypes(ulong baseAddress, string mask = "*")
        {
            var symbols = new List<SymbolInfo>(16);
            Win32.SymEnumTypesByName(_hProcess, baseAddress, mask, (ref SymbolInfo symbol, uint size, IntPtr context) =>
            {
                symbols.Add(symbol);
                return true;
            }, IntPtr.Zero);
            return symbols;
        }

        public IList<SourceFile> EnumSourceFiles(ulong baseAddress, string mask = "*")
        {
            var files = new List<SourceFile>(4);
            Win32.SymEnumSourceFiles(_hProcess, baseAddress, mask, (source, context) =>
            {
                var sf = new SourceFile()
                {
                    BaseAddress = source.ModuleBase,
                };
                if (source.FileName != null)
                {
                    var buff = new byte[1024];
                    //Marshal.Copy(source.FileName,buff,0,)
                    //sf.FileName = source.FileName;
                }
                files.Add(sf);
                return true;
            }, IntPtr.Zero);

            return files;
        }

        public int GetTypeIndexFromName(ulong baseAddress, string name)
        {
            var symbol = SymbolInfo.Create();
            return Win32.SymGetTypeFromName(_hProcess, baseAddress, name, ref symbol) ? symbol.TypeIndex : 0;
        }

        public bool GetTypeFromName(ulong baseAddress, string name, ref SymbolInfo type)
            => Win32.SymGetTypeFromName(_hProcess, baseAddress, name, ref type);

        public bool Refresh() => Win32.SymRefreshModuleList(_hProcess);

        public string GetTypeInfoName(ulong dllBase, int typeIndex)
        {
            StringBuilder name = new StringBuilder(1024);
            if (Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.Name, out name))
            {
                var strName = name.ToString();
                //Marshal.FreeCoTaskMem(new IntPtr(name));
                return strName;
            }
            return null;
        }

        public SymbolTag GetSymbolTag(ulong dllBase, int typeIndex)
        {
            var tag = SymbolTag.Null;
            Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.Tag, out tag);
            return tag;
        }

        public Variant GetSymbolValue(ulong dllBase, int typeIndex)
        {
            var value = new Variant();
            Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.Value, out value);
            return value;
        }

        public ulong GetSymbolLength(ulong dllBase, int typeIndex)
        {
            Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.Length, out ulong value);
            return value;
        }

        public int GetSymbolDataKind(ulong dllBase, int typeIndex)
        {
            int value = 0;
            Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.DataKind, out value);
            return value;
        }

        public UdtKind GetSymbolUdtKind(ulong dllBase, int typeIndex)
        {
            if (Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.UdtKind, out int value))
            {
                return (UdtKind)value;
            }
            return UdtKind.Unknown;
        }

        public int GetSymbolType(ulong dllBase, int typeIndex)
        {
            Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.Type, out int value);
            return value;
        }

        public int GetSymbolBitPosition(ulong dllBase, int typeIndex)
        {
            if (Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.BitPosition, out int value))
                return value;
            return -1;
        }

        public int GetSymbolCount(ulong dllBase, int typeIndex)
        {
            int value = -1;
            Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.Count, out value);
            return value;
        }

        public int GetSymbolAddressOffset(ulong dllBase, int typeIndex)
        {
            bool success = Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.AddressOffset, out int value);
            return value;
        }

        public BasicType GetSymbolBaseType(ulong dllBase, int typeIndex)
        {
            bool success = Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.BaseType, out int value);
            return (BasicType)value;
        }

        public int GetSymbolOffset(ulong dllBase, int typeIndex)
        {
            bool success = Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.Offset, out int value);
            return value;
        }

        public int GetSymbolChildrenCount(ulong dllBase, int typeIndex)
        {
            bool success = Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.ChildrenCount, out int value);
            return value;
        }

        public StructDescriptor BuildStructDescriptor(ulong dllBase, int typeIndex)
        {
            if (Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.ChildrenCount, out int childrenCount))
            {
                var structDesc = new StructDescriptor(childrenCount);
                if (Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.Length, out ulong size))
                {
                    structDesc.Length = (int)size;
                }
                var childrenParams = new FindChildrenParams { Count = childrenCount };
                structDesc.Length = (int)size;
                if (Win32.SymGetTypeInfo(_hProcess, dllBase, typeIndex, SymbolTypeInfo.FindChildren, ref childrenParams))
                {
                    for (var i = 0; i < childrenParams.Count; i++)
                    {
                        var sym = SymbolInfo.Create();
                        var child = childrenParams.Child[i];
                        if (GetSymbolFromIndex(dllBase, child, ref sym))
                        {
                            if (Win32.SymGetTypeInfo(_hProcess, dllBase, child, SymbolTypeInfo.Offset, out int offset) &&
                                Win32.SymGetTypeInfo(_hProcess, dllBase, child, SymbolTypeInfo.Tag, out SymbolTag tag))
                            {
                                sym.Tag = tag;
                                sym.TypeIndex = child;
                                var member = new StructMember(sym, offset);
                                structDesc.AddMember(member);
                            }
                            else if (Win32.SymGetTypeInfo(_hProcess, dllBase, child, SymbolTypeInfo.Value, out Variant value))
                            {
                                sym.Tag = SymbolTag.Enum;
                                sym.Value = value.lValue;
                                sym.TypeIndex = child;
                                var member = new StructMember(sym, 0);
                                switch (sym.Size)
                                {
                                    case 8:
                                        member.Value = value.lValue;
                                        break;

                                    case 2:
                                        member.Value = value.sValue;
                                        break;

                                    case 1:
                                        member.Value = value.bValue;
                                        break;

                                    default:
                                        member.Value = value.iValue;
                                        break;
                                }
                                structDesc.AddMember(member);
                            }
                        }
                    }
                }
                return structDesc;
            }
            return null;
        }
    }

    internal static class Extensions
    {
        public static bool ThrowIfWin32Failed(this bool ok, int error = 0)
        {
            if (!ok)
                throw new Win32Exception(error != 0 ? error : Marshal.GetLastWin32Error());
            return true;
        }
    }

    #region Model

    public class SourceFile
    {
        public ulong BaseAddress { get; set; }
        public string FileName { get; set; }
    }

    public class SourceFileLine
    {
        public SourceFile SourceFile { get; }
        public string Line { get; }

        public SourceFileLine(SourceFile file, string line)
        {
            SourceFile = file;
            Line = line;
        }
    }

    [DebuggerDisplay("{Name,nq} Base={Base,d}")]
    public sealed class ModuleInfo
    {
        public string Name { get; set; }
        public ulong Base { get; set; }
    }

    [DebuggerDisplay("{Name,nq} offset={Offset,d} size={Size,d}")]
    public sealed class StructMember
    {
        public readonly int Offset;
        public StructDescriptor Parent { get; internal set; }
        public readonly SymbolInfo Symbol;
        public string Name => Symbol.Name;
        public int Size => Symbol.Size;
        public long Value;

        public StructMember(in SymbolInfo symbol, int offset)
        {
            Symbol = symbol;
            Offset = offset;
        }

        public StructMember Clone()
        {
            var member = (StructMember)MemberwiseClone();
            member.Parent = null;
            return member;
        }

        public int TypeId => Symbol.TypeIndex;

        public override string ToString() => $"{Symbol.Name}, size={Symbol.Size}, offset={Offset}, typeid={Symbol.TypeIndex} tag={Symbol.Tag}";
    }

    public sealed class StructDescriptor : IList<StructMember>
    {
        private readonly Dictionary<string, StructMember> _membersByName;
        private readonly List<StructMember> _members;

        internal StructDescriptor(int capacity = 8)
        {
            _membersByName = new Dictionary<string, StructMember>(capacity, StringComparer.InvariantCultureIgnoreCase);
            _members = new List<StructMember>(capacity);
        }

        public int GetOffsetOf(string memberName)
        {
            return _membersByName.TryGetValue(memberName, out var member) ? member.Offset : -1;
        }

        public StructMember GetMember(string memberName)
        {
            return _membersByName.TryGetValue(memberName, out var member) ? member : null;
        }

        public int Length { get; internal set; }

        public int Count => _members.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        StructMember IList<StructMember>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public StructMember this[int index] => _members[index];

        public void AddMember(StructMember member)
        {
            member.Parent = this;
            _membersByName.Add(member.Symbol.Name, member);
            _members.Add(member);
        }

        public IEnumerator<StructMember> GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(StructMember item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, StructMember item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(StructMember item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(StructMember item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(StructMember[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(StructMember item)
        {
            throw new NotImplementedException();
        }
    }

    #endregion Model

    #region Win32

    #region Data

    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum SymbolOptions : uint
    {
        None = 0,
        AllowAbsoluteSymbols = 0x800,
        AllowZeroAddress = 0x1000000,
        AutoPublics = 0x10000,
        CaseInsensitive = 1,
        Debug = 0x80000000,
        DeferredLoads = 0x4,
        DisableSymSrvAutoDetect = 0x2000000,
        ExactSymbols = 0x400,
        FailCriticalErrors = 0x200,
        FavorCompressed = 0x800000,
        FlatDirectory = 0x400000,
        IgnoreCodeViewRecord = 0x80,
        IgnoreImageDir = 0x200000,
        IgnoreNTSymbolPath = 0x1000,
        Include32BitModules = 0x2000,
        LoadAnything = 0x40,
        LoadLines = 0x10,
        NoCPP = 0x8,
        NoImageSearch = 0x20000,
        NoPrompts = 0x80000,
        NoPublics = 0x8000,
        NoUnqualifiedLoads = 0x100,
        Overwrite = 0x100000,
        PublicsOnly = 0x4000,
        Secure = 0x40000,
        UndecorateNames = 0x2
    }

    [Flags]
    public enum SymbolFlags : uint
    {
        None = 0,
        ClrToken = 0x40000,
        Constant = 0x100,
        Export = 0x200,
        Forwarder = 0x400,
        FrameRelative = 0x20,
        Function = 0x800,
        ILRelative = 0x10000,
        Local = 0x80,
        Metadata = 0x20000,
        Parameter = 0x40,
        Register = 0x8,
        RegisterRelative = 0x10,
        Slot = 0x8000,
        Thunk = 0x2000,
        TLSRelative = 0x4000,
        ValuePresent = 1,
        Virtual = 0x1000,
        Null = 0x80000,
        FunctionNoReturn = 0x100000,
        SyntheticZeroBase = 0x200000,
        PublicCode = 0x400000
    }

    [DebuggerDisplay("{Name} {Value} {Scope} {Address}")]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SymbolInfo
    {
        // member name cannot be larger than this (docs says 2000, but seems wasteful in practice)
        private const int MaxSymbolLen = 500;

        public static SymbolInfo Create()
        {
            var symbol = new SymbolInfo();
            symbol.Init();
            return symbol;
        }

        public void Init()
        {
            MaxNameLen = MaxSymbolLen;
            SizeOfStruct = 88;
        }

        public int SizeOfStruct;
        public int TypeIndex;
        private readonly ulong Reserved1, Reserved2;
        public int Index;
        public int Size;
        public ulong ModuleBase;
        public SymbolFlags Flags;
        public long Value;
        public ulong Address;
        public uint Register;
        public uint Scope;
        public SymbolTag Tag;
        public uint NameLen;
        public int MaxNameLen;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxSymbolLen)]
        public string Name;
    }

    internal enum ProcessAccessMask : uint
    {
        Query = 0x400
    }

    #endregion Data

    #region enum

    internal delegate bool SymEnumerateModuleProc(string module, ulong dllBase, IntPtr context);

    internal delegate bool EnumSourceFilesCallback(SOURCEFILE sourceFile, IntPtr context);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SOURCEFILE
    {
        public ulong ModuleBase;
        public IntPtr FileName;
    }

    public enum BasicType
    {
        NoType = 0,
        Void = 1,
        Char = 2,
        WChar = 3,
        Int = 6,
        UInt = 7,
        Float = 8,
        BCD = 9,
        Bool = 10,
        Long = 13,
        ULong = 14,
        Currency = 25,
        Date = 26,
        Variant = 27,
        Complex = 28,
        Bit = 29,
        BSTR = 30,
        Hresult = 31
    }

    public enum UdtKind
    {
        Struct,
        Class,
        Union,
        Interface,
        Unknown = 99
    }

    public enum SymbolTag
    {
        Null,
        Exe,
        Compiland,
        CompilandDetails,
        CompilandEnv,
        Function,
        Block,
        Data,
        Annotation,
        Label,
        PublicSymbol,
        UDT,
        Enum,
        FunctionType,
        PointerType,
        ArrayType,
        BaseType,
        Typedef,
        BaseClass,
        Friend,
        FunctionArgType,
        FuncDebugStart,
        FuncDebugEnd,
        UsingNamespace,
        VTableShape,
        VTable,
        Custom,
        Thunk,
        CustomType,
        ManagedType,
        Dimension,
        CallSite,
        InlineSite,
        BaseInterface,
        VectorType,
        MatrixType,
        HLSLType,
        Caller,
        Callee,
        Export,
        HeapAllocationSite,
        CoffGroup,
        Max
    }

    public enum SymbolTypeInfo
    {
        Tag,
        Name,
        Length,
        Type,
        TypeId,
        BaseType,
        ArrayIndexTypeId,
        FindChildren,
        DataKind,
        AddressOffset,
        Offset,
        Value,
        Count,
        ChildrenCount,
        BitPosition,
        VirtualBaseClass,
        VIRTUALTABLESHAPEID,
        VIRTUALBASEPOINTEROFFSET,
        ClassParentId,
        Nested,
        SymIndex,
        LexicalParent,
        Address,
        ThisAdjust,
        UdtKind,
        IsEquivalentTo,
        CallingConvention,
        IsCloseEquivalentTo,
        GTIEX_REQS_VALID,
        VirtualBaseOffset,
        VirtualBaseDispIndex,
        IsReference,
        IndirectVirtualBaseClass
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct FindChildrenParams
    {
        public int Count;
        public int Start;

        // hopefully no more than 256 members

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 400)]
        public int[] Child;
    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct Variant
    {
        [FieldOffset(0)] public short vt;
        [FieldOffset(8)] public double dValue;
        [FieldOffset(8)] public int iValue;
        [FieldOffset(8)] public long lValue;
        [FieldOffset(8)] public byte bValue;
        [FieldOffset(8)] public short sValue;

        public override string ToString()
        {
            return $"VT: {vt} iValue: {iValue}";
        }
    }

    #endregion enum

    [SuppressUnmanagedCodeSecurity]
    internal static class Win32
    {
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessMask access, bool inheritHandle, int pid);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern SymbolOptions SymSetOptions(SymbolOptions options);

        [DllImport("dbghelp", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SymInitializeW", ExactSpelling = true)]
        public static extern bool SymInitialize(IntPtr hProcess, string searchPath, bool invadeProcess);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymCleanup(IntPtr hProcess);

        [DllImport("dbghelp", SetLastError = true, EntryPoint = "SymLoadModuleExW", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern ulong SymLoadModuleEx(IntPtr hProcess, IntPtr hFile, string imageName, string moduleName,
            ulong baseOfDll, uint dllSize, IntPtr data, uint flags);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymFromAddr(IntPtr hProcess, ulong address, out ulong displacement, ref SymbolInfo symbol);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymEnumerateModules64(IntPtr hProcess, SymEnumerateModuleProc proc, IntPtr context);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymRefreshModuleList(IntPtr hProcess);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymFromName(IntPtr hProcess, string name, ref SymbolInfo symbol);

        public delegate bool EnumSymbolCallback(ref SymbolInfo symbol, uint symbolSize, IntPtr context);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymEnumSymbols(IntPtr hProcess, ulong baseOfDll, string mask, EnumSymbolCallback callback, IntPtr context);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymEnumTypes(IntPtr hProcess, ulong baseOfDll, EnumSymbolCallback callback, IntPtr context);

        [DllImport("dbghelp", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SymEnumSourceFiles(IntPtr hProcess, ulong baseOfDll, string mask, EnumSourceFilesCallback callback, IntPtr context);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymEnumTypesByName(IntPtr hProcess, ulong baseOfDll, string mask, EnumSymbolCallback callback, IntPtr context);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymGetTypeInfo(IntPtr hProcess, ulong baseOfDll, int typeId, SymbolTypeInfo typeinfo, out StringBuilder value);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymGetTypeInfo(IntPtr hProcess, ulong baseOfDll, int typeId, SymbolTypeInfo typeinfo, out SymbolTag tag);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymGetTypeInfo(IntPtr hProcess, ulong baseOfDll, int typeId, SymbolTypeInfo typeinfo, out UdtKind tag);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymGetTypeInfo(IntPtr hProcess, ulong baseOfDll, int typeId, SymbolTypeInfo typeinfo, out Variant value);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymGetTypeInfo(IntPtr hProcess, ulong baseOfDll, int typeId, SymbolTypeInfo typeinfo, out int value);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymGetTypeInfo(IntPtr hProcess, ulong baseOfDll, int typeId, SymbolTypeInfo typeinfo, out ulong value);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymGetTypeInfo(IntPtr hProcess, ulong baseOfDll, int typeId, SymbolTypeInfo typeinfo, ref FindChildrenParams value);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymFromIndex(IntPtr hProcess, ulong dllBase, int index, ref SymbolInfo symbol);

        [DllImport("dbghelp", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SymGetTypeFromName(IntPtr hProcess, ulong dllBase, string name, ref SymbolInfo symbol);

        [DllImport("dbghelp", SetLastError = true)]
        public static extern bool SymGetSourceFile(IntPtr hProcess, ulong dllBase, IntPtr Params, IntPtr FileSpec, IntPtr FilePath, out int size);
    }

    #endregion Win32

    #region WinAPI

    internal class DBG
    {
        public const string Kernale32 = "kernel32";
        public const string DbgHelp = "dbghelp";

        [DllImport(Kernale32, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport(Kernale32, SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessMask access, bool inheritHandle, int pid);

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [DllImport(DbgHelp, SetLastError = true)]
        public static extern SymbolOptions SymSetOptions(SymbolOptions options);

        [DllImport(DbgHelp, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SymInitializeW", ExactSpelling = true)]
        public static extern bool SymInitialize(IntPtr hProcess, string searchPath, bool invadeProcess);

        [DllImport(DbgHelp, SetLastError = true)]
        public static extern bool SymCleanup(IntPtr hProcess);

        [DllImport(DbgHelp, SetLastError = true, EntryPoint = "SymLoadModuleExW", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern ulong SymLoadModuleEx(IntPtr hProcess, IntPtr hFile, string imageName, string moduleName,
            ulong baseOfDll, uint dllSize, IntPtr data, uint flags);
    }

    #endregion WinAPI

    #endregion Symbol
}

/*

 Debug Help Library：https://docs.microsoft.com/zh-cn/windows/win32/debug/debug-help-library
 PBD文件：https://docs.microsoft.com/zh-cn/previous-versions/visualstudio/visual-studio-2010/yd4f8bd1(v=vs.100)?redirectedfrom=MSDN

     */