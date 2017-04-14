using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.LevelLoader
{
	public class Block
    {
        /// <summary>
        ///  x coordinate of the block
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// y coordinate of the block
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// the sprite of the block
        /// </summary>
        public ISprite Sprite { get; set; }

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
        public Block(float x, float y, ISprite sprite)
        {
            X = x;
            Y = y;
            Sprite = sprite;
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
