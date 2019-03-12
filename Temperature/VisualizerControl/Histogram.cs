using GraphControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VisualizerControl
{
    public class Histogram : TransformingObject, IGraphInterface
    {
        public delegate double PartFunc(IProjectile proj);

        private IEngine engine;
        private PartFunc func;
        private IList<HistoBin> histo = new List<HistoBin>();
        private IList<RectangleGeometry> geoList = new List<RectangleGeometry>();

        private class HistoBin
        {
            public HistoBin(double lowEdge)
            {
                this.LowEdge = lowEdge;
            }
            public int Num { get; set; } = 0;
            public double LowEdge { get; set; }
        }

        public Histogram(PartFunc func, IEngine engine, int nBins, Color color, string xTitle) :
            base(xTitle, "Frequency")
        {
            NBins = nBins;
            this.func = func;
            this.engine = engine;

            Brush mybrush = new SolidColorBrush(color);
            for (int i = 0; i < nBins; ++i)
            {
                var geo = new RectangleGeometry();
                geo.Transform = Transform;
                geoList.Add(geo);

                var geoDraw = new GeometryDrawing(mybrush, null, geo);
                drawing.Children.Add(geoDraw);
                
            }

        }

        public int NBins { get; }

        public void UpdateAll()
        {
            List<double> list = new List<double>();

            foreach (var particle in engine.GetProjectiles())
            {
                double value = func(particle);
                list.Add(value);
            }

            list.Sort();

            histo.Clear();

            double range = list.Last() - list.First();
            double binSize = range / NBins;

            int currentBin = -1;
            double currentLow = list.First() - binSize;

            foreach (var number in list)
            {
                if (number < currentLow + binSize || histo.Count == NBins)
                {
                    ++histo[currentBin].Num;
                }
                else
                {
                    histo.Add(new HistoBin(currentLow += binSize));
                    ++currentBin;
                }
            }

            for (int i = 0; i < histo.Count; ++i)
            {
                geoList[i].Rect = new System.Windows.Rect(histo[i].LowEdge, 0, binSize, histo[i].Num);
            }
        }

        public void UpdateTransform(double width, double height, double widthOffset, double heightOffset)
        {
            if (histo.Count == 0)
            {
                return;
            }

            Range range = Range.DefaultRange();

            range.MinX = histo[0].LowEdge;
            range.MinY = 0;
            range.MaxX = histo.Last().LowEdge;
            range.MaxY = histo.Max((x) => x.Num);

            UpdateMatrix(width, height, widthOffset, heightOffset, range);

            UpdateAxes(width, height, widthOffset, heightOffset, range);
        }
    }
}
