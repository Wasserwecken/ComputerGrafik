using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Lib.Visuals.Camera;

namespace SimeraExample
{
	public class PreparedGameWindow : GameWindow
	{
		/// <summary>
		/// Camera for the scenery
		/// </summary>
		public Camera GameCamera { get; set; }

		/// <summary>
		/// Initialises the game window
		/// </summary>
		public PreparedGameWindow()
		{
			GameCamera = new Camera(Vector2.Zero, 1, 0, 2);

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
			GameCamera.ApplyTransform((float)Width / Height);

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
