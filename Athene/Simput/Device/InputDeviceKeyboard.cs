using OpenTK.Input;
using Simput.Mapping;
using System;
using System.Collections.Generic;

namespace Simput.Device
{
	/// <summary>
	/// Captures all input which is made on the Keyboard
	/// </summary>
	internal class InputDeviceKeyboard
		: InputDeviceBase
	{
		/// <summary>
		/// The last gotten state
		/// </summary>
		private KeyboardState OldState { get; set; }

		/// <summary>
		/// Initialises the controller input
		/// </summary>
		public InputDeviceKeyboard(int deviceId)
			: base(InputDeviceType.Keyboard.ToString(), InputDeviceType.Keyboard, deviceId) { }

		/// <summary>
		/// Checks the input of the device by reference of the registered listener
		/// </summary>
		protected override void CheckDevice(object sender, EventArgs e)
		{
			ExecuteOnNewKeyState(keyState =>
			{
				if (!keyState.IsConnected) return;

				foreach (var listener in RegisteredListener)
				{
					foreach (var inputMapItem in listener.InputMapping)
					{
						if (inputMapItem.DeviceId != DeviceId) continue;

						var mapItem = (InputMapItemKeyboard)inputMapItem;

						listener.ProcessInput(this, mapItem, keyState.IsKeyDown(mapItem.KeyboardKey));
					}
				}
			});
		}

		/// <summary>
		/// Checks the keyboard state by id and execute the defined method if the state has been changed
		/// </summary>
		/// <param name="action"></param>
		private void ExecuteOnNewKeyState(Action<KeyboardState> action)
		{
			try
			{
				var newState = DeviceId < 0 ? Keyboard.GetState() : Keyboard.GetState(DeviceId);
				
				if (newState.IsAnyKeyDown || OldState.IsAnyKeyDown != newState.IsAnyKeyDown)
					action(newState);

				OldState = newState;
			}
			catch (Exception)
			{
				// ignored
			}
		}
	}
}
