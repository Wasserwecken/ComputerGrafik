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
	public static class SpriteLoader
	{
        private static List<Texture2D> _textures = new List<Texture2D>();
        private static List<Sprite> _sprites = new List<Sprite>();

        public static  List<Sprite> LoadedSprites { get { return _sprites; } }

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

			textureResult.Disable();
            _textures.Add(textureResult);
			return textureResult;
		}

	    public static Sprite LoadSprite(string path)
	    {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            var spriteResult = new Sprite(path);
            _sprites.Add(spriteResult);
	        return spriteResult;
	    }


    }
}
