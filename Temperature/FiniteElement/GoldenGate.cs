using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FiniteElement
{
    public class GoldenGate : Bridge
    {
        public override Color Color
        {
            get
            {
                return Colors.Gold;
            }
        }

        protected override void initialize()
        {
            double height = Math.Sqrt(3) * .5;
            ThroughArch(-.1, 6, 15, height, 1, new Vector3D(5, -2.5, 5.625));
       }

        private void ThroughArch(double coefficient, double numberOfLayers, double distanceSpan, double height, double averageMass, Vector3D vertexPos)
        {
            //Add projectiles for the arch
            var particlesInLayer = new List<int>();
            for (double yInterval = 0; yInterval <= vertexPos.Y * -2; yInterval += vertexPos.Y * -2 / numberOfLayers)
            {
                int firstLayerNo = 0;
                for (double start = vertexPos.X - distanceSpan / 2; start <= distanceSpan / 2 + vertexPos.X; start += .2)
                {
                    double z = coefficient * (start - vertexPos.X) * (start - vertexPos.X) + vertexPos.Z;
                    addProjectile(new Vector3D(start, vertexPos.Y + yInterval, z), averageMass * (vertexPos.Z /6)/(z+.1));
                    firstLayerNo++;
                }
                particlesInLayer.Add(firstLayerNo);

                int secondLayerNo = 0;
                for (double start = vertexPos.X - distanceSpan / 2 - height; start <= distanceSpan / 2 + vertexPos.X + height; start += .4)
                {
                    double z = -.0927417 * start * start + .927417 * start + 4.17248;
                    if (z < 0)
                    {
                        z = 0;
                    }
                    addProjectile(new Vector3D(start, vertexPos.Y + yInterval, z), averageMass);
                    //equation based on solving for a parabola passing through 3 points (vertex and both x-intercepts) displaced by the height
                    secondLayerNo++;
                }
                addProjectile(new Vector3D(distanceSpan / 2 + vertexPos.X + height, vertexPos.Y + yInterval, 0), averageMass);
                secondLayerNo++;
                particlesInLayer.Add(secondLayerNo);
            }

            //Add connectors within a row
            particlesInLayer[0] = particlesInLayer[0] - 1;
            addConnector(0, 1); //hardcode add first connector because excluded in code 
            int previousLayer = 0;
            foreach (var number in particlesInLayer)
            {
                int noOfParticles = number;
                for (int index = 0; index < noOfParticles; index++)
                {
                    int numb = index + previousLayer;
                    if (numb != previousLayer)
                    {
                        if(numb + 1 < Projectiles.Count)
                        addConnector(numb, numb + 1);
                    }
                }
                previousLayer += noOfParticles;
            }
            
            //ADD ALL THE CONNECTORS
            foreach(var proj in Projectiles)
            {
                foreach(var other in Projectiles)
                {
                    if((proj.Position - other.Position).Length < 1)
                    {
                        addConnector(proj, other);
                    }
                }
            }

            //Add grounding?
        }
    }
}
