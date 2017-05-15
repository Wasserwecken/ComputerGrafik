using System.Diagnostics;
using System.Timers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Lib.Tools;

namespace Lib.Visuals.Window
{
	public class GameCamera
    {
        /// <summary>
        /// Sets the delay and smooth movement of the camera. 1 is direct movement. if higher, more delay
        /// </summary>
        public float Delay { get; set; }
        
        /// <summary>
        /// Dimensions of the current cameras field of view
        /// </summary>
        public Box2D FOV { get; private set; }

        /// <summary>
        /// Size of the axis to the heigt and bottom of the window
        /// </summary>
        public float AxisSizeCurrent { get; private set; }


        /// <summary>
        /// Size of the axis to the heigt and bottom of the window
        /// </summary>
        private float AxisSizeDestination { get; set; }

        /// <summary>
        /// Destination of the movement requirement
        /// </summary>
        private Vector2 PositionDestination { get; set; }
		
		/// <summary>
		/// Current position of the camera
		/// </summary>
		private Vector2 PositionCurrent { get; set; }


		/// <summary>
		/// Initialises the view
		/// </summary>
		public GameCamera(Vector2 startPosition, float axisSize, float cameraDelay)
		{
			PositionCurrent = startPosition;
            AxisSizeCurrent = axisSize;
			Delay = cameraDelay;
		}

		/// <summary>
		/// Moves the camera to the given position
		/// </summary>
		public void SetValues(Vector2 position, float axisSize)
		{
			PositionDestination = position;
            AxisSizeDestination = axisSize;
		}

		/// <summary>
		/// Applies the proteries of the view to the camera
		/// </summary>
		public void ApplyTransform(float aspectRatio)
		{
            PositionCurrent = CalculateDelayedValue(PositionCurrent, PositionDestination, Delay);
            AxisSizeCurrent = CalculateDelayedValue(AxisSizeCurrent, AxisSizeDestination, Delay);

			var transform = Matrix4.Identity;
			transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-PositionCurrent.X / aspectRatio / AxisSizeCurrent, -PositionCurrent.Y / AxisSizeCurrent, 0));

            GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.MultMatrix(ref transform);

            float axisSizeX = AxisSizeCurrent * aspectRatio;
            float axisSizeY = AxisSizeCurrent;
			GL.Ortho(-axisSizeX, axisSizeX, -axisSizeY, axisSizeY, -1f, 1f);
            
            FOV = new Box2D(
                (-axisSizeX + PositionCurrent.X),
                (-axisSizeY + PositionCurrent.Y),
                (axisSizeX * 2),
                (axisSizeY * 2)
                );
		}


        /// <summary>
        /// Calculates a new value with will trie to approach to the given destination like convergence
        /// </summary>
        /// <param name="current"></param>
        /// <param name="destination"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private Vector2 CalculateDelayedValue(Vector2 current, Vector2 destination, float delay)
        {
            if (current == destination)
                return destination;

            var distance = destination - current;
            return current + (distance / delay);
        }

        /// <summary>
        /// Calculates a new value with will trie to approach to the given destination like convergence
        /// </summary>
        /// <param name="current"></param>
        /// <param name="destination"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private float CalculateDelayedValue(float current, float destination, float delay)
        {
            if (current == destination)
                return destination;

            var distance = destination - current;
            return current + (distance / delay);
        }
	}
}
