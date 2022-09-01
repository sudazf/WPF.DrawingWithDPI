using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF.Drawing.Demo.SystemInfos;
using WPF.Drawing.Demo.Utility;
using Point = System.Drawing.Point;

namespace WPF.Drawing.Demo.Drawers
{
    internal class RangeDrawer : BaseDrawer
    {
        private WriteableBitmap _cutBitmap;

        public RangeDrawer(CoordinateInfo coordinateInfo) : base(coordinateInfo)
        {
        }

        protected override void OnUpdateSource(int width, int height)
        {
            _cutBitmap = new WriteableBitmap(width,
                height,
                SystemInfo.Dpi.X,
                SystemInfo.Dpi.Y,
                PixelFormats.Pbgra32,
                null);

            base.OnUpdateSource(width, height);
        }

        protected override void OnDraw(params object[] paras)
        {
            //draw a demo rectangle.
            //left-top is (0.3, 0.7), width is 0.4, height is 0.3

            var phyX = CoordinateInfo.GetDrawerX(0.3);
            var phyY = CoordinateInfo.GetDrawerY(0.7);

            var width = 0.4 / CoordinateInfo.LogicScaleX;
            var height = 0.3 / CoordinateInfo.LogicScaleY;

            DrawRectangle(phyX, phyY, (int)width, (int)height, Colors.Black);
            FillRectangle(phyX, phyY, (int)width, (int)height, Colors.DeepPink, new Thickness(0.5,0.5,0,0));

            var calcFontSize = 20 / (CoordinateInfo.XLogicMax - CoordinateInfo.XLogicMin);
            var maxFontSize = calcFontSize > 50 ? 50 : calcFontSize;

            //if draw string like this，you may not see the string sometimes on zoom in,
            //because of maxFontSize.
            DrawString(phyX + (int)width / 2, phyY + (int)height / 2, "This is a demo Rectangle.", Colors.Black, StringAlignment.Center, (float)maxFontSize);
        }

        protected override void OnZoomIn(Point startPoint, Point endPoint)
        {
            Clear();
            Draw();
            Fit();
        }

        protected override void OnZoomOut()
        {
            Clear();
            Draw();
        }

        /// <summary>
        /// Prevent drawing out of x/y axes. (防止超出 x/y 轴边界)
        /// </summary>
        protected void Fit()
        {
            var minX = CoordinateInfo.GetDrawerX(CoordinateInfo.XLogicMin);
            var minY = CoordinateInfo.GetDrawerY(CoordinateInfo.YLogicMax);
            var maxX = CoordinateInfo.GetDrawerX(CoordinateInfo.XLogicMax);
            var maxY = CoordinateInfo.GetDrawerY(CoordinateInfo.YLogicMin);

            var width = maxX - minX;
            var height = maxY - minY;

            var cutSourceRect = new Rect(minX, minY, width, height);
            var cutDesRect = new Rect(0, 0, _cutBitmap.PixelWidth, _cutBitmap.PixelHeight);
            BitmapUtility.Cut(RenderBitmap, cutSourceRect, _cutBitmap, cutDesRect);

            Clear();

            var renderTargetRect = new Rect(minX + 1, minY, width, height); //留 1 个像素显示 Y 轴
            var renderSourceRect = new Rect(0, 0, _cutBitmap.PixelWidth, _cutBitmap.PixelHeight);
            BitmapUtility.Cut(_cutBitmap, renderSourceRect, RenderBitmap, renderTargetRect);
        }
    }
}
