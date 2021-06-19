using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GUI.Overlay overlay = new GUI.Overlay();
        public MainWindow()
        {
            InitializeComponent(); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pixel.Utility.CaptureScreenToFile("test.png", ImageFormat.Png);
        }
    }
}
