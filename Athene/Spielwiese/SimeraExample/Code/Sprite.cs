using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SimeraExample.Code;

namespace SimeraExample
{
	public class Sprite : ISprite
    {
		/// <summary>
		/// Id of the texture handler from OpenGL
		/// </summary>
		public Texture2D Texture { get; set; }

        /// <summary>
        /// Initialises a sprite
        /// </summary>
        /// <param name="texture"></param>
        public Sprite(Texture2D texture)
		{
		    Texture = texture;
		}

		/// <summary>
		/// Draws the sprite on the screen
		/// </summary>
		public void Draw(Vector2 position, Vector2 scale)
		{
			Texture.Enable();
			GL.Begin(PrimitiveType.Quads);

			var vertices = new Vector2[4]
			{
				new Vector2(0, 0),
				new Vector2(0, 1),
				new Vector2(1, 1),
				new Vector2(1, 0),
			};

			for(int index = 0; index < 4; index ++)
			{ 
				GL.TexCoord2(vertices[index]);

				if (Texture.Height > Texture.Width)
					vertices[index].Y *= (float)Texture.Height / Texture.Width;
				else
					vertices[index].X *= (float)Texture.Width / Texture.Height;

				vertices[index] *= scale;
				vertices[index] += position;

				GL.Vertex2(vertices[index]);
			}

			GL.End();
			Texture.Disable();
		}
	}
}
