using Lib.Tools;
using Lib.Visuals.Graphics;
using OpenTK;
using System.Collections.Generic;
using Lib.Tools.QuadTree;

namespace Lib.LevelLoader.LevelItems
{
    public abstract class LevelItemBase
        : IQuadTreeElement
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
        /// blocktype of the block
        /// </summary>
        public BlockType BlockType { get; set; }

        /// <summary>
        /// collission of the block
        /// </summary>
        public bool Collision { get; set; }

        /// <summary>
        /// damage of the block
        /// </summary>
        public int Damage { get; set; }


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
