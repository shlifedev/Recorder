using Coroutine;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ByulMacro
{ 
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 콘솔 할당용
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll",EntryPoint = "AllocConsole",SetLastError = true, CharSet = CharSet.Auto,CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        private GUI.Overlay overlay = new GUI.Overlay();
        private DateTime lastTime;


        /// <summary>
        /// 새로운 쓰래드에 코루틴용 틱 계산기 할당 
        /// </summary>
        public void InitCoroutine()
        {
            Task.Run(() => {
                while (true)
                {
                    var currTime = DateTime.Now;
                    CoroutineHandler.Tick(currTime - lastTime);
                    lastTime = currTime;
                    Thread.Sleep(1);
                }
            });

            CoroutineHandler.Start(WaitSeconds()); 
        }
        public MainWindow()
        {
            InitializeComponent(); 
            AllocConsole();  
            InitCoroutine();


        }
        private static IEnumerator<Wait> WaitSeconds()
        {
            yield return new Wait(1);
            Console.WriteLine("First thing " + DateTime.Now);
            yield return new Wait(1);
            Console.WriteLine("After 1 second " + DateTime.Now);

            yield return new Wait(1);
            Console.WriteLine("After 2 second " + DateTime.Now);

            yield return new Wait(1);
            Console.WriteLine("After 3 second " + DateTime.Now);
            yield return new Wait(5);
            Console.WriteLine("After 5 seconds " + DateTime.Now);
            yield return new Wait(10);
            Console.WriteLine("After 10 seconds " + DateTime.Now);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pixel.Utility.CaptureScreenToFile("test.png", ImageFormat.Png);
        }
    }
}
