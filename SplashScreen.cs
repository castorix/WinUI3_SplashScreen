using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using GlobalStructures;
using MFPlay;
using static MFPlay.MFPlayTools;
using Microsoft.UI.Xaml.Controls;

namespace WinUI3_SplashScreen
{
    public class SplashScreen
    {
        //[DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        //public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref System.Drawing.Point pptDst, ref System.Drawing.Size psize, IntPtr hdcSrc, ref System.Drawing.Point pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, IntPtr pptDst, IntPtr psize, IntPtr hdcSrc, IntPtr pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;

        public const int ULW_COLORKEY = 0x00000001;
        public const int ULW_ALPHA = 0x00000002;
        public const int ULW_OPAQUE = 0x00000004;

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetObject(IntPtr hFont, int nSize, out BITMAP bm);

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct BITMAP
        {
            public int bmType;
            public int bmWidth;
            public int bmHeight;
            public int bmWidthBytes;
            public short bmPlanes;
            public short bmBitsPixel;
            public IntPtr bmBits;
        }

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public RECT(int Left, int Top, int Right, int Bottom)
            {
                left = Left;
                top = Top;
                right = Right;
                bottom = Bottom;
            }
        }

        public const long STGM_READ = 0x00000000L;
        public const long GENERIC_READ = (0x80000000L);
        public const long GENERIC_WRITE = (0x40000000L);

