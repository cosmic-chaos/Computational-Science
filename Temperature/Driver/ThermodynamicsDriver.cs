using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Thermodynamics;
using VisualizerControl;

namespace Driver
{
    class ThermodynamicsDriver
    {
        static internal void Thermodynamics()
        {
            const double containerSize = 50;
            const double minSpeed = 0;
            const double maxSpeed = 1500;
            //const double mass = 2.992e-26;
            const double mass = 2.992e-18;

            Color color1 = Colors.Aquamarine;
            Color color2 = Colors.Goldenrod;

            const int nParticles = 100;
            const double deltaTime = .0001;
            const double meanFreeTime = 2;

            var cont = new ParticleContainer(containerSize, containerSize, containerSize);
            //Level 3
            //var generator1 = new MaxwellBoltzmann(mass, color1, meanFreeTime, minSpeed, maxSpeed, 273.15, 0, containerSize / 2);
            //var generator2 = new MaxwellBoltzmann(mass, color2, meanFreeTime, minSpeed, maxSpeed, 373.15, containerSize / 2, containerSize);


            //cont.AddRandomParticles(generator1, nParticles/2);
            //cont.AddRandomParticles(generator2, nParticles / 2);

            //Challenge
            var generator = new MaxwellBoltzmann(mass, color1, meanFreeTime, minSpeed, maxSpeed, 150, 0, containerSize);
            cont.AddCube(generator, nParticles);


            //Level 1,2,3
            //var adapter = new ThermodynamicsEngineAdapter(cont);
            //var visualization = new SingleParticleVisualization(adapter);

            //Challenge
            var adapter = new ThermodynamicsEngineAdapter(cont);
            var visualization = new ConnectorVisualization(adapter);

            visualization.Radius = .5;
            var viz = new VisualizerControl.Visualizer(visualization);
            visualization.AddBox(containerSize, new Vector3D(containerSize / 2, containerSize / 2, containerSize / 2));
            viz.TimeIncrement = deltaTime;
            viz.AddHist(ThermodynamicsProjectileAdapter.GetSpeed, adapter, 50, Colors.BlueViolet, "Speed (m/s)");

            //Level 3
            //viz.Add3DGraph("Temperature", (() => adapter.Time), cont.GetTemperatureVec, "Time (s)", "Temperature (K)");
            //viz.AddText("Temperature Difference (K)", cont.GetTemperatureDiff, Colors.CadetBlue);

            //Challenge
            viz.AddSingleGraph("Temperature", () => adapter.Time, cont.GetTemperature, Colors.Aquamarine, "Time (s)", "Temperature (K)");
            viz.AddText("Temperature (K)", cont.GetTemperature, Colors.CadetBlue);

            viz.AutoCamera = true;
            viz.Show();
        }
    }
}
