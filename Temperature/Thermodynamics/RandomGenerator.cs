using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Utility;

namespace Thermodynamics
{
    abstract public class RandomGenerator
    {
        private double mass;
        private Color color;
        private double meanFreeTime;
        private double xStart;
        private double xEnd;

        public RandomGenerator(double mass, Color color, double meanFreeTime, double xStart, double xEnd)
        {
            this.mass = mass;
            this.color = color;
            this.meanFreeTime = meanFreeTime;
            this.xStart = xStart;
            this.xEnd = xEnd;
        }

        static protected Random random { get; } = new Random();

        /**
         * @param grid The container of Particles
         * @return A randomly generated position vector within a ParticleContainer
         */
        protected Vector randomPosition(ParticleContainer grid)
        {
            double x = random.NextDouble() * (xEnd - xStart) + xStart;
            double y = random.NextDouble() * grid.Size.Y;
            double z = random.NextDouble() * grid.Size.Z;

            return new Vector(x, y, z);
        }

        abstract protected double getSpeed();

        public Particle GetRandomParticle(ParticleContainer container)
        {
            Vector velocity = Vector.RandomDirection(getSpeed(), random);

            return new RandomWalkParticle(randomPosition(container), velocity, mass, color, meanFreeTime);
        }

        public Particle GetCubeParticle(Vector position)
        {
            Vector velocity = Vector.RandomDirection(getSpeed(), random);

            return new RandomWalkParticle(position, velocity, mass, color, meanFreeTime);
        }
    }
}
