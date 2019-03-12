using System;

namespace Utility
{
    /// <summary>
    /// A simple three-dimensional vector of doubles
    /// </summary>
    public struct Vector
    {

        /// <summary>
        /// Cartesian variables are the default accessors
        /// </summary>
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        /// <summary>
        /// Cartesian constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="rhs">The vector to copy from</param>
        public Vector(Vector rhs)
        {
            X = rhs.X;
            Y = rhs.Y;
            Z = rhs.Z;
        }

        public override bool Equals(object obj)
        {
            Vector vec = (Vector)obj;
            return X == vec.X && Y == vec.Y && Z == vec.Z; 
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        override public string ToString()
        {
            return X + "\t" + Y + '\t' + Z + '\t' + Magnitude();
        }

        static public Vector operator +(Vector one, Vector two)
        {
            return new Vector(one.X + two.X, one.Y + two.Y, one.Z + two.Z);
        }

        static public Vector operator -(Vector vec)
        {
            return vec * -1;
        }

        static public Vector operator -(Vector one, Vector two)
        {
            return one + (-two);
        }

        static public Vector operator *(Vector vec, double scale)
        {
            return new Vector(vec.X * scale, vec.Y * scale, vec.Z * scale);
        }

        static public Vector operator *(double scale, Vector vec)
        {
            return vec * scale;
        }

        static public Vector operator /(Vector vec, double divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }
            else
            {
                return vec * (1 / divisor);
            }
        }

        static public bool operator ==(Vector one, Vector two)
        {
            return one.X == two.X && one.Y == two.Y && one.Z == two.Z;
        }

        static public bool operator !=(Vector one, Vector two)
        {
            return !(one == two);
        }

        /// <summary>
        /// The magnitude of the vector squared, which is much faster to calculate.
        /// </summary>
        /// <returns>The magnitude of the vector squared</returns>
        public double Mag2()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// The magnitude of the vector
        /// </summary>
        /// <returns>The magnitude of the vector</returns>
        public double Magnitude()
        {
            return Math.Sqrt(Mag2());
        }

        /// <summary>
        /// The azimuthal angle is the angle from the x axis in the x-y plane (phi in physics, theta in math)
        /// </summary>
        /// <returns>The angle from the x axis in the x-y plane</returns>
        public double Azimuthal()
        {
            return Math.Atan2(Y, X);
        }


        /// <summary>
        /// The polar angle of the vector is the angle of declination from the z axis (theta in physics, phi in math)
        /// </summary>
        /// <returns>The angle of declination from the positive z axis</returns>
        public double Polar()
        {
            return Math.Atan(Math.Sqrt(X * X + Y * Y) / Z);
        }


        /// <summary>
        /// Gives the dot product (Euclidean scalar product) of two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Dot(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        /// <summary>
        /// Gives the cross product of two vectors.  (Technically an axial vector, not a vector,
        /// or even better a rank-2 asymmetrical tensor, but we don't need to worry about that in this class.)
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector Cross(Vector v1, Vector v2)
        {
            return new Vector(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        }

        /// <summary>
        /// The angle between two vectors.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double AngleBetween(Vector v1, Vector v2)
        {
            return Math.Acos(Vector.Dot(v1, v2) / (v1.Magnitude() * v2.Magnitude()));
        }

        /// <summary>
        /// A convenient null vector
        /// </summary>
        /// <returns>A vector of magnitude zero</returns>
        public static Vector NullVector()
        {
            return new Vector(0, 0, 0);
        }

        /// <summary>
        /// A normalized version of a given vector
        /// </summary>
        /// <returns>A unit vector that points in the same direction as the input vector</returns>
        public Vector UnitVector()
        {
            double mag2 = Mag2();
            if (mag2 == 0)
            {
                return NullVector();
            }
            else
            {
                return this / Math.Sqrt(mag2);
            }
        }

        /// <summary>
        /// Takes the place of a spherical constructor
        /// </summary>
        /// <param name="r">The radius or magnitude of the vector</param>
        /// <param name="phi">The azimuthal angle, in radians, using the physics convention (this would be theta in math)
        /// It goes from 0 to 2 pi and gives the angle in x-y plane.</param>
        /// <param name="theta">The polar angle, in radians, using the physics convention (this would be phi in math)
        /// It goes from 0 to pi and gives the angle from the positive z axis.</param>
        /// <returns>A vector matching these three parameters</returns>
        public static Vector SphericalVector(double r, double phi, double theta)
        {
            double x = r * Math.Cos(phi) * Math.Sin(theta);
            double y = r * Math.Sin(phi) * Math.Sin(theta);
            double z = r * Math.Cos(theta);

            return new Vector(x, y, z);
        }

        /// <summary>
        /// Creates a vector of a given magnitude which points in a random direction, evenly distributed across the surface of the sphere
        /// </summary>
        /// <param name="magnitude">The magnitude of the random vector</param>
        /// <param name="generator">The Random object used to generate the random vector.  This is defined externally so that users can control the random behavior.</param>
        /// <returns>A vector of the given magnitude that points in a randomly chosen direction</returns>
        public static Vector RandomDirection(double magnitude, Random generator)
        {
            // This algorithm comes from Wolfram Mathworld, "Sphere Point Picking"
            double phi = generator.NextDouble() * 2 * Math.PI;
            double theta = Math.Acos(2 * generator.NextDouble() - 1);

            return Vector.SphericalVector(magnitude, phi, theta);
        }

        /// <summary>
        /// Reflects the vector across a given axis
        /// </summary>
        /// <param name="origin">The origin of the axis about which the vector is to be reflected</param>
        /// <param name="axis">The end point of the axis of reflection that goes through the origin</param>
        /// <returns>A vector that has been reflected about the given axis</returns>
        public Vector ReflectParticle(Vector origin, Vector axis)
        {
            Vector unitN = axis.UnitVector();

            // From Wolfram MathWorld, "Reflection:
            return -this + 2 * origin + 2 * unitN * Dot(this - origin, unitN);
        }

    }
}
