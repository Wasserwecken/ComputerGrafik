using OpenTK.Input;
using Simput.Helper;
using Simput.Mapping;
using System;

namespace Simput.Device
{
	/// <summary>
	/// Tracks the input of a gamepad
	/// </summary>
	internal class InputDeviceGamePad
		: InputDeviceBase
	{
		/// <summary>
		/// The last gotten gamepad state
		/// </summary>
		private GamePadState OldState { get; set; }

		/// <summary>
		/// Initialises the controller input
		/// </summary>
		public InputDeviceGamePad(int deviceId)
			: base("GamePad", InputDeviceType.Hid, deviceId) { }

		/// <summary>
		/// Checks the input of the device by reference of the registered listener
		/// </summary>
		protected override void CheckDevice(object sender, EventArgs e)
		{
			ExecuteOnNewPadState(padState =>
			{
				if (!padState.IsConnected) return;

				InputPropertyInstances = ReflectionHelper.GetPropertiesOfInstanceRecursive(padState, 2);
				foreach (var listener in RegisteredListener)
				{
					foreach (var inputMapItem in listener.InputMapping)
					{
						if (inputMapItem.DeviceId != DeviceId) continue;

						var mapItem = (InputMapItem)inputMapItem;
						var inputValue = mapItem.InputMember.GetValue(InputPropertyInstances[mapItem.InputMember]);

						listener.ProcessInput(this, mapItem, inputValue);
					}
				}
			}
			);
		}

		/// <summary>
		/// Checks the gaepad state by id and execute the defined method if the state has been changed
		/// </summary>
		/// <param name="action"></param>
		private void ExecuteOnNewPadState(Action<GamePadState> action)
		{
			GamePadState newState = new GamePadState();

			try { newState = GamePad.GetState(DeviceId); }
			catch (Exception) { }
			
			//It seems that the equals function misses to check the Triggers
			if (!newState.Equals(OldState) || !newState.Triggers.Equals(OldState.Triggers))
				action(newState);

			OldState = newState;
		}
	}
}
