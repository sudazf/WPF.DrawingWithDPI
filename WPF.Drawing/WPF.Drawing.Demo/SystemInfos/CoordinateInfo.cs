using System;
using System.Drawing;

namespace WPF.Drawing.Demo.SystemInfos
{
    public class CoordinateInfo
    {
        /// <summary>
        /// 坐标轴分为 10 大段
        /// </summary>
        public int AxesBigBlockCount = 10;

        /// <summary>
        /// 坐标轴每个大段分为 2 个小段
        /// </summary>
        public const int AxesSmallBlockCount = 2;

        /// <summary>
        /// 图形与边的距离
        /// </summary>
        public int DistanceFromSize = 60;

        /// <summary>
        /// 原点
        /// </summary>
        public Point OriginPoint;

        /// <summary>
        /// 轴坐标字体
        /// </summary>
        public static Font AxesFont;

        /// <summary>
        /// X轴长度
        /// </summary>
        public int XAxesLength = 0;

        /// <summary>
        /// Y轴长度
        /// </summary>
        public int YAxesLength = 0;

        /// <summary>
        /// X轴逻辑长度 与 X轴像素长度 比
        /// </summary>
        public double LogicScaleX { get; private set; } = 1.0f;

        /// <summary>
        /// Y轴逻辑长度 与 Y轴像素长度 比
        /// </summary>
        public double LogicScaleY { get; private set; } = 1.0f;

        /// <summary>
        /// X轴最大值（像素）
        /// </summary>
        public double XMax { get; private set; }

        /// <summary>
        /// X轴最小值（像素）
        /// </summary>
        public double XMin { get; private set; }

        /// <summary>
        /// Y轴最大值（像素）
        /// </summary>
        public double YMax { get; private set; }

        /// <summary>
        /// Y轴最小值（像素）
        /// </summary>
        public double YMin { get; private set; }

        /// <summary>
        /// X轴最大值（逻辑）
        /// </summary>
        public double XLogicMax { get; private set; } = 1.0f;

        /// <summary>
        /// X轴最小值（逻辑）
        /// </summary>
        public double XLogicMin { get; private set; }

        /// <summary>
        /// Y轴最大值（逻辑）
        /// </summary>
        public double YLogicMax { get; private set; } = 1.0f;

        /// <summary>
        /// Y轴最小值（逻辑）
        /// </summary>
        public double YLogicMin { get; private set; }

        public CoordinateInfo()
        {
            DistanceFromSize = (int)(DistanceFromSize * SystemInfo.Dpi.DpiScale);
            AxesFont = new Font("宋体", 8);
        }

        /// <summary>
        /// X坐标 物理像素值转逻辑坐标值
        /// </summary>
        /// <param name="mousePointX">鼠标所在的物理像素 X 值</param>
        /// <returns>X逻辑坐标</returns>
        public double GetLogicX(int mousePointX)
        {
            return XLogicMin + (mousePointX - OriginPoint.X) * LogicScaleX;
        }
        public double GetLogicY(int mousePointY)
        {
            return YLogicMax - (mousePointY - DistanceFromSize) * LogicScaleY;
        }

        /// <summary>
        /// X坐标 逻辑坐标值转物理像素坐标值
        /// </summary>
        /// <param name="logicX"> X 逻辑坐标值</param>
        /// <returns>X物理像素坐标值</returns>
        public int GetDrawerX(double logicX)
        {
            return (int)((logicX - XLogicMin) / LogicScaleX + OriginPoint.X);
        }
        public int GetDrawerY(double logicY)
        {
            return (int)((YLogicMax - logicY) / LogicScaleY + DistanceFromSize);
        }

        public void UpdateAxesRange(double width, double height)
        {
            XMax = width;
            XMin = 0;
            YMax = height;
            YMin = 0;
            CalculateCoordinateInfo();
        }
        public bool ZoomIn(Point startPoint, Point endPoint)
        {
            var previousMinX = XLogicMin;
            var previousMax = XLogicMax;
            XLogicMax = Math.Round(previousMinX + (endPoint.X - OriginPoint.X) * LogicScaleX, 3);
            XLogicMin = Math.Round(previousMinX + (startPoint.X - OriginPoint.X) * LogicScaleX, 3);

            var previousMinY = YLogicMin;
            var previousMaxY = YLogicMax;
            YLogicMax = Math.Round(previousMaxY - (startPoint.Y - DistanceFromSize) * LogicScaleY, 3);
            YLogicMin = Math.Round(previousMaxY - (endPoint.Y - DistanceFromSize) * LogicScaleY, 3);

            if (XLogicMax - XLogicMin <= 0.001 || YLogicMax - YLogicMin < 0.001)
            {
                XLogicMin = previousMinX;
                XLogicMax = previousMax;
                YLogicMin = previousMinY;
                YLogicMax = previousMaxY;

                return false;
            }
            AxesBigBlockCount = (int)Math.Ceiling((YLogicMax - YLogicMin) * 10 / 1.0f);
            CalculateCoordinateInfo();
            return true;
        }
        public void ZoomOut()
        {
            XLogicMax = 1.0f;
            XLogicMin = 0f;
            YLogicMax = 1.0f;
            YLogicMin = 0f;
            AxesBigBlockCount = 10;
            CalculateCoordinateInfo();
        }

        private void CalculateCoordinateInfo()
        {
            OriginPoint.X = DistanceFromSize;
            OriginPoint.Y = (int)YMax - DistanceFromSize;

            XAxesLength = (int)XMax - 2 * DistanceFromSize;
            YAxesLength = (int)YMax - 2 * DistanceFromSize;

            LogicScaleX = (XLogicMax - XLogicMin) / XAxesLength;
            LogicScaleY = (YLogicMax - YLogicMin) / YAxesLength;
        }
    }
}
