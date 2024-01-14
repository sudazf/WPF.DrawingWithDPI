using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF.Drawing.Demo.SystemInfos;
using WPF.Drawing.Demo.Utility;
using Color = System.Windows.Media.Color;
using Point = System.Drawing.Point;

namespace WPF.Drawing.Demo.Drawers
{
    internal class BaseDrawer : IDraw
    {
        protected readonly CoordinateInfo CoordinateInfo;

        protected WriteableBitmap RenderBitmap;

        public event EventHandler<BitmapSource> OnSourceChanged;
        public BaseDrawer(CoordinateInfo coordinateInfo)
        {
            CoordinateInfo = coordinateInfo;
        }

        public void UpdateSource(int width, int height)
        {
            OnUpdateSource(width, height);
        }
        public void Draw(params object[] paras)
        {
            OnDraw(paras);
        }
        public void Clear()
        {
            OnClear();
        }
        public void ZoomIn(Point startPoint, Point endPoint)
        {
            OnZoomIn(startPoint, endPoint);
        }
        public void ZoomOut()
        {
            OnZoomOut();
        }

        protected virtual void OnUpdateSource(int width, int height)
        {
            RenderBitmap = new WriteableBitmap(width,
                height,
                SystemInfo.Dpi.X,
                SystemInfo.Dpi.Y,
                PixelFormats.Pbgra32,
                null);

            OnSourceChanged?.Invoke(this, RenderBitmap);
        }
        protected virtual void OnDraw(params object[] paras)
        {
        }
        protected virtual void OnClear()
        {
            RenderBitmap.Clear();
        }
        protected virtual void OnZoomIn(Point startPoint, Point endPoint)
        {
            
        }
        protected virtual void OnZoomOut()
        {

        }

        public void DrawString(int x, int y, string text, Color color, StringAlignment stringAlignment = StringAlignment.Near, float fontSize = 13)
        {
            var w = RenderBitmap.PixelWidth;
            var h = RenderBitmap.PixelHeight;
            var stride = RenderBitmap.BackBufferStride;
            var pixelPtr = RenderBitmap.BackBuffer;

            var bitmap2 = new Bitmap(w, h, stride, System.Drawing.Imaging.PixelFormat.Format32bppRgb, pixelPtr);

            RenderBitmap.Lock();

            using (var g = Graphics.FromImage(bitmap2))
            {
                var font = new Font("宋体", fontSize, GraphicsUnit.Pixel);
                var actualX = x;
                var actualY = y;
                if (stringAlignment == StringAlignment.Center)
                {
                    var sizeF = g.MeasureString(text, font);
                    actualX = x - (int)sizeF.Width / 2;
                    actualY = y - (int)sizeF.Height / 2;
                }
                g.DrawString(text, font, new SolidBrush(System.Drawing.Color.FromArgb(color.R, color.G, color.B)), actualX, actualY);
            }

            RenderBitmap.Unlock();
        }
        public void DrawRectangle(int x1, int y1, int width, int height, Color color)
        {
            var w = RenderBitmap.PixelWidth;
            var h = RenderBitmap.PixelHeight;
            var stride = RenderBitmap.BackBufferStride;
            var pixelPtr = RenderBitmap.BackBuffer;

            var bitmap2 = new Bitmap(w, h, stride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, pixelPtr);

            RenderBitmap.Lock();

            using (var g = Graphics.FromImage(bitmap2))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.FromArgb(color.R, color.G, color.B), 1), x1, y1, width, height);
            }

            RenderBitmap.AddDirtyRect(new Int32Rect(0, 0, RenderBitmap.PixelWidth, RenderBitmap.PixelHeight));
            RenderBitmap.Unlock();
        }
        public void FillRectangle(int x1, int y1, int width, int height, Color color, Thickness thickness)
        {
            var w = RenderBitmap.PixelWidth;
            var h = RenderBitmap.PixelHeight;
            var stride = RenderBitmap.BackBufferStride;
            var pixelPtr = RenderBitmap.BackBuffer;

            var bitmap2 = new Bitmap(w, h, stride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, pixelPtr);

            RenderBitmap.Lock();

            using (var g = Graphics.FromImage(bitmap2))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B)),
                    x1 + (float)thickness.Left, y1 + (float)thickness.Top, width, height);
            }

            RenderBitmap.AddDirtyRect(new Int32Rect(0, 0, RenderBitmap.PixelWidth, RenderBitmap.PixelHeight));
            RenderBitmap.Unlock();
        }
        public void DrawLine(int x1, int y1, int x2, int y2, System.Windows.Media.Color color)
        {
            RenderBitmap.DrawLine(x1, y1, x2, y2, color);
        }
        public void DrawLineDotted(int x1, int y1, int x2, int y2, int dotSpace, int dotLength, System.Windows.Media.Color color)
        {
            RenderBitmap.DrawLineDotted(x1, y1, x2, y2, dotSpace, dotLength, color);
        }
    }
}
