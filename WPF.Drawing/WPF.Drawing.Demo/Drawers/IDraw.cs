using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WPF.Drawing.Demo.Drawers
{
    internal interface IDraw
    {
        event EventHandler<BitmapSource> OnSourceChanged;

        void UpdateSource(int width, int height);
        void Draw(params object[] paras);
        void Clear();
        void ZoomIn(System.Drawing.Point startPoint, System.Drawing.Point endPoint);
        void ZoomOut();
    }

    internal enum DrawType
    {
        Axes,
        Range,
        Selector
    }
}
