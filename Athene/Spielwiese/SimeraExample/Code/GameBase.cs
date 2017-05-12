using Lib.Level;
using Lib.LevelLoader;
using Lib.LevelLoader.LevelItems;
using Lib.LevelLoader.Xml;
using Lib.Visuals.Graphics;
using Lib.Visuals.Window;
using OpenTK;
using System;

namespace SimeraExample
{
    public class GameBase
	{
		private GameWindowBase Window { get; }

        private SpriteStatic SpriteTest { get; set; }
        private SpriteStatic SpriteTest2 { get; set; }
        private SpriteStatic SpriteTest3 { get; set; }



        private Level Level { get; set; }

        public GameBase()
		{
			Window = new GameWindowBase(5, 15);

			Window.Load += Window_Load;
			Window.UpdateFrame += Window_UpdateFrame;
			Window.RenderFrame += Window_RenderFrame;
            Window.Run(60);  
        }


		private void Window_Load(object sender, EventArgs e)
		{
            int level = 11;
            XmlLevel levelData = LevelLoader.LoadFromXml(@"Level\Level" + level + ".xml");
            Level = new Level(levelData);
        }

		private void Window_UpdateFrame(object sender, FrameEventArgs e)
		{
			Level.UpdateLogic();
			//setting the camera
			Window.Camera.MoveTo(Level.Players[0].ViewPoint);
		}
		

		private void Window_RenderFrame(object sender, FrameEventArgs e)
		{
            Level.Draw(Window.Camera.FOV);
        }
	}
}