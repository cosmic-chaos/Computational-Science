using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Globalization;

namespace GraphControl
{
    internal class Axis
    {
        private DrawingGroup drawing = new DrawingGroup();
        public Drawing Drawing { get { return drawing; } }
        public Transform Transform
        {
            get { return drawing.Transform; }
            set { drawing.Transform = value; }
        }
        public string Title { get; set; }

        private LineGeometry line = new LineGeometry();
        private Geometry geoAxis;

        private bool bottom;

        private DrawingGroup axisLabels = new DrawingGroup();
        private Dictionary<double, Geometry> labelDict = new Dictionary<double, Geometry>();

        private Geometry makeText(string text, Point position)
        {
            FormattedText thisText = new FormattedText(text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface("Arial"),
                10, Brushes.Black);
            thisText.TextAlignment = TextAlignment.Center;
            return thisText.BuildGeometry(position);
        }
        
        public Axis(string title, bool bottom = false)
        {
            this.bottom = bottom;
            Title = title;

            drawing.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Black, 2), line));

            geoAxis = makeText(title, new Point(0, 0));
            drawing.Children.Add(new GeometryDrawing(Brushes.Black, null, geoAxis));
            drawing.Children.Add(axisLabels);
        }

        public void Update(double width, double height, double min, double max)
        {
            if (bottom)
            {
                line.StartPoint = new Point(0, height);
                line.EndPoint = new Point(width, height);
            }
            else
            {
                line.StartPoint = new Point(0, 0);
                line.EndPoint = new Point(width, 0);
            }

            geoAxis.Transform = new TranslateTransform(width / 2, height / 3);

            makeLabels(width, height, min, max); 
        }

        private void makeLabels(double width, double height, double min, double max)
        {
            axisLabels.Children.Clear();
            if (min == max)
            {
                return;
            }
            int rightHand = orderOfMagnitude(max);
            int leftHand = orderOfMagnitude(min);
            int order = rightHand > leftHand ? rightHand : leftHand;
            double step = Math.Pow(10, order);

            double lh = 0;

            if (min < 0)
            {
                while (lh > min)
                {
                    lh -= step;
                }
                lh += step;
            }
            else if (min > 0)
            {
                while (lh < min)
                {
                    lh += step;
                }
            }

            double rh = 0;

            if (max < 0)
            {
                while (rh > max)
                {
                    rh -= step;
                }
            }
            else if (max > 0)
            {
                while (rh < max)
                {
                    rh += step;
                }
                rh -= step;
            }

            double location = bottom ? 3 * height / 4 : height / 4;

            for (double place = lh; place <= rh; place += step)
            {
                double pos = position(min, max, width, place);

                addPoint(new Point(pos, location), place);
            }
        }

        private int orderOfMagnitude(double num)
        {
            return (int)Math.Floor(Math.Log10(Math.Abs(num)));
        }

        private double position(double min, double max, double width, double x)
        {
            return (x - min) / (max - min) * width;
        }

        private void addPoint(Point position, double value)
        {
            Transform finalTransform;
            TranslateTransform trans = new TranslateTransform(position.X, position.Y);
            if (bottom)
            {
                TransformGroup tr = new TransformGroup();
                tr.Children.Add(trans);
                tr.Children.Add(new RotateTransform(90, position.X, position.Y));
                finalTransform = tr;
            }
            else
            {
                finalTransform = trans;
            }

            Geometry newGeom;
            if (labelDict.ContainsKey(value))
            {
                newGeom = labelDict[value];
            }
            else
            {
                newGeom = makeText(value.ToString(), new Point(0, 0));
                labelDict.Add(value, newGeom);
            }

            newGeom.Transform = finalTransform;
            Drawing newDrawing = new GeometryDrawing(Brushes.Black, null, newGeom); 
            axisLabels.Children.Add(newDrawing);
            
        }
    }
}
