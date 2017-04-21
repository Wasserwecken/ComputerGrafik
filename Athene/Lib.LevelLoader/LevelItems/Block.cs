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
        /// Initializes a empty block with no coordinates and no sprite
        /// </summary>
        public Block()
        {
            
        }

        /// <summary>
        /// Initializes a block
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="sprite">sprite</param>
        /// <param name="blockType">blocktype</param>
        /// <param name="collision"></param>
        /// <param name="damage"></param>
        public Block(float x, float y, ISprite sprite, BlockType blockType, bool collision, int damage)
        {
            X = x;
            Y = y;
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
            Sprite.Draw(new Vector2(X, Y), new Vector2(1f));
        }
    }
}
