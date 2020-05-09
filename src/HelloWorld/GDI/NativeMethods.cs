using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace ZHello.GDI
{
    [SecuritySafeCritical]
    public class NativeMethods
    {
        [Flags]
        public enum KeyState
        {
            None = 0x0,
            Down = 0x1,
            Toggled = 0x2
        }

        internal struct APPBARDATA
        {
            public int cbSize;

            public IntPtr hWnd;

            public uint uCallbackMessage;

            public uint uEdge;

            public RECT rc;

            public IntPtr lParam;
        }

        public struct WNDCLASS
        {
            public int style;

            [MarshalAs(UnmanagedType.FunctionPtr)]
            public WndProc lpfnWndProc;

            public int cbClsExtra;

            public int cbWndExtra;

            public IntPtr hInstance;

            public IntPtr hIcon;

            public IntPtr hCursor;

            public IntPtr hbrBackground;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszMenuName;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszClassName;
        }

        public enum ShellAddToRecentDocs
        {
            Pidl = 1,
            PathA,
            PathW,
            AppIdInfo,
            AppIdInfoIdList,
            Link,
            AppIdInfoLink,
            ShellItem
        }

        public enum FLASHW
        {
            FLASHW_STOP = 0,
            FLASHW_CAPTION = 1,
            FLASHW_TRAY = 2,
            FLASHW_ALL = 3,
            FLASHW_TIMER = 4,
            FLASHW_TIMERNOFG = 12
        }

        internal struct FLASHWINFO
        {
            public uint cbSize;

            public IntPtr hwnd;

            public uint dwFlags;

            public uint uCount;

            public uint dwTimeout;
        }

        [StructLayout(LayoutKind.Explicit)]
        public class PropVariant : IDisposable
        {
            [FieldOffset(0)]
            private ushort valueType;

            [FieldOffset(8)]
            private IntPtr ptr;

            [FieldOffset(8)]
            private int int32;

            public PropVariant(string value)
            {
                if (value != null)
                {
                    valueType = 31;
                    ptr = Marshal.StringToCoTaskMemUni(value);
                }
            }

            public PropVariant(bool value)
            {
                valueType = 11;
                int32 = (value ? (-1) : 0);
            }

            public void Dispose()
            {
                PropVariantClear(this);
                GC.SuppressFinalize(this);
            }
        }

        public enum ShowWindowCommands
        {
            Hide = 0,
            Normal = 1,
            ShowMinimized = 2,
            Maximize = 3,
            ShowMaximized = 3,
            ShowNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActive = 7,
            ShowNA = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimize = 11
        }

        [Serializable]
        public struct WINDOWPLACEMENT
        {
            public int Length;

            public int Flags;

            public ShowWindowCommands ShowCmd;

            public POINT MinPosition;

            public POINT MaxPosition;

            public RECT NormalPosition;

            public static WINDOWPLACEMENT Create()
            {
                WINDOWPLACEMENT wINDOWPLACEMENT = default(WINDOWPLACEMENT);
                wINDOWPLACEMENT.Length = Marshal.SizeOf((object)wINDOWPLACEMENT);
                wINDOWPLACEMENT.MaxPosition = new POINT(-1, -1);
                wINDOWPLACEMENT.MinPosition = new POINT(-1, -1);
                wINDOWPLACEMENT.NormalPosition = default(RECT);
                return wINDOWPLACEMENT;
            }
        }

        public struct BLENDFUNCTION
        {
            public byte BlendOp;

            public byte BlendFlags;

            public byte SourceConstantAlpha;

            public byte AlphaFormat;
        }

        public enum ScrollWindowExFlags
        {
            SW_SCROLLCHILDREN = 1,
            SW_INVALIDATE = 2,
            SW_ERASE = 4,
            SW_SMOOTHSCROLL = 0x10
        }

        public struct Margins
        {
            public int Left;

            public int Right;

            public int Top;

            public int Bottom;
        }

        public struct BITMAPINFO_SMALL
        {
            public int biSize;

            public int biWidth;

            public int biHeight;

            public short biPlanes;

            public short biBitCount;

            public int biCompression;

            public int biSizeImage;

            public int biXPelsPerMeter;

            public int biYPelsPerMeter;

            public int biClrUsed;

            public int biClrImportant;

            public byte bmiColors_rgbBlue;

            public byte bmiColors_rgbGreen;

            public byte bmiColors_rgbRed;

            public byte bmiColors_rgbReserved;
        }

        public struct BITMAPINFO_FLAT
        {
            public int biSize;

            public int biWidth;

            public int biHeight;

            public short biPlanes;

            public short biBitCount;

            public int biCompression;

            public int bmiHeader_biSizeImage;

            public int bmiHeader_biXPelsPerMeter;

            public int bmiHeader_biYPelsPerMeter;

            public int bmiHeader_biClrUsed;

            public int bmiHeader_biClrImportant;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public byte[] bmiColors;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class BITMAPINFOHEADER
        {
            public int biSize;

            public int biWidth;

            public int biHeight;

            public short biPlanes;

            public short biBitCount;

            public int biCompression;

            public int biSizeImage;

            public int biXPelsPerMeter;

            public int biYPelsPerMeter;

            public int biClrUsed;

            public int biClrImportant;

            public BITMAPINFOHEADER()
            {
                biSize = 40;
            }
        }

        public struct HWND : IWin32Window
        {
            private IntPtr _Handle;

            public static readonly HWND Empty = new HWND(IntPtr.Zero);

            public static HWND Desktop => GetDesktopWindow();

            public bool IsEmpty => _Handle == IntPtr.Zero;

            public bool IsVisible => IsWindowVisible(_Handle);

            public IntPtr Handle => _Handle;

            public HWND(IntPtr aValue)
            {
                _Handle = aValue;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                if (obj is HWND)
                {
                    return Equals((HWND)obj);
                }
                if (obj is IntPtr)
                {
                    return Equals((IntPtr)obj);
                }
                return false;
            }

            public bool Equals(IntPtr ptr)
            {
                if (!_Handle.ToInt32().Equals(ptr.ToInt32()))
                {
                    return false;
                }
                return true;
            }

            public bool Equals(HWND hwnd)
            {
                return Equals(hwnd._Handle);
            }

            public bool Equals(IWin32Window window)
            {
                return Equals(window.Handle);
            }

            public override int GetHashCode()
            {
                return _Handle.GetHashCode();
            }

            public override string ToString()
            {
                return "{Handle=0x" + _Handle.ToInt32().ToString("X8") + "}";
            }

            public static bool operator ==(HWND aHwnd1, HWND aHwnd2)
            {
                if ((object)aHwnd1 == null)
                {
                    return (object)aHwnd2 == null;
                }
                return aHwnd1.Equals(aHwnd2);
            }

            public static bool operator ==(IntPtr aIntPtr, HWND aHwnd)
            {
                if ((object)aIntPtr == null)
                {
                    return (object)aHwnd == null;
                }
                return aHwnd.Equals(aIntPtr);
            }

            public static bool operator ==(HWND aHwnd, IntPtr aIntPtr)
            {
                if ((object)aHwnd == null)
                {
                    return (object)aIntPtr == null;
                }
                return aHwnd.Equals(aIntPtr);
            }

            public static bool operator !=(HWND aHwnd1, HWND aHwnd2)
            {
                return !(aHwnd1 == aHwnd2);
            }

            public static bool operator !=(IntPtr aIntPtr, HWND aHwnd)
            {
                return !(aIntPtr == aHwnd);
            }

            public static bool operator !=(HWND aHwnd, IntPtr aIntPtr)
            {
                return !(aHwnd == aIntPtr);
            }

            public static implicit operator IntPtr(HWND aHwnd)
            {
                return aHwnd.Handle;
            }

            public static implicit operator HWND(IntPtr aIntPtr)
            {
                return new HWND(aIntPtr);
            }
        }

        public struct HDC
        {
            private IntPtr _Handle;

            public static readonly HDC Empty = new HDC(0);

            public IntPtr Handle => _Handle;

            public bool IsEmpty => _Handle == IntPtr.Zero;

            public HDC(IntPtr aValue)
            {
                _Handle = aValue;
            }

            public HDC(int aValue)
            {
                _Handle = new IntPtr(aValue);
            }

            public override bool Equals(object aObj)
            {
                if (aObj == null)
                {
                    return false;
                }
                if (aObj is HDC)
                {
                    return Equals((HDC)aObj);
                }
                if (aObj is IntPtr)
                {
                    return Equals((IntPtr)aObj);
                }
                return false;
            }

            public bool Equals(HDC aHDC)
            {
                if (!_Handle.Equals(aHDC._Handle))
                {
                    return false;
                }
                return true;
            }

            public bool Equals(IntPtr aIntPtr)
            {
                if (!_Handle.Equals(aIntPtr))
                {
                    return false;
                }
                return true;
            }

            public override int GetHashCode()
            {
                return _Handle.GetHashCode();
            }

            public override string ToString()
            {
                return "{Handle=0x" + _Handle.ToInt32().ToString("X8") + "}";
            }

            public void Release(HWND window)
            {
                ReleaseDC(window, this);
            }

            //public unsafe IntPtr SelectObject(IntPtr aGDIObj)
            //{
            //	//return NativeMethods.SelectObject((IntPtr)(void*)this, aGDIObj);
            //}

            public HDC CreateCompatible()
            {
                return CreateCompatibleDC(_Handle);
            }

            public IntPtr CreateCompatibleBitmap(int width, int height)
            {
                return NativeMethods.CreateCompatibleBitmap(_Handle, width, height);
            }

            public IntPtr CreateCompatibleBitmap(Rectangle rectangle)
            {
                return CreateCompatibleBitmap(rectangle.Width, rectangle.Height);
            }

            public void Delete()
            {
                DeleteDC(_Handle);
            }

            public static bool operator ==(HDC aHdc1, HDC aHdc2)
            {
                if ((object)aHdc1 == null)
                {
                    return (object)aHdc2 == null;
                }
                return aHdc1.Equals(aHdc2);
            }

            public static bool operator ==(IntPtr aIntPtr, HDC aHdc)
            {
                if ((object)aIntPtr == null)
                {
                    return (object)aHdc == null;
                }
                return aHdc.Equals(aIntPtr);
            }

            public static bool operator ==(HDC aHdc, IntPtr aIntPtr)
            {
                if ((object)aHdc == null)
                {
                    return (object)aIntPtr == null;
                }
                return aHdc.Equals(aIntPtr);
            }

            public static bool operator !=(HDC aHdc1, HDC aHdc2)
            {
                return !(aHdc1 == aHdc2);
            }

            public static bool operator !=(IntPtr aIntPtr, HDC aHdc)
            {
                return !(aIntPtr == aHdc);
            }

            public static bool operator !=(HDC aHdc, IntPtr aIntPtr)
            {
                return !(aHdc == aIntPtr);
            }

            public static implicit operator IntPtr(HDC aHdc)
            {
                return aHdc.Handle;
            }

            public static implicit operator HDC(IntPtr aIntPtr)
            {
                return new HDC(aIntPtr);
            }
        }

        [CLSCompliant(false)]
        public struct COLORREF
        {
            private uint _ColorRef;

            public COLORREF(Color aValue)
            {
                int num = aValue.ToArgb();
                int num2 = (num & 0xFF) << 16;
                num &= 0xFFFF00;
                num |= ((num >> 16) & 0xFF);
                num &= 0xFFFF;
                num = (int)(_ColorRef = (uint)(num | num2));
            }

            public COLORREF(int lRGB)
            {
                _ColorRef = (uint)lRGB;
            }

            public Color ToColor()
            {
                int red = (int)(_ColorRef & 0xFF);
                int green = ((int)_ColorRef >> 8) & 0xFF;
                int blue = ((int)_ColorRef >> 16) & 0xFF;
                return Color.FromArgb(red, green, blue);
            }

            public static COLORREF FromColor(Color aColor)
            {
                return new COLORREF(aColor);
            }

            public static Color ToColor(COLORREF aColorRef)
            {
                return aColorRef.ToColor();
            }
        }

        public struct HHOOK
        {
            private IntPtr _Handle;

            public static readonly HHOOK Empty = new HHOOK(0);

            public IntPtr Handle => _Handle;

            public bool IsEmpty => _Handle == IntPtr.Zero;

            public HHOOK(IntPtr aValue)
            {
                _Handle = aValue;
            }

            public HHOOK(int aValue)
            {
                _Handle = new IntPtr(aValue);
            }

            public override bool Equals(object aObj)
            {
                if (aObj == null)
                {
                    return false;
                }
                if (aObj is HHOOK)
                {
                    return Equals((HHOOK)aObj);
                }
                if (aObj is IntPtr)
                {
                    return Equals((IntPtr)aObj);
                }
                return false;
            }

            public bool Equals(HHOOK aHHOOK)
            {
                if (!_Handle.Equals(aHHOOK._Handle))
                {
                    return false;
                }
                return true;
            }

            public bool Equals(IntPtr aIntPtr)
            {
                if (!_Handle.Equals(aIntPtr))
                {
                    return false;
                }
                return true;
            }

            public override int GetHashCode()
            {
                return _Handle.GetHashCode();
            }

            public override string ToString()
            {
                return "{Handle=0x" + _Handle.ToInt32().ToString("X8") + "}";
            }

            public static bool operator ==(HHOOK aHHook1, HHOOK aHHook2)
            {
                if ((object)aHHook1 == null)
                {
                    return (object)aHHook2 == null;
                }
                return aHHook1.Equals(aHHook2);
            }

            public static bool operator ==(IntPtr aIntPtr, HHOOK aHHook)
            {
                if ((object)aIntPtr == null)
                {
                    return (object)aHHook == null;
                }
                return aHHook.Equals(aIntPtr);
            }

            public static bool operator ==(HHOOK aHHook, IntPtr aIntPtr)
            {
                if ((object)aHHook == null)
                {
                    return (object)aIntPtr == null;
                }
                return aHHook.Equals(aIntPtr);
            }

            public static bool operator !=(HHOOK aHHook1, HHOOK aHHook2)
            {
                return !(aHHook1 == aHHook2);
            }

            public static bool operator !=(IntPtr aIntPtr, HHOOK aHHook)
            {
                return !(aIntPtr == aHHook);
            }

            public static bool operator !=(HHOOK aHHook, IntPtr aIntPtr)
            {
                return !(aHHook == aIntPtr);
            }

            public static implicit operator IntPtr(HHOOK aHHook)
            {
                return aHHook.Handle;
            }

            public static implicit operator HHOOK(IntPtr aIntPtr)
            {
                return new HHOOK(aIntPtr);
            }
        }

        public struct COPYDATASTRUCT : IDisposable
        {
            public IntPtr dwData;

            public int cbData;

            public IntPtr lpData;

            public void Dispose()
            {
                if (lpData != IntPtr.Zero)
                {
                    LocalFree(lpData);
                    lpData = IntPtr.Zero;
                }
            }
        }

        public enum SystemCursors
        {
            OCR_NORMAL = 32512,
            OCR_IBEAM = 32513,
            OCR_WAIT = 32514,
            OCR_CROSS = 32515,
            OCR_UP = 32516,
            OCR_SIZE = 32640,
            OCR_ICON = 32641,
            OCR_SIZENWSE = 32642,
            OCR_SIZENESW = 32643,
            OCR_SIZEWE = 32644,
            OCR_SIZENS = 32645,
            OCR_SIZEALL = 32646,
            OCR_ICOCUR = 32647,
            OCR_NO = 32648,
            OCR_HAND = 32649,
            OCR_APPSTARTING = 32650
        }

        [Flags]
        public enum RasterOperations
        {
            SRCCOPY = 0xCC0020,
            SRCPAINT = 0xEE0086,
            SRCAND = 0x8800C6,
            SRCINVERT = 0x660046,
            SRCERASE = 0x440328,
            NOTSRCCOPY = 0x330008,
            NOTSRCERASE = 0x1100A6,
            MERGECOPY = 0xC000CA,
            MERGEPAINT = 0xBB0226,
            PATCOPY = 0xF00021,
            PATPAINT = 0xFB0A09,
            PATINVERT = 0x5A0049,
            DSTINVERT = 0x550009,
            BLACKNESS = 0x42,
            WHITENESS = 0xFF0062
        }

        public enum MouseMessages
        {
            WM_LBUTTONDOWN = 513,
            WM_LBUTTONUP = 514,
            WM_MOUSEMOVE = 0x200,
            WM_MOUSEWHEEL = 522,
            WM_RBUTTONDOWN = 516,
            WM_RBUTTONUP = 517
        }

        [Flags]
        public enum PrintOptions
        {
            PRF_CHECKVISIBLE = 0x1,
            PRF_NONCLIENT = 0x2,
            PRF_CLIENT = 0x4,
            PRF_ERASEBKGND = 0x8,
            PRF_CHILDREN = 0x10,
            PRF_OWNED = 0x20
        }

        [StructLayout(LayoutKind.Sequential)]
        public class TRACKMOUSEEVENTStruct
        {
            public int cbSize;

            public int dwFlags;

            public IntPtr hwndTrack;

            public int dwHoverTime;

            public TRACKMOUSEEVENTStruct()
                : this(0, IntPtr.Zero, 0)
            {
            }

            public TRACKMOUSEEVENTStruct(int dwFlags, IntPtr hwndTrack, int dwHoverTime)
            {
                cbSize = Marshal.SizeOf(typeof(TRACKMOUSEEVENTStruct));
                this.dwFlags = dwFlags;
                this.hwndTrack = hwndTrack;
                this.dwHoverTime = dwHoverTime;
            }
        }

        public struct NCCALCSIZE_PARAMS
        {
            public RECT rgrc0;

            public RECT rgrc1;

            public RECT rgrc2;

            public IntPtr lppos;

            [SecuritySafeCritical]
            public static NCCALCSIZE_PARAMS GetFrom(IntPtr lParam)
            {
                return (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(lParam, typeof(NCCALCSIZE_PARAMS));
            }

            [SecuritySafeCritical]
            public void SetTo(IntPtr lParam)
            {
                Marshal.StructureToPtr((object)this, lParam, fDeleteOld: false);
            }
        }

        public struct PAINTSTRUCT
        {
            public IntPtr hdc;

            public bool fErase;

            public RECT rcPaint;

            public bool fRestore;

            public bool fIncUpdate;

            public int reserved1;

            public int reserved2;

            public int reserved3;

            public int reserved4;

            public int reserved5;

            public int reserved6;

            public int reserved7;

            public int reserved8;
        }

        public struct POINT
        {
            public int X;

            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }

            public POINT(Point pt)
            {
                X = pt.X;
                Y = pt.Y;
            }

            public Point ToPoint()
            {
                return new Point(X, Y);
            }
        }

        public struct SIZE
        {
            public int Width;

            public int Height;

            public SIZE(int w, int h)
            {
                Width = w;
                Height = h;
            }

            public SIZE(Size size)
            {
                Width = size.Width;
                Height = size.Height;
            }

            public Size ToSize()
            {
                return new Size(Width, Height);
            }
        }

        public enum RegionDataHeaderTypes
        {
            Rectangles = 1
        }

        public struct RGNDATAHEADER
        {
            public int dwSize;

            public int iType;

            public int nCount;

            public int nRgnSize;

            public RECT rcBound;
        }

        public struct RECT
        {
            public int Left;

            public int Top;

            public int Right;

            public int Bottom;

            public RECT(int l, int t, int r, int b)
            {
                Left = l;
                Top = t;
                Right = r;
                Bottom = b;
            }

            public RECT(Rectangle r)
            {
                Left = r.Left;
                Top = r.Top;
                Right = r.Right;
                Bottom = r.Bottom;
            }

            public Rectangle ToRectangle()
            {
                return Rectangle.FromLTRB(Left, Top, Right, Bottom);
            }

            public void Inflate(int width, int height)
            {
                Left -= width;
                Top -= height;
                Right += width;
                Bottom += height;
            }

            public override string ToString()
            {
                return $"x:{Left},y:{Top},width:{Right - Left},height:{Bottom - Top}";
            }

            [SecuritySafeCritical]
            public static RECT FromLParam(IntPtr lParam)
            {
                return (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
            }
        }

        public enum GetClipBoxReturn
        {
            Error,
            NullRegion,
            SimpleRegion,
            ComplexRegion
        }

        public struct MINMAXINFO
        {
            public POINT ptReserved;

            public POINT ptMaxSize;

            public POINT ptMaxPosition;

            public POINT ptMinTrackSize;

            public POINT ptMaxTrackSize;

            [SecuritySafeCritical]
            public static MINMAXINFO GetFrom(IntPtr lParam)
            {
                return (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            }

            [SecuritySafeCritical]
            public void SetTo(IntPtr lParam)
            {
                Marshal.StructureToPtr((object)this, lParam, fDeleteOld: false);
            }
        }

        public struct GESTUREINFO
        {
            public int cbSize;

            public int dwFlags;

            public int dwID;

            public IntPtr hwndTarget;

            [MarshalAs(UnmanagedType.Struct)]
            internal POINTS ptsLocation;

            public int dwInstanceID;

            public int dwSequenceID;

            public long ullArguments;

            public int cbExtraArgs;
        }

        public struct GESTURECONFIG
        {
            public int dwID;

            public int dwWant;

            public int dwBlock;

            public GESTURECONFIG(int dwID, int dwWant, int dwBlock)
            {
                this.dwID = dwID;
                this.dwWant = dwWant;
                this.dwBlock = dwBlock;
            }
        }

        public struct POINTS
        {
            public short x;

            public short y;

            public Point ToPoint()
            {
                return new Point(x, y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class GESTURENOTIFYSTRUCT
        {
            public int cbSize;

            public int dwFlags;

            public IntPtr hwndTarget;

            [MarshalAs(UnmanagedType.Struct)]
            public POINTS ptsLocation;

            public int dwInstanceID;
        }

        public class WMSZ
        {
            public const int WMSZ_LEFT = 1;

            public const int WMSZ_RIGHT = 2;

            public const int WMSZ_TOP = 3;

            public const int WMSZ_TOPLEFT = 4;

            public const int WMSZ_TOPRIGHT = 5;

            public const int WMSZ_BOTTOM = 6;

            public const int WMSZ_BOTTOMLEFT = 7;

            public const int WMSZ_BOTTOMRIGHT = 8;
        }

        public class SWP
        {
            public const int SWP_NOSIZE = 1;

            public const int SWP_NOMOVE = 2;

            public const int SWP_NOZORDER = 4;

            public const int SWP_NOREDRAW = 8;

            public const int SWP_NOACTIVATE = 16;

            public const int SWP_FRAMECHANGED = 32;

            public const int SWP_DRAWFRAME = 32;

            public const int SWP_SHOWWINDOW = 64;

            public const int SWP_HIDEWINDOW = 128;

            public const int SWP_NOCOPYBITS = 256;

            public const int SWP_NOOWNERZORDER = 512;

            public const int SWP_NOREPOSITION = 512;

            public const int SWP_NOSENDCHANGING = 1024;
        }

        public class DC
        {
            public const int DCX_WINDOW = 1;

            public const int DCX_CACHE = 2;

            public const int DCX_NORESETATTRS = 4;

            public const int DCX_CLIPCHILDREN = 8;

            public const int DCX_CLIPSIBLINGS = 16;

            public const int DCX_PARENTCLIP = 32;

            public const int DCX_EXCLUDERGN = 64;

            public const int DCX_INTERSECTRGN = 128;

            public const int DCX_EXCLUDEUPDATE = 256;

            public const int DCX_INTERSECTUPDATE = 512;

            public const int DCX_LOCKWINDOWUPDATE = 1024;

            public const int DCX_VALIDATE = 2097152;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class ANIMATIONINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(ANIMATIONINFO));

            public int iMinAnimate;
        }

        public struct WINDOWPOS
        {
            public IntPtr hWnd;

            public IntPtr hHndInsertAfter;

            public int x;

            public int y;

            public int cx;

            public int cy;

            public int flags;
        }

        public class SC
        {
            public const int SC_SIZE = 61440;

            public const int SC_MOVE = 61456;

            public const int SC_MINIMIZE = 61472;

            public const int SC_MAXIMIZE = 61488;

            public const int SC_NEXTWINDOW = 61504;

            public const int SC_PREVWINDOW = 61520;

            public const int SC_CLOSE = 61536;

            public const int SC_VSCROLL = 61552;

            public const int SC_HSCROLL = 61568;

            public const int SC_MOUSEMENU = 61584;

            public const int SC_KEYMENU = 61696;

            public const int SC_ARRANGE = 61712;

            public const int SC_RESTORE = 61728;

            public const int SC_TASKLIST = 61744;

            public const int SC_SCREENSAVE = 61760;

            public const int SC_HOTKEY = 61776;

            public const int SC_CONTEXTHELP = 61824;

            public const int SC_DRAGMOVE = 61458;

            public const int SC_SYSMENU = 61587;
        }

        public class HT
        {
            public const int HTERROR = -2;

            public const int HTTRANSPARENT = -1;

            public const int HTNOWHERE = 0;

            public const int HTCLIENT = 1;

            public const int HTCAPTION = 2;

            public const int HTSYSMENU = 3;

            public const int HTGROWBOX = 4;

            public const int HTSIZE = 4;

            public const int HTMENU = 5;

            public const int HTHSCROLL = 6;

            public const int HTVSCROLL = 7;

            public const int HTMINBUTTON = 8;

            public const int HTMAXBUTTON = 9;

            public const int HTLEFT = 10;

            public const int HTRIGHT = 11;

            public const int HTTOP = 12;

            public const int HTTOPLEFT = 13;

            public const int HTTOPRIGHT = 14;

            public const int HTBOTTOM = 15;

            public const int HTBOTTOMLEFT = 16;

            public const int HTBOTTOMRIGHT = 17;

            public const int HTBORDER = 18;

            public const int HTREDUCE = 8;

            public const int HTZOOM = 9;

            public const int HTSIZEFIRST = 10;

            public const int HTSIZELAST = 17;

            public const int HTOBJECT = 19;

            public const int HTCLOSE = 20;

            public const int HTHELP = 21;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class NONCLIENTMETRICS
        {
            public int cbSize = Marshal.SizeOf(typeof(NONCLIENTMETRICS));

            public int iBorderWidth;

            public int iScrollWidth;

            public int iScrollHeight;

            public int iCaptionWidth;

            public int iCaptionHeight;

            [MarshalAs(UnmanagedType.Struct)]
            public LOGFONT lfCaptionFont;

            public int iSmCaptionWidth;

            public int iSmCaptionHeight;

            [MarshalAs(UnmanagedType.Struct)]
            public LOGFONT lfSmCaptionFont;

            public int iMenuWidth;

            public int iMenuHeight;

            [MarshalAs(UnmanagedType.Struct)]
            public LOGFONT lfMenuFont;

            [MarshalAs(UnmanagedType.Struct)]
            public LOGFONT lfStatusFont;

            [MarshalAs(UnmanagedType.Struct)]
            public LOGFONT lfMessageFont;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class LOGFONT
        {
            public int lfHeight;

            public int lfWidth;

            public int lfEscapement;

            public int lfOrientation;

            public int lfWeight;

            public byte lfItalic;

            public byte lfUnderline;

            public byte lfStrikeOut;

            public byte lfCharSet;

            public byte lfOutPrecision;

            public byte lfClipPrecision;

            public byte lfQuality;

            public byte lfPitchAndFamily;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string lfFaceName;
        }

        public struct MENUITEMINFO
        {
            public int cbSize;

            public int fMask;

            public int fType;

            public int fState;

            public int wID;

            public IntPtr hSubMenu;

            public IntPtr hbmpChecked;

            public IntPtr hbmpUnchecked;

            public IntPtr dwItemData;

            public string dwTypeData;

            public int cch;

            public IntPtr hbmpItem;
        }

        public class MenuFlags
        {
            public const int MF_INSERT = 0;

            public const int MF_CHANGE = 128;

            public const int MF_APPEND = 256;

            public const int MF_DELETE = 512;

            public const int MF_REMOVE = 4096;

            public const int MF_BYCOMMAND = 0;

            public const int MF_BYPOSITION = 1024;

            public const int MF_SEPARATOR = 2048;

            public const int MF_ENABLED = 0;

            public const int MF_GRAYED = 1;

            public const int MF_DISABLED = 2;

            public const int MF_UNCHECKED = 0;

            public const int MF_CHECKED = 8;

            public const int MF_USECHECKBITMAPS = 512;

            public const int MF_STRING = 0;

            public const int MF_BITMAP = 4;

            public const int MF_OWNERDRAW = 256;

            public const int MF_POPUP = 16;

            public const int MF_MENUBARBREAK = 32;

            public const int MF_MENUBREAK = 64;

            public const int MF_UNHILITE = 0;

            public const int MF_HILITE = 128;

            public const int MF_DEFAULT = 4096;

            public const int MF_SYSMENU = 8192;

            public const int MF_HELP = 16384;

            public const int MF_RIGHTJUSTIFY = 16384;

            public const int MF_MOUSESELECT = 32768;

            public const int MIIM_BITMAP = 128;

            public const int MIIM_CHECKMARKS = 8;

            public const int MIIM_DATA = 32;

            public const int MIIM_FTYPE = 256;

            public const int MIIM_ID = 2;

            public const int MIIM_STATE = 1;

            public const int MIIM_STRING = 64;

            public const int MIIM_SUBMENU = 4;

            public const int MIIM_TYPE = 16;
        }

        public struct NativeMessage
        {
            public IntPtr handle;

            public int msg;

            public IntPtr wParam;

            public IntPtr lParam;

            public int time;

            public Point p;
        }

        public enum ChangeWindowMessageFilterFlags
        {
            Add = 1,
            Remove
        }

        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 32;

            private const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;

            public short dmSpecVersion;

            public short dmDriverVersion;

            public short dmSize;

            public short dmDriverExtra;

            public int dmFields;

            public int dmPositionX;

            public int dmPositionY;

            public ScreenOrientation dmDisplayOrientation;

            public int dmDisplayFixedOutput;

            public short dmColor;

            public short dmDuplex;

            public short dmYResolution;

            public short dmTTOption;

            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;

            public short dmLogPixels;

            public int dmBitsPerPel;

            public int dmPelsWidth;

            public int dmPelsHeight;

            public int dmDisplayFlags;

            public int dmDisplayFrequency;

            public int dmICMMethod;

            public int dmICMIntent;

            public int dmMediaType;

            public int dmDitherType;

            public int dmReserved1;

            public int dmReserved2;

            public int dmPanningWidth;

            public int dmPanningHeight;
        }

        private static class UnsafeNativeMethods
        {
            [DllImport("version.dll", SetLastError = true)]
            internal static extern bool VerQueryValue(byte[] pBlock, string lpSubBlock, out IntPtr lplpBuffer, out int puLen);

            [DllImport("version.dll", SetLastError = true)]
            internal static extern int GetFileVersionInfoSize(string lptstrFilename, out int dwSize);

            [DllImport("version.dll", SetLastError = true)]
            internal static extern bool GetFileVersionInfo(string lptstrFilename, int dwHandleIgnored, int dwLen, byte[] lpData);

            [DllImport("User32.dll")]
            internal static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorResource);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr GetFocus();

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern IntPtr SetFocus(IntPtr hWnd);

            [DllImport("ComCtl32.dll", CharSet = CharSet.Auto)]
            internal static extern int SetWindowSubclass(IntPtr hWnd, Win32SubClassProc newProc, IntPtr uIdSubclass, IntPtr dwRefData);

            [DllImport("ComCtl32.dll", CharSet = CharSet.Auto)]
            internal static extern int RemoveWindowSubclass(IntPtr hWnd, Win32SubClassProc newProc, IntPtr uIdSubclass);

            [DllImport("ComCtl32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr DefSubclassProc(IntPtr hWnd, IntPtr Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
            public static extern int GetTextMetricsA(HandleRef hDC, ref TEXTMETRICA lptm);

            [DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
            public static extern int GetTextMetricsW(HandleRef hDC, ref TEXTMETRIC lptm);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern ushort GlobalAddAtom(string lpString);

            [DllImport("user32.dll")]
            public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

            [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
            internal static extern int SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

            [DllImport("user32.dll")]
            internal static extern bool DrawMenuBar(IntPtr hWnd);

            [DllImport("user32.dll")]
            internal static extern bool DrawFrameControl(HandleRef hdc, ref RECT lprc, uint uType, uint uState);

            [DllImport("user32.dll")]
            internal static extern IntPtr LoadImage(IntPtr hinst, int iconId, uint uType, int cxDesired, int cyDesired, uint fuLoad);

            [DllImport("user32.dll")]
            internal static extern int DestroyIcon(IntPtr hIcon);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern IntPtr CreateWindowEx(int dwExStyle, IntPtr classAtom, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DestroyWindow(IntPtr hwnd);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            internal static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            internal static extern int RegisterClass(ref WNDCLASS lpWndClass);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool UnregisterClass(IntPtr classAtom, IntPtr hInstance);

            [DllImport("GDI32.dll")]
            internal static extern int RestoreDC(IntPtr hdc, int savedDC);

            [DllImport("GDI32.dll")]
            internal static extern int SaveDC(IntPtr hdc);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
            internal static extern int BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
            internal static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

            [DllImport("gdi32.dll", EntryPoint = "GdiAlphaBlend")]
            internal static extern bool AlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, BLENDFUNCTION blendFunction);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
            internal static extern IntPtr SelectObject(HandleRef hdc, HandleRef obj);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
            internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

            [DllImport("gdi32.dll")]
            internal static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

            [DllImport("gdi32.dll")]
            internal static extern int GetDIBits(HandleRef hdc, HandleRef hbm, int arg1, int arg2, IntPtr arg3, ref BITMAPINFO_FLAT bmi, int arg5);

            [DllImport("gdi32.dll")]
            internal static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int width, int height);

            [DllImport("gdi32.dll")]
            internal static extern IntPtr CreateCompatibleBitmap(HandleRef hDC, int width, int height);

            [DllImport("gdi32.dll")]
            internal static extern IntPtr CreateCompatibleDC(HandleRef hDC);

            [DllImport("gdi32.dll")]
            internal static extern IntPtr CreateCompatibleDC(IntPtr hDC);

            //[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
            //internal static extern bool GetTextMetrics(HandleRef hdc, out DevExpress.Utils.Text.TEXTMETRIC lptm);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern int WideCharToMultiByte(int codePage, int flags, [MarshalAs(UnmanagedType.LPWStr)] string wideStr, int chars, [In] [Out] byte[] pOutBytes, int bufferBytes, IntPtr defaultChar, IntPtr pDefaultUsed);

            internal static int GetTextExtentPoint32(HandleRef hDC, string text, ref SIZE size)
            {
                int length = text.Length;
                if (Marshal.SystemDefaultCharSize == 1)
                {
                    length = WideCharToMultiByte(0, 0, text, text.Length, null, 0, IntPtr.Zero, IntPtr.Zero);
                    byte[] array = new byte[length];
                    WideCharToMultiByte(0, 0, text, text.Length, array, array.Length, IntPtr.Zero, IntPtr.Zero);
                    length = Math.Min(text.Length, 8192);
                    return GetTextExtentPoint32A(hDC, array, length, ref size);
                }
                return GetTextExtentPoint32W(hDC, text, text.Length, ref size);
            }

            //internal static void DwmSetWindowAttributeInt(IntPtr handle, NativeVista.DWMWINDOWATTRIBUTE dwmAttribute, int value)
            //{
            //	if (Environment.OSVersion.Version.Major >= 6)
            //	{
            //		int num = Marshal.SizeOf(typeof(int));
            //		IntPtr intPtr = Marshal.AllocHGlobal(num);
            //		try
            //		{
            //			Marshal.WriteInt32(intPtr, value);
            //			DwmSetWindowAttribute(handle, (uint)dwmAttribute, intPtr, (uint)num);
            //		}
            //		finally
            //		{
            //			if (intPtr != IntPtr.Zero)
            //			{
            //				Marshal.FreeHGlobal(intPtr);
            //			}
            //		}
            //	}
            //}

            [DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
            internal static extern int GetTextExtentPoint32A(HandleRef hDC, byte[] lpszString, int byteCount, ref SIZE size);

            [DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
            internal static extern int GetTextExtentPoint32W(HandleRef hDC, [MarshalAs(UnmanagedType.LPWStr)] string text, int len, ref SIZE size);

            [DllImport("GDI32.dll")]
            internal static extern IntPtr GetStockObject(int fnObject);

            [DllImport("GDI32.dll")]
            internal static extern IntPtr CreatePatternBrush(IntPtr hbmp);

            [DllImport("gdi32.dll")]
            internal static extern int SetBkMode(IntPtr hdc, int mode);

            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(HandleRef hDC);

            [DllImport("gdi32.dll")]
            internal static extern bool DeleteDC(IntPtr hDC);

            [DllImport("gdi32.dll")]
            internal static extern bool DeleteObject(HandleRef hObject);

            [DllImport("gdi32.dll")]
            internal static extern bool DeleteObject(IntPtr hObject);

            [DllImport("gdi32.dll", SetLastError = true)]
            internal static extern IntPtr CreateDIBSection(HandleRef hdc, ref BITMAPINFO_FLAT bmi, int iUsage, ref IntPtr ppvBits, IntPtr hSection, int dwOffset);

            [DllImport("gdi32.dll", SetLastError = true)]
            internal static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO_SMALL bmi, int iUsage, int pvvBits, IntPtr hSection, int dwOffset);

            [DllImport("gdi32.dll")]
            internal static extern int GetPaletteEntries(IntPtr hPal, int startIndex, int entries, byte[] palette);

            [DllImport("comctl32.dll", ExactSpelling = true)]
            internal static extern bool _TrackMouseEvent(TRACKMOUSEEVENTStruct tme);

            [DllImport("user32.dll")]
            internal static extern IntPtr TrackPopupMenu(IntPtr menuHandle, int uFlags, int x, int y, int nReserved, IntPtr hwnd, IntPtr par);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
            internal static extern bool GetViewportOrgEx(IntPtr hDC, ref POINT point);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool ScrollWindowEx(IntPtr hWnd, int nXAmount, int nYAmount, RECT rectScrollRegion, ref RECT rectClip, IntPtr hrgnUpdate, ref RECT prcUpdate, int flags);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool ScrollWindowEx(IntPtr hWnd, int nXAmount, int nYAmount, IntPtr rectScrollRegion, ref RECT rectClip, IntPtr hrgnUpdate, ref RECT prcUpdate, int flags);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern int ScrollWindowEx(IntPtr hWnd, int dx, int dy, ref RECT scrollRect, ref RECT clipRect, IntPtr hrgnUpdate, IntPtr updateRect, int flags);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool ScrollWindow(IntPtr hWnd, int nXAmount, int nYAmount, ref RECT rectScrollRegion, ref RECT rectClip);

            [DllImport("USER32.dll")]
            internal static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

            [DllImport("USER32.dll")]
            internal static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, int flags);

            [DllImport("USER32.dll")]
            internal static extern IntPtr GetWindowDC(IntPtr hwnd);

            [DllImport("USER32.dll")]
            internal static extern int GetClassLong(IntPtr hwnd, int flags);

            [DllImport("USER32.dll")]
            internal static extern int GetWindowLong(IntPtr hwnd, int flags);

            [DllImport("USER32.dll")]
            internal static extern int SetWindowLong(IntPtr hwnd, int flags, int val);

            [DllImport("user32.dll")]
            internal static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            [DllImport("USER32.dll")]
            internal static extern IntPtr GetDesktopWindow();

            [DllImport("USER32.dll")]
            internal static extern bool RedrawWindow(IntPtr hwnd, IntPtr rcUpdate, IntPtr hrgnUpdate, int flags);

            [DllImport("USER32.dll")]
            internal static extern short GetAsyncKeyState(int vKey);

            [DllImport("USER32.dll")]
            internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

            [DllImport("USER32.dll")]
            internal static extern IntPtr GetForegroundWindow();

            [DllImport("USER32.dll")]
            internal static extern int SetCapture(IntPtr hWnd);

            [DllImport("USER32.dll")]
            internal static extern bool ReleaseCapture();

            [DllImport("USER32.dll")]
            internal static extern bool IsWindowVisible(IntPtr hWnd);

            [DllImport("USER32.dll", CharSet = CharSet.Auto)]
            internal static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

            [DllImport("USER32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, int lParam);

            [DllImport("USER32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

            [DllImport("USER32.dll")]
            internal static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            internal static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

            [DllImport("USER32.dll")]
            internal static extern int PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("USER32.dll")]
            internal static extern bool IsZoomed(IntPtr hwnd);

            [DllImport("USER32.dll")]
            internal static extern bool IsIconic(IntPtr hwnd);

            [DllImport("USER32.dll")]
            internal static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

            [DllImport("USER32.dll")]
            internal static extern bool ValidateRect(IntPtr hwnd, ref RECT lpRect);

            [DllImport("User32.dll")]
            internal static extern int GetUpdateRect(IntPtr hwnd, ref RECT rect, bool erase);

            [DllImport("USER32.dll")]
            internal static extern IntPtr BeginPaint(IntPtr hWnd, [In] [Out] ref PAINTSTRUCT lpPaint);

            [DllImport("USER32.dll")]
            internal static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

            [DllImport("USER32.dll")]
            internal static extern bool LockWindowUpdate(IntPtr hWndLock);

            [DllImport("USER32.dll")]
            internal static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw);

            [DllImport("gdi32.dll")]
            internal static extern int GetClipBox(IntPtr hdc, out RECT lprc);

            [DllImport("GDI32.dll")]
            internal static extern int CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, int fnCombineMode);

            [DllImport("GDI32.dll")]
            internal static extern int ExcludeClipRect(IntPtr hdc, int left, int top, int right, int bottom);

            [DllImport("GDI32.dll")]
            internal static extern int GetClipRgn(IntPtr hdc, IntPtr hrgn);

            [DllImport("GDI32.dll")]
            internal static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

            [DllImport("GDI32.dll")]
            internal static extern int ExtSelectClipRgn(IntPtr hdc, IntPtr hrgn, int mode);

            [DllImport("gdi32.dll")]
            internal static extern bool LPtoDP(IntPtr hdc, [In] [Out] POINT[] lpPoints, int nCount);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool GetUpdateRgn(IntPtr hwnd, IntPtr hrgn, bool fErase);

            [DllImport("gdi32.dll")]
            internal static extern int GetRegionData(IntPtr hRgn, int dwCount, IntPtr lpRgnData);

            [DllImport("gdi32.dll")]
            internal static extern int OffsetRgn(IntPtr hrgn, int nXOffset, int nYOffset);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern int MapWindowPoints(IntPtr hwndFrom, IntPtr hwndTo, ref POINT lpPoints, [MarshalAs(UnmanagedType.U4)] int cPoints);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern short GetKeyState(int keyCode);

            [DllImport("gdi32.dll")]
            internal static extern int GetRandomRgn(IntPtr hdc, IntPtr hrgn, int iNum);

            [DllImport("GDI32.dll")]
            internal static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

            [DllImport("GDI32.dll")]
            internal static extern bool RectVisible(IntPtr hdc, ref RECT rect);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern bool DragDetect(IntPtr hwnd, POINT pt);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
            internal static extern int GetObject(IntPtr hObject, int nSize, [In] [Out] LOGFONT lf);

            [DllImport("gdi32.dll")]
            internal static extern IntPtr SelectPalette(IntPtr hdc, IntPtr hpal, bool bForceBackground);

            [DllImport("gdi32.dll")]
            internal static extern int RealizePalette(IntPtr hdc);

            [DllImport("User32.dll")]
            internal static extern HDC GetDC(HWND handle);

            [DllImport("Gdi32.dll")]
            internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

            [DllImport("User32.dll")]
            internal static extern IntPtr GetCursor();

            [DllImport("User32.dll")]
            internal static extern bool SetSystemCursor(IntPtr hCursor, int id);

            [DllImport("Gdi32.dll")]
            internal static extern IntPtr CreateSolidBrush(COLORREF aColorRef);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool FillRect(HDC hdc, ref RECT rect, IntPtr hbrush);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool FillRect(IntPtr hdc, ref RECT rect, IntPtr hbrush);

            [DllImport("GDI32.dll")]
            internal static extern bool FillRgn(IntPtr hdc, IntPtr hrgn, IntPtr brush);

            [DllImport("gdi32.dll")]
            internal static extern int GetPixel(IntPtr hdc, int nXPos, int nYPos);

            [DllImport("gdi32.dll")]
            internal static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, int dwRop);

            [DllImport("User32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool UnhookWindowsHookEx(HHOOK aHook);

            [DllImport("User32.dll")]
            internal static extern IntPtr CopyIcon(IntPtr hCursor);

            [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, int dwThreadId);

            [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

            [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern IntPtr GetModuleHandle(string lpModuleName);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
            public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE pSizeDst, IntPtr hdcSrc, ref POINT pptSrc, int crKey, ref BLENDFUNCTION pBlend, int dwFlags);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool InvalidateRgn(IntPtr hWnd, IntPtr hrgn, bool erase);

            [DllImport("user32.dll")]
            internal static extern bool SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, uint dwFlags);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool AdjustWindowRectExForDpi(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle, int dpi);

            [DllImport("user32.dll")]
            internal static extern IntPtr FindWindow(string className, string windowText);

            [DllImport("user32.dll")]
            internal static extern int ShowWindow(IntPtr hWnd, int command);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

            [DllImport("User32.dll")]
            internal static extern IntPtr WindowFromPoint(Point pt);

            [DllImport("User32.dll")]
            internal static extern IntPtr GetWindow(IntPtr hWnd, uint wCmd);

            [DllImport("User32.dll")]
            internal static extern IntPtr SetActiveWindow(IntPtr hWnd);

            [DllImport("User32.dll")]
            internal static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("kernel32.dll")]
            internal static extern int GetCurrentThreadId();

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool EnableWindow(IntPtr hWnd, bool enable);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool IsWindowEnabled(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern bool SystemParametersInfo(int uiAction, int uiParam, [In] [Out] NONCLIENTMETRICS pvParam, int fWinIni);

            [DllImport("user32")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetGestureConfig(IntPtr hWnd, int dwReserved, int cIDs, [In] [Out] GESTURECONFIG[] pGestureConfig, int cbSize);

            [DllImport("UxTheme")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BeginPanningFeedback(IntPtr hWnd);

            [DllImport("UxTheme")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool EndPanningFeedback(IntPtr hWnd, bool fAnimateBack);

            [DllImport("UxTheme")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool UpdatePanningFeedback(IntPtr hwnd, int lTotalOverpanOffsetX, int lTotalOverpanOffsetY, bool fInInertia);

            [DllImport("user32")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool CloseGestureInfoHandle(IntPtr hGestureInfo);

            [DllImport("user32")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetGestureInfo(IntPtr hGestureInfo, ref GESTUREINFO pGestureInfo);

            [DllImport("dwmapi.dll")]
            internal static extern int DwmSetIconicThumbnail(IntPtr hwnd, IntPtr hbitmap, uint flags);

            [DllImport("dwmapi.dll")]
            internal static extern int DwmSetIconicLivePreviewBitmap(IntPtr hwnd, IntPtr hbitmap, IntPtr ptClient, uint flags);

            [DllImport("dwmapi.dll")]
            internal static extern int DwmInvalidateIconicBitmaps(IntPtr hwnd);

            [DllImport("dwmapi.dll")]
            internal static extern int DwmSetWindowAttribute(IntPtr hwnd, uint dwAttributeToSet, IntPtr pvAttributeValue, uint cbAttribute);

            [DllImport("user32.dll")]
            internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool ClientToScreen(IntPtr hwnd, ref POINT point);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool ScreenToClient(IntPtr hwnd, ref POINT point);

            [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern int ExtractIconEx(string fileName, int iconStartingIndex, IntPtr[] largeIcons, IntPtr[] smallIcons, int iconCount);

            [DllImport("shell32.dll")]
            internal static extern void SHAddToRecentDocs(ShellAddToRecentDocs flags, [MarshalAs(UnmanagedType.LPWStr)] string path);

            [DllImport("Ole32.dll", PreserveSig = true)]
            internal static extern void PropVariantClear([In] [Out] PropVariant pvar);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            internal static extern uint RegisterWindowMessage(string lpProcName);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern IntPtr RemoveProp(IntPtr hWnd, string lpString);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern bool SetProp(IntPtr hWnd, string lpString, IntPtr hData);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern IntPtr GetCapture();

            [DllImport("user32.dll")]
            internal static extern void PostQuitMessage(int exitCode);

            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern IntPtr LocalFree(IntPtr p);

            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern IntPtr LocalAlloc(int flag, int size);

            [DllImport("kernel32.dll")]
            internal static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

            [DllImport("user32.dll")]
            internal static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

            [DllImport("shell32.dll", SetLastError = true)]
            internal static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

            [DllImport("gdi32.dll")]
            internal static extern bool SetViewportOrgEx(IntPtr hdc, int X, int Y, out POINT lpPoint);

            [DllImport("user32")]
            internal static extern bool ChangeWindowMessageFilter(int msg, ChangeWindowMessageFilterFlags flags);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern bool RemoveMenu(IntPtr hMenu, int uPosition, int uFlags);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern bool GetMenuItemInfo(IntPtr hMenu, int uItem, bool fByPosition, ref MENUITEMINFO lpmii);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern bool EnableMenuItem(IntPtr hMenu, int uIDEnableItem, int uEnable);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern int GetMenuItemCount(IntPtr hMenu);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool PeekMessage(out NativeMessage lpMsg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax, int wRemoveMsg);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool SystemParametersInfo(int nAction, int nParam, [In] [Out] ANIMATIONINFO value, int ignore);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr GetOpenClipboardWindow();

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

            [DllImport("user32.dll")]
            public static extern int GetMessage(out Message lpMsg, HWND hwnd, int wMsgFilterMin, int mMsgFilterMax);

            [DllImport("user32.dll")]
            public static extern void TranslateMessage(ref Message msg);

            [DllImport("user32.dll")]
            public static extern void DispatchMessage(ref Message msg);

            [DllImport("user32.dll")]
            public static extern bool PostThreadMessage(int threadId, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("shcore.dll", SetLastError = true)]
            public static extern IntPtr GetProcessDpiAwareness([In] IntPtr hProcess, out PROCESS_DPI_AWARENESS value);

            [DllImport("shcore.dll", SetLastError = true)]
            public static extern IntPtr GetDpiForMonitor([In] IntPtr hMonitor, [In] MONITOR_DPI_TYPE dpiType, out uint dpiX, out uint dpiY);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr MonitorFromPoint([In] Point pt, [In] uint flags);

            [DllImport("User32.dll", SetLastError = true)]
            internal static extern IntPtr MonitorFromWindow([In] IntPtr hWnd, [In] int flags);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
        }

        internal struct LanguageCodePage
        {
            internal short language;

            internal short codePage;

            internal void SomeMethod()
            {
                language = 0;
                codePage = 0;
            }
        }

        public delegate IntPtr Win32SubClassProc(IntPtr hWnd, IntPtr Msg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, IntPtr dwRefData);

        public delegate int EditWordBreakProc(string lpch, int ichCurrent, int cch, int code);

        public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        internal struct TEXTMETRICA
        {
            public int tmHeight;

            public int tmAscent;

            public int tmDescent;

            public int tmInternalLeading;

            public int tmExternalLeading;

            public int tmAveCharWidth;

            public int tmMaxCharWidth;

            public int tmWeight;

            public int tmOverhang;

            public int tmDigitizedAspectX;

            public int tmDigitizedAspectY;

            public byte tmFirstChar;

            public byte tmLastChar;

            public byte tmDefaultChar;

            public byte tmBreakChar;

            public byte tmItalic;

            public byte tmUnderlined;

            public byte tmStruckOut;

            public byte tmPitchAndFamily;

            public byte tmCharSet;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TEXTMETRIC
        {
            public int tmHeight;

            public int tmAscent;

            public int tmDescent;

            public int tmInternalLeading;

            public int tmExternalLeading;

            public int tmAveCharWidth;

            public int tmMaxCharWidth;

            public int tmWeight;

            public int tmOverhang;

            public int tmDigitizedAspectX;

            public int tmDigitizedAspectY;

            public char tmFirstChar;

            public char tmLastChar;

            public char tmDefaultChar;

            public char tmBreakChar;

            public byte tmItalic;

            public byte tmUnderlined;

            public byte tmStruckOut;

            public byte tmPitchAndFamily;

            public byte tmCharSet;
        }

        public enum PROCESS_DPI_AWARENESS
        {
            PROCESS_DPI_UNAWARE,
            PROCESS_SYSTEM_DPI_AWARE,
            PROCESS_PER_MONITOR_DPI_AWARE
        }

        public enum MONITOR_DPI_TYPE
        {
            MDT_EFFECTIVE_DPI = 0,
            MDT_ANGULAR_DPI = 1,
            MDT_RAW_DPI = 2,
            MDT_DEFAULT = 0
        }

        public enum MONITORFROMWINDOW_FLAGS
        {
            MONITOR_DEFAULTTONULL,
            MONITOR_DEFAULTTOPRIMARY,
            MONITOR_DEFAULTTONEAREST
        }

        public const int SPI_GETANIMATION = 72;

        public const int SPI_GETNONCLIENTMETRICS = 41;

        public const int PM_NOREMOVE = 0;

        public const int PM_REMOVE = 1;

        public const int EM_SETWORDBREAKPROC = 208;

        public const int EM_GETWORDBREAKPROC = 209;

        public const int WB_LEFT = 0;

        public const int WB_RIGHT = 1;

        public const int WB_ISDELIMITER = 2;

        public const int RGN_AND = 1;

        public const int RGN_OR = 2;

        public const int RGN_XOR = 3;

        public const int RGN_DIFF = 4;

        public const int RGN_COPY = 5;

        public const int MAX_PATH = 260;

        public const int GW_HWNDFIRST = 0;

        public const int GW_HWNDLAST = 1;

        public const int GW_HWNDNEXT = 2;

        public const int GW_HWNDPREV = 3;

        public const int GW_OWNER = 4;

        public const int GW_CHILD = 5;

        public const float USER_DEFAULT_SCREEN_DPI = 96f;

        private static bool? isWindow10;

        private static readonly Version win81version = new Version(6, 3, 9600, 0);

        public static int SYSRGN = 4;

        private static bool? perMonitorDpiAPISupported;

        public static bool IsWindow10
        {
            get
            {
                if (isWindow10.HasValue)
                {
                    return isWindow10.Value;
                }
                //isWindow10 = MouseWheelHelper.IsWindows10();
                return isWindow10.Value;
            }
        }

        internal static bool IsWindows81_Or_Above
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    return Environment.OSVersion.Version >= win81version;
                }
                return false;
            }
        }

        public static bool IsPerMonitorDpiAPISupported
        {
            get
            {
                if (!perMonitorDpiAPISupported.HasValue)
                {
                    //perMonitorDpiAPISupported = (NativeVista.IsWindows2012R2Server || IsWindows81_Or_Above);
                }
                return perMonitorDpiAPISupported.Value;
            }
        }

        public static bool IsWindowOnCurrentDesktop(IntPtr hWnd)
        {
            if (!IsWindow10)
            {
            }
            return true;
        }

        public static bool VerQueryValue(byte[] pBlock, string lpSubBlock, out IntPtr lplpBuffer, out int puLen)
        {
            return UnsafeNativeMethods.VerQueryValue(pBlock, lpSubBlock, out lplpBuffer, out puLen);
        }

        public static int GetFileVersionInfoSize(string lptstrFilename, out int dwSize)
        {
            return UnsafeNativeMethods.GetFileVersionInfoSize(lptstrFilename, out dwSize);
        }

        public static bool GetFileVersionInfo(string lptstrFilename, int dwHandleIgnored, int dwLen, byte[] lpData)
        {
            return UnsafeNativeMethods.GetFileVersionInfo(lptstrFilename, dwHandleIgnored, dwLen, lpData);
        }

        public static int SetWindowSubclass(IntPtr hWnd, Win32SubClassProc newProc, IntPtr uIdSubclass, IntPtr dwRefData)
        {
            return UnsafeNativeMethods.SetWindowSubclass(hWnd, newProc, uIdSubclass, dwRefData);
        }

        public static IntPtr GetFocus()
        {
            return UnsafeNativeMethods.GetFocus();
        }

        public static IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorResource)
        {
            return UnsafeNativeMethods.LoadCursor(hInstance, lpCursorResource);
        }

        public static IntPtr SetFocus(IntPtr hWnd)
        {
            return UnsafeNativeMethods.SetFocus(hWnd);
        }

        public static int RemoveWindowSubclass(IntPtr hWnd, Win32SubClassProc newProc, IntPtr uIdSubclass)
        {
            return UnsafeNativeMethods.RemoveWindowSubclass(hWnd, newProc, uIdSubclass);
        }

        public static IntPtr DefSubclassProc(IntPtr hWnd, IntPtr Msg, IntPtr wParam, IntPtr lParam)
        {
            return UnsafeNativeMethods.DefSubclassProc(hWnd, Msg, wParam, lParam);
        }

        public static bool FlashWindowEx(IntPtr hWnd, FLASHW flags, int count, int timeout)
        {
            FLASHWINFO fLASHWINFO = default(FLASHWINFO);
            fLASHWINFO.cbSize = (uint)Marshal.SizeOf((object)fLASHWINFO);
            fLASHWINFO.hwnd = hWnd;
            fLASHWINFO.dwFlags = (uint)flags;
            fLASHWINFO.uCount = (uint)count;
            fLASHWINFO.dwTimeout = (uint)timeout;
            return FlashWindowEx(fLASHWINFO);
        }

        internal static bool FlashWindowEx(FLASHWINFO info)
        {
            return UnsafeNativeMethods.FlashWindowEx(ref info);
        }

        internal static int SHAppBarMessage(int dwMessage, ref APPBARDATA pData)
        {
            return UnsafeNativeMethods.SHAppBarMessage(dwMessage, ref pData);
        }

        public static bool DrawMenuBar(IntPtr hWnd)
        {
            return UnsafeNativeMethods.DrawMenuBar(hWnd);
        }

        public static bool DrawFrameControl(HandleRef hdc, RECT lprc, int type, int state)
        {
            return UnsafeNativeMethods.DrawFrameControl(hdc, ref lprc, (uint)type, (uint)state);
        }

        public static short GlobalAddAtom(string lpString)
        {
            return (short)UnsafeNativeMethods.GlobalAddAtom(lpString);
        }

        public static IntPtr RemoveProp(IntPtr hWnd, string lpString)
        {
            return UnsafeNativeMethods.RemoveProp(hWnd, lpString);
        }

        public static IntPtr GetCapture()
        {
            return UnsafeNativeMethods.GetCapture();
        }

        public static bool SetProp(IntPtr hWnd, string lpString, IntPtr hData)
        {
            return UnsafeNativeMethods.SetProp(hWnd, lpString, hData);
        }

        public static IntPtr LoadImage(IntPtr hinst, int iconId, int uType, int cxDesired, int cyDesired, int fuLoad)
        {
            return UnsafeNativeMethods.LoadImage(hinst, iconId, (uint)uType, cxDesired, cyDesired, (uint)fuLoad);
        }

        public static int DestroyIcon(IntPtr hIcon)
        {
            return UnsafeNativeMethods.DestroyIcon(hIcon);
        }

        public static IntPtr CreateWindowEx(int dwExStyle, IntPtr classAtom, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam)
        {
            return UnsafeNativeMethods.CreateWindowEx(dwExStyle, classAtom, lpWindowName, dwStyle, x, y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
        }

        public static bool DestroyWindow(IntPtr hWnd)
        {
            return UnsafeNativeMethods.DestroyWindow(hWnd);
        }

        public static IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            return UnsafeNativeMethods.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        public static int RegisterClass(ref WNDCLASS lpWndClass)
        {
            return UnsafeNativeMethods.RegisterClass(ref lpWndClass);
        }

        public static bool UnregisterClass(IntPtr classAtom, IntPtr hInstance)
        {
            return UnsafeNativeMethods.UnregisterClass(classAtom, hInstance);
        }

        public static int RestoreDC(IntPtr hdc, int savedDC)
        {
            return UnsafeNativeMethods.RestoreDC(hdc, savedDC);
        }

        public static int SaveDC(IntPtr hdc)
        {
            return UnsafeNativeMethods.SaveDC(hdc);
        }

        public static int BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop)
        {
            return UnsafeNativeMethods.BitBlt(hDC, x, y, nWidth, nHeight, hSrcDC, xSrc, ySrc, dwRop);
        }

        public static int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop)
        {
            return UnsafeNativeMethods.BitBlt(hDC, x, y, nWidth, nHeight, hSrcDC, xSrc, ySrc, dwRop);
        }

        public static bool AlphaBlend(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hdcSrc, int xSrc, int ySrc, int nWidthSrc, int nHeightSrc, BLENDFUNCTION blendFunction)
        {
            return UnsafeNativeMethods.AlphaBlend(hDC, x, y, nWidth, nHeight, hdcSrc, xSrc, ySrc, nWidthSrc, nHeightSrc, blendFunction);
        }

        public static IntPtr SelectObject(HandleRef hdc, HandleRef obj)
        {
            return UnsafeNativeMethods.SelectObject(hdc, obj);
        }

        public static bool GetClientRect(IntPtr hWnd, out RECT rect)
        {
            return UnsafeNativeMethods.GetClientRect(hWnd, out rect);
        }

        public static IntPtr SelectObject(IntPtr hdc, IntPtr obj)
        {
            return UnsafeNativeMethods.SelectObject(hdc, obj);
        }

        public static IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse)
        {
            return UnsafeNativeMethods.CreateRoundRectRgn(nLeftRect, nTopRect, nRightRect, nBottomRect, nWidthEllipse, nHeightEllipse);
        }

        public static int GetDIBits(HandleRef hdc, HandleRef hbm, int arg1, int arg2, IntPtr arg3, ref BITMAPINFO_FLAT bmi, int arg5)
        {
            return UnsafeNativeMethods.GetDIBits(hdc, hbm, arg1, arg2, arg3, ref bmi, arg5);
        }

        public static IntPtr CreateCompatibleBitmap(IntPtr hDC, int width, int height)
        {
            return UnsafeNativeMethods.CreateCompatibleBitmap(hDC, width, height);
        }

        public static IntPtr CreateCompatibleBitmap(HandleRef hDC, int width, int height)
        {
            return UnsafeNativeMethods.CreateCompatibleBitmap(hDC, width, height);
        }

        public static IntPtr CreateCompatibleDC(HandleRef hDC)
        {
            return UnsafeNativeMethods.CreateCompatibleDC(hDC);
        }

        //public static bool GetTextMetrics(HandleRef hDC, out DevExpress.Utils.Text.TEXTMETRIC lptm)
        //{
        //	return UnsafeNativeMethods.GetTextMetrics(hDC, out lptm);
        //}

        public static IntPtr CreateCompatibleDC(IntPtr hDC)
        {
            return UnsafeNativeMethods.CreateCompatibleDC(hDC);
        }

        public static bool DeleteDC(HandleRef hDC)
        {
            return UnsafeNativeMethods.DeleteDC(hDC);
        }

        public static bool DeleteDC(IntPtr hDC)
        {
            return UnsafeNativeMethods.DeleteDC(hDC);
        }

        public static bool DeleteObject(HandleRef hObject)
        {
            return UnsafeNativeMethods.DeleteObject(hObject);
        }

        public static bool DeleteObject(IntPtr hObject)
        {
            return UnsafeNativeMethods.DeleteObject(hObject);
        }

        public static int SetBkMode(IntPtr hdc, int mode)
        {
            return UnsafeNativeMethods.SetBkMode(hdc, mode);
        }

        public static IntPtr GetStockObject(int fnObject)
        {
            return UnsafeNativeMethods.GetStockObject(fnObject);
        }

        public static IntPtr CreatePatternBrush(IntPtr hbmp)
        {
            return UnsafeNativeMethods.CreatePatternBrush(hbmp);
        }

        public static IntPtr CreateDIBSection(HandleRef hdc, ref BITMAPINFO_FLAT bmi, int iUsage, ref IntPtr ppvBits, IntPtr hSection, int dwOffset)
        {
            return UnsafeNativeMethods.CreateDIBSection(hdc, ref bmi, iUsage, ref ppvBits, hSection, dwOffset);
        }

        public static IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO_SMALL bmi, int iUsage, int pvvBits, IntPtr hSection, int dwOffset)
        {
            return UnsafeNativeMethods.CreateDIBSection(hdc, ref bmi, iUsage, pvvBits, hSection, dwOffset);
        }

        public static int GetPaletteEntries(IntPtr hPal, int startIndex, int entries, byte[] palette)
        {
            return UnsafeNativeMethods.GetPaletteEntries(hPal, startIndex, entries, palette);
        }

        public static bool TrackMouseEvent(TRACKMOUSEEVENTStruct tme)
        {
            return UnsafeNativeMethods._TrackMouseEvent(tme);
        }

        public static IntPtr TrackPopupMenu(IntPtr menuHandle, int uFlags, int x, int y, int nReserved, IntPtr hwnd, IntPtr par)
        {
            return UnsafeNativeMethods.TrackPopupMenu(menuHandle, uFlags, x, y, nReserved, hwnd, par);
        }

        public static bool GetViewportOrgEx(IntPtr hDC, ref POINT point)
        {
            return UnsafeNativeMethods.GetViewportOrgEx(hDC, ref point);
        }

        public static bool ScrollWindowEx(IntPtr hWnd, int nXAmount, int nYAmount, RECT rectScrollRegion, ref RECT rectClip, IntPtr hrgnUpdate, ref RECT prcUpdate, int flags)
        {
            return UnsafeNativeMethods.ScrollWindowEx(hWnd, nXAmount, nYAmount, rectScrollRegion, ref rectClip, hrgnUpdate, ref prcUpdate, flags);
        }

        public static bool ScrollWindowEx(IntPtr hWnd, int nXAmount, int nYAmount, IntPtr rectScrollRegion, ref RECT rectClip, IntPtr hrgnUpdate, ref RECT prcUpdate, int flags)
        {
            return UnsafeNativeMethods.ScrollWindowEx(hWnd, nXAmount, nYAmount, rectScrollRegion, ref rectClip, hrgnUpdate, ref prcUpdate, flags);
        }

        public static int ScrollWindowEx(IntPtr hWnd, int dx, int dy, ref RECT scrollRect, ref RECT clipRect, IntPtr hrgnUpdate, IntPtr updateRect, int flags)
        {
            return UnsafeNativeMethods.ScrollWindowEx(hWnd, dx, dy, ref scrollRect, ref clipRect, hrgnUpdate, updateRect, flags);
        }

        public static bool ScrollWindow(IntPtr hWnd, int nXAmount, int nYAmount, ref RECT rectScrollRegion, ref RECT rectClip)
        {
            return UnsafeNativeMethods.ScrollWindow(hWnd, nXAmount, nYAmount, ref rectScrollRegion, ref rectClip);
        }

        public static int ReleaseDC(IntPtr hWnd, IntPtr hDC)
        {
            return UnsafeNativeMethods.ReleaseDC(hWnd, hDC);
        }

        public static IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, int flags)
        {
            return UnsafeNativeMethods.GetDCEx(hWnd, hrgnClip, flags);
        }

        public static IntPtr GetWindowDC(IntPtr hWnd)
        {
            return UnsafeNativeMethods.GetWindowDC(hWnd);
        }

        public static int GetClassLong(IntPtr hWnd, int flags)
        {
            return UnsafeNativeMethods.GetClassLong(hWnd, flags);
        }

        public static int GetWindowLong(IntPtr hWnd, int flags)
        {
            return UnsafeNativeMethods.GetWindowLong(hWnd, flags);
        }

        public static int SetWindowLong(IntPtr hWnd, int flags, int val)
        {
            return UnsafeNativeMethods.SetWindowLong(hWnd, flags, val);
        }

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            return UnsafeNativeMethods.SetWindowLong(hWnd, nIndex, dwNewLong);
        }

        public static IntPtr GetDesktopWindow()
        {
            return UnsafeNativeMethods.GetDesktopWindow();
        }

        public static bool RedrawWindow(IntPtr hWnd, IntPtr rcUpdate, IntPtr hrgnUpdate, int flags)
        {
            return UnsafeNativeMethods.RedrawWindow(hWnd, rcUpdate, hrgnUpdate, flags);
        }

        public static short GetAsyncKeyState(int vKey)
        {
            return UnsafeNativeMethods.GetAsyncKeyState(vKey);
        }

        public static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags)
        {
            return UnsafeNativeMethods.SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags);
        }

        public static IntPtr GetForegroundWindow()
        {
            return UnsafeNativeMethods.GetForegroundWindow();
        }

        public static int GetCurrentThreadId()
        {
            return UnsafeNativeMethods.GetCurrentThreadId();
        }

        public static int SetCapture(IntPtr hWnd)
        {
            return UnsafeNativeMethods.SetCapture(hWnd);
        }

        public static bool ReleaseCapture()
        {
            return UnsafeNativeMethods.ReleaseCapture();
        }

        public static bool IsWindowVisible(IntPtr hWnd)
        {
            return UnsafeNativeMethods.IsWindowVisible(hWnd);
        }

        public static IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, int lParam)
        {
            return UnsafeNativeMethods.SendMessage(hWnd, Msg, wParam, lParam);
        }

        public static bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow)
        {
            return UnsafeNativeMethods.ShowScrollBar(hWnd, wBar, bShow);
        }

        public static IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam)
        {
            return UnsafeNativeMethods.SendMessage(hWnd, Msg, wParam, lParam);
        }

        public static int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam)
        {
            return UnsafeNativeMethods.SendMessage(hWnd, Msg, wParam, lParam);
        }

        public static IntPtr GetOpenClipboardWindow()
        {
            return UnsafeNativeMethods.GetOpenClipboardWindow();
        }

        public static int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId)
        {
            return UnsafeNativeMethods.GetWindowThreadProcessId(hWnd, out lpdwProcessId);
        }

        public static int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT cds)
        {
            return UnsafeNativeMethods.SendMessage(hWnd, Msg, wParam, ref cds);
        }

        public static int PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam)
        {
            return UnsafeNativeMethods.PostMessage(hWnd, Msg, wParam, lParam);
        }

        public static bool IsZoomed(IntPtr hWnd)
        {
            return UnsafeNativeMethods.IsZoomed(hWnd);
        }

        public static bool IsIconic(IntPtr hWnd)
        {
            return UnsafeNativeMethods.IsIconic(hWnd);
        }

        public static bool GetWindowRect(IntPtr hWnd, ref RECT lpRect)
        {
            return UnsafeNativeMethods.GetWindowRect(hWnd, ref lpRect);
        }

        public static bool ValidateRect(IntPtr hWnd, ref RECT lpRect)
        {
            return UnsafeNativeMethods.ValidateRect(hWnd, ref lpRect);
        }

        public static IntPtr BeginPaint(IntPtr hWnd, [In] [Out] ref PAINTSTRUCT lpPaint)
        {
            return UnsafeNativeMethods.BeginPaint(hWnd, ref lpPaint);
        }

        public static bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint)
        {
            return UnsafeNativeMethods.EndPaint(hWnd, ref lpPaint);
        }

        public static bool LockWindowUpdate(IntPtr hWnd)
        {
            return UnsafeNativeMethods.LockWindowUpdate(hWnd);
        }

        public static int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw)
        {
            return UnsafeNativeMethods.SetWindowRgn(hWnd, hRgn, redraw);
        }

        public static int GetClipBox(IntPtr hdc, out RECT lprc)
        {
            return UnsafeNativeMethods.GetClipBox(hdc, out lprc);
        }

        public static int CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, int fnCombineMode)
        {
            return UnsafeNativeMethods.CombineRgn(hrgnDest, hrgnSrc1, hrgnSrc2, fnCombineMode);
        }

        public static int ExcludeClipRect(IntPtr hdc, int left, int top, int right, int bottom)
        {
            return UnsafeNativeMethods.ExcludeClipRect(hdc, left, top, right, bottom);
        }

        public static int GetClipRgn(IntPtr hdc, IntPtr hrgn)
        {
            return UnsafeNativeMethods.GetClipRgn(hdc, hrgn);
        }

        public static int SelectClipRgn(IntPtr hdc, IntPtr hrgn)
        {
            return UnsafeNativeMethods.SelectClipRgn(hdc, hrgn);
        }

        public static int ExtSelectClipRgn(IntPtr hdc, IntPtr hrgn, int mode)
        {
            return UnsafeNativeMethods.ExtSelectClipRgn(hdc, hrgn, mode);
        }

        public static bool LPtoDP(IntPtr hdc, [In] [Out] POINT[] lpPoints, int nCount)
        {
            return UnsafeNativeMethods.LPtoDP(hdc, lpPoints, nCount);
        }

        public static int GetUpdateRect(IntPtr hWnd, ref RECT lpRect, bool erase)
        {
            return UnsafeNativeMethods.GetUpdateRect(hWnd, ref lpRect, erase);
        }

        public static bool GetUpdateRgn(IntPtr hWnd, IntPtr hrgn, bool erase)
        {
            return UnsafeNativeMethods.GetUpdateRgn(hWnd, hrgn, erase);
        }

        public static int GetRegionData(IntPtr hRgn, int dwCount, IntPtr lpRgnData)
        {
            return UnsafeNativeMethods.GetRegionData(hRgn, dwCount, lpRgnData);
        }

        public static int OffsetRgn(IntPtr hRgn, int nXOffset, int nYOffset)
        {
            return UnsafeNativeMethods.OffsetRgn(hRgn, nXOffset, nYOffset);
        }

        public static KeyState GetKeyState(Keys key)
        {
            KeyState keyState = KeyState.None;
            short keyState2 = UnsafeNativeMethods.GetKeyState((int)key);
            if ((keyState2 & 0x8000) == 32768)
            {
                keyState |= KeyState.Down;
            }
            if ((keyState2 & 1) == 1)
            {
                keyState |= KeyState.Toggled;
            }
            return keyState;
        }

        public static int MapWindowPoints(IntPtr hwndFrom, IntPtr hwndTo, ref POINT lpPoints, [MarshalAs(UnmanagedType.U4)] int cPoints)
        {
            return UnsafeNativeMethods.MapWindowPoints(hwndFrom, hwndTo, ref lpPoints, cPoints);
        }

        public static int GetRandomRgn(IntPtr hdc, IntPtr hrgn, int iNum)
        {
            return UnsafeNativeMethods.GetRandomRgn(hdc, hrgn, iNum);
        }

        public static IntPtr CreateRectRgn(Rectangle rect)
        {
            return CreateRectRgn(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        public static IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
        {
            return UnsafeNativeMethods.CreateRectRgn(nLeftRect, nTopRect, nRightRect, nBottomRect);
        }

        public static bool RectVisible(IntPtr hdc, ref RECT rect)
        {
            return UnsafeNativeMethods.RectVisible(hdc, ref rect);
        }

        public static bool DragDetect(IntPtr hWnd, POINT pt)
        {
            return UnsafeNativeMethods.DragDetect(hWnd, pt);
        }

        public static int GetObject(IntPtr hObject, int nSize, [In] [Out] LOGFONT lf)
        {
            return UnsafeNativeMethods.GetObject(hObject, nSize, lf);
        }

        public static IntPtr SelectPalette(IntPtr hdc, IntPtr hpal, bool bForceBackground)
        {
            return UnsafeNativeMethods.SelectPalette(hdc, hpal, bForceBackground);
        }

        public static int RealizePalette(IntPtr hdc)
        {
            return UnsafeNativeMethods.RealizePalette(hdc);
        }

        public static HDC GetDC(HWND handle)
        {
            return UnsafeNativeMethods.GetDC(handle);
        }

        public static int GetDeviceCaps(HDC hdc, int nIndex)
        {
            return UnsafeNativeMethods.GetDeviceCaps(hdc, nIndex);
        }

        public static IntPtr GetCursor()
        {
            return UnsafeNativeMethods.GetCursor();
        }

        public static bool SetSystemCursor(IntPtr hCursor, int id)
        {
            return UnsafeNativeMethods.SetSystemCursor(hCursor, id);
        }

        internal static IntPtr CreateSolidBrush(COLORREF aColorRef)
        {
            return UnsafeNativeMethods.CreateSolidBrush(aColorRef);
        }

        public static bool FillRect(IntPtr hdc, ref RECT rect, IntPtr hbrush)
        {
            return UnsafeNativeMethods.FillRect(hdc, ref rect, hbrush);
        }

        public static bool FillRect(HDC hdc, ref RECT rect, IntPtr hbrush)
        {
            return UnsafeNativeMethods.FillRect(hdc, ref rect, hbrush);
        }

        public static bool FillRgn(IntPtr hdc, IntPtr hrgn, IntPtr hbrush)
        {
            return UnsafeNativeMethods.FillRgn(hdc, hrgn, hbrush);
        }

        public static int GetPixel(IntPtr hdc, int nXPos, int nYPos)
        {
            return UnsafeNativeMethods.GetPixel(hdc, nXPos, nYPos);
        }

        public static bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, int dwRop)
        {
            return UnsafeNativeMethods.StretchBlt(hdcDest, nXOriginDest, nYOriginDest, nWidthDest, nHeightDest, hdcSrc, nXOriginSrc, nYOriginSrc, nWidthSrc, nHeightSrc, dwRop);
        }

        public static bool UnhookWindowsHookEx(HHOOK aHook)
        {
            return UnsafeNativeMethods.UnhookWindowsHookEx(aHook);
        }

        public static IntPtr CopyIcon(IntPtr hCursor)
        {
            return UnsafeNativeMethods.CopyIcon(hCursor);
        }

        public static IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, int dwThreadId)
        {
            return UnsafeNativeMethods.SetWindowsHookEx(idHook, lpfn, hMod, dwThreadId);
        }

        public static IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam)
        {
            return UnsafeNativeMethods.CallNextHookEx(hhk, nCode, wParam, lParam);
        }

        public static IntPtr GetModuleHandle(string lpModuleName)
        {
            return UnsafeNativeMethods.GetModuleHandle(lpModuleName);
        }

        public static bool SetViewportOrgEx(IntPtr hDC, int x, int y, out POINT point)
        {
            return UnsafeNativeMethods.SetViewportOrgEx(hDC, x, y, out point);
        }

        public static bool ChangeWindowMessageFilter(int msg, ChangeWindowMessageFilterFlags flags)
        {
            return UnsafeNativeMethods.ChangeWindowMessageFilter(msg, flags);
        }

        internal static bool IsLayoutRTL(IntPtr hwnd)
        {
            return (GetWindowLong(hwnd, -20) & 0x400000) != 0;
        }

        public static int ReadInt32(IntPtr ptr, int ofs)
        {
            return Marshal.ReadInt32(ptr, ofs);
        }

        public static void WriteInt32(IntPtr ptr, int ofs, int val)
        {
            Marshal.WriteInt32(ptr, ofs, val);
        }

        public static bool IsKeyboardContextMenuMessage(Message msg)
        {
            if (msg.Msg != 123)
            {
                return false;
            }
            Point point = new Point(msg.LParam.ToInt32());
            if (point.X == -1 && point.Y == -1)
            {
                return true;
            }
            return false;
        }

        public static void ExcludeClipRect(IntPtr hdc, Rectangle rect)
        {
            UnsafeNativeMethods.ExcludeClipRect(hdc, rect.X, rect.Y, rect.Right, rect.Bottom);
        }

        public static Rectangle[] GetClipRectsFromHDC(IntPtr hWnd, IntPtr hdc, bool offsetPoints)
        {
            IntPtr intPtr = CreateRectRgn(0, 0, 0, 0);
            try
            {
                if (UnsafeNativeMethods.GetRandomRgn(hdc, intPtr, SYSRGN) != 1)
                {
                    return null;
                }
                if (offsetPoints)
                {
                    POINT lpPoints = default(POINT);
                    UnsafeNativeMethods.MapWindowPoints(IntPtr.Zero, hWnd, ref lpPoints, 1);
                    if (IsLayoutRTL(hWnd))
                    {
                        Control control = Control.FromHandle(hWnd);
                        if (control != null)
                        {
                            lpPoints.X = lpPoints.X * -1 + control.Width;
                        }
                    }
                    UnsafeNativeMethods.OffsetRgn(intPtr, lpPoints.X, lpPoints.Y);
                }
                RECT[] array = RectsFromRegion(intPtr);
                if (array == null || array.Length == 0)
                {
                    return null;
                }
                Rectangle[] array2 = new Rectangle[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    array2[i] = array[i].ToRectangle();
                }
                return array2;
            }
            finally
            {
                UnsafeNativeMethods.DeleteObject(intPtr);
            }
        }

        public static int SignedLOWORD(IntPtr n)
        {
            return SignedLOWORD((int)(long)n);
        }

        public static int SignedLOWORD(int n)
        {
            return (short)(n & 0xFFFF);
        }

        public static int SignedHIWORD(IntPtr n)
        {
            return SignedHIWORD((int)(long)n);
        }

        public static int SignedHIWORD(int n)
        {
            return (short)((n >> 16) & 0xFFFF);
        }

        public static Point FromMouseLParam(ref Message m)
        {
            return new Point(SignedLOWORD(m.LParam), SignedHIWORD(m.LParam));
        }

        public static RECT[] RectsFromRegion(IntPtr hRgn)
        {
            RECT[] array = null;
            int regionData = UnsafeNativeMethods.GetRegionData(hRgn, 0, IntPtr.Zero);
            if (regionData != 0)
            {
                IntPtr zero = IntPtr.Zero;
                zero = Marshal.AllocCoTaskMem(regionData);
                UnsafeNativeMethods.GetRegionData(hRgn, regionData, zero);
                RGNDATAHEADER rGNDATAHEADER = (RGNDATAHEADER)Marshal.PtrToStructure(zero, typeof(RGNDATAHEADER));
                if (rGNDATAHEADER.iType == 1)
                {
                    array = new RECT[rGNDATAHEADER.nCount];
                    int dwSize = rGNDATAHEADER.dwSize;
                    for (int i = 0; i < rGNDATAHEADER.nCount; i++)
                    {
                        IntPtr ptr = new IntPtr(zero.ToInt64() + dwSize + Marshal.SizeOf(typeof(RECT)) * i);
                        array[i] = (RECT)Marshal.PtrToStructure(ptr, typeof(RECT));
                    }
                }
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(zero);
                }
            }
            return array;
        }

        public static IntPtr CreateSolidBrush(Color aColor)
        {
            return UnsafeNativeMethods.CreateSolidBrush(new COLORREF(aColor));
        }

        public static IntPtr CreateSolidBrush(int argb)
        {
            return UnsafeNativeMethods.CreateSolidBrush(new COLORREF(argb));
        }

        public static Region CreateRoundRegion(Rectangle windowBounds, int ellipseSize)
        {
            IntPtr intPtr = UnsafeNativeMethods.CreateRoundRectRgn(windowBounds.X, windowBounds.Y, windowBounds.Width + 1, windowBounds.Height + 1, ellipseSize, ellipseSize);
            Region result = Region.FromHrgn(intPtr);
            DeleteObject(intPtr);
            return result;
        }

        public static bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE pSizeDst, IntPtr hdcSrc, ref POINT pptSrc, int crKey, ref BLENDFUNCTION pBlend, int dwFlags)
        {
            return UnsafeNativeMethods.UpdateLayeredWindow(hwnd, hdcDst, ref pptDst, ref pSizeDst, hdcSrc, ref pptSrc, crKey, ref pBlend, dwFlags);
        }

        public static int GetMessage(out Message lpMsg, HWND hwnd, int wMsgFilterMin = 0, int mMsgFilterMax = 0)
        {
            return UnsafeNativeMethods.GetMessage(out lpMsg, hwnd, wMsgFilterMin, mMsgFilterMax);
        }

        public static void TranslateMessage(ref Message msg)
        {
            UnsafeNativeMethods.TranslateMessage(ref msg);
        }

        public static void DispatchMessage(ref Message msg)
        {
            UnsafeNativeMethods.DispatchMessage(ref msg);
        }

        public static bool PostThreadMessage(int threadId, int msg)
        {
            return PostThreadMessage(threadId, msg, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool PostThreadMessage(int threadId, int msg, IntPtr wParam, IntPtr lParam)
        {
            return UnsafeNativeMethods.PostThreadMessage(threadId, msg, wParam, lParam);
        }

        public static bool InvalidateRgn(IntPtr hWnd, IntPtr hrgn, bool erase)
        {
            return UnsafeNativeMethods.InvalidateRgn(hWnd, hrgn, erase);
        }

        public static bool SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags)
        {
            return UnsafeNativeMethods.SetLayeredWindowAttributes(hwnd, crKey, bAlpha, (uint)dwFlags);
        }

        public static bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle)
        {
            return UnsafeNativeMethods.AdjustWindowRectEx(ref lpRect, dwStyle, bMenu, dwExStyle);
        }

        public static bool AdjustWindowRectExDPI(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle, int dpi)
        {
            if (!IsWindow10)
            {
                return AdjustWindowRectEx(ref lpRect, dwStyle, bMenu, dwExStyle);
            }
            return UnsafeNativeMethods.AdjustWindowRectExForDpi(ref lpRect, dwStyle, bMenu, dwExStyle, dpi);
        }

        public static IntPtr FindWindow(string className, string windowText)
        {
            return UnsafeNativeMethods.FindWindow(className, windowText);
        }

        public static int ShowWindow(IntPtr hWnd, int command)
        {
            return UnsafeNativeMethods.ShowWindow(hWnd, command);
        }

        public static bool ShowWindow(IntPtr hWnd, ShowWindowCommands command)
        {
            return UnsafeNativeMethods.ShowWindow(hWnd, command);
        }

        public static bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl)
        {
            return UnsafeNativeMethods.GetWindowPlacement(hWnd, out lpwndpl);
        }

        public static bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl)
        {
            return UnsafeNativeMethods.SetWindowPlacement(hWnd, ref lpwndpl);
        }

        public static IntPtr WindowFromPoint(Point pt)
        {
            return UnsafeNativeMethods.WindowFromPoint(pt);
        }

        public static IntPtr GetWindow(IntPtr hWnd, int wCmd)
        {
            return UnsafeNativeMethods.GetWindow(hWnd, (uint)wCmd);
        }

        public static IntPtr SetActiveWindow(IntPtr hWnd)
        {
            return UnsafeNativeMethods.SetActiveWindow(hWnd);
        }

        public static bool SetForegroundWindow(IntPtr hWnd)
        {
            return UnsafeNativeMethods.SetForegroundWindow(hWnd);
        }

        public static bool EnableWindow(IntPtr hWnd, bool enable)
        {
            return UnsafeNativeMethods.EnableWindow(hWnd, enable);
        }

        public static bool IsWindowEnabled(IntPtr hWnd)
        {
            return UnsafeNativeMethods.IsWindowEnabled(hWnd);
        }

        public static bool SystemParametersInfo(int uiAction, int uiParam, NONCLIENTMETRICS pvParam, int fWinIni)
        {
            return UnsafeNativeMethods.SystemParametersInfo(uiAction, uiParam, pvParam, fWinIni);
        }

        public static bool SetGestureConfig(IntPtr hWnd, int dwReserved, int cIDs, [In] [Out] GESTURECONFIG[] pGestureConfig, int cbSize)
        {
            return UnsafeNativeMethods.SetGestureConfig(hWnd, dwReserved, cIDs, pGestureConfig, cbSize);
        }

        public static bool BeginPanningFeedback(IntPtr hWnd)
        {
            return UnsafeNativeMethods.BeginPanningFeedback(hWnd);
        }

        public static bool EndPanningFeedback(IntPtr hWnd, bool fAnimateBack)
        {
            return UnsafeNativeMethods.EndPanningFeedback(hWnd, fAnimateBack);
        }

        public static bool UpdatePanningFeedback(IntPtr hwnd, int lTotalOverpanOffsetX, int lTotalOverpanOffsetY, bool fInInertia)
        {
            return UnsafeNativeMethods.UpdatePanningFeedback(hwnd, lTotalOverpanOffsetX, lTotalOverpanOffsetY, fInInertia);
        }

        public static bool CloseGestureInfoHandle(IntPtr hGestureInfo)
        {
            return UnsafeNativeMethods.CloseGestureInfoHandle(hGestureInfo);
        }

        public static bool GetGestureInfo(IntPtr hGestureInfo, ref GESTUREINFO pGestureInfo)
        {
            return UnsafeNativeMethods.GetGestureInfo(hGestureInfo, ref pGestureInfo);
        }

        public static int DwmSetIconicThumbnail(IntPtr hwnd, IntPtr hbitmap, int flags)
        {
            return UnsafeNativeMethods.DwmSetIconicThumbnail(hwnd, hbitmap, (uint)flags);
        }

        public static int DwmSetIconicLivePreviewBitmap(IntPtr hwnd, IntPtr hbitmap, IntPtr ptClient, int flags)
        {
            return UnsafeNativeMethods.DwmSetIconicLivePreviewBitmap(hwnd, hbitmap, ptClient, (uint)flags);
        }

        public static int DwmSetWindowAttribute(IntPtr hwnd, int dwAttributeToSet, IntPtr pvAttributeValue, int cbAttribute)
        {
            return UnsafeNativeMethods.DwmSetWindowAttribute(hwnd, (uint)dwAttributeToSet, pvAttributeValue, (uint)cbAttribute);
        }

        //public static void DwmSetWindowAttributeInt(IntPtr handle, NativeVista.DWMWINDOWATTRIBUTE dwmAttribute, int value)
        //{
        //	UnsafeNativeMethods.DwmSetWindowAttributeInt(handle, dwmAttribute, value);
        //}

        public static int DwmInvalidateIconicBitmaps(IntPtr hwnd)
        {
            return UnsafeNativeMethods.DwmInvalidateIconicBitmaps(hwnd);
        }

        public static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount)
        {
            return UnsafeNativeMethods.GetWindowText(hWnd, lpString, nMaxCount);
        }

        public static bool ClientToScreen(IntPtr hwnd, ref POINT point)
        {
            return UnsafeNativeMethods.ClientToScreen(hwnd, ref point);
        }

        public static bool ScreenToClient(IntPtr hwnd, ref POINT point)
        {
            return UnsafeNativeMethods.ScreenToClient(hwnd, ref point);
        }

        public static int ExtractIconEx(string fileName, int iconStartingIndex, IntPtr[] largeIcons, IntPtr[] smallIcons, int iconCount)
        {
            return UnsafeNativeMethods.ExtractIconEx(fileName, iconStartingIndex, largeIcons, smallIcons, iconCount);
        }

        public static void SHAddToRecentDocs(ShellAddToRecentDocs flags, [MarshalAs(UnmanagedType.LPWStr)] string path)
        {
            UnsafeNativeMethods.SHAddToRecentDocs(flags, path);
        }

        public static void PropVariantClear([In] [Out] PropVariant pvar)
        {
            UnsafeNativeMethods.PropVariantClear(pvar);
        }

        public static int RegisterWindowMessage(string lpProcName)
        {
            return (int)UnsafeNativeMethods.RegisterWindowMessage(lpProcName);
        }

        public static void PostQuitMessage(int exitCode)
        {
            UnsafeNativeMethods.PostQuitMessage(exitCode);
        }

        public static IntPtr LocalFree(IntPtr p)
        {
            return UnsafeNativeMethods.LocalFree(p);
        }

        public static IntPtr LocalAlloc(int flag, int size)
        {
            return UnsafeNativeMethods.LocalAlloc(flag, size);
        }

        public static void CopyMemory(IntPtr dest, IntPtr src, int count)
        {
            UnsafeNativeMethods.CopyMemory(dest, src, (uint)count);
        }

        public static bool ShowWindowAsync(IntPtr hWnd, int nCmdShow)
        {
            return UnsafeNativeMethods.ShowWindowAsync(hWnd, nCmdShow);
        }

        public static void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID)
        {
            UnsafeNativeMethods.SetCurrentProcessExplicitAppUserModelID(AppID);
        }

        public static IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert)
        {
            return UnsafeNativeMethods.GetSystemMenu(hWnd, bRevert);
        }

        public static bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem)
        {
            return UnsafeNativeMethods.AppendMenu(hMenu, uFlags, uIDNewItem, lpNewItem);
        }

        public static bool RemoveMenu(IntPtr hMenu, int uPosition, int uFlags)
        {
            return UnsafeNativeMethods.RemoveMenu(hMenu, uPosition, uFlags);
        }

        public static void GetMenuItemInfo(IntPtr hMenu, int uItem, bool fByPosition, ref MENUITEMINFO lpmii)
        {
            UnsafeNativeMethods.GetMenuItemInfo(hMenu, uItem, fByPosition, ref lpmii);
        }

        public static void EnableMenuItem(IntPtr hMenu, int uIDEnableItem, int uEnable)
        {
            UnsafeNativeMethods.EnableMenuItem(hMenu, uIDEnableItem, uEnable);
        }

        public static int GetMenuItemCount(IntPtr hMenu)
        {
            return UnsafeNativeMethods.GetMenuItemCount(hMenu);
        }

        public static bool PeekMessage(out NativeMessage lpMsg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax, int wRemoveMsg)
        {
            return UnsafeNativeMethods.PeekMessage(out lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax, wRemoveMsg);
        }

        public static bool SystemParametersInfo(int nAction, int nParam, ANIMATIONINFO value, int ignore)
        {
            return UnsafeNativeMethods.SystemParametersInfo(nAction, nParam, value, ignore);
        }

        public static SizeF GetFontAutoScaleDimensions(Font font, Control control)
        {
            using (Graphics graphics = control.CreateGraphics())
            {
                IntPtr hdc = graphics.GetHdc();
                try
                {
                    return GetFontAutoScaleDimensions(font, hdc);
                }
                finally
                {
                    graphics.ReleaseHdc(hdc);
                }
            }
        }

        public static SizeF GetFontAutoScaleDimensions(Font font)
        {
            return GetFontAutoScaleDimensions(font, IntPtr.Zero);
        }

        public static SizeF GetFontAutoScaleDimensions(Font font, IntPtr sourceDC)
        {
            SizeF empty = SizeF.Empty;
            IntPtr intPtr = sourceDC;
            if (intPtr == IntPtr.Zero)
            {
                intPtr = CreateCompatibleDC(IntPtr.Zero);
            }
            if (!(intPtr == IntPtr.Zero))
            {
                IntPtr intPtr2 = font.ToHfont();
                HandleRef handleRef = new HandleRef(font, intPtr);
                try
                {
                    HandleRef obj = new HandleRef(font, intPtr2);
                    HandleRef obj2 = new HandleRef(font, SelectObject(handleRef, obj));
                    try
                    {
                        TEXTMETRIC lptm = default(TEXTMETRIC);
                        GetTextMetrics(handleRef, ref lptm);
                        empty.Height = (float)lptm.tmHeight;
                        if ((lptm.tmPitchAndFamily & 1) == 0)
                        {
                            empty.Width = (float)lptm.tmAveCharWidth;
                            return empty;
                        }
                        SIZE size = default(SIZE);
                        UnsafeNativeMethods.GetTextExtentPoint32(handleRef, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", ref size);
                        empty.Width = (float)(int)Math.Round((double)((float)size.Width / (float)"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Length));
                        return empty;
                    }
                    finally
                    {
                        SelectObject(handleRef, obj2);
                    }
                }
                finally
                {
                    if (sourceDC == IntPtr.Zero)
                    {
                        DeleteDC(handleRef);
                    }
                    DeleteObject(intPtr2);
                }
            }
            throw new Exception();
        }

        internal static int GetTextMetrics(HandleRef hDC, ref TEXTMETRIC lptm)
        {
            if (Marshal.SystemDefaultCharSize == 1)
            {
                TEXTMETRICA lptm2 = default(TEXTMETRICA);
                int textMetricsA = UnsafeNativeMethods.GetTextMetricsA(hDC, ref lptm2);
                lptm.tmHeight = lptm2.tmHeight;
                lptm.tmAscent = lptm2.tmAscent;
                lptm.tmDescent = lptm2.tmDescent;
                lptm.tmInternalLeading = lptm2.tmInternalLeading;
                lptm.tmExternalLeading = lptm2.tmExternalLeading;
                lptm.tmAveCharWidth = lptm2.tmAveCharWidth;
                lptm.tmMaxCharWidth = lptm2.tmMaxCharWidth;
                lptm.tmWeight = lptm2.tmWeight;
                lptm.tmOverhang = lptm2.tmOverhang;
                lptm.tmDigitizedAspectX = lptm2.tmDigitizedAspectX;
                lptm.tmDigitizedAspectY = lptm2.tmDigitizedAspectY;
                lptm.tmFirstChar = (char)lptm2.tmFirstChar;
                lptm.tmLastChar = (char)lptm2.tmLastChar;
                lptm.tmDefaultChar = (char)lptm2.tmDefaultChar;
                lptm.tmBreakChar = (char)lptm2.tmBreakChar;
                lptm.tmItalic = lptm2.tmItalic;
                lptm.tmUnderlined = lptm2.tmUnderlined;
                lptm.tmStruckOut = lptm2.tmStruckOut;
                lptm.tmPitchAndFamily = lptm2.tmPitchAndFamily;
                lptm.tmCharSet = lptm2.tmCharSet;
                return textMetricsA;
            }
            return UnsafeNativeMethods.GetTextMetricsW(hDC, ref lptm);
        }

        public static PROCESS_DPI_AWARENESS GetProcessDpiAwareness()
        {
            PROCESS_DPI_AWARENESS value = PROCESS_DPI_AWARENESS.PROCESS_DPI_UNAWARE;
            if (IsPerMonitorDpiAPISupported)
            {
                using (Process process = Process.GetCurrentProcess())
                {
                    UnsafeNativeMethods.GetProcessDpiAwareness(process.Handle, out value);
                    return value;
                }
            }
            return value;
        }

        public static bool ProcessWmDpiChanged(Message msg)
        {
            if (msg.Msg == 736)
            {
                //DpiChangedEventArgs dpiChangedEventArgs = new DpiChangedEventArgs(msg.HWnd, msg.WParam, msg.LParam);
                //IPerMonitorDpiAware client = dpiChangedEventArgs.Client;
                //if (client != null)
                //{
                //	return client.OnDpiScaleChanged(dpiChangedEventArgs);
                //}
            }
            return false;
        }

        public static IntPtr MonitorFromPoint(Point point, MONITORFROMWINDOW_FLAGS flags = MONITORFROMWINDOW_FLAGS.MONITOR_DEFAULTTONEAREST)
        {
            return UnsafeNativeMethods.MonitorFromPoint(point, (uint)flags);
        }

        public static float GetDpiForMonitor(IntPtr hMonitor, MONITOR_DPI_TYPE dpiType = MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI)
        {
            uint dpiY = 96u;
            if (IsPerMonitorDpiAPISupported || IsWindow10)
            {
                UnsafeNativeMethods.GetDpiForMonitor(hMonitor, dpiType, out uint _, out dpiY);
                switch (dpiType)
                {
                    case MONITOR_DPI_TYPE.MDT_ANGULAR_DPI:
                    case MONITOR_DPI_TYPE.MDT_RAW_DPI:
                        return (float)(double)dpiY / 102f;
                }
            }
            return (float)(double)dpiY / 96f;
        }

        public static Rectangle GetPhysicalMonitorBounds(Screen screen)
        {
            DEVMODE devMode = default(DEVMODE);
            if (UnsafeNativeMethods.EnumDisplaySettings(screen.DeviceName, -1, ref devMode))
            {
                return new Rectangle(devMode.dmPositionX, devMode.dmPositionY, devMode.dmPelsWidth, devMode.dmPelsHeight);
            }
            return screen.Bounds;
        }

        public static float GetDpiScaleFactor(IntPtr hWnd, MONITORFROMWINDOW_FLAGS flags = MONITORFROMWINDOW_FLAGS.MONITOR_DEFAULTTONULL, MONITOR_DPI_TYPE dpiType = MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI)
        {
            uint dpiY = 96u;
            if (IsPerMonitorDpiAPISupported)
            {
                IntPtr hMonitor = UnsafeNativeMethods.MonitorFromWindow(hWnd, (int)flags);
                UnsafeNativeMethods.GetDpiForMonitor(hMonitor, dpiType, out uint _, out dpiY);
            }
            return (float)(double)dpiY / 96f;
        }
    }
}