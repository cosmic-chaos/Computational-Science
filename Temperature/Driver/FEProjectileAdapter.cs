using FiniteElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Visualizer
{
    class FEProjectileAdapter : IProjectile
    {
        private SimpleProjectile proj;

        public FEProjectileAdapter(SimpleProjectile proj)
        {
            this.proj = proj;
        }

        public Color Color
        {
            get
            {
                return Colors.IndianRed;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public double Mass
        {
            get
            {
                return proj.Mass;
            }

            set
            {
                proj.Mass = value;
            }
        }

        public Vector3D Acceleration()
        {
            return proj.Acceleration;
        }

        public Vector3D Position()
        {
            return proj.Position;
        }

        public Vector3D Velocity()
        {
            return proj.Velocity;
        }
    }
}
