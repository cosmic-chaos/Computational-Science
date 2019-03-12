using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizerControl
{
    public class SingleParticleWithRemovalVisualization : SingleParticleVisualization
    {
        private IEngineWithRemoval myEngine { get { return (IEngineWithRemoval)engine; } }

        public SingleParticleWithRemovalVisualization(IEngineWithRemoval engine) :
            base(engine)
        { }

        public override bool Update(double newTime, bool trace)
        {
            foreach (var projectile in myEngine.ParticlesToRemove())
            {
                removeProjectile(projectile);
            }

            foreach (var projectile in myEngine.ParticlesToAdd())
            {
                addProjectile(projectile);
            }

            return base.Update(newTime, trace);
        }


    }
}
