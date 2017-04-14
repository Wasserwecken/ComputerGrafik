using System.Diagnostics;
using System.Timers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lib.Visuals.Camera
{
	public class Camera
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
		/// Position where the movement order originally started
		/// </summary>
		private Vector2 PositionSource { get; set; }

		/// <summary>
		/// Destination of the movement requirement
		/// </summary>
		private Vector2 PositionDestination { get; set; }
		
		/// <summary>
		/// Current position of the camera
		/// </summary>
		private Vector2 PositionCurrent { get; set; }

		/// <summary>
		/// Type of the transition to the new position
		/// </summary>
		private TransitionType MovementType { get; set; }
		
		/// <summary>
		/// Stopwatch to measure animation time
		/// </summary>
		private Stopwatch AnimationTimer { get; set; }

		/// <summary>
		/// Time to reach the new destination position
		/// </summary>
		private int AnimatonTime { get; set; }


		/// <summary>
		/// Initialises the camera with defualt values
		/// </summary>
		public Camera()
		{
			PositionCurrent = Vector2.Zero;
			Rotation = 0;
			Zoom = 1;
			AxisSize = 10;
			AnimationTimer = new Stopwatch();
		}

		/// <summary>
		/// Initialises the view
		/// </summary>
		public Camera(Vector2 startPosition, float startZoom, float startRotation, int axisSize)
		{
			PositionCurrent = startPosition;
			Rotation = startRotation;
			Zoom = startZoom;
			AxisSize = axisSize;
			AnimationTimer = new Stopwatch();
		}

		/// <summary>
		/// Moves the camera to the given position
		/// </summary>
		public void MoveTo(Vector2 newPosition, TransitionType type, int animationTime)
		{
			PositionSource = PositionCurrent;
			PositionDestination = newPosition;
			MovementType = type;
			AnimatonTime = animationTime;

			AnimationTimer.Restart();
		}

		/// <summary>
		/// Applies the proteries of the view to the camera
		/// </summary>
		public void ApplyTransform(float aspectRatio)
		{
			CalculateCurrentPosition();

			var transform = Matrix4.Identity;
			transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-PositionCurrent.X / aspectRatio, -PositionCurrent.Y, 0));
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

			if (AnimationTimer.ElapsedMilliseconds >= AnimatonTime)
			{
				PositionCurrent = PositionDestination;
				AnimationTimer.Stop();
			}
			else if (AnimatonTime > 0 && AnimationTimer.ElapsedMilliseconds > 0)
			{
				var transitionProgress = (float)AnimationTimer.ElapsedMilliseconds / AnimatonTime;
				var transitionFactor = GetTransitionFactor(MovementType, transitionProgress);
				var distance = (PositionDestination - PositionSource);

				PositionCurrent = PositionSource + (distance * transitionFactor);
			}
		}

		/// <summary>
		/// Gets for the x values between 0 and 1 the fitting easing y value
		/// </summary>
		/// <param name="type"></param>
		/// <param name="x"></param>
		/// <returns></returns>
		private float GetTransitionFactor(TransitionType type, float x)
		{
			switch(type)
			{
				case TransitionType.Instant:
					return 1;

				case TransitionType.Linear:
					return x;

				case TransitionType.QuadraticInOut:
					return (x * x) / ((2 * x * x) - (2 * x) + 1);

				case TransitionType.QuadraticOut:
					return -((x - 1) * (x - 1) * (x - 1) * (x - 1)) + 1;

				case TransitionType.CubicInOut:
					return (x * x * x) / ((3 * x * x) - (3 * x) + 1);

				default:
					return 0;
			}
		}
	}
}
