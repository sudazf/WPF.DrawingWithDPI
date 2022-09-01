using System.Windows;
using System.Windows.Media;

namespace WPF.Drawing.Demo.SystemInfos
{
    public class SystemInfo
    {
        public static Dpi Dpi;

        /// <summary>
        /// 获取当前 DPI
        /// </summary>
        /// <param name="visual"></param>
        public static void GetDpiFromVisual(Visual visual)
        {
            var source = PresentationSource.FromVisual(visual);

            var dpiX = 96.0;
            var dpiY = 96.0;

            if (source?.CompositionTarget != null)
            {
                dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
            }

            Dpi = new Dpi(dpiX, dpiY);
        }
    }
}
