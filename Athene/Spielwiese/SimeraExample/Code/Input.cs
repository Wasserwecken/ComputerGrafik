using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using Simput;

namespace SimeraExample
{
	public class Input
		: IInputLayoutActions
	{
		public float Scale { get; set; }
		public float Rotation { get; set; }
		public Vector2 Position { get; set; }
		public bool Reset { get; set; }

		public InputDeviceType LastInputDevice { get; set; }
		public string LastInputDeviceDescription { get; set; }
		public int LastInputDeviceId { get; set; }
	}
}
