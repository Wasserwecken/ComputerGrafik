using Lib.Tools;
using Lib.Visuals.Graphics;
using OpenTK;
using System.Collections.Generic;
using Lib.Tools.QuadTree;
using Lib.LevelLoader.LevelItems;

namespace Lib.Level.Base
{
    public abstract class LevelItemBase
        : IQuadTreeElement
    {
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
            HitBox = new Box2D(startPosition.X, startPosition.Y, boxSize.X, boxSize.Y);
            AttachedSprites = new List<ISprite>();
		}
    }
}
