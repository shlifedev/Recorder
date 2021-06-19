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
    public static class Compare
    {
        static Stopwatch sw = new Stopwatch();
        public static void TestCompare(Bitmap src, Bitmap template)
        {
            //스톱워치 시작
            sw.Start();
            using Mat _src = BitmapConverter.ToMat(src); 
            using Mat _template = BitmapConverter.ToMat(template);
            using Mat _matchResult = new Mat();
 
            //탬플릿 매칭 실행
            Cv2.MatchTemplate(_src, _template, _matchResult, TemplateMatchModes.CCoeffNormed);
          
            //매칭이 된 결과물 출력
            _matchResult.MinMaxLoc(out var minVal, out var maxVal, out var minLoc, out var maxLoc); 


            //결과 출력을 위해 복제
            using var result = _src.Clone();
            result.Rectangle(maxLoc, new Point(maxLoc.X + _template.Width, maxLoc.Y + _template.Height), Scalar.Red, 3);

           
            //중앙 포지션 서치
            var centerX = maxLoc.X + (_template.Width / 2);
            var centerY = maxLoc.Y + (_template.Height / 2);
            var boxSize = 20;
            result.Rectangle(new Point(centerX - boxSize, centerY + -boxSize), new Point(centerX + boxSize, centerY + boxSize), Scalar.GreenYellow, 2);
           
            sw.Stop(); 
            Console.Write(sw.ElapsedMilliseconds);
            Cv2.ImShow("result", result);
        }
    }
}
