using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace SimeraExample
{
	public class SpriteLoader
	{
		public static Texture2D LoadFromFile(string path)
		{
			if (!File.Exists(path))
				throw new FileNotFoundException(path);

			var textureFile = new Bitmap(path);
			textureFile.RotateFlip(RotateFlipType.RotateNoneFlipY);

			var textureDimensions = new Rectangle(0, 0, textureFile.Width, textureFile.Height);
			var textureData = textureFile.LockBits
				(textureDimensions,
				ImageLockMode.ReadOnly,
				textureFile.PixelFormat);
			
			var textureHandle = GL.GenTexture();
			GL.TexImage2D(
				TextureTarget.Texture2D,
				0,
				PixelInternalFormat.Rgba,
				textureData.Width,
				textureData.Height,
				0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
				PixelType.UnsignedByte,
				textureData.Scan0);

			textureFile.UnlockBits(textureData);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.Clamp);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.Clamp);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
			
			return new Texture2D(textureHandle, textureFile.Width, textureFile.Height);
		}
	}
}
