using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Visuals.Graphics;

namespace Lib.LevelLoader.LevelItems
{
    public class LevelItemBase
    {
        /// <summary>
        ///  x coordinate of the item
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// y coordinate of the item
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// the sprite of the item
        /// </summary>
        public ISprite Sprite { get; set; }
    }
}
