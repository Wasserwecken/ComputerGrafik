using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Tools.QuadTree
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
        public abstract bool InsertElement(IQuadTreeElement newElement);

        /// <summary>
        /// Returns all intersecting or including elements in the given range
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public abstract List<IQuadTreeElement> GetElements(Box2D range);
    }
}
