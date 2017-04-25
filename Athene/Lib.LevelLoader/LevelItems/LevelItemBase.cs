using Lib.Tools;
using Lib.Visuals.Graphics;
using OpenTK;
using System.Collections.Generic;

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
        public Box2D HitBox { get; set; }

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
            HitBox = new Box2D(startPosition.X, startPosition.Y, boxSize.X, boxSize.Y);
            AttachedSprites = new List<ISprite>();
		}

        /// <summary>
        /// Handles a collision with this object with another object in the level
        /// </summary>
        /// <param name="collidingBlock"></param>
        public abstract void ReactToCollision(LevelItemBase collidingBlock);
    }
}
