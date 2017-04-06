using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using OpenTK.Input;
using Simput;

namespace SimeraExample
{
	public class GameBase
	{
		private PreparedGameWindow Window { get; }
		private Input InputActions { get; set; }

		private Sprite SpriteTrophy { get; set; }
		private Sprite SpriteDirtEndLeft { get; set; }
		private Sprite SpriteDirtEndRight { get; set; }
		private Sprite SpriteDirtMiddle { get; set; }

		public GameBase()
		{
			InputActions = new Input();
			var layout = new InputLayout<Input>(InputActions);
			layout.AddMappingGamePad(0, pad => pad.Triggers.Right, inp => inp.Scale, val => 1 - val);
			layout.AddMappingGamePad(0, pad => pad.Buttons.A, inp => inp.Reset, val => val == ButtonState.Pressed);
			layout.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.Position, val => val.LengthSquared < 0.001f ? Vector2.Zero : Vector2.Multiply(val, 0.01F));
			layout.AddMappingGamePad(0, pad => pad.ThumbSticks.Right, inp => inp.Rotation, val => val.LengthSquared < 0.5f ? 0 : (float) Math.Atan2(val.X, val.Y));


			Window = new PreparedGameWindow();

			Window.Load += Window_Load;
			Window.UpdateFrame += Window_UpdateFrame;
			Window.RenderFrame += Window_RenderFrame;

			Window.Run(60);
		}

		private void Window_Load(object sender, EventArgs e)
		{
			SpriteTrophy = new Sprite("Pics/trophy.png");
			SpriteDirtEndLeft = new Sprite("Pics/dirt_end_left.png");
			//SpriteDirtEndRight = new Sprite("Pics/dirt_end_right.png");
			//SpriteDirtMiddle = new Sprite("Pics/dirt_middle.png");
		}

		private void Window_UpdateFrame(object sender, FrameEventArgs e)
		{
			Window.Camera.Position = Vector2.Add(Window.Camera.Position, InputActions.Position);
			Window.Camera.Rotation = InputActions.Rotation;
			Window.Camera.Zoom = InputActions.Scale;

			if (InputActions.Reset)
				Window.Camera.Position = Vector2.Zero;
		}

		private void Window_RenderFrame(object sender, FrameEventArgs e)
		{
			SpriteTrophy.Draw(Vector2.Zero, new Vector2(0.005f));
			//SpriteDirtEndLeft.Draw(Vector2.Zero, new Vector2(0.005f));
			//SpriteDirtMiddle.Draw(Vector2.Zero, new Vector2(0.005f));
			//SpriteDirtEndRight.Draw(Vector2.Zero, new Vector2(0.005f));

			Console.Clear();
			Console.WriteLine("Camera:");
			Console.WriteLine("\tPosition: X {0} | Y {1}", Window.Camera.Position.X, Window.Camera.Position.Y);
			Console.WriteLine("\tZoom: {0}", Window.Camera.Zoom);
			Console.WriteLine("\tRotation: {0}", Window.Camera.Rotation);
		}
	}
}