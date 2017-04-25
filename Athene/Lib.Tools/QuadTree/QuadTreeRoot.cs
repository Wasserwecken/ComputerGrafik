﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Tools.QuadTree
{
    public class QuadTreeRoot
    {
        /// <summary>
        /// Root node
        /// </summary>
        private QuadTreeNode RootNode { get; set; }


        /// <summary>
        /// Initialises the root node of the quadtree
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="elementLimit"></param>
        /// <param name="initialElements"></param>
        public QuadTreeRoot(Box2D size, int elementLimit, List<IQuadTreeElement> initialElements)
        {
            RootNode = new QuadTreeNode(size, elementLimit, initialElements);
        }


        /// <summary>
        /// Inserts a element into the node
        /// </summary>
        /// <param name="newElement"></param>
        public void InsertElement(IQuadTreeElement newElement)
        {
            RootNode.InsertElement(newElement);
        }

        /// <summary>
        /// Returns all intersecting or including elements in the given range
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<IQuadTreeElement> GetElements(Box2D range)
        {
            return RootNode.GetElements(range);
        }
    }
}
