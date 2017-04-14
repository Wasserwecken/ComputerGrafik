using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Lib.Input.Listener;

namespace Lib.Input.Device
{
	/// <summary>
	/// Base for an input device. Implementing basic tasks
	/// </summary>
	internal abstract class InputDeviceBase
		: IInputDeviceSimput
	{
		/// <summary>
		/// Id of the assigend device / instance
		/// </summary>
		public int DeviceId { get; set; }

		/// <summary>
		/// Percise description for the device
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Type of the device which is capturing the input
		/// </summary>
		public InputDeviceType DeviceType { get; set; }

		/// <summary>
		/// Timer for polling the device
		/// </summary>
		private Timer PollingTimer { get; set; }

		/// <summary>
		/// Llistener which will be informed when a input is registered
		/// </summary>
		protected List<IInputListener> RegisteredListener { get; set; }

		/// <summary>
		/// Initialises the controller input
		/// </summary>
		/// <param name="description"></param>
		/// <param name="deviceType"></param>
		/// <param name="deviceId"></param>
		protected InputDeviceBase(string description, InputDeviceType deviceType, int deviceId)
		{
			Description = description;
			DeviceType = deviceType;
			DeviceId = deviceId;

			RegisteredListener = new List<IInputListener>();

			InitialisePolling();
		}

		/// <summary>hg
		/// Adds a listener to the input device
		/// </summary>
		/// <param name="listener"></param>
		public void RegisterListener(IInputListener listener)
		{
			if (!RegisteredListener.Any(f => f.Equals(listener)))
				RegisteredListener.Add(listener);
		}

		/// <summary>
		/// Initialises and starts the timer for polling the device
		/// </summary>
		private void InitialisePolling()
		{
			PollingTimer = new Timer(10);
			PollingTimer.Elapsed += CheckDevice;
			PollingTimer.Start();
		}

		/// <summary>
		/// Checks the state of the device and has to inform the assigend listener
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected abstract void CheckDevice(object sender, EventArgs e);
	}
}
