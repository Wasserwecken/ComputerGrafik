using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace SimeraExample
{
	public class GameBase
	{
		private PreparedGameWindow Window { get; }

		public GameBase()
		{
			Window = new PreparedGameWindow();

			Window.Load += Window_Load;
			Window.UpdateFrame += Window_UpdateFrame;
			Window.RenderFrame += Window_RenderFrame;

			Window.Run(60);
		}

		private void Window_Load(object sender, EventArgs e)
		{
			
		}

		private void Window_UpdateFrame(object sender, FrameEventArgs e)
		{

		}

		private void Window_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Begin(PrimitiveType.Quads);

			GL.Vertex2(0, 0);
			GL.Vertex2(1, 0);
			GL.Vertex2(1, 1);
			GL.Vertex2(0, 1);

			GL.End();
		}
	}
}