using Lib.Visuals.Graphics;
using OpenTK;
using OpenTK.Input;
using System;
using Lib.Input;
using Lib.Logic;
using System.Collections.Generic;
using Lib.LevelLoader;
using Lib.Visuals.Window;

namespace SimeraExample
{
	public class GameBase
	{
		private GameWindowBase Window { get; }
		private InputActionObject InputActions { get; set; }

		private SpriteStatic SpriteTest { get; set; }

		

	    private Level Level { get; set; }
		private FakePlayer Player { get; set; }

        public GameBase()
		{
			InputActions = new InputActionObject();
			var layout = new InputLayout<InputActionObject>(InputActions);
			layout.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveLeft, (inval, curval) => inval.Length > 0.01 && inval.X > 0 ? -inval.X : 0);
			layout.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveRight, (inval, curval) => inval.Length > 0.01 && inval.X < 0 ? inval.X : 0);
			layout.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveUp, (inval, curval) => inval.Length > 0.01 && inval.Y > 0 ? inval.Y : 0);
			layout.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveDown, (inval, curval) => inval.Length > 0.01 && inval.Y < 0 ? -inval.Y : 0);
			layout.AddMappingGamePad(0, pad => pad.Buttons.A, inp => inp.Jump, (inval, curval) => inval == ButtonState.Pressed);

			layout.AddMappingKeyboard(Key.Left, inp => inp.MoveLeft, (inval, curval) => inval ? + 1 : 0);
			layout.AddMappingKeyboard(Key.Right, inp => inp.MoveRight, (inval, curval) => inval ? + 1 : 0);
			layout.AddMappingKeyboard(Key.Up, inp => inp.MoveUp, (inval, curval) => inval ? + 1 : 0);
			layout.AddMappingKeyboard(Key.Down, inp => inp.MoveDown, (inval, curval) => inval ? + 1 : 0);
			layout.AddMappingKeyboard(Key.Space, inp => inp.Jump, (inval, curval) => inval);


			Window = new GameWindowBase();

			Window.Load += Window_Load;
			Window.UpdateFrame += Window_UpdateFrame;
			Window.RenderFrame += Window_RenderFrame;
            Window.Run(60);
           
        }


		private void Window_Load(object sender, EventArgs e)
		{
			InputActions.PropertyChanged += InputActions_PropertyChanged;
		    Level = LevelLoader.LoadLevel(8);
			Player = new FakePlayer();
		}

		private void InputActions_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (InputActions.Jump)
				Player.Jump();
		}

		private void Window_UpdateFrame(object sender, FrameEventArgs e)
		{
			var moveDirection = new Vector2(InputActions.MoveRight - InputActions.MoveLeft, InputActions.MoveUp - InputActions.MoveDown);

			Player.Move(moveDirection.X, moveDirection.Y);
			Player.ExecuteLogic();

			//offset for the camera for a better view
			float maxOffset = 1f;

			var x = Player.Position.X + (maxOffset * moveDirection.X);
			var y = Player.Position.Y + (maxOffset * moveDirection.Y);

			//setting the camera
			Window.Camera.MoveTo(new Vector2(x, y));
		}
		

		private void Window_RenderFrame(object sender, FrameEventArgs e)
		{
            Level.Draw();
			Player.Draw();

			////Console.Clear();
			////Console.WriteLine("Player:");
			////Console.WriteLine("\tPosition X: {0}", Player.Position.X);
			////Console.WriteLine("\tPosition Y: {0}", Player.Position.Y);
			////Console.WriteLine("Energy:");
			////Console.WriteLine("\tX: {0}", Player.Energy.X);
			////Console.WriteLine("\tY: {0}", Player.Energy.Y);
			////Console.WriteLine("Force:");
			////Console.WriteLine("\tX: {0}", Player.Force.X);
			////Console.WriteLine("\tY: {0}", Player.Force.Y);
		}
	}
}