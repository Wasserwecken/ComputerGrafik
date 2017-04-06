using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimeraExample
{
	public class Texture2D
	{
		public int Id { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public Texture2D(int id, int width, int height)
		{
			Id = id;
			Width = width;
			Height = height;
		}
	}
}
