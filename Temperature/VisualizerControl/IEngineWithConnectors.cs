using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizerControl
{
    public interface IEngineWithConnectors : IEngine
    {
        List<Tuple<int, int>> GetConnectors();
        List<Tuple<int, int>> ConnectorsToRemove();
    }
}
