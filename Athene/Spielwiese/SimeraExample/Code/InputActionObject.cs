using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lib.Input;
using OpenTK.Input;

namespace SimeraExample
{
	public class InputActionObject
		: IInputLayoutActions, INotifyPropertyChanged
	{
		private bool _jump;

		public float Vertical { get; set; }
		public float Horizontal { get; set; }

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
			Vertical = 0;
			Horizontal = 0;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
