using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Input.Device
{
	/// <summary>
	/// Creates instances of input devices for requested types and id's
	/// </summary>
	internal static class InputDeviceFactory
	{
		/// <summary>
		/// Already created devices, used as "cache"
		/// </summary>
		private static List<IInputDeviceSimput> InputDevices { get; } = new List<IInputDeviceSimput>();

		/// <summary>
		/// Returns a input device from the cache when its already created, otherwise, creates a new instance.
		/// each listen to only one type and id
		/// </summary>
		public static IInputDeviceSimput GetInputDeviceOf<TType>(int deviceId)
			where TType : IInputDeviceSimput
		{
			var inputDevice = InputDevices.FirstOrDefault(device => device.DeviceId == deviceId && device.GetType() == typeof(TType));

			if (inputDevice == null)
			{
				inputDevice = (TType)Activator.CreateInstance(typeof(TType), deviceId);
				InputDevices.Add(inputDevice);
			}

			return inputDevice;
		}
	}
}
