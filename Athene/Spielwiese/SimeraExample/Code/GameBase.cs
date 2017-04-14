using Lib.Visuals.Graphics;
using OpenTK;
using OpenTK.Input;
using System;
using Lib.Input;

namespace SimeraExample
{
	public class GameBase
	{
		private PreparedGameWindow Window { get; }
		private Input InputActions { get; set; }

		private SpriteAnimated AnimTest { get; set; }
		private SpriteStatic SpriteTest { get; set; }
        

        public GameBase()
		{
			InputActions = new Input();
			var layout = new InputLayout<Input>(InputActions);
			layout.AddMappingGamePad(0, pad => pad.Triggers.Right, inp => inp.Scale, val => 1.2f - val);
			layout.AddMappingGamePad(0, pad => pad.Buttons.A, inp => inp.Reset, val => val == ButtonState.Pressed);
			layout.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.PositionX, val => val.LengthSquared < 0.001f ? 0 : val.X * 0.01f);
			layout.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.PositionY, val => val.LengthSquared < 0.001f ? 0 : val.Y * 0.01f);
			layout.AddMappingGamePad(0, pad => pad.ThumbSticks.Right, inp => inp.Rotation, val => val.LengthSquared < 0.5f ? 0 : (float)Math.Atan2(val.X, val.Y));

			layout.AddMappingKeyboard(Key.Space, inp => inp.Reset, val => val);
			layout.AddMappingKeyboard(Key.Up, inp => inp.PositionY, val => val ? 0.01f : 0);
			layout.AddMappingKeyboard(Key.Down, inp => inp.PositionY, val => val ? -0.01f : 0);
			layout.AddMappingKeyboard(Key.Left, inp => inp.PositionX, val => val ? 0.01f : 0);
			layout.AddMappingKeyboard(Key.Right, inp => inp.PositionX, val => val ? -0.01f : 0);
			

			Window = new PreparedGameWindow();

			Window.Load += Window_Load;
			Window.UpdateFrame += Window_UpdateFrame;
			Window.RenderFrame += Window_RenderFrame;
            Window.Run(60);
           
        }

		private void Window_Load(object sender, EventArgs e)
		{

			AnimTest = new SpriteAnimated();
			AnimTest.AddAnimation("Pics/Worm/idle", 1000);
			AnimTest.AddAnimation("Pics/Worm/walk", 1000);
			AnimTest.StartAnimation("idle");

			SpriteTest = new SpriteStatic("Pics/bigtree.png");
		}

		private void Window_UpdateFrame(object sender, FrameEventArgs e)
		{
			if (InputActions.Reset)
				Window.GameCamera.MoveTo(new Vector2(1, 1), Lib.Visuals.Camera.TransitionType.QuadraticInOut, 2000);
		}

		private void Window_RenderFrame(object sender, FrameEventArgs e)
		{
			AnimTest.Draw(Vector2.Zero, Vector2.One);
			SpriteTest.Draw(Vector2.One, Vector2.One);
		}
	}
}