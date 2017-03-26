using Simput.Helper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Simput.Mapping
{
	/// <summary>
	/// A list which stores a mapping of controls for a device type
	/// </summary>
	/// <typeparam name="TInputObject">Type of the input device</typeparam>
	/// <typeparam name="TActionsOject">Type of the ingame actions</typeparam>
	public class InputMapList<TInputObject, TActionsOject>
		: List<InputMapItem>
		where TActionsOject : class
	{
		/// <summary>
		/// Adds a mapping by using lambda expressions
		/// </summary>
		/// <typeparam name="TInputMemberType">Type of the member from the input object</typeparam>
		/// <typeparam name="TActionMemberType">Type of the member from the ingame action object</typeparam>
		/// <param name="deviceId"></param>
		/// <param name="inputMember"></param>
		/// <param name="actionMember"></param>
		/// <param name="converter"></param>
		public void AddMapping<TInputMemberType, TActionMemberType>(
			int deviceId,
			Expression<Func<TInputObject, TInputMemberType>> inputMember,
			Expression<Func<TActionsOject, TActionMemberType>> actionMember,
			Func<TInputMemberType, TActionMemberType> converter
			)
		{
			var mapItem = new InputMapItem
			{
				DeviceId = deviceId,
				InputMember = PropertyHelper.GetPropertyInfoByExpression(inputMember),
				ActionMember = PropertyHelper.GetPropertyInfoByExpression(actionMember),
				Converter = converter
			};

			Add(mapItem);
		}

		/// <summary>
		/// Adds a mapping by using lambda expressions
		/// </summary>
		/// <typeparam name="TInputMemberType">Type of the member from the input object</typeparam>
		/// <typeparam name="TActionMemberType">Type of the member from the ingame action object</typeparam>
		/// <param name="inputMember"></param>
		/// <param name="actionMember"></param>
		/// <param name="converter"></param>
		public void AddMapping<TInputMemberType, TActionMemberType>(
			Expression<Func<TInputObject, TInputMemberType>> inputMember,
			Expression<Func<TActionsOject, TActionMemberType>> actionMember,
			Func<TInputMemberType, TActionMemberType> converter
			)
		{
			AddMapping(-1, inputMember, actionMember, converter);
		}
	}
}
