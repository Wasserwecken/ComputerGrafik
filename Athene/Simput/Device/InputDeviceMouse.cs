using OpenTK.Input;
using Simput.Helper;
using System;
using System.Collections.Generic;
using Simput.Mapping;

namespace Simput.Device
{
	/// <summary>
	/// Tracks the input of a mouse
	/// </summary>
	internal class InputDeviceMouse
		: InputDeviceBase
	{
		/// <summary>
		/// Singleton instance
		/// </summary>
		private static InputDeviceMouse _instance;

		/// <summary>
		/// The last gotten mouse state
		/// </summary>
		private Dictionary<int, MouseState> OldStates { get; set; }

		/// <summary>
		/// Initialises the mouse input
		/// </summary>
		private InputDeviceMouse()
			: base(InputDeviceType.Mouse.ToString(), InputDeviceType.Mouse)
		{
			OldStates = new Dictionary<int, MouseState>();
		}

		/// <summary>
		/// Returns the singleton instance
		/// </summary>
		public static InputDeviceMouse Instance => _instance ?? (_instance = new InputDeviceMouse());

		/// <summary>
		/// Checks the input of the device by reference of the registered listener
		/// </summary>
		protected override void CheckDevices(object sender, EventArgs e)
		{
			foreach (var deviceId in RequestedDevices)
			{
				ExecuteOnNewMouseState(deviceId, mState =>
				{
					if (!mState.IsConnected) return;

					InputPropertyInstances = ReflectionHelper.GetPropertiesOfInstanceRecursive(mState, 2);
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
				});
			}
		}



		/// <summary>
		/// Checks the mouse state by id and execute the defined method if the state has been changed
		/// </summary>
		/// <param name="deviceId"></param>
		/// <param name="action"></param>
		private void ExecuteOnNewMouseState(int deviceId, Action<MouseState> action)
		{
			try
			{
				var state = deviceId < 0 ? Mouse.GetState() : Mouse.GetState(deviceId);

				if (!OldStates.TryGetValue(deviceId, out MouseState oldState))
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
