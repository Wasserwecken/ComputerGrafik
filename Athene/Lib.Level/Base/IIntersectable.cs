using Lib.Level.Base;
using Lib.Tools;
using Lib.Tools.QuadTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Base
{
    public interface IIntersectable
        : IQuadTreeElement
    {
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
