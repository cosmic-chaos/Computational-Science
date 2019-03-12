using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GraphControl
{
    class Legend
    {
        private DrawingGroup drawing = new DrawingGroup();
        public Drawing Drawing { get { return drawing; } }
        public Transform Transform
        {
            get { return drawing.Transform; }
            set { drawing.Transform = value; }
        }

        private IList<Tuple<string, Color>> list = new List<Tuple<string, Color>>();

        public void AddTimeline(Timeline timeline)
        {

            list.Add(new Tuple<string, Color>(timeline.Name, timeline.Color));
            update();
        }

        private void update()
        {
            drawing.Children.Clear();

            double currentY = 0;
            foreach (var tuple in list)
            {
                Brush myBrush = new SolidColorBrush(tuple.Item2);
                FormattedText thisText = new FormattedText(tuple.Item1, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface("Arial"),
        10, myBrush);
                double boxSize = thisText.Height;
                Geometry geo = new RectangleGeometry(new Rect(0, currentY, boxSize, boxSize));
                drawing.Children.Add(new GeometryDrawing(myBrush, null, geo));
                Geometry textGeo = thisText.BuildGeometry(new Point(1.5 * boxSize, currentY));
                drawing.Children.Add(new GeometryDrawing(myBrush, null, textGeo));
                currentY += boxSize;
            }
        }
    }
}
