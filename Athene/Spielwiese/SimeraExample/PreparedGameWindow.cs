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
		private Input InputActions { get; set; }

		public PreparedGameWindow()
		{
			InputActions = new Input();
			
			var inputLayout = new InputLayout<Input>(InputActions);
			inputLayout.AddMappingGamePad(0, i => i.Triggers.Left, a => a.Scale, i => 1 - i);
			inputLayout.AddMappingGamePad(0, i => i.ThumbSticks.Left, a => a.OffsetX, i => i.X);
			inputLayout.AddMappingGamePad(0, i => i.ThumbSticks.Left, a => a.OffsetY, i => i.Y);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.ClearColor(Color.CornflowerBlue);
			
			base.OnRenderFrame(e);

			SwapBuffers();
		}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="e"></param>
		//protected override void OnResize(EventArgs e)
		//{
		//	GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

		//	Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(((float)Math.PI / 4), Width / (float)Height, 1.0f, 64.0f);
		//	GL.MatrixMode(MatrixMode.Projection);
		//	GL.LoadMatrix(ref projection);

		//	base.OnResize(e);
		//}
	}
}
