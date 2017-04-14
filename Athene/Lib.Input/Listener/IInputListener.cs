using Lib.Input.Device;
using Lib.Input.Mapping;
using System.Collections.ObjectModel;

namespace Lib.Input.Listener
{
	/// <summary>
	/// Defines a listener which processes inputs from devices
	/// </summary>
	internal interface IInputListener
	{
		/// <summary>
		/// Mapping, on wich input actions the listener will react and modify the game actions
		/// </summary>
		ObservableCollection<IInputMapItem> InputMapping { get; set; }

		/// <summary>
		/// Object which contains the actions and will be modified
		/// </summary>
		IInputLayoutActions InputActions { get; set; }

		/// <summary>
		/// Processes an input which is triggered by a device
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="triggeredMapping"></param>
		/// <param name="inputValue"></param>
		void ProcessInput(IInputDeviceSimput sender, IInputMapItem triggeredMapping, object inputValue);
	}
}
