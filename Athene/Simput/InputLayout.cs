using Simput.Listener;
using Simput.Mapping;
using System.Collections.Generic;
using Simput.Device;

namespace Simput
{
	/// <summary>
	/// Defines a Layout for multiple input devices
	/// </summary>
	/// <typeparam name="TInputLayoutActions"></typeparam>
	public class InputLayout<TInputLayoutActions>
		where TInputLayoutActions : IInputLayoutActions
	{
		/// <summary>
		/// All listener that are registered for this layout
		/// </summary>
		public List<IInputListener> RegisteredListener { get; set; }

		/// <summary>
		/// Actions which will be modified by this layout
		/// </summary>
		private TInputLayoutActions BoundActions { get; set; }

		/// <summary>
		/// Initialises the layout
		/// </summary>
		/// <param name="actions"></param>
		public InputLayout(TInputLayoutActions actions)
		{
			BoundActions = actions;
			RegisteredListener = new List<IInputListener>();
		}

		/// <summary>
		/// Adds mapping for a gamepad to the actions
		/// </summary>
		/// <param name="mapping"></param>
		public void AddGamePadMapping(List<InputMapItem> mapping)
		{
			RegisteredListener.Add(new InputListener(BoundActions, mapping, InputDeviceGamePad.Instance));
		}

		/// <summary>
		/// Adds mapping for a mouse to the actions
		/// </summary>
		/// <param name="mapping"></param>
		public void AddMouseMapping(List<InputMapItem> mapping)
		{
			RegisteredListener.Add(new InputListener(BoundActions, mapping, InputDeviceMouse.Instance));
		}

		/// <summary>
		/// Adds mapping for a keyboard to the actions
		/// </summary>
		/// <param name="mapping"></param>
		public void AddKeyboardMapping(List<InputMapItemKeyboard> mapping)
		{
			RegisteredListener.Add(new InputListener(BoundActions, mapping, InputDeviceKeyboard.Instance));
		}
	}
}
