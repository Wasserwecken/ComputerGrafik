using OpenTK.Input;
using Simput.Mapping;
using System.Collections.Generic;
using Simput.Device;

namespace Simput.Listener
{
	/// <summary>
	/// Base for an input listener
	/// </summary>
	internal class InputListener
		: IInputListener
	{
		/// <summary>
		/// Mapping, on wich input actions the listener will react and modify the game actions
		/// </summary>
		public IEnumerable<IInputMapItem> InputMapping { get; set; }

		/// <summary>
		/// Object which contains the actions and will be modified
		/// </summary>
		public IInputLayoutActions InputActions { get; set; }

		/// <summary>
		/// Initialises the listener
		/// </summary>
		/// <param name="actions"></param>
		/// <param name="mapping"></param>
		/// <param name="device"></param>
		public InputListener(IInputLayoutActions actions, IEnumerable<IInputMapItem> mapping, IInputDeviceSimput device)
		{
			InputActions = actions;
			InputMapping = mapping;

			device.RegisterListener(this);
		}

		/// <summary>
		/// Processes an input which will be triggered by a device
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="triggeredMapping"></param>
		/// <param name="inputValue"></param>
		public void ProcessInput(IInputDevice sender, IInputMapItem triggeredMapping, object inputValue)
		{
			var inputPraras = new[] { inputValue };
			var mapItem = triggeredMapping;

			var newActionValue = mapItem.Converter.Method.Invoke(mapItem.Converter.Target, inputPraras);
			var oldActionValue = mapItem.ActionMember.GetValue(InputActions);

			if (!oldActionValue.Equals(newActionValue))
			{
				mapItem.ActionMember.SetValue(InputActions, newActionValue);

				InputActions.LastInputDevice = sender.DeviceType;
				InputActions.LastInputDeviceDescription = sender.Description;
				InputActions.LastInputDeviceId = mapItem.DeviceId;
			}
		}
	}
}
