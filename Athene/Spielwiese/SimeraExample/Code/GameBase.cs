using Lib.Visuals.Graphics;
using OpenTK;
using OpenTK.Input;
using System;
using Lib.Input;
using Lib.Input.Mapping;
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
		private LevelItemPlayerActions InputActions { get; set; }

		private SpriteStatic SpriteTest { get; set; }

		

	    private Level Level { get; set; }

        public GameBase()
		{
			Window = new GameWindowBase();

			Window.Load += Window_Load;
			Window.UpdateFrame += Window_UpdateFrame;
			Window.RenderFrame += Window_RenderFrame;
            Window.Run(60);  
        }


		private void Window_Load(object sender, EventArgs e)
		{
		    Level = LevelLoader.LoadLevel(9);

		
		}

		private void Window_UpdateFrame(object sender, FrameEventArgs e)
		{
			//Player1.UpdateLogic();
			Level.UpdateLogic();
			//setting the camera
			Window.Camera.MoveTo(Level.Players[0].ViewPoint);
		}
		

		private void Window_RenderFrame(object sender, FrameEventArgs e)
		{
            //Console.Clear();
            Level.Draw();
			//Player1.Draw();
            //Console.WriteLine("Player Position: (" + Player1.Position.X + "|" + Player1.Position.Y + ")");

            
        }
	}
}