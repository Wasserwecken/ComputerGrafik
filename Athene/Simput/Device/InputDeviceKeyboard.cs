using OpenTK.Input;
using Simput.Mapping;
using System;
using System.Collections.Generic;

namespace Simput.Device
{
	/// <summary>
	/// Captures all input which is made on the keyboard
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
		/// Analyses the current state of the gamepad and informs all assigend listener about
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

						var newValue = keyState.IsKeyDown(mapItem.KeyboardKey);
						var oldValue = OldState.IsKeyDown(mapItem.KeyboardKey);

						if (newValue != oldValue)
							listener.ProcessInput(this, mapItem, newValue);
					}
				}
			});
		}

		/// <summary>
		/// Checks the keyboard state and executes the given method if the state has been changed
		/// </summary>
		private void ExecuteOnNewKeyState(Action<KeyboardState> action)
		{
			var newState = new KeyboardState();

			try { newState = DeviceId < 0 ? Keyboard.GetState() : Keyboard.GetState(DeviceId); }
			catch (Exception) { }
				
			if (newState.IsAnyKeyDown || OldState.IsAnyKeyDown != newState.IsAnyKeyDown)
				action(newState);

			OldState = newState;
		}
	}
}
