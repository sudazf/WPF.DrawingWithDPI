namespace WPF.Drawing.Demo.SystemInfos
{
    public struct Dpi
    {
        public double X { get; }
        public double Y { get; }
        public double DpiScale { get; }

        public Dpi(double x, double y)
        {
            X = x;
            Y = y;

            DpiScale = X / 96;
        }
    }
}
