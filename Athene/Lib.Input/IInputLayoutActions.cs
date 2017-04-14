using OpenTK.Input;

namespace Lib.Input
{
	/// <summary>
	/// Defines a object which can be modified by simput
	/// </summary>
	public interface IInputLayoutActions
	{
		/// <summary>
		/// The last device that triggered an input
		/// </summary>
		InputDeviceType LastInputDevice { get; set; }

		/// <summary>
		/// The last device description that triggered an input
		/// </summary>
		string LastInputDeviceDescription { get; set; }

		/// <summary>
		/// The last device id that triggered an input
		/// </summary>
		int LastInputDeviceId { get; set; }

	}
}
