using OpenTK.Input;
using Simput.Listener;

namespace Simput.Device
{
	/// <summary>
	/// Defines a dives which can be used for the simput lib
	/// </summary>
	internal interface IInputDeviceSimput
		: IInputDevice
	{
		/// <summary>
		/// Id of the device
		/// </summary>
		int DeviceId { get; set; }

		/// <summary>
		/// Registers a listener for the device
		/// </summary>
		/// <param name="listener"></param>
		void RegisterListener(IInputListener listener);
	}
}
