using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simput.Device;
using Simput.Mapping;

namespace Simput.Listener
{
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
		/// Processes an input which will be triggered by a device
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="triggeredMapping"></param>
		/// <param name="inputValue"></param>
		void ProcessInput(IInputDeviceSimput sender, IInputMapItem triggeredMapping, object inputValue);
	}
}
