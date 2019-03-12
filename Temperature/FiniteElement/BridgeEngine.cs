using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace FiniteElement
{
    public class BridgeEngine
    {
        private Bridge bridge;

        private Vector3D location = new Vector3D(0, 0, 0);
        private Vector3D force = new Vector3D(0, 0, 0);
        private double area = 0;

        public BridgeEngine(Bridge bridge)
        {
            this.bridge = bridge;
            structure = bridge.structure;
        }

        private double time = 0;

        public double Time { get { return time; } }

        private ParticleStructure structure;

        public List<SimpleProjectile> GetProjectiles()
        {
            return structure.Projectiles;
        }

        public void Tick(double dt)
        {
            time += dt;
            bridge.AddForce(location, force, area);
            structure.Update(dt);
        }

        public void SetForce(Vector3D location, Vector3D force, double area)
        {
            this.location = location;
            this.force = force;
            this.area = area;
        }

        public List<Tuple<int, int>> ConnectorsToRemove()
        {
            return structure.ConnectorsToBreak();
        }

        public List<Tuple<int, int>> Connectors
        {
            get
            {
                return structure.Connectors;
            }
        }
    }
}
