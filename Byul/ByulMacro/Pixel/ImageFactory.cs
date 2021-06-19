﻿using OpenCvSharp;
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
        public static Dictionary<string, CreateImage> imageContainer = new Dictionary<string, CreateImage>();


        public static CreateImage GetImageWithTag(string tag) {
            var result = imageContainer.TryGetValue(tag, out CreateImage value);
            if (result == false)
                return null;
            else
                return value; 
        }   
        private static void RegImage(CreateImage img, string tag)
        {
            if (tag == null) return;

            if(imageContainer.ContainsKey(tag) == false) 
                imageContainer.Add(tag, img); 
        }
        public static CreateImage CreateScreenCropImage(OpenCvSharp.Point start, OpenCvSharp.Point end, string tag)
        {
            CreateImage ci = new CreateImage(CreateImage.Type.Cropped);
            var screen = Utility.CaptureScreenToCvMat();
            var sliceMat = screen.SubMat(new Rect(start.X, start.Y, end.X, end.Y));
            ci.Bitmap = sliceMat.ToBitmap();
            ci.tag = tag;
            if (tag == null)
            {
                System.Random r = new Random();
                string rsTemp = null;
                for(int i = 0; i < 8; i++)
                {
                    var val =  r.Next(int.MinValue, int.MaxValue);
                    rsTemp += val.ToString();
                }
                ci.tag = rsTemp;
            }

            ci.filename = null;
            RegImage(ci, tag);
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
            RegImage(ci, tag);
            return ci;
        }
    }
}
