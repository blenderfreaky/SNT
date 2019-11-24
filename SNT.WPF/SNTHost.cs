namespace SNT.WPF
{
    using System.Windows;
    using System.Windows.Media;

    public class SNTHost : FrameworkElement
    {
        public MainWindow MainWindow { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            float fac = 1f;

            drawingContext.DrawRectangle(Brushes.Black, null, new Rect(0, 0, MainWindow.ImageWidth * fac, MainWindow.ImageHeight * fac));

            foreach (var particle in MainWindow.World.Particles)
            {
                drawingContext.DrawEllipse(Brushes.White, null, new Point(particle.Position.X * fac, particle.Position.Y * fac), particle.Mass, particle.Mass);
            }
        }
    }
}
