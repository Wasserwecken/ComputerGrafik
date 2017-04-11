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
		/// Initialises the gamepad input
		/// </summary>
		public InputDeviceGamePad(int deviceId)
			: base("GamePad", InputDeviceType.Hid, deviceId) { }

		/// <summary>
		/// Analyses the current state of the gamepad and informs all assigend listener about
		/// changes
		/// </summary>
		protected override void CheckDevice(object sender, EventArgs e)
		{
			ExecuteOnNewPadState(padState =>
			{
				if (!padState.IsConnected) return;

				var newInputPropertyInstances = ReflectionHelper.GetPropertiesOfInstanceRecursive(padState, 2);
				var oldInputPropertyInstances = ReflectionHelper.GetPropertiesOfInstanceRecursive(OldState, 2);

				foreach (var listener in RegisteredListener)
				{
					foreach (var inputMapItem in listener.InputMapping)
					{
						if (inputMapItem.DeviceId != DeviceId) continue;

						var mapItem = (InputMapItem)inputMapItem;

						var newValue = mapItem.InputMember.GetValue(newInputPropertyInstances[mapItem.InputMember]);
						var oldValue = mapItem.InputMember.GetValue(oldInputPropertyInstances[mapItem.InputMember]);

						if (!newValue.Equals(oldValue))
							listener.ProcessInput(this, mapItem, newValue);
					}
				}
			}
			);
		}

		/// <summary>
		/// Checks the gamepad state and executes the given method if the state has been changed
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
