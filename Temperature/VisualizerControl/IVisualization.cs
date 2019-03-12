using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace VisualizerControl
{
    public interface IVisualization
    {
        void Initialize();
        void SetCollection(Model3DCollection collection);
        List<Vector3D> GetAllProjectiles();
        bool Update(double newTime, bool trace);
    }
}
