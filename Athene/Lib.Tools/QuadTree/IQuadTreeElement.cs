using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Tools.QuadTree
{
    public interface IQuadTreeElement
    {
        /// <summary>
        /// Hitbox of the element in the quadtree
        /// </summary>
        Box2D HitBox { get; set; }
    }
}
