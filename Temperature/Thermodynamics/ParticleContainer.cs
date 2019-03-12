using FiniteElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Utility;

namespace Thermodynamics
{
    public class ParticleContainer : ParticleStructure
    {
        public Vector Size { get; set; }
        


        public ParticleContainer(double xSize, double ySize, double zSize)
        {
            Size = new Vector(xSize, ySize, zSize);
        }

        public double GetTemperature()
        {
            double kinetic = 0;

            foreach (Particle p in Projectiles)
            {
                kinetic += .5 * p.Mass * p.Velocity.Magnitude() * p.Velocity.Magnitude();
            }

            const double boltzmann = 1.38e-23;

            return kinetic / (Projectiles.Count * 1.5 * boltzmann);
        }

        public Vector3D GetTemperatureVec()
        {
            double kinetic1 = 0;
            double kinetic2 = 0;


            foreach (Particle p in Projectiles)
            {
                if (p.Position.X >= 0 && p.Position.X <= Size.X / 2)
                {
                    kinetic1 += .5 * p.Mass * p.Velocity.Magnitude() * p.Velocity.Magnitude();
                }
                else if (p.Position.X >= Size.X / 2 && p.Position.X <= Size.X)
                {
                    kinetic2 += .5 * p.Mass * p.Velocity.Magnitude() * p.Velocity.Magnitude();
                }
            }

            const double boltzmann = 1.38e-23;

            double temp1 = kinetic1 / (Projectiles.Count * 1.5 * boltzmann);
            double temp2 = kinetic2 / (Projectiles.Count * 1.5 * boltzmann);
            return new Vector3D(temp1, temp2, Math.Abs(temp1 - temp2));
        }

        public double GetTemperatureDiff()
        {
            double kinetic1 = 0;
            double kinetic2 = 0;


            foreach (Particle p in Projectiles)
            {
                if (p.Position.X >= 0 && p.Position.X <= Size.X / 2)
                {
                    kinetic1 += .5 * p.Mass * p.Velocity.Magnitude() * p.Velocity.Magnitude();
                }
                else if (p.Position.X >= Size.X / 2 && p.Position.X <= Size.X)
                {
                    kinetic2 += .5 * p.Mass * p.Velocity.Magnitude() * p.Velocity.Magnitude();
                }
            }

            const double boltzmann = 1.38e-23;

            double temp1 = kinetic1 / (Projectiles.Count * 1.5 * boltzmann);
            double temp2 = kinetic2 / (Projectiles.Count * 1.5 * boltzmann);
            return Math.Abs(temp1 - temp2);
        }

        public void addParticle(Particle part)
        {
            Projectiles.Add(part);
        }

        public void AddRandomParticles(RandomGenerator generator, int number)
        {
            for (int i = 0; i < number; ++i)
            {
                addParticle(generator.GetRandomParticle(this));
            }
        }

        public void AddCube(RandomGenerator generator, int particles)
        {
            int particlesPerSide = (int)Math.Ceiling(Math.Pow(particles, 1.0 / 3.0));
            //space between particles
            double space = Size.X / particlesPerSide;


            for (double x = 0; x <= particlesPerSide; x++)
            {
                for (double y = 0; y <= particlesPerSide; y++)
                {
                    for (double z = 0; z <= particlesPerSide; z++)
                    {
                        Vector position = new Vector(x * space, y * space, z * space);

                        addParticle(generator.GetCubeParticle(position));
                    }
                }
            }

            double SPRINGCONSTANT = 10000;

            foreach ( Particle p1 in Projectiles)
            {
                foreach (Particle p2 in Projectiles)
                {
                    if (!p1.Equals(p2) && (p1.Position - p2.Position).Magnitude() <= space * Math.Sqrt(3))
                    {
                        AddConnector(p1, p2, SPRINGCONSTANT);
                    }
                }
            }
        }

        public void AdvanceParticles(double deltaTime)
        {
            foreach (Particle proj in Projectiles)
            {
                checkParticle(proj);
                proj.Update(deltaTime);
            }
        }

        private void checkParticle(Particle particle)
        {
            Vector newVec = particle.Position;
            if (particle.Position.X < 0 || particle.Position.X > Size.X)
            {
                particle.Velocity = new Vector(-particle.Velocity.X, particle.Velocity.Y, particle.Velocity.Z);
                if (particle.Position.X < 0)
                {
                    newVec.X = 0;
                }
                if (particle.Position.X > Size.X)
                {
                    newVec.X = Size.X;
                }
            }
            if (particle.Position.Y < 0 || particle.Position.Y > Size.Y)
            {
                particle.Velocity = new Vector(particle.Velocity.X, -particle.Velocity.Y, particle.Velocity.Z);
                if (particle.Position.Y < 0)
                {
                    newVec.Y = 0;
                }
                if (particle.Position.Y > Size.Y)
                {
                    newVec.Y = Size.Y;
                }
            }
            if (particle.Position.Z < 0 || particle.Position.Z > Size.Z)
            {
                particle.Velocity = new Vector(particle.Velocity.X, particle.Velocity.Y, -particle.Velocity.Z);
                if (particle.Position.Z < 0)
                {
                    newVec.Z = 0;
                }
                if (particle.Position.Z > Size.Z)
                {
                    newVec.Z = Size.Z;
                }
            }
            particle.Position = newVec;          
        }
    }
}
