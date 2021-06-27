using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameOverlay.Drawing;
using GameOverlay.Windows; 
namespace AutoHamster.OverlayGUI
{
    public class Renderer : IDisposable
    {
        private GraphicsWindow _window;
        private GameOverlay.Drawing.Graphics gfx; 
        public System.Action<GraphicsFactory, GameOverlay.Drawing.Graphics> onDraw;
        public GraphicsFactory GF;
        public Renderer(int sizeX, int sizeY, System.Action<GraphicsFactory, GameOverlay.Drawing.Graphics> onDraw)
        {
            this.onDraw = onDraw;
            GF = new GraphicsFactory();
            gfx = new GameOverlay.Drawing.Graphics()
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true
            };
            
            _window = new GraphicsWindow(0, 0, (int)sizeX, (int)sizeY, gfx)
            {
                FPS = 240,
                IsTopmost = true,
                IsVisible = true
            };
            _window.DestroyGraphics += OnDestroy;
            _window.DrawGraphics += OnDraw;
            _window.SetupGraphics += OnSetup;

         
        }

        public void Run()
        {
            _window.Create(); 
        }
        public void SetWindowSize(int x, int y)
        {
            _window.Resize(x, y);
        }
        public void SetWindowPos(int x, int y)
        {
            _window.Move(x, y);
        }
        public virtual void OnSetup(object sender, SetupGraphicsEventArgs e)
        {
            var gfx = e.Graphics;
            //Console.WriteLine("Overlay - OnSetup");
            GF.Init(gfx);  
        }

        public virtual void OnDestroy(object sender, DestroyGraphicsEventArgs e)
        {
           // Console.WriteLine("Overlay - OnDestroy");
        }
        public virtual void OnDraw(object sender, DrawGraphicsEventArgs e)
        {
            var gfx = e.Graphics;
     
            gfx.ClearScene();
            onDraw?.Invoke(GF, gfx); 
        }

        public void Dispose()
        {
            _window.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
