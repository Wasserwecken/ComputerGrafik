using OpenTK.Input;

namespace Simput
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
		/// The last device that triggered an input
		/// </summary>
		string LastInputDeviceDescription { get; set; }

		/// <summary>
		/// The last device that triggered an input
		/// </summary>
		int LastInputDeviceId { get; set; }

	}
}
