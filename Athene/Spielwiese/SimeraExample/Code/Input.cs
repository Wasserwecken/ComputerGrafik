using Lib.Input;
using OpenTK.Input;

namespace SimeraExample
{
	public class Input
		: IInputLayoutActions
	{
		public float Vertical { get; set; }
		public float Horizontal { get; set; }

		public InputDeviceType LastInputDevice { get; set; }
		public string LastInputDeviceDescription { get; set; }
		public int LastInputDeviceId { get; set; }

		public Input()
		{
		}
	}
}
