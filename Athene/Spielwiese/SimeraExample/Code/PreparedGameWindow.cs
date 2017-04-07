using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simput.Mapping;
using Simput;

namespace SimeraExample
{
	public class PreparedGameWindow : GameWindow
	{
		/// <summary>
		/// Camera for the scenery
		/// </summary>
		public View Camera { get; set; }

		/// <summary>
		/// Initialises the game window
		/// </summary>
		public PreparedGameWindow()
		{
			Camera = new View(Vector2.Zero, 1, 0);

			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			Camera.ApplyTransform();

			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.ClearColor(Color.CornflowerBlue);

			base.OnRenderFrame(e);

			SwapBuffers();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			
			GL.Viewport(0, 0, Width, Height);
			GL.Ortho(0, Width, 0, Height, 0f, 1f);
		}
	}
}
