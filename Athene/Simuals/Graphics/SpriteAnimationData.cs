using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simuals.Graphics
{
	/// <summary>
	/// Data for a sprite animation
	/// </summary>
	public class SpriteAnimationData
	{
		/// <summary>
		/// Name of the animation
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Time in milliseconds for playback of the whole animation
		/// </summary>
		public int PlaybackTime { get; set; }

		/// <summary>
		/// Assigned textures for animation
		/// </summary>
		public List<StaticSprite> AnimationTextures { get; set; }
	}
}
