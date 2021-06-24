using GameOverlay.Drawing;
using GameOverlay.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOSAuto.Overlay
{
    public class GraphicsFactory
    {
        bool init = false;
        private readonly Dictionary<string, SolidBrush> _brushes = new Dictionary<string, SolidBrush>();
        private readonly Dictionary<string, Font> _fonts = new Dictionary<string, Font>();
        private readonly Dictionary<string, Image> _images = new Dictionary<string, Image>();
        public void Init(Graphics gfx)
        {
   
            if (init == false)
            {
                _brushes["black"] = gfx.CreateSolidBrush(0, 0, 0);
                _brushes["white"] = gfx.CreateSolidBrush(255, 255, 255);
                _brushes["red"] = gfx.CreateSolidBrush(255, 0, 0);
                _brushes["green"] = gfx.CreateSolidBrush(0, 255, 0);
                _brushes["blue"] = gfx.CreateSolidBrush(0, 0, 255);
                _brushes["background"] = gfx.CreateSolidBrush(0x33, 0x36, 0x3F);
                _brushes["grid"] = gfx.CreateSolidBrush(255, 255, 255, 0.2f);
                _brushes["random"] = gfx.CreateSolidBrush(0, 0, 0);
                _brushes["black 50%"] = gfx.CreateSolidBrush(0, 0, 0, 0.5f);

                _fonts["arial"] = gfx.CreateFont("Arial", 12);
                _fonts["arial_big"] = gfx.CreateFont("Arial", 15);
                _fonts["consolas"] = gfx.CreateFont("Consolas", 13);
                _fonts["consolas-mid"] = gfx.CreateFont("Consolas", 11);
            }
            init = true;
        }

        public Font GetFont(string value) {return _fonts[value]; }
        public SolidBrush GetBrush(string value) { return _brushes[value]; } 

    }
}
