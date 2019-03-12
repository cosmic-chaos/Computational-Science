using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace VisualizerControl
{
    public class SingleParticleVisualization : IVisualization
    {
        protected IEngine engine { get; private set; }

        private MeshGeometry3D sphereMesh = new MeshGeometry3D();
        private Dictionary<Color, Material> materialDict = new Dictionary<Color, Material>();

        protected Model3DCollection collection { get; private set; }

        private List<InternalProjectile> projectiles = new List<InternalProjectile>();

        private class InternalProjectile
        {
            public GeometryModel3D GeoObject { get; set; }
            public TranslateTransform3D Transform { get; set; }
            public IProjectile UnderlyingProjectile { get; set; }
        }

        public double Radius { get; set; } = 1;
        public int SpherePrecision { get; set; } = 40;

        public SingleParticleVisualization(IEngine engine)
        {
            this.engine = engine;
        }

        public List<Vector3D> GetAllProjectiles()
        {
            var list = new List<Vector3D>();
            foreach (var part in projectiles)
            {
                list.Add(part.UnderlyingProjectile.Position);
            }
            return list;
        }

        protected List<IProjectile> getProjectiles()
        {
            var list = new List<IProjectile>();
            foreach (var part in projectiles)
            {
                list.Add(part.UnderlyingProjectile);
            }
            return list;
        }

        virtual public void Initialize()
        {
            Sphere3D sphere = new Sphere3D(Radius, SpherePrecision);
            sphereMesh.Positions = sphere.Points;
            sphereMesh.TriangleIndices = sphere.TriangleIndices;
            fillProjectiles();
        }

        virtual public bool Update(double newTime, bool trace)
        {
            engine.Tick(newTime);

            //
            //if (Double.IsNaN(newPos.X) || Double.IsNaN(newPos.Y) || Double.IsNaN(newPos.Z))
            //{
            //    return false;
            //}
            if (trace)
            {
                foreach (var projectile in engine.GetProjectiles())
                {
                    addProjectile(projectile);
                }
            }
            else
            {
                foreach (var projectile in projectiles)
                {
                    Vector3D newPos = projectile.UnderlyingProjectile.Position;
                    projectile.Transform.OffsetX = newPos.X;
                    projectile.Transform.OffsetY = newPos.Y;
                    projectile.Transform.OffsetZ = newPos.Z;
                }
            }
            return true;
        }

        private void fillProjectiles()
        {
            foreach (var proj in engine.GetProjectiles())
            {
                addProjectile(proj);
            }
        }

        public void AddGround(double side, Vector3D center, string filename)
        {
            MakeSquare(new Vector3D(center.X + side / 2, center.Y - side / 2, center.Z),
               new Vector3D(center.X + side / 2, center.Y + side / 2, center.Z),
               new Vector3D(center.X - side / 2, center.Y - side / 2, center.Z),
               new Vector3D(center.X - side / 2, center.Y + side / 2, center.Z), filename);
        }

        public void AddBox(double side, Vector3D center)
        {
            const double specularCoefficient = 1;

            MeshGeometry3D mesh = new MeshGeometry3D();
            for (int ix = -1; ix <= 1; ix += 2)
                for (int iy = -1; iy <= 1; iy += 2)
                    for (int iz = -1; iz <= 1; iz += 2)
                    {
                        mesh.Positions.Add(new Point3D(center.X + ix * side / 2, center.Y + iy * side / 2, center.Z + iz * side / 2));
                    }

            mesh.TriangleIndices = new Int32Collection() {0, 1, 2, 1, 3, 2, // low yz
                0, 2, 4, 2, 6, 4, // low xy
                0, 4, 1, 1, 4, 5, // low xz
                4, 6, 5, 5, 6, 7, // high yz
                1, 5, 3, 3, 5, 7, // high xy
                3, 6, 2, 3, 7, 6 // high xz
            };

            var brush = new SolidColorBrush(Colors.LightSlateGray);
            var material = new SpecularMaterial(brush, specularCoefficient);
            GeometryModel3D geo = new GeometryModel3D(mesh, material);
            collection.Add(geo);

        }

        private Point3D convertToPoint3D(Vector3D vec)
        {
            return new Point3D(vec.X, vec.Y, vec.Z);
        }

        private void MakeSquare(Vector3D upperLeft, Vector3D upperRight, Vector3D lowerLeft, Vector3D lowerRight, string filename)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection()
            { convertToPoint3D(upperLeft), convertToPoint3D(upperRight), convertToPoint3D(lowerRight), convertToPoint3D(lowerLeft) };
            mesh.TriangleIndices = new Int32Collection() { 0, 1, 2, 0, 2, 3 };
            mesh.TextureCoordinates = new PointCollection() { new Point(0, 1), new Point(1, 1), new Point(1, 0), new Point(0, 0) };
            var brush = new ImageBrush();
            if (System.IO.File.Exists(@"..\..\..\..\VisualizerControl\" + filename))
            {
                brush.ImageSource = new BitmapImage(new Uri(@"..\..\..\..\VisualizerControl\" + filename, UriKind.Relative));
            }
            else
            {
                brush.ImageSource = new BitmapImage(new Uri(@"..\..\..\VisualizerControl\" + filename, UriKind.Relative));
            }
            var material = new DiffuseMaterial(brush);
            GeometryModel3D geo = new GeometryModel3D(mesh, material);
            geo.BackMaterial = material;
            collection.Insert(0, geo);
        }

        protected void addProjectile(IProjectile proj)
        {
            if (!materialDict.ContainsKey(proj.Color))
            {
                materialDict.Add(proj.Color, new DiffuseMaterial(new SolidColorBrush(proj.Color)));
            }
            GeometryModel3D geo = new GeometryModel3D(sphereMesh, materialDict[proj.Color]);
            collection.Add(geo);

            TranslateTransform3D trans = new TranslateTransform3D(proj.Position);
            geo.Transform = trans;
            projectiles.Add(new InternalProjectile { GeoObject = geo, Transform = trans, UnderlyingProjectile = proj });
        }

        protected void removeProjectile(IProjectile proj)
        {
            InternalProjectile internalObject = null;
            foreach (var pro in projectiles)
            {
                if (pro.UnderlyingProjectile.Position == proj.Position)
                {
                    internalObject = pro;
                    break;
                }
            }

            if (internalObject != null)
            {
                collection.Remove(internalObject.GeoObject);
                projectiles.Remove(internalObject);
            }
        }

        public void SetCollection(Model3DCollection collection)
        {
            this.collection = collection;
        }
    }
}
