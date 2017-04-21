using Lib.Input.Device;
using Lib.Input.Helper;
using Lib.Input.Listener;
using Lib.Input.Mapping;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Lib.Input
{
	/// <summary>
	/// Defines a Layout for multiple input devices
	/// </summary>
	/// <typeparam name="TInputLayoutActions"></typeparam>
	public class InputLayout<TInputLayoutActions>
		where TInputLayoutActions : IInputLayoutActions
	{
		/// <summary>
		/// Listener for the gamepad input
		/// </summary>
		private IInputListener GamePadListener { get; }

		/// <summary>
		/// Listener for the keyboard input
		/// </summary>
		private IInputListener KeyboardListener { get; }

		/// <summary>
		/// Listener for the mouse input
		/// </summary>
		private IInputListener MouseListener { get; }


		/// <summary>
		/// Initialises the layout
		/// </summary>
		/// <param name="actions"></param>
		/// <param name="mappingList"></param>
		public InputLayout(TInputLayoutActions actions, InputMapList<TInputLayoutActions> mappingList)
		{
			GamePadListener = new InputListener<InputDeviceGamePad>(actions);
			KeyboardListener = new InputListener<InputDeviceKeyboard>(actions);
			MouseListener = new InputListener<InputDeviceMouse>(actions);

			//Adds the mapping list to the layout and share it with its listeners
			foreach (var item in mappingList)
			{
				if (item.DeviceType == typeof(InputDeviceGamePad))
					GamePadListener.InputMapping.Add(item);
				if (item.DeviceType == typeof(InputDeviceKeyboard))
					KeyboardListener.InputMapping.Add(item);
				if (item.DeviceType == typeof(InputDeviceMouse))
					MouseListener.InputMapping.Add(item);
			}
		}
	}
}
