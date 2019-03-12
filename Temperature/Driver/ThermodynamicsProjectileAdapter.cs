using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Thermodynamics;
using VisualizerControl;

namespace Driver
{
    class ThermodynamicsProjectileAdapter : IProjectile
    {
        private Particle part;

        public ThermodynamicsProjectileAdapter(Particle part)
        {
            this.part = part;
        }

        public Color Color
        {
            get
            {
                return part.Color;
            }

            set
            {
                part.Color = value; 
            }
        }

        public double Mass
        {
            get
            {
                return part.Mass;
            }

            set
            {
               part.Mass = value;
            }
        }

        public Vector3D Acceleration 
        {
            get { return new Vector3D(0, 0, 0); }
        }

        public Vector3D Position
        {
            get { return convertToVector3D(part.Position); }
        }

        public Vector3D Velocity
        {
            get { return convertToVector3D(part.Velocity); }
        }

        static private Vector3D convertToVector3D(Utility.Vector vec)
        {
            return new Vector3D(vec.X, vec.Y, vec.Z);
        }

        static public double GetSpeed(IProjectile proj)
        {
            return proj.Velocity.Length;
        }
    }
}
