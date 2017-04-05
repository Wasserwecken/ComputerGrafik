using OpenTK.Input;
using Simput.Helper;
using Simput.Mapping;
using System;

namespace Simput.Device
{
	/// <summary>
	/// Tracks the input of a mouse
	/// </summary>
	internal class InputDeviceMouse
		: InputDeviceBase
	{
		/// <summary>
		/// The last gotten mouse state
		/// </summary>
		private MouseState OldState { get; set; }

		/// <summary>
		/// Initialises the mouse input
		/// </summary>
		public InputDeviceMouse(int deviceId)
			: base(InputDeviceType.Mouse.ToString(), InputDeviceType.Mouse, deviceId) { }
		
		/// <summary>
		/// Checks the input of the device by reference of the registered listener
		/// </summary>
		protected override void CheckDevice(object sender, EventArgs e)
		{
			ExecuteOnNewMouseState(mState =>
			{
				if (!mState.IsConnected) return;

				InputPropertyInstances = ReflectionHelper.GetPropertiesOfInstanceRecursive(mState, 2);
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
			});
		}



		/// <summary>
		/// Checks the mouse state by id and execute the defined method if the state has been changed
		/// </summary>
		/// <param name="action"></param>
		private void ExecuteOnNewMouseState(Action<MouseState> action)
		{
			try
			{
				var newState = DeviceId < 0 ? Mouse.GetState() : Mouse.GetState(DeviceId);
				
				if (!newState.Equals(OldState))
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
