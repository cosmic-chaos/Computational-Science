using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VisualizerControl
{
    public class ConnectorVisualization : SingleParticleVisualization
    {
        private IEngineWithConnectors myEngine { get { return (IEngineWithConnectors)engine; } }

        private class Connector
        {
            public GeometryModel3D GeoObject { get; set; }
            public TranslateTransform3D Translation { get; set; }
            public AxisAngleRotation3D PhiRotation { get; set; } // using physics convention - this is azimuth
            public AxisAngleRotation3D ThetaRotation { get; set; } // this is polar
            public ScaleTransform3D Scale { get; set; }
            public IProjectile Proj1 { get; set; }
            public IProjectile Proj2 { get; set; }
        }

        private List<Connector> connectors = new List<Connector>();

        public double CylinderRadius { get; set; } = .1;
        public Color ConnectorColor { get; set; } = Colors.LimeGreen;
        public int CylinderPrecision { get; set; } = 40;

        private MeshGeometry3D cylinderMesh = new MeshGeometry3D();
        private Material cylinderMaterial;

        public ConnectorVisualization(IEngineWithConnectors engine) :
            base(engine)
        { }

        public override void Initialize()
        {
            base.Initialize();

            Cylinder3D cylinder = new Cylinder3D(CylinderRadius, 1, CylinderPrecision);
            cylinderMesh.Positions = cylinder.Points;
            cylinderMesh.TriangleIndices = cylinder.TriangleIndices;
            cylinderMaterial = new SpecularMaterial(new SolidColorBrush(ConnectorColor), 1);
            //cylinderMaterial = new DiffuseMaterial(new SolidColorBrush(ConnectorColor));

            var projectiles = getProjectiles();
            foreach (var connector in myEngine.GetConnectors())
            {
                connectors.Add(createConnector(projectiles[connector.Item1], projectiles[connector.Item2]));
            }
        }

        public override bool Update(double newTime, bool trace)
        {
            bool result = base.Update(newTime, trace);

            foreach (var connector in myEngine.ConnectorsToRemove())
            {
                removeConnector(connector.Item1, connector.Item2);
            }

            for (int i = 0; i < connectors.Count; ++i)
            {
                if (trace)
                {
                    var newCon = createConnector(connectors[i].Proj1, connectors[i].Proj2);
                    connectors[i] = newCon;
                }
                adjustConnector(connectors[i]);
            }

            return result;
        }

        private Connector createConnector(IProjectile proj1, IProjectile proj2)
        {
            var diffVec = proj1.Position - proj2.Position;

            GeometryModel3D geo = new GeometryModel3D(cylinderMesh, cylinderMaterial);
            collection.Add(geo);

            var translate = new TranslateTransform3D();
            var rotatePhi = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);
            var rotateTheta = new AxisAngleRotation3D();
            var transform = new Transform3DGroup();
            var scale = new ScaleTransform3D();

            transform.Children.Add(scale);
            transform.Children.Add(new RotateTransform3D(rotatePhi));
            transform.Children.Add(new RotateTransform3D(rotateTheta));
            transform.Children.Add(translate);

            geo.Transform = transform;
            var newCon = new Connector
            {
                GeoObject = geo,
                Translation = translate,
                PhiRotation = rotatePhi,
                ThetaRotation = rotateTheta,
                Scale = scale,
                Proj1 = proj1,
                Proj2 = proj2
            };

            adjustConnector(newCon);
            return newCon;
        }

        private void adjustConnector(Connector connect)
        {
            var diff = connect.Proj2.Position - connect.Proj1.Position;
            var length = diff.Length;
            // Scale
            connect.Scale.ScaleZ = length;

            // Translate
            var midpoint = (connect.Proj1.Position + connect.Proj2.Position) / 2;
            connect.Translation.OffsetX = midpoint.X;
            connect.Translation.OffsetY = midpoint.Y;
            connect.Translation.OffsetZ = midpoint.Z;

            // Rotate
            double theta = Math.Acos(diff.Z / length);
            double phi = Math.Atan2(diff.Y, diff.X);
            var perp = new Vector3D(-Math.Sin(phi), Math.Cos(phi), 0);

            connect.ThetaRotation.Angle = radiansToDegrees(theta);
            connect.ThetaRotation.Axis = perp;
            connect.PhiRotation.Angle = radiansToDegrees(phi);
        }

        private void removeConnector(int index1, int index2)
        {
            int index = -1;
            var projectiles = getProjectiles();
            for (int i = 0; i < connectors.Count; ++i)
            {
                if ((projectiles.IndexOf(connectors[i].Proj1) == index1 && projectiles.IndexOf(connectors[i].Proj2) == index2) ||
                        (projectiles.IndexOf(connectors[i].Proj1) == index2 && projectiles.IndexOf(connectors[i].Proj2) == index1))
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                //throw new KeyNotFoundException("Cannot identify connector from indices" + index1 + " and " + index2);
                return;
            }

            collection.Remove(connectors[index].GeoObject);
            connectors.Remove(connectors[index]);
        }

        private double radiansToDegrees(double input)
        {
            return input * 180 / Math.PI;
        }

    }
}
