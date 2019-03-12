using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphControl
{
    public struct Range
    { 
        static public Range DefaultRange()
        {
            return new Range(Double.MaxValue, Double.MaxValue, Double.MinValue, Double.MinValue);
        }

        public Range(double minx, double miny, double maxx, double maxy)
        {
            MinX = minx;
            MinY = miny;
            MaxX = maxx;
            MaxY = maxy;
        }

        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }

        public double Width
        {
            get
            {
                return MaxX - MinX;
            }
        }

        public double Height
        {
            get
            {
                return MaxY - MinY;
            }
        }

        public void setMinMaxX(double xVal)
        {
            if (xVal < MinX) { MinX = xVal; }
            if (xVal > MaxX) { MaxX = xVal; }
        }

        public void setMinMaxY(double yVal)
        {
            if (yVal < MinY) { MinY = yVal; }
            if (yVal > MaxY) { MaxY = yVal; }
        }

        public void setMinMax(double xVal, double yVal)
        {
            setMinMaxX(xVal);
            setMinMaxY(yVal);
        }

        public void AdjustRange(ref Range other)
        {
            if (MinX < other.MinX) { other.MinX = MinX; }
            if (MinY < other.MinY) { other.MinY = MinY; }
            if (MaxX > other.MaxX) { other.MaxX = MaxX; }
            if (MaxY > other.MaxY) { other.MaxY = MaxY; }
        }

    }
}
