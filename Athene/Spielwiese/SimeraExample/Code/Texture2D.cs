using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace SimeraExample
{
	public class Texture2D
	{
		public int Id { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

        public string Path { get; set; }

		public Texture2D() { }

		public Texture2D(int id, int width, int height, string path)
		{
			Id = id;
			Width = width;
			Height = height;
            Path = path;
		}

		public void Enable()
		{
			GL.BindTexture(TextureTarget.Texture2D, Id);
		}
		public void Disable()
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

	}
}
