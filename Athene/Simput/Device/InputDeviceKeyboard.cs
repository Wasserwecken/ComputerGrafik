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
		/// Singleton instance
		/// </summary>
		private static InputDeviceKeyboard _instance;

		/// <summary>
		/// The last gotten state
		/// </summary>
		private Dictionary<int, KeyboardState> OldStates { get; set; }

		/// <summary>
		/// Initialises the controller input
		/// </summary>
		private InputDeviceKeyboard()
			: base(InputDeviceType.Keyboard.ToString(), InputDeviceType.Keyboard)
		{
			OldStates = new Dictionary<int, KeyboardState>();
		}

		/// <summary>
		/// Returns the singleton instance
		/// </summary>
		public static InputDeviceKeyboard Instance => _instance ?? (_instance = new InputDeviceKeyboard());

		/// <summary>
		/// Checks the input of the device by reference of the registered listener
		/// </summary>
		protected override void CheckDevices(object sender, EventArgs e)
		{
			foreach (var deviceId in RequestedDevices)
			{
				ExecuteOnNewKeyState(deviceId, keyState =>
				{
					if (!keyState.IsConnected) return;

					foreach (var listener in RegisteredListener)
					{
						foreach (var inputMapItem in listener.InputMapping)
						{
							if (inputMapItem.DeviceId != deviceId) continue;

							var mapItem = (InputMapItemKeyboard)inputMapItem;

							listener.ProcessInput(this, mapItem, keyState.IsKeyDown(mapItem.KeyboardKey));
						}
					}
				});
			}
		}

		/// <summary>
		/// Checks the keyboard state by id and execute the defined method if the state has been changed
		/// </summary>
		/// <param name="deviceId"></param>
		/// <param name="action"></param>
		private void ExecuteOnNewKeyState(int deviceId, Action<KeyboardState> action)
		{
			try
			{
				var state = deviceId < 0 ? Keyboard.GetState() : Keyboard.GetState(deviceId);

				if (!OldStates.TryGetValue(deviceId, out KeyboardState oldState))
					OldStates.Add(deviceId, state);

				if (state.IsAnyKeyDown || oldState.IsAnyKeyDown != state.IsAnyKeyDown)
					action(state);

				OldStates[deviceId] = state;
			}
			catch (Exception)
			{
				// ignored
			}
		}
	}
}
