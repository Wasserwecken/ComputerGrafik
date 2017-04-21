using System;

namespace Lib.Logic
{
	/// <summary>
	/// http://gizma.com/easing/
	/// </summary>
	public static class Easing
	{
		/// <summary>
		/// no easing, no acceleration
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float Linear(float time, float duration)
		{
			return time / duration;
		}

		/// <summary>
		///  acceleration until halfway, then deceleration
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float Bezier(float time, float duration)
		{
			return time * time * (3f - (2f * time)) * duration;
		}

		/// <summary>
		/// accelerating from zero velocity
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float QuadraticIn(float time, float duration)
		{
			time /= duration;
			return time * time;
		}

		/// <summary>
		/// decelerating to zero velocity
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float QuadraticOut(float time, float duration)
		{
			time /= duration;
			return -1 * time * (time - 2);
		}

		/// <summary>
		/// acceleration until halfway, then deceleration
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float QuadraticInOut(float time, float duration)
		{
			time /= duration;

			if (time < 1)
				return 0.5f * time * time;

			time--;
			return -0.5f * (time * (time - 2) - 1);
		}

		/// <summary>
		/// accelerating from zero velocity
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float CubicIn(float time, float duration)
		{
			time /= duration;
			return time * time * time;
		}

		/// <summary>
		/// decelerating to zero velocity
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float CubicOut(float time, float duration)
		{
			time /= duration;
			time--;
			return time * time * time + 1;
		}

		/// <summary>
		///  acceleration until halfway, then deceleration
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float CubicInOut(float time, float duration)
		{
			time /= duration / 2;
			if (time < 1)
				return 0.5f * time * time * time;

			time -= 2;
			return 0.5f * (time * time * time) - 2;
		}

		/// <summary>
		/// accelerating from zero velocity
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float QuarticIn(float time, float duration)
		{
			time /= duration;
			return time * time * time * time;
		}

		/// <summary>
		/// decelerating to zero velocity
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float QuarticOut(float time, float duration)
		{
			time /= duration;
			time--;
			return -1 * (time * time * time * time - 1);
		}

		/// <summary>
		/// acceleration until halfway, then deceleration
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float QuarticInOut(float time, float duration)
		{
			time /= duration / 2;
			if (time < 2)
				return 0.5f * time * time * time * time;

			time -= 2;
			return -0.5f * (time * time * time * time - 2);
		}


		/// <summary>
		/// accelerating from zero velocity
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float SinusoidalIn(float time, float duration)
		{
			return -1 * (float)Math.Cos(time / duration * (Math.PI / 2f)) + 1;
		}

		/// <summary>
		/// decelerating to zero velocity
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float SinusoidalOut(float time, float duration)
		{
			return (float)Math.Sin(time / duration * (Math.PI / 2f));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="time"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static float SinusoidalInOut(float time, float duration)
		{
			return 0.5f * ((float)Math.Cos(Math.PI * time / duration) - 1);
		}
	}
}
