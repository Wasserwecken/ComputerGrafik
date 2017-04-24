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
        private SpriteStatic SpriteTest2 { get; set; }
        private SpriteStatic SpriteTest3 { get; set; }



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

            SpriteTest = new SpriteStatic("Pics/water_top.png");
            SpriteTest.StartTextureScroll(new Vector2(1, 0), 5000);

            SpriteTest2 = new SpriteStatic("Pics/water_deep.png");
            SpriteTest2.StartTextureScroll(new Vector2(1, 0), 5000);

            SpriteTest3 = new SpriteStatic("Pics/forest_pack_35.png");
            SpriteTest3.StartTextureScroll(new Vector2(-1, 0), 1000);
        }

		private void Window_UpdateFrame(object sender, FrameEventArgs e)
		{
			Level.UpdateLogic();
			//setting the camera
			Window.Camera.MoveTo(Level.Players[0].ViewPoint);
		}
		

		private void Window_RenderFrame(object sender, FrameEventArgs e)
		{
            Level.Draw();

            SpriteTest.Draw(new Vector2(1, 3), Vector2.One);
            SpriteTest.Draw(new Vector2(2, 3), Vector2.One);
            SpriteTest.Draw(new Vector2(3, 3), Vector2.One);

            SpriteTest2.Draw(new Vector2(1, 2), Vector2.One);
            SpriteTest2.Draw(new Vector2(2, 2), Vector2.One);
            SpriteTest2.Draw(new Vector2(3, 2), Vector2.One);

            SpriteTest2.Draw(new Vector2(1, 1), Vector2.One);
            SpriteTest2.Draw(new Vector2(2, 1), Vector2.One);
            SpriteTest2.Draw(new Vector2(3, 1), Vector2.One);

            SpriteTest3.Draw(new Vector2(-1, 1), Vector2.One);
            SpriteTest3.Draw(new Vector2(-2, 1), Vector2.One);
            SpriteTest3.Draw(new Vector2(-1, 0), Vector2.One);
            SpriteTest3.Draw(new Vector2(-2, 0), Vector2.One);
        }
	}
}