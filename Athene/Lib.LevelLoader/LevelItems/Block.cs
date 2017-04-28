using System;
using Lib.Visuals.Graphics;
using OpenTK;
using System.Collections.Generic;

namespace Lib.LevelLoader.LevelItems
{
	public class Block : LevelItemBase
    {		
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
            Sprite.Draw(HitBox.Position, new Vector2(1f));
            foreach (var attachedSprite in AttachedSprites)
                attachedSprite.Draw(HitBox.Position, new Vector2(1f));
        }
    }
}
