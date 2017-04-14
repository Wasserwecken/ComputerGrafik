using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Simuals.Graphics
{
	/// <summary>
	/// Basis functionality for a sprite
	/// </summary>
	public class SpriteBase
	{
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
				var textCoordinate = new Vector2(vertices[index].X, 1 - vertices[index].Y);
				GL.TexCoord2(textCoordinate);

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
