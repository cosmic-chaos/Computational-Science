using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FiniteElement
{
    abstract public class Bridge
    {
        private const int maxParticles = 1000;
        private const double maxMass = 1000;
        private const double minWidth = 10; // No particles on the ground between 0 and minWidth allowed
        private const double maxSpringLength = 1;
        private const double springConst = 10000;
        private const double breakingForce = 1.05;
        private const double damping = .9;

        private const double gravitationalFieldStrength = 9.8;

        internal ParticleStructure structure = new ParticleStructure(); // Needs to be visible to the engine

        public Bridge()
        {
            initialize();
            check();
            structure.GravitationalField = new Vector3D(0, 0, -gravitationalFieldStrength);
            structure.GroundForceOn = true;
            structure.MinimumDistance = 0;
            structure.LockGround = true;
            structure.DampingCoefficient = damping;
        }

        abstract protected void initialize();

        abstract public Color Color { get; }

        virtual public void AddForce(Vector3D location, Vector3D force, double area)
        {
            var actingProj = new List<SimpleProjectile>();
            double highestZ = 0;
            foreach(var proj in Projectiles)
            {
                if(proj.Position.X >= 2.5 && proj.Position.X <= 7.5)
                {
                    if(proj.Position.Y >= -2.5 && proj.Position.Y <= 2.5)
                    {
                        if(proj.Position.Z >= highestZ)
                        {
                            highestZ = proj.Position.Z;
                            actingProj.Add(proj);
                        }
                    }
                }
            }

            foreach(var proj in actingProj)
            {
                proj.AddForce(Vector3D.Divide(force, actingProj.Count));
            }
        }

        private void check()
        {
            if (structure.Projectiles.Count > maxParticles)
            {
                throw new IllegalBridgeException("Too many particles: " + structure.Projectiles.Count);
            }
            double totalMass = 0;
            bool somethingInMiddle = false;
            foreach (var proj in structure.Projectiles)
            {
                if (proj.Position.X > 0 && proj.Position.X < minWidth)
                {
                    somethingInMiddle = true;
                }
                if (proj.Position.Z < 0)
                {
                    throw new IllegalBridgeException("Particle with negative z position");
                }
                if (proj.Position.Z == 0 && proj.Position.X > 0 && proj.Position.X < minWidth)
                {
                    throw new IllegalBridgeException("Particle on ground in illegal area");
                }
                totalMass += proj.Mass;
            }
            if (!somethingInMiddle)
            {
                throw new IllegalBridgeException("No particles in the middle of bridge!");
            }
            if (totalMass > maxMass)
            {
                throw new IllegalBridgeException("Too much mass: " + totalMass);
            }
            foreach (var con in structure.Connectors)
            {
                double distance = (structure.Projectiles[con.Item1].Position - structure.Projectiles[con.Item2].Position).Length;
                if (distance > maxSpringLength)
                {
                    throw new IllegalBridgeException("Spring too long: Spring is " + distance + " m long");
                }
            }

        }

        protected void addProjectile(Vector3D position, double mass)
        {
            addProjectile(new SimpleProjectile(position, new Vector3D(0, 0, 0), mass));
        }

        protected void addProjectile(SimpleProjectile proj)
        {
            proj.Velocity = new Vector3D(0, 0, 0);
            structure.AddProjectile(proj);
        }

        protected List<SimpleProjectile> Projectiles { get { return structure.Projectiles; } }

        protected void addConnector(int index1, int index2)
        {
            addConnector(structure.Projectiles[index1], structure.Projectiles[index2]);
        }

        protected void addConnector(Vector3D position1, Vector3D position2)
        {
            addConnector(findParticle(position1), findParticle(position2));
        }

        private SimpleProjectile findParticle(Vector3D position)
        {
            double minDistance = double.MaxValue;
            SimpleProjectile projectile = null;
            foreach (var proj in structure.Projectiles)
            {
                double distance = (position - proj.Position).LengthSquared;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    projectile = proj;
                }
            }
            return projectile;
        }

        protected void addConnector(SimpleProjectile proj1, SimpleProjectile proj2)
        {
            structure.AddConnector(proj1, proj2, springConst, breakingForce);
        }
    }
}
