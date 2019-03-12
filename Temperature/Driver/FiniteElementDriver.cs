using FiniteElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Visualizer
{
    class FiniteElementDriver
    {
        public static void FiniteElement()
        {
            var structure = new ParticleStructure();
            structure.GroundForceOn = true;

            var engine = new StructureEngine();
            engine.Structure = structure;

            var adapter = new FEEngineAdapter(engine);

            VisualizerNamespace.Visualizer.Radius = .1;
            VisualizerNamespace.Visualizer viz = new VisualizerNamespace.Visualizer(adapter);
            viz.SpherePrecision = 40;
            viz.CylinderRadius = .01;
            viz.TimeIncrement = .01;
            viz.AddGround(10, new Vector3D(0, 0, 0), "dirt.jpg");

            //viz.Add3DGraph("Center of Mass", (() => viz.Time), structure.CenterOfMass, "Time (s)", "Position (m)");

            addConnectorsToVisualizer(viz, structure, adapter);
            viz.Show();
        }

        private static void addConnectorsToVisualizer(VisualizerNamespace.Visualizer viz, ParticleStructure structure,
            FEEngineAdapter adapter)
        {
            var projectiles = adapter.GetProjectiles();
            foreach (var connect in structure.Connectors)
            {
                viz.AddConnector(projectiles[connect.Item1], projectiles[connect.Item2]);
            }
        }
    }
}
