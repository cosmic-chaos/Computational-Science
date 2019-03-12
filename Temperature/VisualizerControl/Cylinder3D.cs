using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VisualizerControl
{
    class Cylinder3D
    {
        private int nSegments;
        private double radius;
        private double height;
        public Point3DCollection Points { get; set; } = new Point3DCollection();
        public Int32Collection TriangleIndices { get; set; } = new Int32Collection();

        public double Radius
        {
            get { return radius; }
            set { radius = value; CalculateGeometry(); }
        }

        public double Height
        {
            get { return height; }
            set { height = value; CalculateGeometry(); }
        }

        public int Separators
        {
            get { return nSegments; }
            set { nSegments = value; CalculateGeometry(); }
        }

        public Cylinder3D(double radius, double height, int nSegments = 16)
        {
            this.nSegments = nSegments;
            this.radius = radius;
            this.height = height;
            CalculateGeometry();
        }

        private void CalculateGeometry()
        {
            double phiSeg = 2 * Math.PI / nSegments;

            // The center of the top and bottom
            Points.Add(new Point3D(0, 0, radius / 2)); 
            Points.Add(new Point3D(0, 0, -radius / 2)); 

            // Points along the edge
            for (double iphi = 0; iphi < 2 * Math.PI; iphi += phiSeg)
            {
                double x = radius * Math.Cos(iphi);
                double y = radius * Math.Sin(iphi);
                double z = height / 2;
                Points.Add(new Point3D(x, y, z)); // Top ring is all even numbers, starting at 2 to 2 * nSegments
                Points.Add(new Point3D(x, y, -z)); // Bottom ring is all odd numbers, starting at 3 to 2 * nSegments + 1
            }

            const int topCenterIndex = 0;
            const int bottomCenterIndex = 1;
            const int firstTopRing = 2;
            int lastTopRing = 2 * nSegments;

            // Top
            for (int index = firstTopRing; index <= lastTopRing; index += 2)
            {
                int nextIndex = index == lastTopRing ? firstTopRing : index + 2;

                // Top circle
                TriangleIndices.Add(index);
                TriangleIndices.Add(nextIndex);
                TriangleIndices.Add(topCenterIndex);

                // Bottom circle - opposite orientation to face out
                TriangleIndices.Add(index + 1);
                TriangleIndices.Add(bottomCenterIndex);
                TriangleIndices.Add(nextIndex + 1);

                // Sides
                TriangleIndices.Add(index);
                TriangleIndices.Add(index + 1);
                TriangleIndices.Add(nextIndex);

                TriangleIndices.Add(index + 1);
                TriangleIndices.Add(nextIndex + 1);
                TriangleIndices.Add(nextIndex);
            }
        }
    }
}
