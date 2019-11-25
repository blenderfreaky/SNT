namespace SNT.WPF
{
    using System;
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public World World { get; }

        public Task WorkingTask { get; }

        public DrawingImage Bitmap { get; set; }

        public int ImageWidth { get; }
        public int ImageHeight { get; }

        public MainWindow()
        {
            InitializeComponent();

            var size = new Vector2(ImageWidth = 800, ImageHeight = 800);

            World = new World(-size, size,
                gravity: 0.1f, damping: 10f, theta: 1f);

            PopulateWorld(800, ImageWidth / 2, ImageHeight / 2, 0.8f, 0.6f, 10f, (p, _) =>
                {
                    //return Vector2.Zero;
                    return new Vector2(p.Y, -p.X) / -1000f;
                });

           // World.AddParticle(new Vector2(0, 0), new Vector2(0, 0), 5000000);
            //World.AddParticle(new Vector2(1, -1) * 200, new Vector2(-1, 0), 5000);
            //World.AddParticle(new Vector2(1, 1) * -200, new Vector2(1, 0), 50);

            //WorkingTask = Task.Run(MainLoop);

            snt.MainWindow = this;

            var timer = new DispatcherTimer(DispatcherPriority.Render, Dispatcher);
            timer.Tick += (s, e) =>
            {
                Step();
                Step();
                Step();
                snt.InvalidateVisual();
                };
            timer.Interval = TimeSpan.FromMilliseconds(1000 / 40f);
            timer.Start();
        }

        private void MainLoop()
        {
            Start:
            try
            {
                while (true)
                {
                    Step();
                }
            }
            catch (Exception e)
            {
                goto Start;
            }
        }

        private void Step()
        {
            World.Step();
        }

        private void PopulateWorld(int amount, float areaX, float areaY, float areaCoverage, float massFac, float massBase, Func<Vector2, Random, Vector2> velocityCalc)
        {
            Random random = new Random();

            for (int i = 0; i < amount; i++)
            {
                //var p_x = (areaX * areaCoverage * (float)random.NextDouble()) * 2 - (areaX * areaCoverage);
                //var p_y = (areaY * areaCoverage * (float)random.NextDouble()) * 2 - (areaY * areaCoverage);
                var theta = (float)random.NextDouble() * MathF.PI * 2;
                var r = (float)random.NextDouble() * areaX * areaCoverage;

                var p_x = MathF.Cos(theta) * r;
                var p_y = MathF.Sin(theta) * r;

                var pos = new Vector2(p_x, p_y);

                var vel = velocityCalc(pos, random);

                //pos = Vector2.Zero;

                var m = (massFac * (float)random.NextDouble()) + massBase;

                World.AddParticle(pos, vel, m);
            }
        }
    }
}
