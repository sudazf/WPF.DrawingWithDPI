using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF.Drawing.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void CreateBitMap()
        {
            //96 dpi
            var bitmap1 = new WriteableBitmap(800, 800, 96, 96, PixelFormats.Pbgra32, null);

            var bitmap1 = new WriteableBitmap(800, 800, 96, 96, PixelFormats.Pbgra32, null);

        }
    }
}
