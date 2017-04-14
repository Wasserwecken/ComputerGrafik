using System.Diagnostics;
using System.Timers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lib.Visuals.Window
{
	public class GameCamera
	{
		/// <summary>
		/// Rotation of the camera, negative values gonne rotate clockwise
		/// </summary>
		public float Rotation { get; set; }

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
		public float CameraDelay { get; set; }
		

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
			Rotation = 0;
			Zoom = 1;
			PositionCurrent = startPosition;
			AxisSize = axisSize;
			CameraDelay = cameraDelay;
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
			transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-Rotation));
			transform = Matrix4.Mult(transform, Matrix4.CreateScale(Zoom, Zoom, 1));

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.MultMatrix(ref transform);
			GL.Ortho(-AxisSize * aspectRatio, AxisSize * aspectRatio, -AxisSize, AxisSize, -1f, 1f);
		}

		/// <summary>
		/// Calculates the current camera position, based of the movement animation
		/// </summary>
		private void CalculateCurrentPosition()
		{
			if (PositionCurrent == PositionDestination)
				return;

			var distance = PositionDestination - PositionCurrent;
			PositionCurrent = PositionCurrent + (distance / CameraDelay);
		}
	}
}
