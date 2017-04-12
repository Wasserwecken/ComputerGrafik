﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using OpenTK.Input;
using SimeraExample.Code;
using Simput;

namespace SimeraExample
{
	public class GameBase
	{
		private PreparedGameWindow Window { get; }
		private Input InputActions { get; set; }

	    private Level Level1 { get; set; }
        

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
            Level1 = new Level();
            Level1.InitTestData();


      //      SpriteTrophy = new Sprite(TextureLoader.LoadFromFile("Pics/trophy.png"));
      //      SpriteDirtEndLeft = new Sprite(TextureLoader.LoadFromFile("Pics/dirt_end_left.png"));
      //      SpriteDirtEndRight = new Sprite(TextureLoader.LoadFromFile("Pics/dirt_end_right.png"));
      //      SpriteDirtMiddle = new Sprite(TextureLoader.LoadFromFile("Pics/dirt_middle.png"));


		    //var anList = TextureLoader.LoadAnimationImages("Pics/Worm/frame-", 1, 8, ImageExtension.png);
      //      AnimatedSprite = new AnimatedSprite(anList, 1);

		}

		private void Window_UpdateFrame(object sender, FrameEventArgs e)
		{
			Window.Camera.Position = Vector2.Add(Window.Camera.Position, new Vector2(InputActions.PositionX, InputActions.PositionY));
			Window.Camera.Rotation = InputActions.Rotation;
			Window.Camera.Zoom = InputActions.Scale;

			if (InputActions.Reset)
				Window.Camera.Position = Vector2.Zero;
		}

		private void Window_RenderFrame(object sender, FrameEventArgs e)
		{
			//SpriteTrophy.Draw(Vector2.Zero, new Vector2(0.005f));
			//SpriteDirtEndLeft.Draw(new Vector2(-0.5f, -0.5f), new Vector2(0.005f));
			//SpriteDirtMiddle.Draw(new Vector2(0f, -0.5f), new Vector2(0.005f));
			//SpriteDirtEndRight.Draw(new Vector2(0.5f, -0.5f), new Vector2(0.005f));
   //         AnimatedSprite.Draw(new Vector2(-1.5f, -1f), new Vector2(0.005f));
		 
            Level1.Draw();

            Console.Clear();
			Console.WriteLine("Camera:");
			Console.WriteLine("\tPosition: X {0} | Y {1}", Window.Camera.Position.X, Window.Camera.Position.Y);
			Console.WriteLine("\tZoom: {0}", Window.Camera.Zoom);
			Console.WriteLine("\tRotation: {0}", Window.Camera.Rotation);
			Console.WriteLine("\nInput:");
			Console.WriteLine("\tLast device: {0}", InputActions.LastInputDeviceDescription);
			Console.WriteLine("\t:Device Id: {0}", InputActions.LastInputDeviceId);
		}
	}
}