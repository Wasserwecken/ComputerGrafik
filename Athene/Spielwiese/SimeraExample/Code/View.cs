using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SimeraExample
{
	public class View
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
		/// 1 is no zoom
		/// </summary>
		public float Zoom { get; set; }

		/// <summary>
		/// Initialises the view
		/// </summary>
		public View(Vector2 startPosition)
		{
			Position = startPosition;
		}

		/// <summary>
		/// Initialises the view
		/// </summary>
		public View(Vector2 startPosition, float startZoom, float startRotation)
			: this(startPosition)
		{
			Rotation = startRotation;
			Zoom = startZoom;
		}

		/// <summary>
		/// Applies the proteries of the view to the camera
		/// </summary>
		public void ApplyTransform()
		{
			var transform = Matrix4.Identity;

			transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-Position.X, -Position.Y, 0));
			transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-Rotation));
			transform = Matrix4.Mult(transform, Matrix4.CreateScale(Zoom, Zoom, 1));

			GL.LoadIdentity();
			GL.MultMatrix(ref transform);
		}
	}
}
