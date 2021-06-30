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
        public string Tag = "";
        /// <summary>
        /// 경로
        /// </summary>
        public string Filename = "";
        private Mat _mat;
        private Bitmap _bitmap;
        private ImageSource _imageSource;
        public bool IsTemporaryImage = false;
        public Type CreatedType;
        public CreateImage(Type createdType)
        {
            CreatedType = createdType;
        }
        public Mat Mat
        {
            get
            {
                if (_mat == null)
                {
                    _mat = BitmapConverter.ToMat(_bitmap);
                }
                return _mat;
            }
        }
        public Bitmap Bitmap { get => _bitmap; set => _bitmap = value; }
        public ImageSource ImageSource
        {
            get
            {
                if (_imageSource == null)
                {
                    _imageSource = Utility.BitMapToImageSource(Bitmap);
                }
                return _imageSource;
            }
        }
    }
}