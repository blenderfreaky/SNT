namespace SNT.WPF
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Threading;

    public class SNTHost : FrameworkElement
    {
        private static System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
        public MainWindow MainWindow { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            float fac = 1f;

            drawingContext.DrawRectangle(Brushes.Black, null, new Rect(0, 0, MainWindow.ImageWidth * fac, MainWindow.ImageHeight * fac));
            if (!s.IsRunning) s.Start();
            s.Stop();
            int b = 0;
            if(s.ElapsedMilliseconds < 20)
            {
                b = (int)(20 - s.ElapsedMilliseconds);
                Thread.Sleep(b);
            }
            float a = 0; try { a = 1000 / s.ElapsedMilliseconds + b; } catch { }

            drawingContext.DrawText(new FormattedText(a.ToString(), System.Globalization.CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface("Arail"), 32, Brushes.White), new Point(10, 10));
            s.Reset();
            s.Start();
            foreach (var particle in World.Particles)
            {
                float speed = Math.Min(particle.Velocity.Length(), 1);
                var brush = new SolidColorBrush(Color.FromArgb(255, (byte)(speed * 256 - 1), (byte)(256- speed * 256), (byte)0));
                drawingContext.DrawEllipse(brush, null, new Point(particle.Position.X * fac, particle.Position.Y * fac), particle.Mass, particle.Mass);
            }
        }
    }
}
