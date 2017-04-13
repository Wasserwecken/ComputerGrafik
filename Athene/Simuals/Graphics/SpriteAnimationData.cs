using System;
using System.Collections.Generic;
using System.IO;
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
		/// Calculates the time how long a frame will last on the screen
		/// </summary>
		public int TimePerFrame { get; set; }

		/// <summary>
		/// Assigned textures for animation
		/// </summary>
		public List<Texture> AnimationTextures { get; set; }

		/// <summary>
		/// Initialises animation data. the name of the given directory will be used as animation name
		/// </summary>
		/// <param name="path"></param>
		/// <param name="playbackTime"></param>
		public SpriteAnimationData(string path, int playbackTime)
		{
			Name = new DirectoryInfo(path).Name;
			AnimationTextures = TextureManager.GetAnimationTextures(path);
			PlaybackTime = playbackTime;
			TimePerFrame = PlaybackTime / AnimationTextures.Count;
		}
	}
}
