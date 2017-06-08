using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lib.Visuals.Graphics
{
	/// <summary>
	/// Static spricte with no "texture" animation
	/// </summary>
	public class SpriteStatic
		: SpriteBase, ISprite
    {
		/// <summary>
		/// Texture that will be used for the sprite
		/// </summary>
		public Texture SpriteTexture { get; set; }


        /// <summary>
        /// Initialises a sprite
        /// </summary>
        public SpriteStatic(Vector2 size, string path)
            : this(size, path, TextureWrapMode.Repeat, TextureWrapMode.MirroredRepeat) { }

        /// <summary>
        /// Initialises a sprite
        /// </summary>
        /// <param name="path"></param>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        public SpriteStatic(Vector2 size, string path, TextureWrapMode horizontal, TextureWrapMode vertical)
            : base(size)
        {
            SpriteTexture = TextureManager.GetTexture(path, horizontal, vertical);
        }

        /// <summary>
        /// Draws the sprite on the screen
        /// </summary>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        public void Draw(Vector2 position, Vector2 scale)
		{
			Draw(position, scale, SpriteTexture);
		}
	}
}
