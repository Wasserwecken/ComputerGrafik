using Lib.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Lib.Visuals.Graphics
{
	/// <summary>
	/// Basis functionality for a sprite
	/// </summary>
	public class SpriteBase
	{
		/// <summary>
		/// Flips the texture of the sprite on the Y axis
		/// </summary>
		public bool FlipTextureHorizontal { get; set; }

		/// <summary>
		/// Flips the texture of the sprite on the Y axis
		/// </summary>
		public bool FlipTextureVertical { get; set; }

        /// <summary>
        /// Size of the sprite
        /// </summary>
        public Vector2 Size { get; private set; }
        

        /// <summary>
        /// Coordinates where the texture is placed on the square
        /// </summary>
        private Vector2[] TextureCoordinates { get; set; }

        /// <summary>
        /// Basic coordinates which defines the size of the sprite
        /// </summary>
        private Vector2[] BaseVertecieCoordinates { get; set; }

        /// <summary>
        /// Watch to stop the tamive for a scroll animation
        /// </summary>
        private Stopwatch ScrollTimer { get; set; }

        /// <summary>
        /// Normalized vector for the scroll direction
        /// </summary>
        private Vector2 ScrollDirection { get; set; }

        /// <summary>
        /// Length of one cycle for scrolling the texture in milliseconds
        /// </summary>
        private float ScrollCycleDuration { get; set; }


        /// <summary>
        /// Initialises a sprite
        /// </summary>
        public SpriteBase(Vector2 size)
		{
			FlipTextureHorizontal = false;
			FlipTextureVertical = true;
            SetTextureCoordinates(0, 0, 1, 1);
            SetSize(size);
		}
        

        /// <summary>
        /// Sets specific coordinates of the texture
        /// </summary>
        /// <param name="textureCoordinates"></param>
        public void SetTextureCoordinates(float x, float y, float width, float height)
        {
            TextureCoordinates = new[]
            {
                new Vector2(x, y),
                new Vector2(x, y + height),
                new Vector2(x + width, y + height),
                new Vector2(x + width, y)
            };
        }

        /// <summary>
        /// Sets a new size for the sprite
        /// </summary>
        /// <param name=""></param>
        public void SetSize(Vector2 size)
        {
            Size = size;
            BaseVertecieCoordinates = new[]
            {
                new Vector2(0, 0),
                new Vector2(0, size.Y),
                new Vector2(size.X, size.Y),
                new Vector2(size.X, 0)
            };
        }
        
        /// <summary>
        /// Draws the sprite on the screen
        /// </summary>
        public void Draw(Vector2 position, Vector2 scale, Texture spriteTexture)
        {
            spriteTexture.Enable();
            GL.Begin(PrimitiveType.Quads);

            for (int index = 0; index < 4; index++)
            {
                SetTextureCoordinate(TextureCoordinates[index]);
                SetVertexPosition(BaseVertecieCoordinates[index], position, scale);
            }

            GL.End();
            spriteTexture.Disable();
        }

        /// <summary>
        /// Starts endless scrolling the texture in a given direction and speed
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="cycleDuration">time in milliseconds</param>
        public void StartTextureScroll(Vector2 direction, float cycleDuration)
        {
            ScrollCycleDuration = cycleDuration;
            ScrollDirection = direction;

            ScrollTimer = new Stopwatch();
            ScrollTimer.Start();
        }

        /// <summary>
        /// Stop scrolling the texture and draws it as usual
        /// </summary>
        public void StopTextureScroll()
        {
            ScrollCycleDuration = 0;
            ScrollDirection = Vector2.Zero;
            ScrollTimer = null;
        }

        /// <summary>
        /// Sets the texture coordinate for a singe vertex
        /// </summary>
        /// <param name="vertex"></param>
        private void SetTextureCoordinate(Vector2 vertex)
        {
            //flipping the texture
            float texturePositionY;
            if (FlipTextureVertical)
                texturePositionY = 1 - vertex.Y;
            else
                texturePositionY = vertex.Y;

            float texturePositionX;
            if (FlipTextureHorizontal)
                texturePositionX = 1 - vertex.X;
            else
                texturePositionX = vertex.X;

            //move the texture for a "scroll"-animation if activated
            if (ScrollTimer != null)
            {
                float animationProcess = ((ScrollTimer.ElapsedMilliseconds % ScrollCycleDuration) / ScrollCycleDuration);
                texturePositionX += ScrollDirection.X * animationProcess;
                texturePositionY += ScrollDirection.Y * animationProcess;
            }

            GL.TexCoord2(new Vector2(texturePositionX, texturePositionY));
        }

	    /// <summary>
	    /// Sets the vertex position for a singe vertex in the
	    /// </summary>
	    /// <param name="vertex"></param>
	    /// <param name="position"></param>
	    /// <param name="scale"></param>
	    /// <param name="textureWidth"></param>
	    /// <param name="textureHeight"></param>
	    private void SetVertexPosition(Vector2 vertex, Vector2 position, Vector2 scale)
        {
            vertex *= scale;
            vertex += position;

            GL.Vertex2(vertex);
        }
	}
}
