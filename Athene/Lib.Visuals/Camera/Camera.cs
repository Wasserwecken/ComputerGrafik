using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lib.Visuals.Camera
{
	public class Camera
	{
		/// <summary>
		/// Current position of the camera
		/// </summary>
		public Vector2 Position { get; set; }

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
		/// Initialises the view
		/// </summary>
		public Camera(Vector2 startPosition, float startZoom, float startRotation, int axisSize)
		{
			Position = startPosition;
			Rotation = startRotation;
			Zoom = startZoom;
			AxisSize = axisSize;
		}

		/// <summary>
		/// Applies the proteries of the view to the camera
		/// </summary>
		public void ApplyTransform(float aspectRatio)
		{
			var transform = Matrix4.Identity;

			transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-Position.X / aspectRatio, -Position.Y, 0));
			transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-Rotation));
			transform = Matrix4.Mult(transform, Matrix4.CreateScale(Zoom, Zoom, 1));

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.MultMatrix(ref transform);
			GL.Ortho(-AxisSize * aspectRatio, AxisSize * aspectRatio, -AxisSize, AxisSize, -1f, 1f);
		}
	}
}
