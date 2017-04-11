﻿using System;
using System.Reflection;

namespace Simput.Mapping
{
	/// <summary>
	/// Defines a mapping item which is the connection between the input device and game actions
	/// </summary>
	internal interface IInputMapItem
	{
		/// <summary>
		/// Property which will be modified by the input action
		/// </summary>
		PropertyInfo ActionMember { get; set; }

		/// <summary>
		/// Converter, which will convert the input value to the ingame action value
		/// </summary>
		Delegate Converter { get; set; }

		/// <summary>
		/// ID of the device where the mapping should be checked
		/// </summary>
		int DeviceId { get; set; }
	}
}
