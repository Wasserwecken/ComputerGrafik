using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace SimeraExample
{
	public static class TextureLoader
	{
        private static List<Texture2D> _textures = new List<Texture2D>();
       
        public static Texture2D LoadFromFile(string path)
		{
			if (!File.Exists(path))
				throw new FileNotFoundException(path);

            if (_textures.Count(t => t.Path == path) > 0)
                return _textures.Where(t => t.Path == path).ToArray()[0];

			//Loading the texture from the storage, locking the bits
			var textureFile = new Bitmap(path);
		
			textureFile.RotateFlip(RotateFlipType.RotateNoneFlipY);
			var textureDimensions = new Rectangle(0, 0, textureFile.Width, textureFile.Height);

			var textureData = textureFile.LockBits(
				textureDimensions,
				ImageLockMode.ReadOnly,
				textureFile.PixelFormat);

			//Setting up the open GL texture
			var textureResult = new Texture2D(GL.GenTexture(), textureDimensions.Width, textureDimensions.Height, path);
			textureResult.Enable();

			GL.TexImage2D(
				TextureTarget.Texture2D,
				0,
				GetInternalPixelFormatBy(textureFile.PixelFormat),
				textureData.Width,
				textureData.Height,
				0,
				GetInputPixelFormatBy(textureFile.PixelFormat),
				PixelType.UnsignedByte,
				textureData.Scan0);

			textureFile.UnlockBits(textureData);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.Clamp);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.Clamp);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

			textureResult.Disable();
            _textures.Add(textureResult);
			return textureResult;
		}


	    public static List<Sprite> LoadAnimationImages(string path, int from, int to, ImageExtension extension)
	    {
            var returnList = new List<Sprite>();
            for(int i = from; i <= to; i++)
            {
                string filePath = path + i + "." + extension.ToString();
                if(File.Exists(filePath))
                    returnList.Add(new Sprite(LoadFromFile(filePath)));
            }
	        return returnList;
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

    public enum ImageExtension
    {
        png,
        jpg
    }
}
