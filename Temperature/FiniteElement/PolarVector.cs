using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace FiniteElement
{
    class PolarVector
    {
        public double Radius { get; set; }
        public double Angle1 { get; set; }
        public double Angle2 { get; set; }

        //Doesn't work I bet.
        public PolarVector(Vector3D vector)
        {
            Radius = vector.Length;
            if (vector.Z >= 0 && vector.X >= 0)
            {
                Angle1 = Math.Atan(vector.Z / vector.X);
            }
            else if (vector.X < 0 && vector.Z >= 0)
            {
                Angle1 = Math.Atan(vector.Z / vector.X) + Math.PI;
            }
            else if (vector.X < 0 && vector.Z < 0)
            {
                Angle1 = Math.Atan(vector.Z / vector.X) + Math.PI;
            }

            if (vector.Y >= 0 && vector.X >= 0)
            {
                Angle2 = Math.Atan(vector.Y / vector.X);
            }
            else if (vector.X < 0 && vector.Z >= 0)
            {
                Angle2 = Math.Atan(vector.Y / vector.X) + Math.PI;
            }
            else if (vector.X < 0 && vector.Z < 0)
            {
                Angle2 = Math.Atan(vector.Y / vector.X) + Math.PI;
            }
        }

        public PolarVector(double radius, double angle1, double angle2)
        {
            //Radius is length
            //Angle 1 is the angle between the X and Z axis
            //Angle 2 is the angle between the X and Y axis
            Radius = radius;
            Angle1 = angle1;
            Angle2 = angle2;
        }

        public Vector3D Convert()
        {
            //Equation creds to Dr. Condie
            return new Vector3D(Radius * Math.Sin(Angle2) * Math.Sin(Angle1), Radius * Math.Sin(Angle2) * Math.Cos(Angle1), Radius * Math.Cos(Angle2));
        }
    }
}

