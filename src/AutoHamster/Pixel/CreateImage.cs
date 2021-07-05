using Newtonsoft.Json;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        [JsonIgnore]
        private Mat _mat;
        [JsonIgnore]
        private Bitmap _bitmap;
        [JsonIgnore]
        private ImageSource _imageSource;
        public bool IsTemporaryImage = false;
        public Type CreatedType;

        public byte[] BitmapSerialized
        {
            get
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    _bitmap.Save(ms, ImageFormat.Bmp);
                    return ms.ToArray();
                } 
            }
            set
            {
                using (MemoryStream ms = new MemoryStream(value))
                {
                    Bitmap = new Bitmap(ms);
                }
            }
        }
        public CreateImage(Type createdType)
        {
            CreatedType = createdType;
        }
        [JsonIgnore]
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
        [JsonIgnore]
        public Bitmap Bitmap { get {
                if(_bitmap == null)
                {
                    if(BitmapSerialized != null)
                    {
                        using MemoryStream ms = new MemoryStream(BitmapSerialized);
                        _bitmap = new Bitmap(ms);
                    }
                }
                return _bitmap;
            } set { _bitmap = value; } }
        [JsonIgnore]
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


 
        public static void SaveToFile(CreateImage obj, string saveDirectory = "images", string saveFileName = "default")
        {
            var di = new System.IO.DirectoryInfo(saveDirectory);
            if(di.Exists == false) 
                System.IO.Directory.CreateDirectory(saveDirectory); 
            string filename = (saveFileName == "default") ? obj.Tag + ".bytes" : saveFileName;
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            byte[] compData = Ionic.Zlib.GZipStream.CompressString(data);
            System.IO.File.WriteAllBytes(filename, compData);
     
     
        }

        public static CreateImage LoadFromFile(string path)
        {
            byte[] datas = System.IO.File.ReadAllBytes(path); 
            string uncompData = Ionic.Zlib.GZipStream.UncompressString(datas);
            CreateImage uncompImage = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateImage>(uncompData);
            return uncompImage;
        }
    }
}