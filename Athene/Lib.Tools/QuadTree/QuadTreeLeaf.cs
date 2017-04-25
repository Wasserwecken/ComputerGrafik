using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Tools.QuadTree
{
    internal class QuadTreeLeaf
        : QuadTreeQuadrant
    {
        /// <summary>
        /// List of elements in a leaf
        /// </summary>
        public List<IQuadTreeElement> Elements { get; set; }


        /// <summary>
        /// Initialises a leaf
        /// </summary>
        public QuadTreeLeaf(Box2D size, int elementLimit)
            : base(size, elementLimit)
        {
            Elements = new List<IQuadTreeElement>();
        }

        /// <summary>
        /// Initialises a tree leaf
        /// </summary>
        /// <param name="initialElements"></param>
        public QuadTreeLeaf(Box2D size, int elementLimit, List<IQuadTreeElement> initialElements)
            : this(size, elementLimit)
        {
            foreach (IQuadTreeElement element in initialElements)
                InsertElement(element);
        }


        /// <summary>
        /// Inserts a new element into the node, splits the node if there are 
        /// </summary>
        /// <param name="newElement"></param>
        public override bool InsertElement(IQuadTreeElement newElement)
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
        public override List<IQuadTreeElement> GetElementsIn(Box2D range)
        {
            var result = new List<IQuadTreeElement>();

            foreach (IQuadTreeElement element in Elements)
            {
                if (element.HitBox.IsInside(range) || element.HitBox.IntersectsWith(range))
                    result.Add(element);
            }

            return result;
        }
    }
}
