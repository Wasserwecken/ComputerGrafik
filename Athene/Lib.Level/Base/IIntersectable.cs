using Lib.Level.Base;
using Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Base
{
    public interface IIntersectable
    {
        /// <summary>
        /// Hitbox for the intersection
        /// </summary>
        Box2D HitBox { get; set; }

        /// <summary>
        /// Defines if the intersection has to be corrected or not
        /// </summary>
        bool HasCollisionCorrection { get; set; }
        
        /// <summary>
        /// React to collisions with given items
        /// </summary>
        /// <param name="intersectingItems"></param>
        void HandleCollisions(List<IIntersectable> intersectingItems);
    }
}
