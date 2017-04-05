using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using Simput;

namespace SimeraExample
{
	public class Input
		: IInputLayoutActions
	{
		private float _fovY;

		public float Scale
		{
			get { return _fovY; }
			set
			{
				if (value <= 0)
					_fovY = 0.1f;
				else if (value > 1)
					_fovY = 1f;
				else
					_fovY = value;
			}
		}

		public float OffsetX { get; set; }
		public float OffsetY { get; set; }

		public InputDeviceType LastInputDevice { get; set; }
		public string LastInputDeviceDescription { get; set; }
		public int LastInputDeviceId { get; set; }

		public Input()
		{
			Scale = 0;
		}
	}
}
