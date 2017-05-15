using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Physics
{
    public class EnergyObjectProperties
    {
        /// <summary>
        /// Interia of the object. 1 is for instant movment, as higher the value there will be more "sliding"
        /// </summary>
        public Vector2 Momentum { get; set; }

        /// <summary>
        /// Mass of the object
        /// </summary>
        public float Mass { get; set; }

        /// <summary>
        /// Normal speed of the object in the environment
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Initialises the properties
        /// </summary>
        /// <param name="momentumHorizontal"></param>
        /// <param name="momentumVertical"></param>
        /// <param name="movementSpeed"></param>
        /// <param name="mass"></param>
        public EnergyObjectProperties(float momentumHorizontal, float momentumVertical, float speed, float mass)
        {
            Momentum = new Vector2(momentumHorizontal, momentumVertical);
            Mass = mass;
            Speed = speed;
        }
    }
}
