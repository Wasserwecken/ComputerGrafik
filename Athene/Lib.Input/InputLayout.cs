using Lib.Input.Device;
using Lib.Input.Helper;
using Lib.Input.Listener;
using Lib.Input.Mapping;
using OpenTK.Input;
using System;
using System.Linq.Expressions;

namespace Lib.Input
{
	/// <summary>
	/// Defines a Layout for multiple input devices
	/// </summary>
	/// <typeparam name="TInputLayoutActions"></typeparam>
	public class InputLayout<TInputLayoutActions>
		where TInputLayoutActions : IInputLayoutActions
	{
		/// <summary>
		/// Listener for the gamepad input
		/// </summary>
		private IInputListener GamePadListener { get; }

		/// <summary>
		/// Listener for the keyboard input
		/// </summary>
		private IInputListener KeyboardListener { get; set; }

		/// <summary>
		/// Listener for the mouse input
		/// </summary>
		private IInputListener MouseListener { get; set; }


		/// <summary>
		/// Initialises the layout
		/// </summary>
		/// <param name="actions"></param>
		public InputLayout(TInputLayoutActions actions)
		{
			GamePadListener = new InputListener<InputDeviceGamePad>(actions);
			KeyboardListener = new InputListener<InputDeviceKeyboard>(actions);
			MouseListener = new InputListener<InputDeviceMouse>(actions);
		}


		/// <summary>
		/// Adds a mapping by using lambda expressions
		/// </summary>
		public void AddMappingGamePad<TInputMemberType, TActionMemberType>(
			Expression<Func<GamePadState, TInputMemberType>> inputMember,
			Expression<Func<TInputLayoutActions, TActionMemberType>> actionMember,
			Func<TInputMemberType, TActionMemberType> converter
			)
		{
			AddMappingGamePad(-1, inputMember, actionMember, converter);
		}

		/// <summary>
		/// Adds a mapping by using lambda expressions
		/// </summary>
		public void AddMappingGamePad<TInputMemberType, TActionMemberType>(
			int deviceId,
			Expression<Func<GamePadState, TInputMemberType>> inputMember,
			Expression<Func<TInputLayoutActions, TActionMemberType>> actionMember,
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

			GamePadListener.InputMapping.Add(mapItem);
		}

		/// <summary>
		/// Adds a mapping by using lambda expressions
		/// </summary>
		public void AddMappingMouse<TInputMemberType, TActionMemberType>(
			Expression<Func<MouseState, TInputMemberType>> inputMember,
			Expression<Func<TInputLayoutActions, TActionMemberType>> actionMember,
			Func<TInputMemberType, TActionMemberType> converter
			)
		{
			AddMappingMouse(-1, inputMember, actionMember, converter);
		}

		/// <summary>
		/// Adds a mapping by using lambda expressions
		/// </summary>
		public void AddMappingMouse<TInputMemberType, TActionMemberType>(
			int deviceId,
			Expression<Func<MouseState, TInputMemberType>> inputMember,
			Expression<Func<TInputLayoutActions, TActionMemberType>> actionMember,
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

			MouseListener.InputMapping.Add(mapItem);
		}

		/// <summary>
		/// 
		/// </summary>
		public void AddMappingKeyboard<TActionMemberType>(
			Key inputKey,
			Expression<Func<TInputLayoutActions, TActionMemberType>> actionMember,
			Func<bool, TActionMemberType> converter
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

			KeyboardListener.InputMapping.Add(mapItem);
		}
	}
}
