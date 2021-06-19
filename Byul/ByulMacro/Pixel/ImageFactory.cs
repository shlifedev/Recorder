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

  
    public static class ImageFactory
    {


        public static CreateImage GetCIWithTag(string tag) {
            return null;
        }
        public static CreateImage CreateScreenCropImage(OpenCvSharp.Point start, OpenCvSharp.Point end, string tag)
        {
            CreateImage ci = new CreateImage(CreateImage.Type.Cropped);
            var screen = Utility.CaptureScreenToCvMat();
            var sliceMat = screen.SubMat(new Rect(start.X, start.Y, end.X, end.Y));
            ci.Bitmap = sliceMat.ToBitmap();
            ci.tag = tag;
            ci.filename = null;
            return ci;
        }

        public static CreateImage CreateFromFile(string filename, string tag)
        {
            CreateImage ci = new CreateImage(CreateImage.Type.File);
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
