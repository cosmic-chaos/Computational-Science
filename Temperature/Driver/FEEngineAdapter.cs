using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiniteElement;

namespace Visualizer
{
    class FEEngineAdapter : IEngine
    {
        private StructureEngine engine;

        public FEEngineAdapter(StructureEngine engine)
        {
            this.engine = engine;
        }

        public double Time
        {
            get
            {
                return engine.Time;
            }
        }

        public IList<IProjectile> GetProjectiles()
        {
            var list = new List<IProjectile>();
            foreach (var proj in engine.Structure.Projectiles)
            {
                list.Add(new FEProjectileAdapter(proj));
            }
            return list;
        }

        public IList<IProjectile> ProjectilesToAdd()
        {
            return new List<IProjectile>();
        }

        public IList<IProjectile> ProjectilesToRemove()
        {
            return new List<IProjectile>();
        }

        public void Tick(double newTime)
        {
            engine.Tick(newTime - Time);
        }
    }
}
