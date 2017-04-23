using System;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.LevelLoader.LevelItems
{
	public class Block : LevelItemBase
    {
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
	    /// Initializes a block
	    /// </summary>
	    /// <param name="x">x coordinate</param>
	    /// <param name="y">y coordinate</param>
	    /// <param name="startPosition"></param>
	    /// <param name="sprite">sprite</param>
	    /// <param name="blockType">blocktype</param>
	    /// <param name="collision"></param>
	    /// <param name="damage"></param>
	    public Block(Vector2 startPosition, ISprite sprite, BlockType blockType, bool collision, int damage)
			: base(startPosition, new Vector2(1,1))
        {
            Sprite = sprite;
            BlockType = blockType;
            Collision = collision;
            Damage = damage;
        }

        /// <summary>
        /// Draws the block to it's coordinates
        /// </summary>
        public void Draw()
        {
            Sprite.Draw(Position, new Vector2(1f));
        }

        /// <summary>
        /// reacts to a collision
        /// </summary>
        /// <param name="collidingBlock">the colliding block</param>
        /// <param name="oldPosition">old position</param>
        public override void ReactToCollision(LevelItemBase collidingBlock, Vector2 oldPosition)
        {
            
        }
    }
}
