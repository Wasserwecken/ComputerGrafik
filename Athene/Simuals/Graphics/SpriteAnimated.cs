using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Simuals.Graphics
{
	/// <summary>
	/// Sprite with a texture animation
	/// </summary>
    public class SpriteAnimated
		: SpriteBase
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
		/// Sets the animation that should be played back
		/// </summary>
		/// <param name="animationName"></param>
		public void SetAnimation(string animationName)
		{
			ActiveAnimation = Animations.FirstOrDefault(f => f.Name == animationName);
		}

		/// <summary>
		/// Draws the texture on the screen
		/// </summary>
		/// <param name="position"></param>
		/// <param name="scale"></param>
		public void Draw(Vector2 position, Vector2 scale)
        {
			var normalizedDeltaTime = (TimeSource.ElapsedMilliseconds % ActiveAnimation.PlaybackTime);
			var frame = normalizedDeltaTime 

			if (!_timeSource.IsRunning)
				_timeSource.Start();



			var sprite = GetTextureFromTime((float)_timeSource.Elapsed.TotalSeconds);
            sprite.Draw(position, scale);
        }
    }
}
