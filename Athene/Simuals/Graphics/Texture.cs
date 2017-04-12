using OpenTK.Graphics.OpenGL;

namespace Simuals.Graphics
{
	/// <summary>
	/// A registered open gl texture
	/// </summary>
	public class Texture
	{
		/// <summary>
		/// Internal open gl texture id
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Width of the texture
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Height of the texture
		/// </summary>
		public int Height { get; set; }

        /// <summary>
		/// Initialises the texture with a given id
		/// </summary>
		public Texture(int id)
		{
			Id = id;
		}
		
		/// <summary>
		/// Activates the texture for rendering
		/// </summary>
		public void Enable()
		{
			GL.BindTexture(TextureTarget.Texture2D, Id);
		}

		/// <summary>
		/// Deactivates the texture for rendering
		/// </summary>
		public void Disable()
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

	}
}
