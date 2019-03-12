using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteElement
{ 
    public class StructureEngine
    {
        private double time = 0;

        public double Time { get { return time; } }

        public ParticleStructure Structure { get; set; }

        public void Tick(double dt)
        {
            time += dt;
            Structure.Update(dt);
        }
    }
}
