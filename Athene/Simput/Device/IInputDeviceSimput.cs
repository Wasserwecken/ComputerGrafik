using OpenTK.Input;
using Simput.Listener;

namespace Simput.Device
{
	/// <summary>
	/// Input device for simput
	/// </summary>
	internal interface IInputDeviceSimput
		: IInputDevice
	{
		/// <summary>
		/// Registers a listener for the device
		/// </summary>
		/// <param name="listener"></param>
		void RegisterListener(IInputListener listener);
	}
}
