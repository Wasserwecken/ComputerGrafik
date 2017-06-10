using Lib.Level.Base;
using Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.QuadTree
{
    internal abstract class QuadTreeQuadrant
    {
        /// <summary>
        /// Size informations about the node
        /// </summary>
        public Box2D Size { get; private set; }

        /// <summary>
        /// Number of elements that are allowed before splitting
        /// </summary>
        public int ElementLimit { get; private set; }

        /// <summary>
        /// Gets the sum of all elements in the quadrant
        /// </summary>
        public abstract int ElementCount { get; }


        /// <summary>
        /// Initialises the base
        /// </summary>
        /// <param name="elementLimit"></param>
        public QuadTreeQuadrant(Box2D size, int elementLimit)
        {
            Size = size;
            ElementLimit = elementLimit;
        }


        /// <summary>
        /// Inserts a new element into the node, splits the node if there are 
        /// </summary>
        /// <param name=""></param>
        public abstract bool InsertElement(IIntersectable newElement);

        /// <summary>
        /// removes a lement from a node
        /// </summary>
        /// <param name=""></param>
        public abstract bool RemoveElement(IIntersectable element);

        /// <summary>
        /// Returns all intersecting or including elements in the given range
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public abstract List<IIntersectable> GetElementsIn(Box2D range);
    }
}
