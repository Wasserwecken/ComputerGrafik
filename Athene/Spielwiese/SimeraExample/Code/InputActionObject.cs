using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lib.Input;
using Lib.Logic;
using OpenTK.Input;

namespace SimeraExample
{
	public class InputActionObject
		: IInputLayoutActions, INotifyPropertyChanged
	{
		private bool _jump;
		
		public float MoveLeft { get; set; }
		public float MoveRight { get; set; }
		public float MoveUp { get; set; }
		public float MoveDown { get; set; }

		public bool Jump
		{
			get
			{
				return _jump;
			}
			set
			{
				_jump = value;
				OnPropertyChanged();
			}
		}

		public InputDeviceType LastInputDevice { get; set; }
		public string LastInputDeviceDescription { get; set; }
		public int LastInputDeviceId { get; set; }

		public InputActionObject()
		{
			Jump = false;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
