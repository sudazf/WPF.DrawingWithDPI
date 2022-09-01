using System.Windows.Media;
using WPF.Drawing.Demo.SystemInfos;

namespace WPF.Drawing.Demo.Drawers
{
    internal class SelectorDrawer : BaseDrawer
    {
        public SelectorDrawer(CoordinateInfo coordinateInfo) : base(coordinateInfo)
        {
        }

        protected override void OnDraw(params object[] paras)
        {
            Clear();

            var x1 = (int)paras[0];
            var y1 = (int)paras[1];
            var width = (int)paras[2];
            var height = (int)paras[3];

            DrawRectangle((int)(x1 * SystemInfo.Dpi.DpiScale), (int)(y1 * SystemInfo.Dpi.DpiScale),
                (int)(width * SystemInfo.Dpi.DpiScale), (int)(height * SystemInfo.Dpi.DpiScale), Colors.Black);
        }
    }
}
