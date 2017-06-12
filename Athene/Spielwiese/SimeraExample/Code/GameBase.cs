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
			Window = new GameWindowBase(4, 15);
            //Window.WindowState = WindowState.Fullscreen;

			Window.Load += Window_Load;
			Window.UpdateFrame += Window_UpdateFrame;
			Window.RenderFrame += Window_RenderFrame;
            Window.Run(60);  
        }


		private void Window_Load(object sender, EventArgs e)
		{
            int level = 999;
            XmlLevel levelData = LevelLoader.LoadFromXml(@"Level\Level" + level + ".xml");
            Level = new Level(levelData);
        }

		private void Window_UpdateFrame(object sender, FrameEventArgs e)
		{
            //calculate all logic within the lever (players / enemies / etc)
			Level.UpdateLogic(Window.Camera.FOV);

            float axisSize = 0f;
            float playersViewRange = 7f;

            if (Window.AspectRatio > 0)
            {
                float distanceX = (Level.MaxPlayersDistance.X + playersViewRange) / Window.AspectRatio;
                float distanceY = (Level.MaxPlayersDistance.Y + playersViewRange);

                axisSize = Math.Max(distanceX, distanceY);
            }

            axisSize /= 2; //divided because there are 2 axes in one direction
            Window.Camera.SetValues(Level.PlayersCenter, axisSize);
		}
		

		private void Window_RenderFrame(object sender, FrameEventArgs e)
		{
            Level.Draw();
        }
	}
}