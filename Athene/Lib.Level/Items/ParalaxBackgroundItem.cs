using Lib.Visuals.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Items
{
    public class ParalaxBackgroundItem
    {
        /// <summary>
        /// Used as Z-level of item
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Relative movement factor to the source
        /// </summary>
        public float DisplacementFactor { get; set; }

        /// <summary>
        /// Sprite of the background
        /// </summary>
        public SpriteStatic Sprite { get; set; }


        /// <summary>
        /// Initialises the item
        /// </summary>
        public ParalaxBackgroundItem(int index, float displacementFactor, SpriteStatic sprite)
        {
            Index = index;
            DisplacementFactor = displacementFactor;
            Sprite = sprite;
        }
    }
}
