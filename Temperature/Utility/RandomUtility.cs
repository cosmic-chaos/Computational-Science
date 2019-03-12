using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class RandomUtility
    {
        // Box-Muller transform, copied from stackoverflow.com/questions/218060/random-gaussian-variables
        static public double RandomGaussian(double mean, double sd, Random random)
        {
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();
            double ranNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + sd * ranNormal;
        }
    }
}
