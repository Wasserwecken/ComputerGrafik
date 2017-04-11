﻿using System;
using System.Reflection;

namespace Simput.Mapping
{
	/// <summary>
	/// Defines a mapping item which is the connection between the input device and game actions
	/// </summary>
	internal class InputMapItem
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
