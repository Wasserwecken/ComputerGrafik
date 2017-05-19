using Lib.Level.Base;
using Lib.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.QuadTree
{
    [DebuggerDisplay("Leaf: Elements={ElementCount}")]
    internal class QuadTreeLeaf
        : QuadTreeQuadrant
    {
        /// <summary>
        /// List of elements in a leaf
        /// </summary>
        public List<IIntersectable> Elements { get; set; }

        /// <summary>
        /// Gets the sum of all elements in the quadrant
        /// </summary>
        public override int ElementCount => Elements.Count;


        /// <summary>
        /// Initialises a leaf
        /// </summary>
        public QuadTreeLeaf(Box2D size, int elementLimit)
            : base(size, elementLimit)
        {
            Elements = new List<IIntersectable>();
        }

        /// <summary>
        /// Initialises a tree leaf
        /// </summary>
        /// <param name="initialElements"></param>
        public QuadTreeLeaf(Box2D size, int elementLimit, List<IIntersectable> initialElements)
            : this(size, elementLimit)
        {
            foreach (IIntersectable element in initialElements)
                InsertElement(element);
        }


        /// <summary>
        /// Inserts a new element into the node, splits the node if there are 
        /// </summary>
        /// <param name="newElement"></param>
        public override bool InsertElement(IIntersectable newElement)
        {
            if (Elements.Count >= ElementLimit)
                return false;
            
            Elements.Add(newElement);
            return true;
        }

        /// <summary>
        /// Returns all intersecting or including elements in the given range
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public override List<IIntersectable> GetElementsIn(Box2D range)
        {
            var result = new List<IIntersectable>();

            foreach (IIntersectable element in Elements)
            {
                if (element.HitBox.IsInside(range) || element.HitBox.IntersectsWith(range))
                    result.Add(element);
            }

            return result;
        }
    }
}
