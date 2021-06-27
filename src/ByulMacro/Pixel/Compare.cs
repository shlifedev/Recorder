using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.XImgProc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = OpenCvSharp.Point;

namespace Pixel
{ 
    /// <summary>
    /// 이미지 비교 클래스
    /// </summary>
    public static class Compare
    {
        public class Result : IDisposable
        {
            public Bitmap src;
            public Bitmap template; 
            public Mat resultMat;
            /// <summary>
            /// 서칭된 포인트의 중앙값
            /// </summary>
            public Point center;
            /// <summary>
            /// Loc
            /// </summary>
            public Point maxLoc;
            /// <summary>
            /// 이미지 정확도
            /// </summary>
            public double maxVal;
            /// <summary>
            /// 회색 음영으로 테스트 했는지에 대한 여부
            /// </summary>
            public bool isGrayScale;
            public double accurity;

            public void Dispose()
            {
                resultMat.Dispose();
            }

            public bool IsMatch() { return maxVal >= accurity; }

        }
        /// <summary>
        /// 이미지 정확도에 따라 true false 반환
        /// </summary> 
        public static bool isMatchImg(Bitmap src, Bitmap cur, double accurity)
        {
            bool isMatch = false;
            using (Mat mat1 = src.ToMat())
            {
                using (Mat mat2 = cur.ToMat())
                {
                    using (Mat mat3 = new Mat())
                    {
                        Cv2.MatchTemplate((InputArray)mat1, (InputArray)mat2, (OutputArray)mat3, TemplateMatchModes.CCoeffNormed);
                        double n = accurity * 0.01;
                        double oVal;
                        Cv2.MinMaxLoc((InputArray)mat3, out double _, out oVal, out OpenCvSharp.Point _, out OpenCvSharp.Point _);
                        if (oVal >= n)
                            isMatch = true;
                    }
                }
            }
            return isMatch;
        }

        public static Result Match(this Bitmap src, Bitmap template, double accurity = 0.75f, bool grayscaleCheck = true)
        {
            Result result = new Result();
            Match(src, out result.resultMat, out result.center, out result.maxLoc, template, accurity, grayscaleCheck);
            return result;
        }
        public static void Match(this Bitmap src, out Mat oResult, out Point oCenter, out Point oMaxLoc, Bitmap template, double accurity = 0.75f, bool grayscaleCheck = true)
        { 
            double threshhold = accurity;

            using Mat _src = BitmapConverter.ToMat(src);
            using Mat _template = BitmapConverter.ToMat(template);
            using Mat _matchResult = new Mat();

            if (grayscaleCheck)
            {
                Cv2.CvtColor(_src, _src, ColorConversionCodes.BGR2GRAY);
                Cv2.CvtColor(_template, _template, ColorConversionCodes.BGR2GRAY);
            }
            //탬플릿 매칭 실행
            Cv2.MatchTemplate(_src, _template, _matchResult, TemplateMatchModes.CCoeffNormed);
            //매칭이 된 결과물 출력
            _matchResult.MinMaxLoc(out var minVal, out var maxVal, out var minLoc, out var maxLoc);
            //결과 출력을 위해 복제 및 그리기
            Mat result = _src.Clone();
            result.Rectangle(maxLoc, new Point(maxLoc.X + _template.Width, maxLoc.Y + _template.Height), Scalar.Red, 3);

            //중앙 포지션 서치 및 그리기
            float centerX = maxLoc.X + (_template.Width / 2);
            float centerY = maxLoc.Y + (_template.Height / 2);
            var boxSize = 20;
            result.Rectangle(new Point(centerX - boxSize, centerY + -boxSize), new Point(centerX + boxSize, centerY + boxSize), Scalar.LightPink, 2);
            if (maxVal >= threshhold)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("(matching) maxval:" + maxVal);
                Console.ForegroundColor = ConsoleColor.White;
                oMaxLoc = maxLoc;
                oCenter = new Point(centerX, centerY);
                oResult = result;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("(not matching) maxval:" + maxVal);
                Console.ForegroundColor = ConsoleColor.White;
                //_src.SaveImage("debug/err_src.png");
                //_template.SaveImage("debug/err_template.png");
                //_matchResult.SaveImage("debug/err_matchResult.png");
                //result.SaveImage("debug/err_result.png"); 
                oMaxLoc = new Point(0, 0);
                oCenter = new Point(0, 0);
                oResult = null;
            }
        }
    }
}
