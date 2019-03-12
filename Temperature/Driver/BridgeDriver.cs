using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Visualizer
{
    class BridgeDriver
    {
        static public void Bridge()
        {
            var bridge = new FiniteElement.GoldenGate(); // Replace this with your own class

            bridge.AddForce(new Vector3D(5, 0, 5), new Vector3D(0, 0, -10000), 25);

            var adapter = new BridgeEngineAdapter(bridge);

            VisualizerNamespace.Visualizer.Radius = .05;
            VisualizerNamespace.Visualizer viz = new VisualizerNamespace.Visualizer(adapter);
            viz.SpherePrecision = 40;
            viz.CylinderRadius = .01;
            viz.TimeIncrement = .001;
            viz.AddGround(5, new Vector3D(-5, 0, 0), "dirt.jpg");
            viz.AddGround(5, new Vector3D(15, 0, 0), "dirt.jpg");
            viz.AddGround(5, new Vector3D(5, 0, 0), "lava.jpg");

            addConnectorsToVisualizer(viz, adapter);
            viz.Show();
        }

        private static void addConnectorsToVisualizer(VisualizerNamespace.Visualizer viz, BridgeEngineAdapter adapter)
        {
            var projectiles = adapter.GetProjectiles();
            foreach (var connect in adapter.Connectors)
            {
                viz.AddConnector(projectiles[connect.Item1], projectiles[connect.Item2]);
            }
        }

    }
}

