# WPF.DrawingWithDPI

Welcome to the WPF.DrawingWithDPI

WPF draw high resolution image.

This is blurry image:
![Blurry](https://user-images.githubusercontent.com/3366672/187870067-456efb18-aeff-4763-998b-1f4ada274696.png)

This is high quality image. (Due to image upload problem, actually, runing in application is more clearly than this).
![Not Blurry](https://user-images.githubusercontent.com/3366672/187869673-f6993bba-15d6-4c27-8d3d-0bef75573fc8.png)

1. Using mouse left button dragging to zoom in/out.

2. Switch to blurry image by uncomment last row code:
```cs
private void UpdateDrawSource()
{
    //绘图需要按 DPI 比例调整，否则达不到高清效果
    var renderWidth = ImageHolder.ActualWidth * SystemInfo.Dpi.DpiScale;
    var renderHeight = ImageHolder.ActualHeight * SystemInfo.Dpi.DpiScale;

    _drawHelper.UpdateDrawers(renderWidth, renderHeight);
    //_drawHelper.UpdateDrawers(ImageHolder.ActualWidth, ImageHolder.ActualHeight);
}
```
