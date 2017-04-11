using OpenTK.Input;
using System;
using System.Reflection;

namespace Simput.Mapping
{
	/// <summary>
	/// Specific map item for the keyboard, because the keyboard works with enums, not with properties
	/// </summary>
	internal class InputMapItemKeyboard
		: IInputMapItem
	{
		/// <summary>
		/// ID of the device where the mapping should be checked
		/// </summary>
		public int DeviceId { get; set; }

		/// <summary>
		/// Key which is bind to an ingame action
		/// </summary>
		public Key KeyboardKey { get; set; }

		/// <summary>
		/// Property which will be modified by the input action
		/// </summary>
		public PropertyInfo ActionMember { get; set; }

		/// <summary>
		/// Converter, which will convert the input value to the ingame action value
		/// </summary>
		public Delegate Converter { get; set; }
	}
}