        [DllImport("Shlwapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern HRESULT SHCreateStreamOnFile(string pszFile, int grfMode, out System.Runtime.InteropServices.ComTypes.IStream ppstm);

        [DllImport("GdiPlus.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern GpStatus GdiplusStartup(out IntPtr token, ref StartupInput input, out StartupOutput output);

        [DllImport("GdiPlus.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void GdiplusShutdown(IntPtr token);

        [StructLayout(LayoutKind.Sequential)]
        public struct StartupInput
        {
            public int GdiplusVersion;             // Must be 1

            // public DebugEventProc DebugEventCallback; // Ignored on free builds
            public IntPtr DebugEventCallback;

            public bool SuppressBackgroundThread;     // FALSE unless you're prepared to call 
                                                      // the hook/unhook functions properly

            public bool SuppressExternalCodecs;       // FALSE unless you want GDI+ only to use
                                                      // its internal image codecs.

            public static StartupInput GetDefault()
            {
                StartupInput result = new StartupInput();
                result.GdiplusVersion = 1;
                // result.DebugEventCallback = null;
                result.SuppressBackgroundThread = false;
                result.SuppressExternalCodecs = false;
                return result;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct StartupOutput
        {
            // The following 2 fields won't be used.  They were originally intended 
            // for getting GDI+ to run on our thread - however there are marshalling
            // dealing with function *'s and what not - so we make explicit calls
            // to gdi+ after the fact, via the GdiplusNotificationHook and 
            // GdiplusNotificationUnhook methods.
            public IntPtr hook;//not used
            public IntPtr unhook;//not used.
        }

        [DllImport("GdiPlus.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern GpStatus GdipCreateBitmapFromFile(string filename, out IntPtr bitmap);

        [DllImport("GdiPlus.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern GpStatus GdipCreateBitmapFromStream(System.Runtime.InteropServices.ComTypes.IStream Stream, out IntPtr bitmap);

        [DllImport("GdiPlus.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern GpStatus GdipCreateHBITMAPFromBitmap(HandleRef nativeBitmap, out IntPtr hbitmap, int argbBackground);

        public enum GpStatus : int
        {
            Ok = 0,
            GenericError = 1,
            InvalidParameter = 2,
            OutOfMemory = 3,
            ObjectBusy = 4,
            InsufficientBuffer = 5,
            NotImplemented = 6,
            Win32Error = 7,
            WrongState = 8,
            Aborted = 9,
            FileNotFound = 10,
            ValueOverflow = 11,
            AccessDenied = 12,
            UnknownImageFormat = 13,
            FontFamilyNotFound = 14,
            FontStyleNotFound = 15,
            NotTrueTypeFont = 16,
            UnsupportedGdiplusVersion = 17,
            GdiplusNotInitialized = 18,
            PropertyNotFound = 19,
            PropertyNotSupported = 20,
            ProfileNotFound = 21,
        }

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        public const int WS_OVERLAPPED = 0x00000000,
            WS_POPUP = unchecked((int)0x80000000),
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_CAPTION = 0x00C00000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_TABSTOP = 0x00010000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_OVERLAPPEDWINDOW = 0xcf0000;

        public const int WS_EX_DLGMODALFRAME = 0x00000001;
        public const int WS_EX_NOPARENTNOTIFY = 0x00000004;
        public const int WS_EX_TOPMOST = 0x00000008;
        public const int WS_EX_ACCEPTFILES = 0x00000010;
        public const int WS_EX_TRANSPARENT = 0x00000020;

        public const int WS_EX_MDICHILD = 0x00000040;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_WINDOWEDGE = 0x00000100;
        public const int WS_EX_CLIENTEDGE = 0x00000200;
        public const int WS_EX_CONTEXTHELP = 0x00000400;

        public const int WS_EX_RIGHT = 0x00001000;
        public const int WS_EX_LEFT = 0x00000000;
        public const int WS_EX_RTLREADING = 0x00002000;
        public const int WS_EX_LTRREADING = 0x00000000;
        public const int WS_EX_LEFTSCROLLBAR = 0x00004000;
        public const int WS_EX_RIGHTSCROLLBAR = 0x00000000;

        public const int WS_EX_CONTROLPARENT = 0x00010000;
        public const int WS_EX_STATICEDGE = 0x00020000;
        public const int WS_EX_APPWINDOW = 0x00040000;

        public const int WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        public const int WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);

        public const int WS_EX_LAYERED = 0x00080000;

        public const int WS_EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children

        public const int WS_EX_NOREDIRECTIONBITMAP = 0x00200000;

        public const int WS_EX_LAYOUTRTL = 0x00400000; // Right to left mirroring

        public const int WS_EX_COMPOSITED = 0x02000000;
        public const int WS_EX_NOACTIVATE = 0x08000000;

        public const int CS_USEDEFAULT = unchecked((int)0x80000000);
        public const int CS_DBLCLKS = 8;
        public const int CS_VREDRAW = 1;
        public const int CS_HREDRAW = 2;
        public const int COLOR_BACKGROUND = 1;
        public const int COLOR_WINDOW = 5;
        public const int IDC_ARROW = 32512;
        public const int IDC_IBEAM = 32513;
        public const int IDC_WAIT = 32514;
        public const int IDC_CROSS = 32515;
        public const int IDC_UPARROW = 32516;

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CallWindowProc(IntPtr wndProc, IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int DefWindowProc(IntPtr hWnd, uint uMsg, UIntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short RegisterClass(ref WNDCLASS wc);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short RegisterClassEx(ref WNDCLASSEX lpwcx);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);

        public delegate int WNDPROC(IntPtr hwnd, uint uMsg, UIntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WNDCLASS
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint style;
            public WNDPROC lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string lpszClassName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WNDCLASSEX
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public int style;
            public WNDPROC lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;
        }

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        const int GWL_STYLE = (-16);
        const int GWL_EXSTYLE = (-20);
        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 4)
            {
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
        public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        // public static IntPtr GetWindowLong(HandleRef hWnd, int nIndex)
        public static long GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return GetWindowLong32(hWnd, nIndex);
            }
            return GetWindowLongPtr64(hWnd, nIndex);
        }

        [DllImport("User32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        public static extern long GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("User32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern long GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public const int SPI_GETWORKAREA = 0x30;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, [In, Out] ref RECT pvParam, uint fWinIni);

        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOREDRAW = 0x0008;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
        public const int SWP_SHOWWINDOW = 0x0040;
        public const int SWP_HIDEWINDOW = 0x0080;
        public const int SWP_NOCOPYBITS = 0x0100;
        public const int SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
        public const int SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */
        public const int SWP_DRAWFRAME = SWP_FRAMECHANGED;
        public const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;
        public const int SWP_DEFERERASE = 0x2000;
        public const int SWP_ASYNCWINDOWPOS = 0x4000;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);


        private DispatcherTimer dTimer;
        private TimeSpan tsFadeoutDuration;
        private DateTime tsFadeoutEnd;
        IntPtr hWndSplash = IntPtr.Zero;
        private WNDPROC delegateWndProc;
        private IntPtr hBitmap = IntPtr.Zero;
        private IntPtr initToken = IntPtr.Zero;

        public void Initialize()
        {
            StartupInput input = StartupInput.GetDefault();
            StartupOutput output;           
            GpStatus nStatus = GdiplusStartup(out initToken, ref input, out output);
        }

        public void DisplaySplash(IntPtr hWnd, IntPtr hBitmap, string sVideo)
        {
            this.hBitmap = hBitmap;
            delegateWndProc = Win32WndProc;
            WNDCLASSEX wcex = new WNDCLASSEX();
            wcex.cbSize = Marshal.SizeOf(typeof(WNDCLASSEX));
            wcex.style = (CS_HREDRAW | CS_VREDRAW | CS_DBLCLKS);
            wcex.hbrBackground = (IntPtr)COLOR_BACKGROUND + 1;
            wcex.cbClsExtra = 0;
            wcex.cbWndExtra = 0;
            wcex.hInstance = Marshal.GetHINSTANCE(this.GetType().Module); // Process.GetCurrentProcess().Handle;
            wcex.hIcon = IntPtr.Zero;
            wcex.hCursor = LoadCursor(IntPtr.Zero, (int)IDC_ARROW);
            wcex.lpszMenuName = null;
            wcex.lpszClassName = "Win32Class";
            //wind_class.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(delegWndProc);
            wcex.lpfnWndProc = delegateWndProc;
            wcex.hIconSm = IntPtr.Zero;
            short nRet = RegisterClassEx(ref wcex);
            if (nRet == 0)
            {
                int nError = Marshal.GetLastWin32Error();
                if (nError != 1410) //0x582 ERROR_CLASS_ALREADY_EXISTS
                    return;
            }
            string sClassName = wcex.lpszClassName;
            int nWidth = 0, nHeight = 0;
            if (hBitmap != IntPtr.Zero)
            {
                BITMAP bm;
                GetObject(hBitmap, Marshal.SizeOf(typeof(BITMAP)), out bm);
                nWidth = bm.bmWidth;
                nHeight = bm.bmHeight;
            }
            //hWndSplash = CreateWindowEx(WS_EX_TOOLWINDOW | WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_TOPMOST | WS_EX_NOACTIVATE, sClassName, "Win32 window", WS_POPUP | WS_VISIBLE, 400, 400, nWidth, nHeight, hWnd, IntPtr.Zero, wcex.hInstance, IntPtr.Zero);
            hWndSplash = CreateWindowEx(WS_EX_TOOLWINDOW | WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_TOPMOST, sClassName, "Win32 window", WS_POPUP | WS_VISIBLE, 400, 400, nWidth, nHeight, hWnd, IntPtr.Zero, wcex.hInstance, IntPtr.Zero);
            if (hBitmap != IntPtr.Zero)
            {
                SetPictureToLayeredWindow(hWndSplash, hBitmap);
                CenterToScreen(hWndSplash);
            }
            if (sVideo != null)
            {
                MFPlayer pPlayer = new MFPlayer(this, hWndSplash, sVideo);
            }           
        }

        public class MFPlayer : IMFPMediaPlayerCallback
        {
            public MFPlayer(SplashScreen ss, IntPtr hWnd, string sVideo)
            {                
                HRESULT hr = MFPCreateMediaPlayer(sVideo, false, MFP_CREATION_OPTIONS.MFP_OPTION_NONE, this, hWnd, out m_pMediaPlayer);
                m_hWndParent = hWnd;
                m_ss = ss;
            }

            SplashScreen m_ss = null;
            IntPtr m_hWndParent = IntPtr.Zero;
            IMFPMediaPlayer m_pMediaPlayer;

            public void OnMediaPlayerEvent(MFP_EVENT_HEADER pEventHeader)
            {
                switch (pEventHeader.eEventType)
                {
                    case MFP_EVENT_TYPE.MFP_EVENT_TYPE_MEDIAITEM_CREATED:
                        break;

                    case MFP_EVENT_TYPE.MFP_EVENT_TYPE_MEDIAITEM_SET:
                        {
                            SIZE szVideo, szARVideo;
                            HRESULT hr = m_pMediaPlayer.GetNativeVideoSize(out szVideo, out szARVideo);
                            if (hr == HRESULT.S_OK)
                            {
                                SetWindowPos(m_hWndParent, IntPtr.Zero, 0, 0, szVideo.cx, szVideo.cy, SWP_NOMOVE | SWP_NOZORDER | SWP_NOACTIVATE);
                                m_ss.CenterToScreen(m_hWndParent);
                                hr = m_pMediaPlayer.Play();
                            }
                        }
                        break;
                }
                return;
            }
        }

        public void HideSplash(int nSeconds)
        {
            dTimer = new DispatcherTimer();
            dTimer.Interval = TimeSpan.FromMilliseconds(60);
            tsFadeoutDuration = TimeSpan.FromSeconds(nSeconds);
            tsFadeoutEnd = DateTime.UtcNow + tsFadeoutDuration;
            dTimer.Tick += Dt_Tick;
            dTimer.Start();
        }

        public async System.Threading.Tasks.Task<IntPtr> GetBitmap(string sBitmapFile)
        {
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr pBitmap = IntPtr.Zero;
            // Uri uri = new System.Uri("ms-appx:///Assets/Butterfly.png");
            //            Some APIs require package identity and are not supported in unpackaged apps, such as:
            //            ApplicationData
            //            StorageFile.GetFileFromApplicationUriAsync
            // var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
            string sDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sPath = sDirectory + sBitmapFile;

            //var storageFile = await Windows.Storage.StorageFile.GetFileFromPathAsync(sPath);
            //Stream stream = await storageFile.OpenStreamForReadAsync();
            // new GPStream(stream)

            System.Runtime.InteropServices.ComTypes.IStream pstm;
            HRESULT hr = SHCreateStreamOnFile(sPath, (int)STGM_READ, out pstm);
            if (hr == HRESULT.S_OK)
            {
                GpStatus nStatus = GdipCreateBitmapFromStream(pstm, out pBitmap);
                if (nStatus == GpStatus.Ok)
                {
                    GdipCreateHBITMAPFromBitmap(new HandleRef(this, pBitmap), out hBitmap, System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.FromArgb(0)));
                }
            }
            return (hBitmap);
        }

        // Adapted from WPF source code
        public void Dt_Tick(object sender, object e)
        {
            DateTime dtNow = DateTime.UtcNow;
            if (dtNow >= tsFadeoutEnd)
            {
                if (dTimer != null)
                {
                    dTimer.Stop();
                    dTimer = null;
                }
                if (hWndSplash != IntPtr.Zero)
                {
                    DestroyWindow(hWndSplash);
                }
                if (hBitmap != IntPtr.Zero)
                {
                    DeleteObject(hBitmap);
                    hBitmap = IntPtr.Zero;
                }
                GdiplusShutdown(initToken);            
            }
            else
            {
                double nProgress = (tsFadeoutEnd - dtNow).TotalMilliseconds / tsFadeoutDuration.TotalMilliseconds;
                BLENDFUNCTION bf = new BLENDFUNCTION();
                bf.BlendOp = AC_SRC_OVER;
                bf.AlphaFormat = AC_SRC_ALPHA;
                bf.SourceConstantAlpha = (byte)(255 * nProgress);               
                UpdateLayeredWindow(hWndSplash, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0, ref bf, ULW_ALPHA);
            }
        }

        private void SetPictureToLayeredWindow(IntPtr hWnd, IntPtr hBitmap)
        {
            BITMAP bm;
            GetObject(hBitmap, Marshal.SizeOf(typeof(BITMAP)), out bm);
            System.Drawing.Size sizeBitmap = new System.Drawing.Size(bm.bmWidth, bm.bmHeight);

            IntPtr hDCScreen = GetDC(IntPtr.Zero);
            IntPtr hDCMem = CreateCompatibleDC(hDCScreen);
            IntPtr hBitmapOld = SelectObject(hDCMem, hBitmap);

            BLENDFUNCTION bf = new BLENDFUNCTION();
            bf.BlendOp = AC_SRC_OVER;
            bf.SourceConstantAlpha = 255;
            bf.AlphaFormat = AC_SRC_ALPHA;

            RECT rectWnd;
            GetWindowRect(hWnd, out rectWnd);

            System.Drawing.Point ptSrc = new System.Drawing.Point();
            System.Drawing.Point ptDest = new System.Drawing.Point(rectWnd.left, rectWnd.top);
 
            IntPtr pptSrc = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(System.Drawing.Point)));
            Marshal.StructureToPtr(ptSrc, pptSrc, false);           

            IntPtr pptDest = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(System.Drawing.Point)));
            Marshal.StructureToPtr(ptDest, pptDest, false);           

            IntPtr psizeBitmap = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(System.Drawing.Size)));
            Marshal.StructureToPtr(sizeBitmap, psizeBitmap, false);

            bool bRet = UpdateLayeredWindow(hWnd, hDCScreen, pptDest, psizeBitmap, hDCMem, pptSrc, 0, ref bf, ULW_ALPHA);
            //bool bRet = UpdateLayeredWindow(hWnd, hDCScreen, ref ptDest, ref sizeBitmap, hDCMem, ref ptSrc, ColorTranslator.ToWin32(System.Drawing.Color.White), ref bf, ULW_ALPHA);

            Marshal.FreeHGlobal(pptSrc);
            Marshal.FreeHGlobal(pptDest);
            Marshal.FreeHGlobal(psizeBitmap);

            SelectObject(hDCMem, hBitmapOld);
            DeleteDC(hDCMem);
            ReleaseDC(IntPtr.Zero, hDCScreen);
        }

        public void CenterToScreen(IntPtr hWnd)
        {
            RECT rcWorkArea = new RECT();
            SystemParametersInfo(SPI_GETWORKAREA, 0, ref rcWorkArea, 0);
            RECT rc;
            GetWindowRect(hWnd, out rc);
            int nX = System.Convert.ToInt32((rcWorkArea.left + rcWorkArea.right) / (double)2 - (rc.right - rc.left) / (double)2);
            int nY = System.Convert.ToInt32((rcWorkArea.top + rcWorkArea.bottom) / (double)2 - (rc.bottom - rc.top) / (double)2);
            SetWindowPos(hWnd, IntPtr.Zero, nX, nY, -1, -1, SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED | SWP_NOACTIVATE);
        }

        private int Win32WndProc(IntPtr hwnd, uint msg, UIntPtr wParam, IntPtr lParam)
        {
            //int wmId, wmEvent;
            switch (msg)
            { 
                default:
                    break;
            }
            return DefWindowProc(hwnd, msg, wParam, lParam);
        }
    }
}
