using FiniteElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Visualizer
{
    class BridgeEngineAdapter : IEngine
    {
        private BridgeEngine engine;

        public BridgeEngineAdapter(Bridge bridge)
        {
            engine = new BridgeEngine(bridge);
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
            foreach (var proj in engine.GetProjectiles())
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
            var list = new List<IProjectile>();
            foreach (var con in engine.ConnectorsToRemove())
            {
                list.Add(new FEProjectileAdapter(engine.GetProjectiles()[con.Item1]));
                list.Add(new FEProjectileAdapter(engine.GetProjectiles()[con.Item2]));
            }
            return list;
        }

        public void Tick(double newTime)
        {
            engine.Tick(newTime - Time);
        }

        public List<Tuple<int, int>> Connectors
        {
            get
            {
                return engine.Connectors;
            }
        }

        public void SetForce(Vector3D location, Vector3D force, double area)
        {
            engine.SetForce(location, force, area);
        }
    }
}
