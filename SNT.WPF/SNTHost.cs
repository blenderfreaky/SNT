namespace SNT.WPF
{
    using System;
    using System.Numerics;
    using System.Windows;
    using System.Windows.Media;

    public class SNTHost : FrameworkElement
    {
        public MainWindow MainWindow { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //float fac = MainWindow.ImageWidth / (MainWindow.World.Max - MainWindow.World.Min).X;
            //float x = MainWindow.World.Min.X;
            //float y = MainWindow.World.Min.Y;
            float x = MainWindow.ImageWidth / 2;
            float y = MainWindow.ImageHeight / 2;

            Pen tracePen = new Pen(Brushes.PaleVioletRed, 1);
            Pen speedPen = new Pen(Brushes.Yellow, 1);

            drawingContext.DrawRectangle(Brushes.Black, null,
                new Rect(0, 0, MainWindow.ImageWidth, MainWindow.ImageHeight));

            var trace = new Vector2[10];

            foreach (var particle in MainWindow.World.Particles)
            {
                var size = MathF.Log10(particle.Mass);

                drawingContext.DrawEllipse(Brushes.White, null,
                    new Point(
                        particle.Position.X + x,
                        particle.Position.Y + y),
                    size, size);

                drawingContext.DrawLine(speedPen,
                    new Point(
                        particle.Position.X + x,
                        particle.Position.Y + y),
                    new Point(
                        particle.Position.X + particle.Velocity.X * 10 + x,
                        particle.Position.Y + particle.Velocity.Y * 10 + y));

                continue;

                particle.Trace.CopyTo(trace, 0);
                int traceIndex = particle.TraceIndex;

                Point previousTrace = new Point(trace[traceIndex].X, trace[traceIndex].Y);

                for (int i = 1; i < 2; i++)
                {
                    var idx = (traceIndex + i) % trace.Length;
                    Point currentTrace = new Point(trace[idx].X, trace[idx].Y);

                    if (currentTrace == previousTrace) continue;

                    drawingContext.DrawLine(tracePen, previousTrace, currentTrace);

                    previousTrace = currentTrace;
                }
            }

            //if (MainWindow.World.Quadtree != null) DrawQuadtree(MainWindow.World.Quadtree);

            void DrawQuadtree(Quadtree t, int depth = 0)
            {
                drawingContext.DrawRectangle(null, tracePen, new Rect(
                    t.Min.X + x, t.Min.Y + y,
                    t.Max.X - t.Min.X, t.Max.Y - t.Min.Y));

                var s = MathF.Log10(t.TotalMass) / depth;
                drawingContext.DrawEllipse(Brushes.Red, null, new Point(t.CenterOfMass.X + x, t.CenterOfMass.Y + y), s, s);

                if (t.Nodes == null) return;
                if (depth > 5) return;

                for (int i = 0; i < 4; i++) DrawQuadtree(t.Nodes[i], depth + 1);
            }
        }
    }
}
