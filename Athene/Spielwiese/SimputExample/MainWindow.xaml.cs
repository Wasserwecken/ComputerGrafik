using OpenTK.Input;
using Simput;
using Simput.Helper;
using Simput.Mapping;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Key = OpenTK.Input.Key;

namespace SimputExample
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var gameActions = new InGameActions();
			var testLayout = new InputLayout<InGameActions>(gameActions);

			var padMapping = new InputMapList<GamePadState, InGameActions>();
			var mouseMapping = new InputMapList<MouseState, InGameActions>();
			var keyMapping = new InputMapListKeyboard<InGameActions>();


			padMapping.AddMapping(0, pad => pad.Buttons.A,					act => act.BoolValue,	inp => inp == ButtonState.Pressed);
			padMapping.AddMapping(0, pad => pad.ThumbSticks.Left.Length,	act => act.FloatValue,	inp => inp);

			padMapping.AddMapping(1, pad => pad.Buttons.A,					act => act.BoolValue,	inp => inp == ButtonState.Pressed);
			padMapping.AddMapping(1, pad => pad.ThumbSticks.Left.Length,	act => act.FloatValue,	inp => inp);

			mouseMapping.AddMapping(ms => ms.LeftButton,	act => act.BoolValue,	inp => inp == ButtonState.Pressed);
			mouseMapping.AddMapping(ms => ms.X,				act => act.FloatValue,	inp => inp);

			keyMapping.AddMapping(Key.Space, act => act.BoolValue, con => con);


			testLayout.AddGamePadMapping(padMapping);
			testLayout.AddMouseMapping(mouseMapping);
			testLayout.AddKeyboardMapping(keyMapping);








			var displayProps = new List<TextBlock>();

			foreach(var item in ReflectionHelper.GetPropertiesOfInstanceRecursive(gameActions, 0))
			{
				var newBlock = new TextBlock {DataContext = gameActions};
				newBlock.SetBinding(TextBlock.TextProperty, new Binding(item.Key.Name));

				displayProps.Add(newBlock);
			}

			ActionList.ItemsSource = displayProps;
		}
	}
}
