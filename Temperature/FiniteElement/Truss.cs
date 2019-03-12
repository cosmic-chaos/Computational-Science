using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace FiniteElement
{
    public class Truss : ParticleStructure
    {
        public Truss()
        {
            //Make right grounding
            //Make left grounding
            //Make inner rope bridge projectile
            for (int x = 10; x >= 0; x = x - 1)
            {
                var proj = new SimpleProjectile(new Vector3D(x, 0, 4 * x * x + 5), new Vector3D(0, 0, 0), 1);
                AddProjectile(proj);
            }

            for(int x = 10; x >= 0; x = x - 1)
            {
                var proj = new SimpleProjectile(new Vector3D(x, 0, 4 * x * x + 4), new Vector3D(0, 0, 0), 1);
            }
            //Make outer rope bridge projectile
            //Make connectors
        }
    }
}
