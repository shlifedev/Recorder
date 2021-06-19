using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace Pixel
{
    public static class Utility
    {
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


        public static InputArray CreateInputArray(Bitmap bitmap) {
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
        public static void SaveMat(Mat m, string filename)
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

        private class User32
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

