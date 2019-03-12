using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace FiniteElement
{
    public class Grounding : ParticleStructure
    {
        public Grounding(Vector3D leftBottomCorner, double length, double width, double height)
        {
            //Add projectiles.
            for (int k = 0; k < height; k++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        leftBottomCorner = leftBottomCorner + new Vector3D(0, 1, 0);
                        AddCube(leftBottomCorner, 1);
                        leftBottomCorner = leftBottomCorner + new Vector3D(0, 1, 0);
                    }
                    leftBottomCorner = leftBottomCorner + new Vector3D(1, 0, 0);
                }
                leftBottomCorner = leftBottomCorner + new Vector3D(0, 0, 1);
            }
            //Add connectors.
        }

        private static void AddCube(Vector3D leftBottomCorner, double length)
        {
            
        }
    }
}
