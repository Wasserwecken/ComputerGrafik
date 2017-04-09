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
	public class Sprite
	{
		/// <summary>
		/// Id of the texture handler from OpenGL
		/// </summary>
		public Texture2D Texture { get; set; }
     

		/// <summary>
		/// Initialises a sprite
		/// </summary>
		/// <param name="path"></param>
		public Sprite(string path)
		{
			Texture = SpriteLoader.LoadFromFile(path);
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

				vertices[index].X *= Texture.Width;
				vertices[index].Y *= Texture.Height;
				vertices[index] *= scale;
				vertices[index] += position;

				GL.Vertex2(vertices[index]);
			}

			GL.End();
			Texture.Disable();
		}
	}
}
