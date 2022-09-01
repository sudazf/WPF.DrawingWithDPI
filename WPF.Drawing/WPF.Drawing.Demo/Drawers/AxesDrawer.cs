using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Media;
using WPF.Drawing.Demo.SystemInfos;

namespace WPF.Drawing.Demo.Drawers
{
    internal class AxesDrawer : BaseDrawer
    {
        public AxesDrawer(CoordinateInfo coordinateInfo) : base(coordinateInfo)
        {
        }

        protected override void OnDraw(params object[] paras)
        {
            Clear();

            //draw a demo x/y axes.
            DrawCoordinate();
        }

        protected override void OnZoomIn(Point startPoint, Point endPoint)
        {
            Draw();
        }

        protected override void OnZoomOut()
        {
            Draw();
        }

        private void DrawCoordinate()
        {
            try
            {
                float bigStepLen = 0;//坐标轴两个长刻度线间的距离
                float smallStepLen = 0;//坐标轴两个短刻度线间的距离
                double bigStepLenLogic = 0d;
                double smallStepLenLogic = 0d;

                #region 画纵坐标
                PointF tmpPoint = new PointF(CoordinateInfo.OriginPoint.X, CoordinateInfo.OriginPoint.Y);
                float tmpPointY = 0;

                bigStepLen = CoordinateInfo.YAxesLength * 1.0f / CoordinateInfo.AxesBigBlockCount;
                smallStepLen = bigStepLen / CoordinateInfo.AxesSmallBlockCount;
                bigStepLenLogic = Math.Round((CoordinateInfo.YLogicMax - CoordinateInfo.YLogicMin) / CoordinateInfo.AxesBigBlockCount, 3);
                smallStepLenLogic = Math.Round(bigStepLenLogic / CoordinateInfo.AxesSmallBlockCount, 3);

                //绘制 Y 轴                    
                DrawLine((int)tmpPoint.X, (int)tmpPoint.Y, (int)tmpPoint.X, (int)tmpPoint.Y - CoordinateInfo.YAxesLength, Colors.Black);

                for (int bigStepIndex = 0; bigStepIndex < CoordinateInfo.AxesBigBlockCount; bigStepIndex++)
                {
                    tmpPointY = tmpPoint.Y - bigStepLen * bigStepIndex;
                    if (bigStepIndex > 0)
                    {
                        DrawLineDotted((int)tmpPoint.X, (int)tmpPointY, (int)tmpPoint.X + CoordinateInfo.XAxesLength, (int)tmpPointY, 4, 4, Colors.Gray);
                    }

                    //绘制长刻度线处的文本
                    var value = CoordinateInfo.YLogicMin + bigStepLenLogic * bigStepIndex;
                    DrawGraduationText(value, tmpPoint.X - 18, tmpPointY - 2, Colors.Black);

                    for (int smallStepIndex = 1; smallStepIndex < CoordinateInfo.AxesSmallBlockCount; smallStepIndex++)
                    {
                        float tmpHeight = tmpPointY - smallStepIndex * smallStepLen;
                        DrawLineDotted((int)tmpPoint.X, (int)tmpHeight, (int)tmpPoint.X + CoordinateInfo.XAxesLength, (int)tmpHeight, 4, 4, Colors.Gray);

                        if (CoordinateInfo.AxesBigBlockCount < 10)
                        {
                            var smallValue = value + smallStepLenLogic * smallStepIndex;
                            DrawGraduationText(smallValue, (int)tmpPoint.X - 10 - 5, (int)tmpHeight - 4, Colors.Black);
                        }
                    }
                }

                tmpPointY = tmpPoint.Y - bigStepLen * CoordinateInfo.AxesBigBlockCount;
                DrawLine((int)tmpPoint.X, (int)tmpPointY, (int)tmpPoint.X - 5, (int)tmpPointY, Colors.Red);

                //绘制最上面一条长刻度线处的文本
                var maxY = CoordinateInfo.YLogicMin + bigStepLenLogic * CoordinateInfo.AxesBigBlockCount;
                DrawGraduationText(maxY, tmpPoint.X - 5 - 10, tmpPointY, Colors.Black);

                //绘制最上面一条长刻度线上的数据类型的文本
                DrawYAxesDataTypeText("Y", tmpPoint.X, tmpPointY, 5 * 2);

                #endregion

                #region 画横坐标

                DrawLine(CoordinateInfo.OriginPoint.X, CoordinateInfo.OriginPoint.Y, CoordinateInfo.XAxesLength + CoordinateInfo.OriginPoint.X, CoordinateInfo.OriginPoint.Y, Colors.Black);
                tmpPoint.Y = CoordinateInfo.OriginPoint.Y;
                tmpPoint.X = CoordinateInfo.OriginPoint.X;
                bigStepLen = CoordinateInfo.XAxesLength * 1.0f / CoordinateInfo.AxesBigBlockCount;
                smallStepLen = bigStepLen / CoordinateInfo.AxesSmallBlockCount;
                bigStepLenLogic = Math.Round((CoordinateInfo.XLogicMax - CoordinateInfo.XLogicMin) / CoordinateInfo.AxesBigBlockCount, 3);
                smallStepLenLogic = Math.Round(bigStepLenLogic / CoordinateInfo.AxesSmallBlockCount, 3);

                for (int bigStepIndex = 0; bigStepIndex < CoordinateInfo.AxesBigBlockCount; bigStepIndex++)
                {
                    tmpPoint.X = CoordinateInfo.OriginPoint.X + bigStepLen * bigStepIndex;
                    if (bigStepIndex > 0)
                    {
                        DrawLineDotted((int)tmpPoint.X, (int)tmpPoint.Y, (int)tmpPoint.X, (int)tmpPoint.Y - CoordinateInfo.YAxesLength, 4, 4, Colors.Gray);
                    }

                    var stepCoe = bigStepLen * CoordinateInfo.LogicScaleX;
                    var stepCoe2 = Math.Round(stepCoe, 3);
                    var value = CoordinateInfo.XLogicMin + stepCoe2 * bigStepIndex;
                    DrawGraduationText(value, tmpPoint.X + 10, tmpPoint.Y + 5 + 4, Colors.Black);
                    for (int smallStepIndex = 1; smallStepIndex < CoordinateInfo.AxesSmallBlockCount; smallStepIndex++)
                    {
                        float tmpWidth = tmpPoint.X + smallStepIndex * smallStepLen;
                        DrawLineDotted((int)tmpWidth, (int)tmpPoint.Y, (int)tmpWidth, (int)tmpPoint.Y - CoordinateInfo.YAxesLength, 4, 4, Colors.Gray);

                        if (CoordinateInfo.AxesBigBlockCount < 10)
                        {
                            var smallValue = value + smallStepLenLogic * smallStepIndex;
                            DrawGraduationText(smallValue, (int)tmpWidth + 10, tmpPoint.Y + 5 + 4, Colors.Black);
                        }
                    }
                }

                tmpPoint.X = CoordinateInfo.OriginPoint.X + bigStepLen * CoordinateInfo.AxesBigBlockCount;
                DrawLine((int)tmpPoint.X, (int)tmpPoint.Y, (int)tmpPoint.X, (int)tmpPoint.Y + 5, Colors.Red);

                var maxX = CoordinateInfo.XLogicMin + bigStepLenLogic * CoordinateInfo.AxesBigBlockCount;
                DrawGraduationText(maxX, tmpPoint.X + 10, tmpPoint.Y + 5 + 4, Colors.Black);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Fail to draw axis, error info:" + ex.Message, ex);
            }
        }
        private void DrawGraduationText(double value, float rightCenterPointX, float rightCenterPointY, System.Windows.Media.Color color)
        {
            var offsetX = 16;
            var offsetY = -4;
            var valueAsString = value.ToString(CultureInfo.InvariantCulture);
            if (valueAsString.Contains("."))
            {
                var dotString = valueAsString.Split('.')[1];
                offsetX += (int)(4 * dotString.Length * SystemInfo.Dpi.DpiScale);
            }

            DrawString((int)rightCenterPointX - offsetX, (int)rightCenterPointY + offsetY, value.ToString(CultureInfo.InvariantCulture), color);
        }
        private void DrawYAxesDataTypeText(string text, float rightPointX, float rightPointY, float width)
        {
            float textHeight = 20;
            DrawString((int)(rightPointX - width), (int)(rightPointY - textHeight - CoordinateInfo.AxesFont.Height), text, Colors.Black);
        }
    }
}
