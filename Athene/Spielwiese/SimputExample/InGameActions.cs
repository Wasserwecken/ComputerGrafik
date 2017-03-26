using OpenTK.Input;
using Simput;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimputExample
{
	public class InGameActions
		: IInputLayoutActions, INotifyPropertyChanged
	{
		private bool _boolValue;
		private float _floatValue;
		private InputDeviceType _lastInputDevice;
		private string _lastInputDeviceDescription;
		private int _lastInputDeviceId;

		public bool BoolValue
		{
			get { return _boolValue; }
			set
			{
				_boolValue = value;
				NotifyPropertyChanged();
			}
		}

		public float FloatValue
		{
			get { return _floatValue; }
			set
			{
				_floatValue = value;
				NotifyPropertyChanged();
			}
		}

		public InputDeviceType LastInputDevice
		{
			get { return _lastInputDevice; }
			set
			{
				_lastInputDevice = value;
				NotifyPropertyChanged();
			}
		}


		/// <summary>
		/// The last device that triggered an input
		/// </summary>
		public string LastInputDeviceDescription
		{
			get { return _lastInputDeviceDescription; }
			set
			{
				_lastInputDeviceDescription = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		/// The last device that triggered an input
		/// </summary>
		public int LastInputDeviceId
		{
			get { return _lastInputDeviceId; }
			set
			{
				_lastInputDeviceId = value;
				NotifyPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
