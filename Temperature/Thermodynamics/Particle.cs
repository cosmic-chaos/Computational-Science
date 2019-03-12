using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Utility;

namespace Thermodynamics
{
    public class Particle
    {
        public Vector Position { get; set; }
        public Vector Velocity { get; set; }
        public double Mass { get; set; }
        public Color Color { get; set; }

        private Vector acceleration = new Vector(0, 0, 0);
        public Vector Acceleration { get { return acceleration; } }
        private Vector forces = new Vector(0, 0, 0);
        public Vector Forces { get { return forces; } }

        public Particle(Vector position, Vector velocity, double mass, Color color)
        {
            this.Position = position;
            this.Velocity = velocity;
            this.Mass = mass;
            this.Color = color;
        }
      

        public void AddForce(Vector force)
        {
            forces += force;
        }

        private void setAccel()
        {
            acceleration = Forces / Mass;
        }

        private void accelerate(double timeIncrement)
        {
            Velocity += Acceleration * timeIncrement;
        }

        private void advancePosition(double timeIncrement)
        {
            Position += Velocity * timeIncrement;
        }

        virtual public void Update(double timeIncrement)
        {
            setAccel();
            accelerate(timeIncrement);
            advancePosition(timeIncrement);
            forces = new Vector(0, 0, 0);
        }

        //virtual public void Update(double timeIncrement)
        //{
        //    Position += Velocity * timeIncrement;
        //}
    }
}
