using System;
using System.Reflection;

namespace Simput.Mapping
{
	/// <summary>
	/// Information about a speciic mapping for a input object to an ingame action
	/// </summary>
	public class InputMapItem
		: IInputMapItem
	{
		/// <summary>
		/// Property which holds the input information
		/// </summary>
		public PropertyInfo InputMember { get; set; }

		/// <summary>
		/// Property which will be modified by the input action
		/// </summary>
		public PropertyInfo ActionMember { get; set; }

		/// <summary>
		/// Converter, which will convert the input value to the ingame action value
		/// </summary>
		public Delegate Converter { get; set; }

		/// <summary>
		/// ID of the device where the mapping should be checked
		/// </summary>
		public int DeviceId { get; set; }
	}
}
