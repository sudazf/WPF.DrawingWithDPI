using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using WPF.Drawing.Demo.Drawers;
using WPF.Drawing.Demo.SystemInfos;

namespace WPF.Drawing.Demo
{
    internal class DrawHelper
    {
        private readonly Dictionary<DrawType, IDraw> _drawers;
        private readonly CoordinateInfo _coordinateInfo;

        public Dictionary<DrawType, IDraw> Drawers => _drawers;

        public DrawHelper()
        {
            _drawers = new Dictionary<DrawType, IDraw>();
            _coordinateInfo = new CoordinateInfo();

            _drawers[DrawType.Axes] = new AxesDrawer(_coordinateInfo);
            _drawers[DrawType.Range] = new RangeDrawer(_coordinateInfo);
            _drawers[DrawType.Selector] = new SelectorDrawer(_coordinateInfo);
        }

        public void UpdateDrawers(double renderWidth, double renderHeight)
        {
            _coordinateInfo.UpdateAxesRange(renderWidth, renderHeight);

            foreach (var drawer in _drawers.Values)
            {
                drawer.UpdateSource((int)renderWidth, (int)renderHeight);
            }
        }

        public void DrawAxes()
        {
            _drawers[DrawType.Axes].Draw();
        }
        public void DrawToolTip(Point mousePoint)
        {
            _drawers[DrawType.Selector].Clear();

            if (CheckAvailableArea(mousePoint))
            {
                var logicX = _coordinateInfo.GetLogicX(mousePoint.X);
                var logicY = _coordinateInfo.GetLogicY(mousePoint.Y);
                var tip = $"({logicX:F4}, {logicY:F4})";

                var baseDrawer = (BaseDrawer)_drawers[DrawType.Selector];
                baseDrawer.DrawString(mousePoint.X - 20, mousePoint.Y - 15, tip, Colors.Red);
            }
        }

        public bool CheckAvailableArea(Point mousePoint)
        {
            var logicX = _coordinateInfo.GetLogicX(mousePoint.X);
            var logicY = _coordinateInfo.GetLogicY(mousePoint.Y);

            var minX = Math.Round(_coordinateInfo.XLogicMin, 4);
            var maxX = Math.Round(_coordinateInfo.XLogicMax, 4);
            var minY = Math.Round(_coordinateInfo.YLogicMin, 4);
            var maxY = Math.Round(_coordinateInfo.YLogicMax, 4);

            var logicX2 = Math.Round(logicX, 4);
            var logicY2 = Math.Round(logicY, 4);

            if (logicX2 >= minX && logicX2 <= maxX &&
                logicY2 >= minY && logicY2 <= maxY)
            {
                return true;
            }
            return false;
        }

        public void ClearSelector()
        {
            _drawers[DrawType.Selector].Clear();
        }

        public void DrawSelector(int i, int i1, int i2, int i3)
        {
            _drawers[DrawType.Selector].Draw(i, i1, i2, i3);
        }

        public void ZoomIn(Point startPoint, Point endPoint)
        {
            if (!_coordinateInfo.ZoomIn(startPoint, endPoint)) return;

            foreach (var drawer in _drawers.Values)
            {
                drawer.ZoomIn(startPoint, endPoint);
            }
        }

        public void ZoomOut()
        {
            _coordinateInfo.ZoomOut();
            foreach (var drawer in _drawers.Values)
            {
                drawer.ZoomOut();
            }
        }

        public void DrawRange()
        {
            _drawers[DrawType.Range].Draw();
        }
    }
}
