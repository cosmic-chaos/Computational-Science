using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Utility;

namespace Thermodynamics
{
    class RandomWalkParticle : Particle
    {
        static private Random random = new Random();

        private double meanFreeTime;
        private double timeCounter = 0;

        public RandomWalkParticle(Vector position, Vector velocity, double mass, Color color, double meanFreeTime) :
            base(position, velocity, mass, color)
        {
            this.meanFreeTime = meanFreeTime;
        }

        private void collision()
        {
            Velocity = Vector.RandomDirection(Velocity.Magnitude(), random);
        }

    override public void Update(double timeIncrement)
        {
            timeCounter += timeIncrement;
            if (timeCounter > meanFreeTime)
            {
                collision();
                timeCounter = 0;
            }
            base.Update(timeIncrement);
        }
    }
}
