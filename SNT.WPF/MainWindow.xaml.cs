namespace SNT.WPF
{
    using System;
    using System.Numerics;
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

            World = new World(new Vector2(0, 0), new Vector2(ImageWidth = 800, ImageHeight = 800), gravity: 100f, damping: 5, theta:0.1f);

            PopulateWorld(1000, ImageWidth, ImageHeight, 0.1f, 2, (p, r) =>
            {
                return new Vector2();
                var centerDist = p - new Vector2(ImageWidth/2, ImageHeight/2);
                return new Vector2(centerDist.Y, -centerDist.X) / 1000;
            });

            WorkingTask = Task.Run(MainLoop);

            snt.MainWindow = this;

            var timer = new DispatcherTimer(DispatcherPriority.Input, Dispatcher);
            timer.Tick += (s, e) => snt.InvalidateVisual();
            timer.Interval = TimeSpan.FromMilliseconds(10);
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

        private void PopulateWorld(int amount, float areaX, float areaY, float massFac, float massBase, Func<Vector2, Random, Vector2> velocityCalc)
        {
            Random random = new Random();

            for (int i = 0; i < amount; i++)
            {
                var p_x = (areaX /2f * (float)random.NextDouble()) + (areaX / 4f);
                var p_y = (areaY /2f * (float)random.NextDouble()) + (areaY / 4f);

                var pos = new Vector2(p_x, p_y);

                var m = massFac * (float)random.NextDouble() + massBase;

                World.AddParticle(pos, velocityCalc(pos, random), m);
            }
        }
    }
}
