using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Lib.Input.Helper;
using Lib.Input;
using Lib.Input.Device;
using OpenTK.Input;

namespace Lib.Input.Mapping
{
	public class InputMapList<TInputLayoutActions>
		: List<IInputMapItem>
		where TInputLayoutActions : IInputLayoutActions
	{
		/// <summary>
		/// Adds a mapping by using lambda expressions
		/// </summary>
		public void AddMappingGamePad<TInputMemberType, TActionMemberType>(
			int deviceId,
			Expression<Func<GamePadState, TInputMemberType>> inputMember,
			Expression<Func<TInputLayoutActions, TActionMemberType>> actionMember,
			Func<TInputMemberType, TActionMemberType, TActionMemberType> converter
			)
		{
			var mapItem = new InputMapItem
			{
				DeviceId = deviceId,
				DeviceType = typeof(InputDeviceGamePad),
				InputMember = PropertyHelper.GetPropertyInfoByExpression(inputMember),
				ActionMember = PropertyHelper.GetPropertyInfoByExpression(actionMember),
				Converter = converter
			};

			Add(mapItem);
		}

		/// <summary>
		/// Adds a mapping by using lambda expressions
		/// </summary>
		public void AddMappingMouse<TInputMemberType, TActionMemberType>(
			int deviceId,
			Expression<Func<MouseState, TInputMemberType>> inputMember,
			Expression<Func<TInputLayoutActions, TActionMemberType>> actionMember,
			Func<TInputMemberType, TActionMemberType, TActionMemberType> converter
			)
		{
			var mapItem = new InputMapItem
			{
				DeviceId = deviceId,
				DeviceType = typeof(InputDeviceMouse),
				InputMember = PropertyHelper.GetPropertyInfoByExpression(inputMember),
				ActionMember = PropertyHelper.GetPropertyInfoByExpression(actionMember),
				Converter = converter
			};

			Add(mapItem);
		}

		/// <summary>
		/// Adds a mapping by using lambda expressions
		/// </summary>
		public void AddMappingKeyboard<TActionMemberType>(
			Key inputKey,
			Expression<Func<TInputLayoutActions, TActionMemberType>> actionMember,
			Func<bool, TActionMemberType, TActionMemberType> converter
			)
		{
			AddMappingKeyboard(-1, inputKey, actionMember, converter);
		}

		/// <summary>
		/// 
		/// </summary>
		public void AddMappingKeyboard<TActionMemberType>(
			int deviceId,
			Key inputKey,
			Expression<Func<TInputLayoutActions, TActionMemberType>> actionMember,
			Func<bool, TActionMemberType, TActionMemberType> converter
			)
		{
			var mapItem = new InputMapItemKeyboard
			{
				DeviceId = deviceId,
				DeviceType = typeof(InputDeviceKeyboard),
				KeyboardKey = inputKey,
				ActionMember = PropertyHelper.GetPropertyInfoByExpression(actionMember),
				Converter = converter
			};

			Add(mapItem);
		}
	}
}
