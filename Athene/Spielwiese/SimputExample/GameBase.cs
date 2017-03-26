using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace SimputExample
{
	public class GameBase
	{
		private GameWindow Window;
		

		public GameBase(GameWindow window)
		{
			this.Window = window;

			Window.Load += Window_Load;
			Window.UpdateFrame += Window_UpdateFrame;
			Window.RenderFrame += Window_RenderFrame;
		}

		private void Window_Load(object sender, EventArgs e)
		{
			
		}

		private void Window_UpdateFrame(object sender, FrameEventArgs e)
		{

		}

		private void Window_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.ClearColor(Color.Black);
			GL.Clear(ClearBufferMask.ColorBufferBit);

			Window.SwapBuffers();
		}
	}
}