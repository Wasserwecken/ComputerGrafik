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
		/// Zoom of the camera. 1 is standard
		/// </summary>
		public float Zoom { get; set; }

		/// <summary>
		/// Size of the axis to the heigt and bottom of the window
		/// </summary>
		public int AxisSize { get; set; }

		/// <summary>
		/// Sets the delay and smooth movement of the camera. 1 is direct movement. if higher, more delay
		/// </summary>
		public float Delay { get; set; }

        /// <summary>
        /// Dimensions of the current cameras field of view
        /// </summary>
        public Box2D FOV { get; private set; }
		

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
		public GameCamera(Vector2 startPosition, int axisSize, float cameraDelay)
		{
			Zoom = 0.5f;
			PositionCurrent = startPosition;
			AxisSize = axisSize;
			Delay = cameraDelay;
		}

		/// <summary>
		/// Moves the camera to the given position
		/// </summary>
		public void MoveTo(Vector2 newPosition)
		{
			PositionDestination = newPosition;
		}

		/// <summary>
		/// Applies the proteries of the view to the camera
		/// </summary>
		public void ApplyTransform(float aspectRatio)
		{
			CalculateCurrentPosition();

			var transform = Matrix4.Identity;
			transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-PositionCurrent.X / aspectRatio / AxisSize, -PositionCurrent.Y / AxisSize, 0));
			transform = Matrix4.Mult(transform, Matrix4.CreateScale(Zoom, Zoom, 1));

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.MultMatrix(ref transform);

            float axisSizeX = AxisSize * aspectRatio;
            float axisSizeY = AxisSize;
			GL.Ortho(-axisSizeX, axisSizeX, -axisSizeY, axisSizeY, -1f, 1f);


            axisSizeX = axisSizeX / Zoom;
            axisSizeY = axisSizeY / Zoom;

            FOV = new Box2D(
                (-axisSizeX + PositionCurrent.X),
                (-axisSizeY + PositionCurrent.Y),
                (axisSizeX * 2),
                (axisSizeY * 2)
                );
		}

		/// <summary>
		/// Calculates the current camera position, based of the movement animation
		/// </summary>
		private void CalculateCurrentPosition()
		{
			if (PositionCurrent == PositionDestination)
				return;

			var distance = PositionDestination - PositionCurrent;
			PositionCurrent = PositionCurrent + (distance / Delay);
		}
	}
}
