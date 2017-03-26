using OpenTK.Input;
using Simput.Helper;
using System;
using System.Collections.Generic;
using Simput.Mapping;

namespace Simput.Device
{
	/// <summary>
	/// Tracks the input of a gamepad
	/// </summary>
	internal class InputDeviceGamePad
		: InputDeviceBase
	{
		/// <summary>
		/// Singleton instance
		/// </summary>
		private static InputDeviceGamePad _instance;

		/// <summary>
		/// The last gotten gamepad state
		/// </summary>
		private Dictionary<int, GamePadState> OldStates { get; set; }

		/// <summary>
		/// Initialises the controller input
		/// </summary>
		private InputDeviceGamePad()
			: base("GamePad", InputDeviceType.Hid)
		{
			OldStates = new Dictionary<int, GamePadState>();
		}

		/// <summary>
		/// Returns the singleton instance
		/// </summary>
		public static InputDeviceGamePad Instance => _instance ?? (_instance = new InputDeviceGamePad());

		/// <summary>
		/// Checks the input of the device by reference of the registered listener
		/// </summary>
		protected override void CheckDevices(object sender, EventArgs e)
		{
			foreach (var deviceId in RequestedDevices)
			{
				ExecuteOnNewPadState(deviceId, padState =>
				{
					if (!padState.IsConnected) return;

					InputPropertyInstances = ReflectionHelper.GetPropertiesOfInstanceRecursive(padState, 2);
					foreach (var listener in RegisteredListener)
					{
						foreach (var inputMapItem in listener.InputMapping)
						{
							if (inputMapItem.DeviceId != deviceId) continue;

							var mapItem = (InputMapItem)inputMapItem;
							var inputValue = mapItem.InputMember.GetValue(InputPropertyInstances[mapItem.InputMember]);

							listener.ProcessInput(this, mapItem, inputValue);
						}
					}
				}
				);
			}
		}

		/// <summary>
		/// Checks the game pad state by id and execute the defined method if the state has been changed
		/// </summary>
		/// <param name="deviceId"></param>
		/// <param name="action"></param>
		private void ExecuteOnNewPadState(int deviceId, Action<GamePadState> action)
		{
			try
			{
				var state = GamePad.GetState(deviceId);

				if (!OldStates.TryGetValue(deviceId, out GamePadState oldState))
					OldStates.Add(deviceId, state);

				if (!state.Equals(oldState))
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
