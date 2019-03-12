using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VisualizerControl
{
    class Sphere3D
    {
        private int nSegments;
        private double radius;
        public Point3DCollection Points { get; set; } = new Point3DCollection();
        public Int32Collection TriangleIndices { get; set; } = new Int32Collection();

        public double Radius
        {
            get { return radius; }
            set { radius = value; CalculateGeometry(); }
        }

        public int Separators
        {
            get { return nSegments; }
            set { nSegments = value; CalculateGeometry(); }
        }

        public Sphere3D(double radius, int nSegments = 16)
        {
            this.nSegments = nSegments;
            this.radius = radius;
            CalculateGeometry();
        }

        private void CalculateGeometry()
        {
            double thetaSeg = Math.PI / nSegments;
            double phiSeg = 2 * Math.PI / nSegments;

            Points.Add(new Point3D(0, 0, radius));

            for (double itheta = thetaSeg; itheta < Math.PI; itheta += thetaSeg)
            {
                for (double iphi = 0; iphi < 2 * Math.PI; iphi += phiSeg)
                {
                    double x = radius * Math.Cos(iphi) * Math.Sin(itheta);
                    double y = radius * Math.Sin(iphi) * Math.Sin(itheta);
                    double z = radius * Math.Cos(itheta);
                    Points.Add(new Point3D(x, y, z));
                }
            }

            Points.Add(new Point3D(0, 0, -radius));

            // Top ring
            for (int index = 1; index <= nSegments; ++index)
            {
                TriangleIndices.Add(0);
                TriangleIndices.Add(index);
                TriangleIndices.Add(index == nSegments ? 1 : index + 1);
            }

            // Middle section
            int maxTheta = nSegments * (nSegments - 2); // Index of the last point of the second-to-last theta ring
            for (int thetaIndex = 1; thetaIndex <= maxTheta; thetaIndex += nSegments)
            {
                for (int phiIndex = 0; phiIndex < nSegments; ++phiIndex)
                {
                    int thisPoint = thetaIndex + phiIndex;
                    int nextPhi = phiIndex == nSegments - 1 ? thetaIndex : thisPoint + 1;
                    int nextTheta = thisPoint + nSegments;
                    int nextThetaPhi = nextPhi + nSegments;

                    TriangleIndices.Add(thisPoint);
                    TriangleIndices.Add(nextTheta);
                    TriangleIndices.Add(nextThetaPhi);

                    TriangleIndices.Add(thisPoint);
                    TriangleIndices.Add(nextThetaPhi);
                    TriangleIndices.Add(nextPhi);
                }
            }

            // Bottom ring
            int lastThetaRing = maxTheta + 1;
            int lastIndex = nSegments * (nSegments - 1) + 1;
            for (int index = lastThetaRing; index < lastIndex; ++index)
            {
                TriangleIndices.Add(lastIndex);
                TriangleIndices.Add(index == lastIndex - 1 ? lastThetaRing : index + 1);
                TriangleIndices.Add(index);
            }
        }
    }
}
