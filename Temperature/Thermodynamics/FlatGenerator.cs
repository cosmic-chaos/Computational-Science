using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Thermodynamics
{
    public class FlatGenerator : RandomGenerator
    {
        private double min;
        private double max;

        public FlatGenerator(double mass, Color color, double meanFreeTime, double min, double max, double xStart, double xEnd) :
            base(mass, color, meanFreeTime, xStart, xEnd)
        {
            this.min = min;
            this.max = max;
        }

        override protected double getSpeed()
        {
            return random.NextDouble() * (max - min) + min;
        }
    }
}
