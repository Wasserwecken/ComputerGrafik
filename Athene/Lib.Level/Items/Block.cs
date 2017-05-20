using System;
using Lib.Visuals.Graphics;
using OpenTK;
using System.Collections.Generic;
using Lib.Level.Base;
using Lib.LevelLoader.LevelItems;

namespace Lib.Level.Items
{
	public class Block
        : LevelItemBase, IDrawable, IIntersectable
    {

        /// <summary>
        /// Environment type of the block
        /// </summary>
        public EnvironmentType Environment { get; set; }

        /// <summary>
        /// Initializes a block
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="startPosition"></param>
        /// <param name="sprite">sprite</param>
        /// <param name="environmentType">environment type</param>
        /// <param name="collision"></param>
        /// <param name="damage"></param>
        public Block(Vector2 startPosition, ISprite sprite, EnvironmentType environmentType, bool collision, int damage)
			: base(startPosition, new Vector2(1f, 1f))
        {
            Sprite = sprite;
            Environment = environmentType;
            HasCollisionCorrection = collision;
            Damage = damage;
        }

        /// <summary>
        /// Draws the block accroding to it's coordinates
        /// </summary>
        public void Draw()
        {
            Sprite.Draw(HitBox.Position, new Vector2(1f));
            foreach (var attachedSprite in AttachedSprites)
                attachedSprite.Draw(HitBox.Position, new Vector2(1f));
        }

        /// <summary>
        /// A block will not react to a collision
        /// </summary>
        /// <param name="intersectingItems"></param>
        public void HandleCollisions(List<IIntersectable> intersectingItems) { }
    }
}
