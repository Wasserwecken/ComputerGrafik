using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Visuals.Camera
{
	/// <summary>
	/// Defines the flow of a transition
	/// http://gizma.com/easing/
	/// </summary>
	public enum TransitionType
	{
		/// <summary>
		/// Moves instant to another point
		/// </summary>
		Instant,

		/// <summary>
		/// Linear movement
		/// </summary>
		Linear,

		/// <summary>
		/// Quadric acceleration and then quadric stop
		/// </summary>
		QuadraticInOut,

		/// <summary>
		/// 
		/// </summary>
		QuadraticOut,

		/// <summary>
		/// Like quadricInOut, but harder
		/// </summary>
		CubicInOut
	}
}
