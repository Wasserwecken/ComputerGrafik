using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Lib.LevelLoader
{
	/// <summary>
	/// Represents any object in the level
	/// </summary>
	public class LevelObject
	{
		/// <summary>
		/// Position of the object in the level
		/// </summary>
		public Vector2 Position { get; set; }
	}
}
