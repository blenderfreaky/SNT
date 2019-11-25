namespace SNT.WPF
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    public class SNTHost : FrameworkElement
    {
        public MainWindow MainWindow { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            float fac = 1f;
            foreach (var particle in MainWindow.World.Particles)
            {
                float speed = Math.Min(particle.Velocity.Length(), 1);
                var brush = new SolidColorBrush(Color.FromArgb(255, (byte)(speed * 256 - 1), (byte)(256- speed * 256), (byte)0));
                drawingContext.DrawEllipse(brush, null, new Point(particle.Position.X * fac, particle.Position.Y * fac), particle.Mass, particle.Mass);
            }
        }
    }
}
