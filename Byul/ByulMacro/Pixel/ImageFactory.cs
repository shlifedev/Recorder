using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Range = OpenCvSharp.Range;

namespace Pixel
{

    public class CreateImage
    {
        public enum Type { File, Cropped }

        /// <summary>
        /// 구분태그
        /// </summary>
        public string tag = "";

        /// <summary>
        /// 경로
        /// </summary>
        public string filename = "";

        private Mat mat;
        private Bitmap bitmap;
        private ImageSource imageSource;

 
        public Mat Mat
        {
            get
            {
                if(mat == null)
                {
                    mat = BitmapConverter.ToMat(bitmap);
                }
                return mat;
            }
        }
        public Bitmap Bitmap { get => bitmap; set => bitmap = value; }
        public ImageSource ImageSource
        {
            get { 
                if(imageSource == null)
                {
                    imageSource = Utility.BitMapToImageSource(Bitmap);
                }
                return imageSource;
            }
        }


 
  

    }
    public static class ImageFactory
    {


        public static CreateImage GetCIWithTag(string tag) {
            return null;
        }
        public static CreateImage CreateScreenCropImage(OpenCvSharp.Point start, OpenCvSharp.Point end, string tag)
        {
            CreateImage ci = new CreateImage();
            var screen = Utility.CaptureScreenToCvMat();
            var sliceMat = screen.SubMat(new Rect(start.X, start.Y, end.X, end.Y));
            ci.Bitmap = sliceMat.ToBitmap();
            ci.tag = tag;
            ci.filename = null;
            return ci;
        }

        public static CreateImage CreateFromFile(string filename, string tag)
        {
            CreateImage ci = new CreateImage();
            ci.Bitmap = (Bitmap)Bitmap.FromFile(filename);
            ci.tag = tag;
            if (tag == null)
            {
                ci.tag = filename;
            }
            ci.filename = filename;
            return ci;
        }
    }
}
