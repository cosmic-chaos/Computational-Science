using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Thermodynamics
{
    public class MaxwellBoltzmann : RandomGenerator
    {
        private double mass;
        private double min;
        private double max;
        private double temp;

        public MaxwellBoltzmann(double mass, Color color, double meanFreeTime, double min, double max, double temp, double xStart, double xEnd) : base(mass, color, meanFreeTime, xStart, xEnd)
        {
            this.mass = mass;
            this.min = min;
            this.max = max;
            this.temp = temp;
        }

        protected override double getSpeed()
        {

            while (true)
            {
                double prob = random.NextDouble();
                double s = random.NextDouble() * (max - min) + min;

                if (getProb(s) >= prob)
                {
                    return s;
                }
            }

        }

        double getProb(double speed)
        {
            const double boltzmann = 1.38e-23;
            return 4 * Math.PI * Math.Pow(mass / (2.0 * Math.PI * boltzmann * temp), 1.5) * speed * speed * Math.Pow(Math.E, (-(mass * speed * speed) / (2.0 * boltzmann * temp)));

        }
    }
}
