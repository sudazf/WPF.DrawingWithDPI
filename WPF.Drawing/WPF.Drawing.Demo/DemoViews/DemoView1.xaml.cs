using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPF.Drawing.Demo.Drawers;
using WPF.Drawing.Demo.SystemInfos;

namespace WPF.Drawing.Demo.DemoViews
{
    /// <summary>
    /// DemoView1.xaml 的交互逻辑
    /// </summary>
    public partial class DemoView1 : UserControl
    {
        private DrawHelper _drawHelper;
        private bool _dragToScale;
        private Point _startLocation;
        public DemoView1()
        {
            //fixed: if use this custom control in WinForms application, visual studio cashed on design mode.
            bool isInWpfDesignerMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            bool isInFormsDesignerMode = System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv";

            if (isInWpfDesignerMode || isInFormsDesignerMode)
            {
                // is in any designer mode
            }
            else
            {
                SizeChanged += OnControlSizeChanged;
            }

            InitializeComponent();
        }

        private void OnControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SystemInfo.GetDpiFromVisual(ImageHolder);
            InitDrawSource();
            UpdateDrawSource();
        }

        private void InitDrawSource()
        {
            if (_drawHelper == null)
            {
                _drawHelper = new DrawHelper();

                foreach (var drawerPair in _drawHelper.Drawers)
                {
                    switch (drawerPair.Key)
                    {
                        case DrawType.Axes:
                            drawerPair.Value.OnSourceChanged += OnAxesSourceChanged;
                            break;
                        case DrawType.Range:
                            drawerPair.Value.OnSourceChanged += OnRangeSourceChanged;
                            break;
                        case DrawType.Selector:
                            drawerPair.Value.OnSourceChanged += OnSelectorSourceChanged;
                            break;
                    }
                }
            }
        }
        private void UpdateDrawSource()
        {
            //绘图需要按 DPI 比例调整，否则达不到高清效果
            var renderWidth = ImageHolder.ActualWidth * SystemInfo.Dpi.DpiScale;
            var renderHeight = ImageHolder.ActualHeight * SystemInfo.Dpi.DpiScale;

            _drawHelper.UpdateDrawers(renderWidth, renderHeight);
            //_drawHelper.UpdateDrawers(ImageHolder.ActualWidth, ImageHolder.ActualHeight);
        }

        private void OnAxesSourceChanged(object sender, BitmapSource source)
        {
            AxesImage.Source = source;
            _drawHelper.DrawAxes();
        }
        private void OnRangeSourceChanged(object sender, BitmapSource source)
        {
            RangeImage.Source = source;
            _drawHelper.DrawRange();
        }
        private void OnSelectorSourceChanged(object sender, BitmapSource source)
        {
            SelectorImage.Source = source;
        }

        private void OnSelectorImageMouseMove(object sender, MouseEventArgs e)
        {
            var mousePoint = e.GetPosition(SelectorImage);
            _drawHelper.DrawToolTip(new System.Drawing.Point((int)(mousePoint.X * SystemInfo.Dpi.DpiScale), (int)(mousePoint.Y * SystemInfo.Dpi.DpiScale)));

            if (!_dragToScale) return;

            var x = Math.Min(_startLocation.X, mousePoint.X);
            var y = Math.Min(_startLocation.Y, mousePoint.Y);
            var w = Math.Abs(_startLocation.X - mousePoint.X);
            var h = Math.Abs(_startLocation.Y - mousePoint.Y);

            _drawHelper.DrawSelector((int)x, (int)y, (int)w, (int)h);

        }
        private void OnSelectorImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            var mousePoint = e.GetPosition(SelectorImage);

            if (_drawHelper.CheckAvailableArea(new System.Drawing.Point((int)(mousePoint.X * SystemInfo.Dpi.DpiScale), (int)(mousePoint.Y * SystemInfo.Dpi.DpiScale))))
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    _dragToScale = true;
                }

                _startLocation = new Point((int)mousePoint.X, (int)mousePoint.Y);
            }
        }
        private void OnSelectorImageMouseUp(object sender, MouseButtonEventArgs e)
        {
            var mousePoint = e.GetPosition(SelectorImage);
            if (!_drawHelper.CheckAvailableArea(new System.Drawing.Point((int)(mousePoint.X * SystemInfo.Dpi.DpiScale), (int)(mousePoint.Y * SystemInfo.Dpi.DpiScale))))
            {
                _dragToScale = false;
                _drawHelper.ClearSelector();
                return;
            }

            var ex = mousePoint.X <= ImageHolder.ActualWidth ? mousePoint.X : ImageHolder.ActualWidth;
            var ey = mousePoint.Y <= ImageHolder.ActualHeight ? mousePoint.Y : ImageHolder.ActualHeight;

            if (_dragToScale)
            {
                _dragToScale = false;

                if (_startLocation.X < ex && ex - _startLocation.X >= 10)
                {
                    //Zoom in
                    _drawHelper.ZoomIn(new System.Drawing.Point((int)(_startLocation.X * SystemInfo.Dpi.DpiScale), (int)(_startLocation.Y * SystemInfo.Dpi.DpiScale)),
                        new System.Drawing.Point((int)(mousePoint.X * SystemInfo.Dpi.DpiScale), (int)(mousePoint.Y * SystemInfo.Dpi.DpiScale)));
                }
                else if (_startLocation.X > ex && _startLocation.X - ex >= 10)
                {
                    //Zoom out
                    _drawHelper.ZoomOut();
                }
            }

            _drawHelper.ClearSelector();

        }
    }
}
