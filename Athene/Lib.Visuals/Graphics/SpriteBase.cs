using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lib.Visuals.Graphics
{
	/// <summary>
	/// Basis functionality for a sprite
	/// </summary>
	public class SpriteBase
	{
		/// <summary>
		/// Flips the texture of the sprite on the Y axis
		/// </summary>
		public bool FlipTextureHorizontal { get; set; }

		/// <summary>
		/// Flips the texture of the sprite on the Y axis
		/// </summary>
		public bool FlipTextureVertical { get; set; }

		/// <summary>
		/// Initialises a sprite
		/// </summary>
		public SpriteBase()
		{
			FlipTextureHorizontal = false;
			FlipTextureVertical = true;
		}

		/// <summary>
		/// Draws the sprite on the screen
		/// </summary>
		public void Draw(Vector2 position, Vector2 scale, Texture spriteTexture)
		{
			spriteTexture.Enable();
			GL.Begin(PrimitiveType.Quads);

			var vertices = new[]
			{
				new Vector2(0, 0),
				new Vector2(0, 1),
				new Vector2(1, 1),
				new Vector2(1, 0),
			};

			for (int index = 0; index < 4; index++)
			{
				//Texture setup, first flipping then setting
				float texturePositionY;
				if (FlipTextureVertical)
					texturePositionY = 1 - vertices[index].Y;
				else
					texturePositionY = vertices[index].Y;

				float texturePositionX;
				if (FlipTextureHorizontal)
					texturePositionX = 1 - vertices[index].X;
				else
					texturePositionX = vertices[index].X;

				GL.TexCoord2(new Vector2(texturePositionX, texturePositionY));


				//Vertex setup
				if (spriteTexture.Height > spriteTexture.Width)
					vertices[index].Y *= (float)spriteTexture.Height / spriteTexture.Width;
				else
					vertices[index].X *= (float)spriteTexture.Width / spriteTexture.Height;

				vertices[index] *= scale;
				vertices[index] += position;

				GL.Vertex2(vertices[index]);
			}

			GL.End();
			spriteTexture.Disable();
		}
	}
}
