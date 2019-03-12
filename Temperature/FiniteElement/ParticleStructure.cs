using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace FiniteElement
{
    public class ParticleStructure
    {
        private List<SimpleProjectile> projectiles = new List<SimpleProjectile>();
        private List<Connector> connectors = new List<Connector>();
        public Vector3D GravitationalField { get; set; } = new Vector3D(0, 0, 0); 
        public double DampingCoefficient { get; set; } = 1; 

        public List<SimpleProjectile> Projectiles { get { return projectiles; } }
        public List<Tuple<int, int>> Connectors
        {
            get
            {
                var list = new List<Tuple<int, int>>();
                foreach (var con in connectors)
                {
                    list.Add(getIndices(con.Projectile1, con.Projectile2));
                }
                return list;
            }
        }
        private Tuple<int, int> getIndices(SimpleProjectile proj1, SimpleProjectile proj2)
        {
            int index1 = projectiles.IndexOf(proj1);
            int index2 = projectiles.IndexOf(proj2);
            return new Tuple<int, int>(index1, index2);
        }
        private List<Connector> connectorsToBreak = new List<Connector>();
        public List<Tuple<int, int>> ConnectorsToBreak()
        {
            var response = new List<Tuple<int, int>>();
            foreach (var con in connectorsToBreak)
            {
                response.Add(getIndices(con.Projectile1, con.Projectile2));
            }
            return response;
        }

        public bool GroundForceOn { get; set; } = false;
        public bool LockGround { get; set; } = false;
        public double MinimumDistance
        {
            get { return Connector.MinimumDistance; }
            set { Connector.MinimumDistance = value; }
        }

        private class Connector
        {
            public Connector(SimpleProjectile proj1, SimpleProjectile proj2, double springConstant, double breakingForce)
            {
                Projectile1 = proj1;
                Projectile2 = proj2;

                unstretchedLength = (proj1.Position - proj2.Position).Length;
                this.springConstant = springConstant;
                this.breakingForce = breakingForce;
            }

            static public double MinimumDistance { get; set; } = .1;

            public SimpleProjectile Projectile1 { get; set; }
            public SimpleProjectile Projectile2 { get; set; }

            private double unstretchedLength;
            private double springConstant;
            private double breakingForce;

            public bool AddForces(double damping, double dt)  // False means that the spring broke
            {
                var displacement = Projectile1.Position - Projectile2.Position;
                double length = displacement.Length;
                if (length < MinimumDistance)
                {
                    Projectile1.Velocity = -Projectile1.Velocity;
                    Projectile2.Velocity = -Projectile2.Velocity;
                }
                else
                {
                    double forceMag = (length - unstretchedLength) * springConstant;
                    if (Math.Abs(length / unstretchedLength) > breakingForce)
                    {
                        return false;
                    }
                    var unitD = displacement;
                    unitD.Normalize();
                    var force1on2 = forceMag * unitD;
                    var force2on1 = -forceMag * unitD;

                    Projectile1.AddForce(force2on1);
                    Projectile2.AddForce(force1on2);

                    Projectile1.Velocity *= Math.Pow(damping, dt);
                    Projectile2.Velocity *= Math.Pow(damping, dt);
                }
                return true;
            }
        }

        public void AddProjectile(SimpleProjectile proj)
        {
            projectiles.Add(proj);
        }

        public void AddConnector(SimpleProjectile proj1, SimpleProjectile proj2, double springConstant, double breakingForce = Double.MaxValue)
        {
            if (proj1 == proj2)
            {
                return;
            }

            foreach (var con in connectors)
            {
                if ((con.Projectile1 == proj1 && con.Projectile2 == proj2) || (con.Projectile1 == proj2 && con.Projectile2 == proj1))
                {
                    return; // This returns without error because I suspect people would like that behavior
                }
            }
            connectors.Add(new Connector(proj1, proj2, springConstant, breakingForce));
        }

        private void addGroundForce(SimpleProjectile proj, double timeIncrement)
        {
            if (LockGround)
            {
                proj.Velocity = new Vector3D(0, 0, 0);
                proj.Position = new Vector3D(proj.Position.X, proj.Position.Y, 0);
                proj.AddForce(-proj.Forces);
            }
            else
            {
                proj.Velocity = new Vector3D(proj.Velocity.X, proj.Velocity.Y, 0);
                proj.Position = new Vector3D(proj.Position.X, proj.Position.Y, 0);
                double forceZ = proj.Forces.Z;
                if (forceZ < 0)
                {
                    proj.AddForce(new Vector3D(0, 0, -forceZ));
                }
            }
        }

        public void Update(double timeIncrement)
        {
            // Break broken connectors
            foreach (var connect in connectorsToBreak)
            {
                connectors.Remove(connect);
            }

            // Add connector forces and break connectors
            foreach (var connect in connectors)
            {
                bool stillThere = connect.AddForces(DampingCoefficient, timeIncrement);
                if (!stillThere)
                {
                    connectorsToBreak.Add(connect);
                }
            }

            // Add global and ground forces
            foreach (var proj in projectiles)
            {
                proj.AddForce(GravitationalField * proj.Mass);
                if (GroundForceOn && proj.Position.Z <= 0)
                {
                    addGroundForce(proj, timeIncrement);
                }
            }

            // Move all projectiles
            foreach (var proj in projectiles)
            {
                proj.Update(timeIncrement);
//                proj.Velocity *= DampingCoefficient;
            }
        }
    }
}
