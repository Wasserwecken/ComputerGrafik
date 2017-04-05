using System;
using System.Collections.Generic;

namespace Simput.Device
{
	internal static class InputDeviceFactory
	{
		/// <summary>
		/// 
		/// </summary>
		private static List<Tuple<int, IInputDeviceSimput>> InputDevices { get; } = new List<Tuple<int, IInputDeviceSimput>>();

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TType"></typeparam>
		/// <param name="deviceId"></param>
		/// <returns></returns>
		public static IInputDeviceSimput GetInputDeviceOf<TType>(int deviceId)
			where TType : IInputDeviceSimput
		{
			IInputDeviceSimput inputDevice = null;
			
			foreach (var device in InputDevices)
			{
				if (device.Item1 == deviceId && device.Item2.GetType() == typeof(TType))
				{
					inputDevice = device.Item2;
					break;
				}
			}

			if (inputDevice == null)
			{
				inputDevice = (TType)Activator.CreateInstance(typeof(TType), deviceId);
				InputDevices.Add(new Tuple<int, IInputDeviceSimput>(deviceId, inputDevice));
			}

			return inputDevice;
		}
	}
}
