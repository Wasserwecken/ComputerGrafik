using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Lib.Visuals.Window
{
    /// <summary>
    /// Extended game window
    /// </summary>
    public class GameWindowBase
        : GameWindow
    {
        /// <summary>
        /// Camera for the scenery
        /// </summary>
        public GameCamera Camera { get; set; }

        /// <summary>
        /// Aspect ratio of the window
        /// </summary>
        public float AspectRatio { get; set; }

		/// <summary>
		/// Initialises the game window
		/// </summary>
		public GameWindowBase(int standardViewableYAxisSize, int cameraDelay)
		{
            WindowState = WindowState.Fullscreen;
			Camera = new GameCamera(Vector2.Zero, standardViewableYAxisSize, cameraDelay);
            AspectRatio = 0;

			//This will be needed to enable transparency
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		/// <summary>
		/// Prepares the render process
		/// </summary>
		/// <param name="e"></param>
		protected override void OnRenderFrame(FrameEventArgs e)
		{
            AspectRatio = (float)Width / Height;
            Camera.ApplyTransform(AspectRatio);

			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.ClearColor(Color.Black);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			base.OnRenderFrame(e);

			SwapBuffers();
		}

		/// <summary>
		/// Handels changes in the windows resolution
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			GL.Viewport(0, 0, Width, Height);
		}
	}
}
