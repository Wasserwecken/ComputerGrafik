using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.LevelLoader.LevelItems
{
    public class LevelItemBase
    {
        /// <summary>
        /// Position of the item
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// the sprite of the item
        /// </summary>
        public ISprite Sprite { get; set; }
    }
}
