using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;


namespace Simuals.Graphics
{
	public static class TextureManager
	{
		/// <summary>
		/// Contains all textures that has been requested
		/// </summary>
		private static Dictionary<string, Texture> _cachedTextures = new Dictionary<string, Texture>();

		/// <summary>
		/// Loads a texture from a given file. Checks also if the texture has been already loaded, and returns it from the cache.
		/// It also caches the texture if it is the first request.
		/// </summary>
		/// <param name="path">Path to the texture</param>
		/// <returns></returns>
        public static Texture GetTextureByPath(string path)
		{
			if (!File.Exists(path))
				throw new FileNotFoundException(path);


			//Check for a cached texture, the path is used as unique identifer
			if (_cachedTextures.TryGetValue(path, out Texture textureRequest))
				return textureRequest;


			//start loading the texture from file
			textureRequest = new Texture(GL.GenTexture());
			var textureFile = new Bitmap(path);
			textureFile.RotateFlip(RotateFlipType.RotateNoneFlipY); // the texture has to be fliped because the texture will be drawn later upside down

			var textureDimensions = new Rectangle(0, 0, textureFile.Width, textureFile.Height);
			var textureData = textureFile.LockBits(textureDimensions, ImageLockMode.ReadOnly, textureFile.PixelFormat);


			//Configuring the open gl texture
			textureRequest.Enable();
			textureRequest.Height = textureData.Height;
			textureRequest.Width = textureData.Width;
			
			GL.TexImage2D(
				TextureTarget.Texture2D,
				0,
				GetInternalPixelFormatBy(textureData.PixelFormat),
				textureData.Width,
				textureData.Height,
				0,
				GetInputPixelFormatBy(textureData.PixelFormat),
				PixelType.UnsignedByte,
				textureData.Scan0);

			textureFile.UnlockBits(textureData);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.Clamp);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.Clamp);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);


			//cleaning up the loading pricess
			textureRequest.Disable();
            _cachedTextures.Add(path, textureRequest);

			return textureRequest;
		}


		/// <summary>
		/// Loads a sequence of textures from a given directory. The pictues will be sorted alphabetical
		/// Only png files are supported.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
	    public static List<Texture> GetAnimationTextures(string path)
	    {
            var animationList = new List<Texture>();

			foreach(var file in Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly).OrderBy(f => f))
			{
				animationList.Add(GetTextureByPath(file));
			}

			return animationList;
        }

		/// <summary>
		/// Gets the correct open gl pixel format for the given file pixel format
		/// </summary>
		/// <param name="pixelFormat"></param>
		/// <returns></returns>
		public static OpenTK.Graphics.OpenGL.PixelFormat GetInputPixelFormatBy(System.Drawing.Imaging.PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
					return OpenTK.Graphics.OpenGL.PixelFormat.Red;

				case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
					return OpenTK.Graphics.OpenGL.PixelFormat.Bgr;

				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
					return OpenTK.Graphics.OpenGL.PixelFormat.Bgra;

				default:
					throw new FileLoadException("Pixel format '{0}' is not supported", pixelFormat.ToString());
			}
		}

		/// <summary>
		/// Gets the correct internal open gl pixel format for the given file pixel format
		/// </summary>
		/// <param name="pixelFormat"></param>
		/// <returns></returns>
		public static PixelInternalFormat GetInternalPixelFormatBy(System.Drawing.Imaging.PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
					return PixelInternalFormat.Luminance;

				case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
					return PixelInternalFormat.Rgb;

				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
					return PixelInternalFormat.Rgba;

				default:
					throw new FileLoadException("Pixel format '{0}' is not supported", pixelFormat.ToString());
			}
		}
	}
}
