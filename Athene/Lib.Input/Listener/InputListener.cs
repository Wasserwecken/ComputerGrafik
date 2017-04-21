using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Lib.Input.Device;
using Lib.Input.Mapping;

namespace Lib.Input.Listener
{
	/// <summary>
	/// Processes the input of the assigend devices
	/// </summary>
	internal class InputListener<TInputDevice>
		: IInputListener
		where TInputDevice : IInputDeviceSimput
	{
		/// <summary>
		/// Mapping, on wich input actions the listener will react and modify the game actions
		/// </summary>
		public ObservableCollection<IInputMapItem> InputMapping { get; set; }

		/// <summary>
		/// Object which contains the actions and will be modified
		/// </summary>
		public IInputLayoutActions InputActions { get; set; }

		/// <summary>
		/// Initialises the listener
		/// </summary>
		/// <param name="actions"></param>
		public InputListener(IInputLayoutActions actions)
		{
			InputActions = actions;
			InputMapping = new ObservableCollection<IInputMapItem>();
			InputMapping.CollectionChanged += InputMapping_CollectionChanged;
		}

		/// <summary>
		/// Whenever the collection changes, the listener has to reorganize its listening devices.
		/// If a item with a is added, the listener will get the inputdevice and registers itself
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InputMapping_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			foreach(var item in e.NewItems)
			{
				var inputDevice = InputDeviceFactory.GetInputDeviceOf<TInputDevice>(((IInputMapItem)item).DeviceId);
				inputDevice.RegisterListener(this);
			}
		}

		/// <summary>
		/// Processes an input from a input device and manipulates the action object on changes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="triggeredMapping"></param>
		/// <param name="inputValue"></param>
		public void ProcessInput(IInputDeviceSimput sender, IInputMapItem triggeredMapping, object inputValue)
		{
			var mapItem = triggeredMapping;
			var oldActionValue = mapItem.ActionMember.GetValue(InputActions);

			var inputPraras = new[] { inputValue, oldActionValue };
			var newActionValue = mapItem.Converter.Method.Invoke(mapItem.Converter.Target, inputPraras);

			if (!oldActionValue.Equals(newActionValue))
			{
				mapItem.ActionMember.SetValue(InputActions, newActionValue);

				InputActions.LastInputDevice = sender.DeviceType;
				InputActions.LastInputDeviceDescription = sender.Description;
				InputActions.LastInputDeviceId = sender.DeviceId;
			}
		}
	}
}
