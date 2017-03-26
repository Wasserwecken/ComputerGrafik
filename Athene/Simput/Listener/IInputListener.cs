using OpenTK.Input;
using Simput.Mapping;
using System.Collections.Generic;

namespace Simput.Listener
{
	/// <summary>
	/// Defines a listener which can react to device inputs
	/// </summary>
	public interface IInputListener
	{
		/// <summary>
		/// Mapping for the input capabilities to the ingame actions
		/// </summary>
		IEnumerable<IInputMapItem> InputMapping { get; set; }

		/// <summary>
		/// 
		/// </summary>
		IInputLayoutActions InputActions { get; set; }

		/// <summary>
		/// Processes an input which will be triggered by a device
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="triggeredMapping"></param>
		/// <param name="inputValue"></param>
		void ProcessInput(IInputDevice sender, IInputMapItem triggeredMapping, object inputValue);
	}
}