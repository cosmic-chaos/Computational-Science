using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace FiniteElement
{
    public class SimpleProjectile
    {
        public Vector3D Position { get; set; }
        public Vector3D Velocity { get; set; }
        private Vector3D acceleration = new Vector3D(0, 0, 0);
        public Vector3D Acceleration { get { return acceleration; } }
        private Vector3D forces = new Vector3D(0, 0, 0);
        public Vector3D Forces { get { return forces; } }

        private double mass;
        public double Mass
        {
            get
            {
                return mass;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("mass", "Mass cannot be zero or negative!");
                }
                else
                {
                    mass = value;
                }
            }
        }

        public SimpleProjectile(Vector3D position, Vector3D velocity, double mass)
        {
            Position = position;
            Velocity = velocity;
            Mass = mass;
        }

        public void AddForce(Vector3D force)
        {
            forces += force;
        }

        private void setAccel()
        {
            acceleration = Forces / mass;
        }

        private void accelerate(double timeIncrement)
        {
            Velocity += Acceleration * timeIncrement;
        }

        private void advancePosition(double timeIncrement)
        {
            Position += Velocity * timeIncrement;
        }

        public void Update(double timeIncrement)
        {
            setAccel();
            accelerate(timeIncrement);
            advancePosition(timeIncrement);
            forces = new Vector3D(0, 0, 0);
        }
    }
}
