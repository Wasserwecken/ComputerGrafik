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
    [DebuggerDisplay("Node: Elements={ElementCount}")]
    internal class QuadTreeNode
        : QuadTreeQuadrant
    {
        /// <summary>
        /// All subnodes of this node
        /// </summary>
        public List<QuadTreeQuadrant> Quadrants { get; set; }

        /// <summary>
        /// Gets the sum of all elements in the quadrant
        /// </summary>
        public override int ElementCount
        {
            get
            {
                int counter = 0;

                foreach (QuadTreeQuadrant quadrant in Quadrants)
                    counter += quadrant.ElementCount;

                return counter;
            }
        }
        
        
        /// <summary>
        /// Initialises a node
        /// </summary>
        public QuadTreeNode(Box2D size, int elementLimit, List<IIntersectable> initialElements)
            : base(size, elementLimit)
        {
            //Initialising the new four quadrants
            Quadrants = new List<QuadTreeQuadrant>();
            var quadrantSizeX = Size.Size.X / 2;
            var quadrantSizeY = Size.Size.Y / 2;

            Quadrants.Add(new QuadTreeLeaf(new Box2D(Size.Center.X, Size.Center.Y, quadrantSizeX, quadrantSizeY), elementLimit));
            Quadrants.Add(new QuadTreeLeaf(new Box2D(Size.Center.X, Size.Position.Y, quadrantSizeX, quadrantSizeY), elementLimit));
            Quadrants.Add(new QuadTreeLeaf(new Box2D(Size.Position.X, Size.Position.Y, quadrantSizeX, quadrantSizeY), elementLimit));
            Quadrants.Add(new QuadTreeLeaf(new Box2D(Size.Position.X, Size.Center.Y, quadrantSizeX, quadrantSizeY), elementLimit));

            //inserting given elements
            foreach (IIntersectable element in initialElements)
                InsertElement(element);
        }


        /// <summary>
        /// Inserts a element into the node
        /// </summary>
        /// <param name="newElement"></param>
        public override bool InsertElement(IIntersectable newElement)
        {
            for (int index = 0; index < Quadrants.Count; index ++)
            {
                var quadrant = Quadrants[index];

                //Check if the element has to be added to the tree
                bool isInside = newElement.HitBox.IsInside(quadrant.Size);
                bool doesIntersect = newElement.HitBox.IntersectsWith(quadrant.Size);
                
                if (isInside || doesIntersect)
                {
                    // if it is not possible to add the element to the quadrant, convert it to
                    // node, and then add the new element again.
                    if (!quadrant.InsertElement(newElement))
                    {
                        Quadrants[index] = new QuadTreeNode(quadrant.Size, quadrant.ElementLimit, ((QuadTreeLeaf)quadrant).Elements);
                        Quadrants[index].InsertElement(newElement);
                    }
                }
            }

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

            foreach(QuadTreeQuadrant quadrant in Quadrants)
            {
                if (quadrant.Size.IsInside(range) || quadrant.Size.IntersectsWith(range))
                    result.AddRange(quadrant.GetElementsIn(range));
            }

            return result.Distinct().ToList();
        }
    }
}
