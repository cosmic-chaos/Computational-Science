using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphControl
{
    public class Timeline
    {
        public string Name { get; set; }
        private Color color;
        public Color Color { get { return color; } }

        private PolyLineSegment line = new PolyLineSegment();
        private PathGeometry geom = new PathGeometry();
        private GeometryDrawing drawing = new GeometryDrawing();
        public Transform Transform
        {
            get
            {
                return geom.Transform;
            }
            set
            {
                geom.Transform = value;
            }
        }

        public delegate double GetValue();
        private GetValue delegateX;
        private GetValue delegateY;

        public double Thickness { get; set; } = 3;

        public Timeline(string name, GetValue valx, GetValue valy, Color color)
        {
            Name = name;
            delegateX = valx;
            delegateY = valy;
            this.color = color;

            drawing.Geometry = geom;
            PathFigure path = new PathFigure();
            geom.Figures.Add(path);
            path.Segments.Add(line);
            geom.Transform = Transform;

            drawing.Pen = new Pen(new SolidColorBrush(color), Thickness);
        }

        public GeometryDrawing getDrawing()
        {
            return drawing;

        }

        private bool justStarted = true;
        private void addPoint(double xVal, double yVal)
        {
            if (justStarted)
            {
                geom.Figures[0].StartPoint = new Point(xVal, yVal);
                justStarted = false;
            }
            line.Points.Add(new Point(xVal, yVal));

            range.setMinMax(xVal: xVal, yVal: yVal);

            if (line.Points.Count > MaximumPoints)
            {
                deletePoint();
            }
        }

        static public double MaximumPoints { get; set; } = 1000;

        private void deletePoint()
        {
            Point toRemove = line.Points[0];

            line.Points.RemoveAt(0);
            if (toRemove.X == range.MinX)
            {
                range.MinX = line.Points.Min(x => x.X);
            }
            if (toRemove.X == range.MaxX)
            {
                range.MaxX = line.Points.Max(x => x.X);
            }

            if (toRemove.Y == range.MinY)
            {
                range.MinY = line.Points.Min(x => x.Y);
            }
            if (toRemove.Y == range.MaxY)
            {
                range.MaxY = line.Points.Max(x => x.Y);
            }
            geom.Figures[0].StartPoint = toRemove;

        }

        public void Update()
        {
            addPoint(delegateX(), delegateY());
        }

        private Range range = Range.DefaultRange();
        public Range Range { get { return range; } }
    }
}
