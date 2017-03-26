using OpenTK.Input;
using Simput.Listener;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;

namespace Simput.Device
{
	internal abstract class InputDeviceBase
		: IInputDeviceSimput
	{
		/// <summary>
		/// Description for the keyboard
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Type of the device which is capturing the input
		/// </summary>
		public InputDeviceType DeviceType { get; set; }

		/// <summary>
		/// Timer for polling the keyboard
		/// </summary>
		private Timer PollingTimer { get; set; }

		/// <summary>
		/// All listener which will be informed when a input is made
		/// </summary>
		protected List<IInputListener> RegisteredListener { get; set; }

		/// <summary>
		/// List of all device ID's which are gonna be polled
		/// </summary>
		protected List<int> RequestedDevices { get; set; }

		/// <summary>
		/// List of all input properties from the device
		/// </summary>
		protected Dictionary<PropertyInfo, object> InputPropertyInstances { get; set; }

		/// <summary>
		/// Initialises the controller input
		/// </summary>
		/// <param name="description"></param>
		/// <param name="deviceType"></param>
		protected InputDeviceBase(string description, InputDeviceType deviceType)
		{
			Description = description;
			DeviceType = deviceType;

			RegisteredListener = new List<IInputListener>();
			RequestedDevices = new List<int>();
			InputPropertyInstances = new Dictionary<PropertyInfo, object>();

			InitialisePolling();
		}

		/// <summary>
		/// Adds a listener to the controller input
		/// </summary>
		/// <param name="listener"></param>
		public void RegisterListener(IInputListener listener)
		{
			RegisteredListener.Add(listener);

			foreach (var mapItem in listener.InputMapping)
			{
				if (!RequestedDevices.Contains(mapItem.DeviceId))
					RequestedDevices.Add(mapItem.DeviceId);
			}
		}

		/// <summary>
		/// Initialises the timer for polling the device
		/// </summary>
		private void InitialisePolling()
		{
			PollingTimer = new Timer(10);
			PollingTimer.Elapsed += CheckDevices;
			PollingTimer.Start();
		}

		protected abstract void CheckDevices(object sender, EventArgs e);
	}
}
