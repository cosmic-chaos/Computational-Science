using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizerControl
{
    /// <summary>
    /// Provides an interface for a kinematics engine or world class to work with the visualizer.
    /// This class should keep track of projectiles and forces and do all updates when told to.
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Moves the time in the engine to newTime and updates all projectiles accordingly.
        /// </summary>
        /// <param name="newTime">The new time the engine will move to</param>
        void Tick(double newTime);
       
        /// <summary>
        /// Returns a list of all the projectiles in the engine.
        /// </summary>
        /// <returns>A list of projectiles in no particular order, all implementing the IProjectile interface</returns>
        IList<IProjectile> GetProjectiles();
    }
}
