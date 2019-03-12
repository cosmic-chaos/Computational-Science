using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using GraphControl;
using System.Windows.Input;

namespace VisualizerControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Visualizer : Window
    {
        private IVisualization visualization;
        private IList<IUpdating> toUpdate = new List<IUpdating>();

        private double timeIncrement = .001;
        public double TimeIncrement
        {
            get { return timeIncrement; }
            set { timeIncrement = value; TimeIncrementSlider.Text = value.ToString(); }
        }

        public bool AutoCamera
        {
            get
            {
                return AutoCameraCheck.IsChecked == true;
            }
            set
            {
                AutoCameraCheck.IsChecked = value;
            }
        }

        public double Time { get; set; } = 0;

        private bool timeRunning = false;
        private Timer timer = new Timer(1);

        public delegate Vector3D VectorFunc();

        public Visualizer(IVisualization visualization)
        {
            this.visualization = visualization;
            InitializeComponent();
            visualization.SetCollection(Group.Children);
            visualization.Initialize();

            TimeIncrementSlider.Text = TimeIncrement.ToString();

//            CamX.Value = Camera.Position.X;
//            CamY.Value = Camera.Position.Y;
//            CamZ.Value = Camera.Position.Z;
        }

        public void AddToGraph(int index, string name, Timeline.GetValue funcX, Timeline.GetValue funcY, Color color)
        {
            GraphUnderlying graphU = toUpdate[index] as GraphUnderlying;
            graphU.AddTimeline(new Timeline(name, funcX, funcY, color));
        }

        public void AddSingleGraph(string name, Timeline.GetValue funcX, Timeline.GetValue funcY, Color color, string xTitle, string yTitle)
        {
            GraphUnderlying graphU = new GraphUnderlying(xTitle, yTitle);
            graphU.AddTimeline(new Timeline(name, funcX, funcY, color));
            AddGraph(new Graph(graphU));
        }

        public void Add3DGraph(string name, Timeline.GetValue funcX, VectorFunc funcY, string xTitle, string yTitle)
        {
            GraphUnderlying graphU = new GraphUnderlying(xTitle, yTitle);
            graphU.AddTimeline(new Timeline("x " + name, funcX, (() => funcY().X), Colors.Red));
            graphU.AddTimeline(new Timeline("y " + name, funcX, (() => funcY().Y), Colors.Green));
            graphU.AddTimeline(new Timeline("z " + name, funcX, (() => funcY().Z), Colors.Blue));
            AddGraph(new Graph(graphU));
        }

        public void AddHist(Histogram.PartFunc func, IEngine engine, int nBins, Color color, string xTitle)
        {
            Histogram hist = new Histogram(func, engine, nBins, color, xTitle);
            AddGraph(new Graph(hist));
        }

        public void AddGraph(Graph graph)
        {
            GraphPanel.RowDefinitions.Add(new RowDefinition());
            GraphPanel.Children.Add(graph);
            Grid.SetRow(graph, GraphPanel.RowDefinitions.Count - 1);
            toUpdate.Add(graph);
        }

        public void AddText(string name, Timeline.GetValue func, Color color)
        {
            var bl = new UpdatingText();
            bl.Title = name;
            bl.Function = func;
            bl.Color = color;

            GraphPanel.Children.Add(bl);
            toUpdate.Add(bl);
        }

        public void Start()
        {
            timer.Elapsed += timer_Tick;
            timer.Start();
        }

        private void adjustCamera()
        {
            var projectiles = visualization.GetAllProjectiles();

            if (projectiles.Count == 0)
            {
                return;
            }
            else if (projectiles.Count == 1)
            {
                double offset = 10; // Because why not
                Vector3D direction = new Vector3D(1, 1, 1);
                direction.Normalize();
                Vector3D pos = projectiles[0] + offset * direction;
                Camera.Position = new Point3D(pos.X, pos.Y, pos.Z);
                Camera.LookDirection = projectiles[0] - pos;
            }
            else
            {
                Vector3D centerOfParticles = center();
                Camera.Position = new Point3D(centerOfParticles.X, centerOfParticles.Y, centerOfParticles.Z);
                Camera.LookDirection = (-centerOfParticles) / centerOfParticles.Length;

                double maxDistance = 0;

                foreach (var proj in projectiles)
                {
                    double bestDistance = camDistance(proj);
                    if (bestDistance > maxDistance)
                        maxDistance = bestDistance;
                }

                Vector3D newPos = maxDistance / centerOfParticles.Length * centerOfParticles;
                Camera.Position = new Point3D(newPos.X, newPos.Y, newPos.Z);
            }

  //          CamX.Value = Camera.Position.X;
  //          CamY.Value = Camera.Position.Y;
  //          CamZ.Value = Camera.Position.Z;
        }

        private Vector3D center()
        {
            Vector3D response = new Vector3D(0, 0, 0);
            var projectiles = visualization.GetAllProjectiles();
            foreach (var proj in projectiles)
            {
                response += proj;
            }
            return response /= projectiles.Count;
        }

        private bool isInCamera(Vector3D vec)
        {
            Vector3D fromCamera = vec - new Vector3D(Camera.Position.X, Camera.Position.Y, Camera.Position.Z);
            fromCamera.Normalize();
            Vector3D lookDirection = Camera.LookDirection;
            lookDirection.Normalize();
            double dot = Vector3D.DotProduct(fromCamera, lookDirection);
            double angleBetween = Math.Acos(dot);
            return angleBetween < Camera.FieldOfView / 2;
        }

        private double camDistance(Vector3D vec)
        {
            double angle = Camera.FieldOfView / 2;

            Vector3D camPosition = new Vector3D(Camera.Position.X, Camera.Position.Y, Camera.Position.Z);
            Vector3D camDirection = Camera.LookDirection - camPosition;
            double distance = distanceLineToPoint(camPosition, camDirection, vec);

            double distanceAlongAxis = distance / Math.Tan(angle);
            double pointDistance = Vector3D.DotProduct(vec, camPosition) / camPosition.Length;
            return distanceAlongAxis + pointDistance;
        }

        // From Wolfram Mathworld,
        // http://mathworld.wolfram.com/Point-LineDistance3-Dimensional.html
        private double distanceLineToPoint(Vector3D lineVec1, Vector3D lineVec2, Vector3D point)
        {
            Vector3D x0x1 = point - lineVec1;
            Vector3D x0x2 = point - lineVec2;
            Vector3D x2x1 = lineVec2 - lineVec1;

            Vector3D numVec = Vector3D.CrossProduct(x0x1, x0x2);
            Vector3D denVec = x2x1;
            return numVec.Length / denVec.Length;
        }

        protected override void OnClosed(EventArgs e)
        {
            timer.Stop();
            base.OnClosed(e);
        }

        private void timer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (timeRunning)
            {
                // Stop the timer to give it time to render
                timer.Stop();
                try
                {
                    Dispatcher.Invoke(() =>
                        {
                            if (Show3DCheck.IsChecked == true)
                            {
                                bool result = visualization.Update(Time += TimeIncrement, TraceCheck.IsChecked == true);
                                if (!result)
                                {
                                    timeRunning = false;
                                }
                            }

                            foreach (var element in toUpdate)
                            {
                                element.Update();
                            }

                            if (AutoCameraCheck.IsChecked == true)
                            {
                                adjustCamera();
                            }

                            // Now restart the timer once it is done computing
                            if (timeRunning)
                            {
                                timer.Start();
                            }
                        });
                }
                catch (Exception)
                {
                    // Do nothing
                }
            }
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            timeRunning = true;
            Start();
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            timeRunning = false;
            timer.Stop();
        }

        private void Camera_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Camera != null)
            {
                double x = CamX.Value;
                double y = CamY.Value;
                double z = CamZ.Value;

                Camera.Position = new Point3D(x, y, z);
            }
        }

        private void TimeIncrement_TextChanged(object sender, TextChangedEventArgs e)
        {
            double result;
            if (Double.TryParse(TimeIncrementSlider.Text, out result))
            {
                TimeIncrement = result;
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            //adjustCamera();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Stop_Button_Click(sender, e);
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Screenshot";
            dlg.DefaultExt = ".jpg";

            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;

                RenderTargetBitmap bitmap = new RenderTargetBitmap((int)ActualWidth, (int)ActualHeight, 96, 96, PixelFormats.Pbgra32);
                bitmap.Render(this);
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                using (Stream fileStream = File.Create(filename))
                {
                    encoder.Save(fileStream);
                }
            }
            Start();
        }

        private void MaxPointsSlider_TextChanged(object sender, TextChangedEventArgs e)
        {
            double result;
            if (Double.TryParse(MaxPointsSlider.Text, out result))
            {
                Timeline.MaximumPoints = result;
            }
        }

        private void AutoCameraCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (AutoCameraCheck.IsChecked == true)
            {
                adjustCamera();
            }
        }

        private Point previousMouse;
        private Transform3D currentTransform = MatrixTransform3D.Identity;

        private void myViewport_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            previousMouse = e.GetPosition(null);
        }

        private void myViewport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentTransform = Camera.Transform;
        }

        private void myViewport_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var diff = e.GetPosition(null) - previousMouse;

                double xSize = myViewport.ActualWidth;
                double ySize = myViewport.ActualHeight;

                const double totalScreen = Math.PI / 2;

                double xRot = diff.X / xSize * totalScreen;
                double yRot = diff.Y / ySize * totalScreen;

                rotate(xRot, yRot);
            }
        }

        private void rotate(double leftRightAngle, double upDownAngle) // in radians
        {
            var middle = center();
            var midpoint = new Point3D(middle.X, middle.Y, middle.Z);
            var axisLR = -Camera.UpDirection;
            var axisUD = Vector3D.CrossProduct(Camera.LookDirection, axisLR);

            var rotationLR = new RotateTransform3D(new AxisAngleRotation3D(axisLR, radiansToDegrees(leftRightAngle)), midpoint);
            var rotationUD = new RotateTransform3D(new AxisAngleRotation3D(axisUD, radiansToDegrees(upDownAngle)), midpoint);
            var rotation = new Transform3DGroup() { Children = new Transform3DCollection() { currentTransform, rotationLR, rotationUD } };

            Camera.Transform = rotation;
            Camera.LookDirection = middle - new Vector3D(Camera.Position.X, Camera.Position.Y, Camera.Position.Z);
        }

       static private double radiansToDegrees(double input)
        {
            return input * 180 / Math.PI;
        }

    }

}
