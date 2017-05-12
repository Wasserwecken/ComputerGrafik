using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;


namespace Lib.Visuals.Graphics
{
	internal static class TextureManager
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
        public static Texture GetTexture(string path)
		{
			if (!File.Exists(path))
				throw new FileNotFoundException(path);

            //Check for a cached texture, the path is used as unique identifer
            Texture textureRequest;
            if (_cachedTextures.TryGetValue(path, out textureRequest))
				return textureRequest;

			//start loading the texture from file
			var textureFile = new Bitmap(path);

			var textureDimensions = new Rectangle(0, 0, textureFile.Width, textureFile.Height);
			var textureData = textureFile.LockBits(textureDimensions, ImageLockMode.ReadOnly, textureFile.PixelFormat);

			//Create a open gl texture
			textureRequest = CreateGLTexture(textureData);

			//clean up
			textureFile.UnlockBits(textureData);
            _cachedTextures.Add(path, textureRequest);

			return textureRequest;
		}

		/// <summary>
		/// Loads a sequence of textures from a given directory. The pictues will be sorted alphabetical
		/// Only png files are supported.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
	    public static List<Texture> GetTextures(string path)
	    {
            var textureList = new List<Texture>();

			foreach(var file in Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly).OrderBy(f => f))
				textureList.Add(GetTexture(file));

			return textureList;
        }

		/// <summary>
		/// Creates a open GL texture based on given bitmap data
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		private static Texture CreateGLTexture(BitmapData data)
		{
			var newTexture = new Texture(GL.GenTexture());
			newTexture.Enable();
			newTexture.Height = data.Height;
			newTexture.Width = data.Width;

			GL.TexImage2D(
				TextureTarget.Texture2D,
				0,
				GetInternalPixelFormatBy(data.PixelFormat),
				data.Width,
				data.Height,
				0,
				GetInputPixelFormatBy(data.PixelFormat),
				PixelType.UnsignedByte,
				data.Scan0);


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Nearest);

            newTexture.Disable();

			return newTexture;
		}

		/// <summary>
		/// Gets the correct open gl pixel format for the given file pixel format
		/// </summary>
		/// <param name="pixelFormat"></param>
		/// <returns></returns>
		private static OpenTK.Graphics.OpenGL.PixelFormat GetInputPixelFormatBy(System.Drawing.Imaging.PixelFormat pixelFormat)
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
		private static PixelInternalFormat GetInternalPixelFormatBy(System.Drawing.Imaging.PixelFormat pixelFormat)
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
