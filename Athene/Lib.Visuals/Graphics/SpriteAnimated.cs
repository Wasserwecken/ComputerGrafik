using OpenTK;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lib.Visuals.Graphics
{
	/// <summary>
	/// Sprite with a texture animation
	/// </summary>
	public class SpriteAnimated
		: SpriteBase, ISprite
    {
		/// <summary>
		/// Availabe animations for the sprite
		/// </summary>
		public List<SpriteAnimationData> Animations { get; set; }
		
		/// <summary>
		/// Active animation cyclus
		/// </summary>
		private SpriteAnimationData ActiveAnimation { get; set; }
		/// <summary>
		/// Watch to measure the elapsed time for the animations
		/// </summary>
		private Stopwatch TimeSource { get; set; }


		/// <summary>
		/// Initialises a animated sprite
		/// </summary>
		public SpriteAnimated()
		{
			TimeSource = new Stopwatch();
			Animations = new List<SpriteAnimationData>();
		}

		/// <summary>
		/// Starts or unpauses the current setted animation
		/// </summary>
		public void StartAnimation()
		{
			TimeSource.Start();
		}

		/// <summary>
		/// Sets and starts a animation
		/// </summary>
		/// <param name="animationName"></param>
		public void StartAnimation(string animationName)
		{
			ActiveAnimation = Animations.FirstOrDefault(f => f.Name == animationName);
			TimeSource.Restart();
		}

		/// <summary>
		/// Stops the current animation
		/// </summary>
		public void StopAnimation()
		{
			TimeSource.Stop();
		}

		/// <summary>
		/// Adds an animation to the sprite. The animation name will be the name of the directory name
		/// </summary>
		/// <param name="path"></param>
		/// <param name="playbacktime"></param>
		public void AddAnimation(string path, int playbacktime)
		{
			Animations.Add(new SpriteAnimationData(path, playbacktime));
		}

		/// <summary>
		/// Draws the texture on the screen
		/// </summary>
		/// <param name="position"></param>
		/// <param name="scale"></param>
		public void Draw(Vector2 position, Vector2 scale)
        {
			var frame = (int)(TimeSource.ElapsedMilliseconds / ActiveAnimation.TimePerFrame) % ActiveAnimation.AnimationTextures.Count;

			base.Draw(position, scale, ActiveAnimation.AnimationTextures[frame]);
        }
    }
}
