using OpenTK;

namespace Lib.Visuals.Graphics
{
	/// <summary>
	/// Defines a sprite
	/// </summary>
	public interface ISprite
    {
        /// <summary>
        /// Flips the texture of the sprite on the Y axis
        /// </summary>
        bool FlipTextureHorizontal { get; set; }

        /// <summary>
        /// Flips the texture of the sprite on the Y axis
        /// </summary>
        bool FlipTextureVertical { get; set; }

        /// <summary>
        /// Draws the sprite on screen
        /// </summary>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        void Draw(Vector2 position, Vector2 scale);
    }
}
