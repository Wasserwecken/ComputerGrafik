using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Input;
using OpenTK.Input;

namespace Lib.Level.Items
{
	public class LevelItemPlayerActions
		: IInputLayoutActions
	{
		/// <summary>
		/// Strenght of the movement to left. Should not be set lower than 0
		/// </summary>
		public float MoveLeft { get; set; }

		/// <summary>
		/// Strenght of the movement to right. Should not be set lower than 0
		/// </summary>
		public float MoveRight { get; set; }

		/// <summary>
		/// Strenght of the movement to up. Should not be set lower than 0
		/// </summary>
		public float MoveUp { get; set; }

		/// <summary>
		/// Strenght of the movement to down. Should not be set lower than 0
		/// </summary>
		public float MoveDown { get; set; }

		/// <summary>
		/// true if the player should execute a jump
		/// </summary>
		public bool Jump { get; set; }

		/// <summary>
		/// Instance of the last device that was used
		/// </summary>
		public InputDeviceType LastInputDevice { get; set; }

		/// <summary>
		/// Description text of the last input device
		/// </summary>
		public string LastInputDeviceDescription { get; set; }

		/// <summary>
		/// The id of the last used input device
		/// </summary>
		public int LastInputDeviceId { get; set; }
	}
}
