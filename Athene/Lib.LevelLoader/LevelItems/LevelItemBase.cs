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

        /// <summary>
        /// Hitbox of the object in the level
        /// </summary>
        public Box2D Box2D { get; set; }

        /// <summary>
        /// Attached Sprites
        /// </summary>
        public List<ISprite> AttachedSprites { get; set; }


        /// <summary>
        /// Initialises a new level item
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="boxSize">boxSize</param>
        protected LevelItemBase(Vector2 startPosition, Vector2 boxSize)
		{
			Position = startPosition;
            Box2D = new Box2D(startPosition.X, startPosition.Y, boxSize.X, boxSize.Y);
            AttachedSprites = new List<ISprite>();
		}

        /// <summary>
        /// Handles a collision with this object with another object in the level
        /// </summary>
        /// <param name="collidingBlock"></param>
        public abstract void ReactToCollision(LevelItemBase collidingBlock);
    }
}
