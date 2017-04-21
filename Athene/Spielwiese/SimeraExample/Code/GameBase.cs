using Lib.Visuals.Graphics;
using OpenTK;
using OpenTK.Input;
using System;
using Lib.Input;
using Lib.Logic;
using System.Collections.Generic;
using Lib.LevelLoader;
using Lib.LevelLoader.LevelItems;
using Lib.Visuals.Window;

namespace SimeraExample
{
	public class GameBase
	{
		private GameWindowBase Window { get; }
		private InputActionObject InputActions { get; set; }

		private SpriteStatic SpriteTest { get; set; }

		

	    private Level Level { get; set; }
		private LevelItemPlayer Player1 { get; set; }

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
		    Level = LevelLoader.LoadLevel(8);


			SpriteAnimated playerSprite = new SpriteAnimated();
			playerSprite.AddAnimation("Pics/Worm/walk", 1000);
			playerSprite.AddAnimation("Pics/Worm/idle", 1000);
			playerSprite.StartAnimation("walk");

			Player1 = new LevelItemPlayer(Vector2.Zero, BlockType.Solid, null, playerSprite);
		}

		private void Window_UpdateFrame(object sender, FrameEventArgs e)
		{
			Player1.UpdateLogic();
			
			//setting the camera
			Window.Camera.MoveTo(Player1.ViewPoint);
		}
		

		private void Window_RenderFrame(object sender, FrameEventArgs e)
		{
            Level.Draw();
			Player1.Draw();
        }
	}
}