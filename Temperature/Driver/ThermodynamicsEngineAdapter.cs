using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thermodynamics;
using VisualizerControl;

namespace Driver
{
    class ThermodynamicsEngineAdapter : IEngineWithConnectors
    {
        private ParticleContainer cont;
        private List<IProjectile> particles = new List<IProjectile>();

        public double Time { get; private set; } = 0;

        public ThermodynamicsEngineAdapter(ParticleContainer cont)
        {
            this.cont = cont;
            foreach (var part in cont.Projectiles)
            {
                particles.Add(new ThermodynamicsProjectileAdapter(part));
            }
        }

        public IList<IProjectile> GetProjectiles()
        {
            return particles;
        }

        public void Tick(double currentTime)
        {
            cont.AdvanceParticles(currentTime - Time);
            Time = currentTime;
        }

        public List<Tuple<int, int>> GetConnectors()
        {
            return cont.Connectors;
        }

        public List<Tuple<int, int>> ConnectorsToRemove()
        {
            return cont.ConnectorsToBreak();
        }
    }
}
