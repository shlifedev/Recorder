using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Pixel.Utility.User32;

namespace Pixel
{


    public static class Utility
    {
        public class AppCaptrue
        {
            [DllImport("user32.dll")]
            public static extern bool GetWindowRect(IntPtr hWnd, out AppCaptureRect lpRect);
            [DllImport("user32.dll")]
            public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

            public static Bitmap CaptrueHwnd(IntPtr hWnd)
            {
                AppCaptureRect rc;
                GetWindowRect(hWnd, out rc);

                Bitmap bmp = new Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics gfxBmp = Graphics.FromImage(bmp);
                IntPtr hdcBitmap = gfxBmp.GetHdc();

                PrintWindow(hWnd, hdcBitmap, 0);

                gfxBmp.ReleaseHdc(hdcBitmap);
                gfxBmp.Dispose();

                return bmp;
            }

            public static Bitmap Captrue(string procName)
            {
                var proc = Process.GetProcessesByName(procName)[0];
                //Console.WriteLine(proc.HandleCount);
                return CaptrueHwnd(proc.MainWindowHandle);
            }
            [StructLayout(LayoutKind.Sequential)]
            public struct AppCaptureRect
            {
                private int _Left;
                private int _Top;
                private int _Right;
                private int _Bottom;

                public AppCaptureRect(AppCaptureRect Rectangle) : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
                {
                }
                public AppCaptureRect(int Left, int Top, int Right, int Bottom)
                {
                    _Left = Left;
                    _Top = Top;
                    _Right = Right;
                    _Bottom = Bottom;
                }

                public int X
                {
                    get { return _Left; }
                    set { _Left = value; }
                }
                public int Y
                {
                    get { return _Top; }
                    set { _Top = value; }
                }
                public int Left
                {
                    get { return _Left; }
                    set { _Left = value; }
                }
                public int Top
                {
                    get { return _Top; }
                    set { _Top = value; }
                }
                public int Right
                {
                    get { return _Right; }
                    set { _Right = value; }
                }
                public int Bottom
                {
                    get { return _Bottom; }
                    set { _Bottom = value; }
                }
                public int Height
                {
                    get { return _Bottom - _Top; }
                    set { _Bottom = value + _Top; }
                }
                public int Width
                {
                    get { return _Right - _Left; }
                    set { _Right = value + _Left; }
                }
                public System.Drawing.Point Location
                {
                    get { return new System.Drawing.Point(Left, Top); }
                    set
                    {
                        _Left = value.X;
                        _Top = value.Y;
                    }
                }
                public System.Drawing.Size Size
                {
                    get { return new System.Drawing.Size(Width, Height); }
                    set
                    {
                        _Right = value.Width + _Left;
                        _Bottom = value.Height + _Top;
                    }
                }

                public static implicit operator Rectangle(AppCaptureRect Rectangle)
                {
                    return new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
                }
                public static implicit operator AppCaptureRect(Rectangle Rectangle)
                {
                    return new AppCaptureRect(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
                }
                public static bool operator ==(AppCaptureRect Rectangle1, AppCaptureRect Rectangle2)
                {
                    return Rectangle1.Equals(Rectangle2);
                }
                public static bool operator !=(AppCaptureRect Rectangle1, AppCaptureRect Rectangle2)
                {
                    return !Rectangle1.Equals(Rectangle2);
                }

                public override string ToString()
                {
                    return "{Left: " + _Left + "; " + "Top: " + _Top + "; Right: " + _Right + "; Bottom: " + _Bottom + "}";
                }

                public override int GetHashCode()
                {
                    return ToString().GetHashCode();
                }

                public bool Equals(AppCaptureRect Rectangle)
                {
                    return Rectangle.Left == _Left && Rectangle.Top == _Top && Rectangle.Right == _Right && Rectangle.Bottom == _Bottom;
                }

                public override bool Equals(object Object)
                {
                    if (Object is AppCaptureRect)
                    {
                        return Equals((AppCaptureRect)Object);
                    }
                    else if (Object is Rectangle)
                    {
                        return Equals(new AppCaptureRect((Rectangle)Object));
                    }

                    return false;
                }
            }
        }
        public static ImageSource BitMapToImageSource(this Bitmap bitmap)
        {
            var h = bitmap.GetHbitmap();
            var bs = Imaging.CreateBitmapSourceFromHBitmap(h, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return bs;
        }

        public static ImageSource MatToImageSource(this Mat mat)
        {
            var b = mat.ToBitmap();
            var h = b.GetHbitmap();
            var bs = Imaging.CreateBitmapSourceFromHBitmap(h, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return bs;
        }

        public static InputArray CreateInputArray(Bitmap bitmap)
        {
            var mat = BitmapConverter.ToMat(bitmap);
            var iarray = OpenCvSharp.InputArray.Create(mat);
            return iarray;
        }
        public static Bitmap LoadFileToBitmap(string path)
        {
            var bitmap = System.Drawing.Bitmap.FromFile(path);
            return (Bitmap)bitmap;
        }


        public static InputArray LoadFileToInputArray(string path)
        {
            var bitmap = (Bitmap)System.Drawing.Bitmap.FromFile(path);
            return CreateInputArray(bitmap);
        }
        public static void SaveMat(this Mat m, string filename)
        {
            BitmapConverter.ToBitmap(m).Save(filename);
        }

        /// <summary>
        /// 스크린 캡쳐후 비트맵 파싱
        /// </summary>
        /// <returns></returns>
        public static Bitmap CaptureScreenToBitmap()
        {
            return (Bitmap)CaptureWindow(User32.GetDesktopWindow());
        }
        /// <summary>
        /// 스크린 캡쳐후 데이터배열로 파싱
        /// </summary>
        /// <returns></returns>
        public static byte[] CaptureScreenToBytes()
        {
            MemoryStream ms = new MemoryStream();
            CaptureScreen().Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();

        }



        public static Image CaptureWindow(IntPtr handle)
        {
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            User32.RECT windowRect = new();
            _ = User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            _ = GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            _ = GDI32.SelectObject(hdcDest, hOld);
            _ = GDI32.DeleteDC(hdcDest);
            _ = User32.ReleaseDC(handle, hdcSrc);
            Image img = Image.FromHbitmap(hBitmap);
            _ = GDI32.DeleteObject(hBitmap);
            return img;
        }
        public static void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }
        public static void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename, format);
        }

        /// <summary>
        /// 데스크톱 윈도우의 핸들을 캡쳐함
        /// </summary>
        /// <returns></returns>
        public static Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        /// <summary>
        /// OpenCv 의 인풋 어레이로 변환
        /// </summary>
        /// <returns></returns>
        public static InputArray CaptureScreenToCvInputArray()
        {
            var mat = BitmapConverter.ToMat(CaptureScreenToBitmap());
            var iarray = OpenCvSharp.InputArray.Create(mat);
            return iarray;
        }
        /// <summary>
        /// OpenCv 의 Mat으로 변환
        /// </summary>
        /// <returns></returns>
        public static Mat CaptureScreenToCvMat()
        {
            var mat = BitmapConverter.ToMat(CaptureScreenToBitmap());
            return mat;
        }
        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }
        public class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        }
    }
}

