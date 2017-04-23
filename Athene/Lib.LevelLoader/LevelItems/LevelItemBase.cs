using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.Geometry;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.LevelLoader.LevelItems
{
    public abstract class LevelItemBase
    {
        /// <summary>
        /// Position of the item
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// the sprite of the item
        /// </summary>
        public ISprite Sprite { get; set; }

        public Box2D Box2D { get; set; }

        /// <summary>
        /// Initialises a new level item
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="boxSize">boxSize</param>
        public LevelItemBase(Vector2 startPosition, Vector2 boxSize)
		{
			Position = startPosition;
            Box2D = new Box2D(startPosition.X, startPosition.Y, boxSize.X, boxSize.Y);
		}

        public abstract void ReactToCollision(LevelItemBase collidingBlock, Vector2 oldPosition);
    }
}
