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
    public class GraphUnderlying : TransformingObject, IGraphInterface
    {
        private List<Timeline> timelines = new List<Timeline>();

        private Legend legend = new Legend();

        public GraphUnderlying(string xTitle, string yTitle) :
            base(xTitle, yTitle)
        {
            drawing.Children.Add(legend.Drawing);
        }

        public void AddTimeline(Timeline timeline)
        {
            timelines.Add(timeline);
            drawing.Children.Insert(0, timeline.getDrawing());
            timeline.Transform = Transform;
            legend.AddTimeline(timeline);
        }

        public void UpdateAll()
        {
            foreach (var timeline in timelines)
            {
                timeline.Update();
            }
        }

        public void UpdateTransform(double width, double height, double widthOffset, double heightOffset)
        {
            Range range = Range.DefaultRange();

            foreach (var timeline in timelines)
            {
                timeline.Range.AdjustRange(ref range);
            }

            if (range.MinX == range.MaxX)
            {
                range.MinX -= 1;
                range.MaxX += 1;
            }
            if (range.MinY == range.MaxY)
            {
                range.MinY -= 1;
                range.MaxY += 1;
            }

            UpdateMatrix(width, height, widthOffset, heightOffset, range);

            UpdateAxes(width, height, widthOffset, heightOffset, range);
            setLegend(width, height, widthOffset, heightOffset);
        }

        private const double legendMarginFactor = .1;

        private void setLegend(double width, double height, double widthOffset, double heightOffset)
        {
            double rhs = width * (1 - legendMarginFactor) + widthOffset;
            double lhs = rhs - legend.Drawing.Bounds.Width;
            double top = legendMarginFactor * height + heightOffset;
            legend.Transform = new TranslateTransform(lhs, top);
        }

    }
}

