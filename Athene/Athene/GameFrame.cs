using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Athene
{
	public class GameFrame
	{
		/// <summary>
		/// Window of the game, where all the logic and redering is attached
		/// </summary>
		public GameWindow Window { get; set; }

		/// <summary>
		/// Initialises the game
		/// </summary>
		public GameFrame()
		{
			Window = new GameWindow();

			Window.Load += Window_Load;
			Window.RenderFrame += Window_RenderFrame;
			Window.UpdateFrame += Window_UpdateFrame;

			Window.Run(60); //FPS
		}

		/// <summary>
		/// Occurs when the game starts. All ressource loading and initial configurations will be handled here
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Load(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Occurs when a logic "tick" should be executed. Only the game logic will be handled here
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_UpdateFrame(object sender, FrameEventArgs e)
		{
		}

		/// <summary>
		/// Renders all game objects, no logic will be placed here. just rendering
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_RenderFrame(object sender, FrameEventArgs e)
		{
		}
	}
}
