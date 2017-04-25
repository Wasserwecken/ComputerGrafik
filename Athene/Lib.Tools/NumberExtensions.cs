using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Tools
{
	public static class NumberExtensions
	{
		/// <summary>
		/// Limits a number to a given range
		/// </summary>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static float LimitToRange(this float value, float min, float max)
		{
			if (value < min)
				return min;

			if (value > max)
				return max;

			return value;
		}



		/// <summary>
		/// /
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="substractValue"></param>
		/// <returns></returns>
		public static float ReduceToZero(this float origin, float substractValue)
		{
			float newOriginRelative = Math.Abs(origin) - Math.Abs(substractValue);
			newOriginRelative = newOriginRelative < 0 ? 0 : newOriginRelative;

			return newOriginRelative * Math.Sign(origin);
		}

	}
}
