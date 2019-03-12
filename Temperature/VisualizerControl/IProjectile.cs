using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VisualizerControl
{
    /// <summary>
    /// Interface for a projectile to be represented in a visualizer
    /// </summary>
    public interface IProjectile
    {
        /// <summary>
        /// The mass of the particle (must be settable)
        /// </summary>
        double Mass { get; set; }

        /// <summary>
        /// The current position of the particle, as a vector
        /// </summary>
        /// <returns>The position vector, translated into a Vector3D type.</returns>
        Vector3D Position { get; }
        
        /// <summary>
        /// The current velocity of the particle, as a vector
        /// </summary>
        /// <returns>The velocity vector, translated into a Vector3D type.</returns>
        Vector3D Velocity { get; }
        
        /// <summary>
        /// The current acceleration of the particle, as a vector
        /// </summary>
        /// <returns>The acceleration vector, translated into a Vector3D type.</returns>
        Vector3D Acceleration { get; }
        
        /// <summary>
        /// The output color of the projectile on the screen.  Your choice of color!
        /// </summary>
        Color Color { get; set; }
    }
}
