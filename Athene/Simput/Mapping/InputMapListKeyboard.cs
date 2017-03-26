using OpenTK.Input;
using Simput.Helper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Simput.Mapping
{
	/// <summary>
	/// A list which stores a mapping of controls for a device type
	/// </summary>
	/// <typeparam name="TActionsOject">Type of the ingame actions</typeparam>
	public class InputMapListKeyboard<TActionsOject>
		: List<InputMapItemKeyboard>
		where TActionsOject : class
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TActionMemberType"></typeparam>
		/// <param name="deviceId"></param>
		/// <param name="inputKey"></param>
		/// <param name="actionMember"></param>
		/// <param name="converter"></param>
		public void AddMapping<TActionMemberType>(
			int deviceId,
			Key inputKey,
			Expression<Func<TActionsOject, TActionMemberType>> actionMember,
			Func<bool, TActionMemberType> converter
			)
		{
			var mapItem = new InputMapItemKeyboard
			{
				DeviceId = deviceId,
				KeyboardKey = inputKey,
				ActionMember = PropertyHelper.GetPropertyInfoByExpression(actionMember),
				Converter = converter
			};

			Add(mapItem);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TActionMemberType"></typeparam>
		/// <param name="inputKey"></param>
		/// <param name="actionMember"></param>
		/// <param name="converter"></param>
		public void AddMapping<TActionMemberType>(
			Key inputKey,
			Expression<Func<TActionsOject, TActionMemberType>> actionMember,
			Func<bool, TActionMemberType> converter
			)
		{
			AddMapping(-1, inputKey, actionMember, converter);
		}
	}
}