using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizerControl
{
    public interface IEngineWithRemoval : IEngine
    {
        List<IProjectile> ParticlesToAdd();
        List<IProjectile> ParticlesToRemove();
    }
}

