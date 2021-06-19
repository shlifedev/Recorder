using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.Windows.Media;

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
         
        public Type CreatedType;

        public CreateImage(Type createdType)
        {
            CreatedType = createdType;
        } 
        public Mat Mat
        {
            get
            {
                if (mat == null)
                {
                    mat = BitmapConverter.ToMat(bitmap);
                }
                return mat;
            }
        }
        public Bitmap Bitmap { get => bitmap; set => bitmap = value; }
        public ImageSource ImageSource
        {
            get
            {
                if (imageSource == null)
                {
                    imageSource = Utility.BitMapToImageSource(Bitmap);
                }
                return imageSource;
            }
        }
    }
}