using OpenTK;

namespace Lib.Logic
{
	public class ObjectProperties
	{
		/// <summary>
		/// Interia of the object. 1 is for instant movment, as higher the value more "sliding"
		/// </summary>
		public Vector2 Interia { get; set; }

		/// <summary>
		/// Mass of the object
		/// </summary>
		public float Mass { get; set; }
	}
}
