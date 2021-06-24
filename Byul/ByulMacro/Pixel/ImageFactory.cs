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
        /// <summary>
        /// 이미지의 구분태그를 기준으로 컨테이너에 생성한다.
        /// </summary>
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

        public static bool IsExistTag(string tag)
        {
            if (imageContainer.ContainsKey(tag)) 
                return true; 
            else
                return false;
        }
        public static CreateImage CreateScreenCropImage(OpenCvSharp.Point start, OpenCvSharp.Point end, string tag)
        {
            if (IsExistTag(tag)) 
                throw new Exception($"tag {tag} already exist. cannot create image"); 
            
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
                ci.IsTemporaryImage = true;
                ci.tag = rsTemp;
                Console.WriteLine("create image temporary " + rsTemp);
            }
            

            ci.filename = null;
            RegImage(ci, tag);
            return ci;
        }

        public static CreateImage CreateFromFile(string filename, string tag)
        {
            if (IsExistTag(tag))
                throw new Exception($"tag {tag} already exist. cannot create image");

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
