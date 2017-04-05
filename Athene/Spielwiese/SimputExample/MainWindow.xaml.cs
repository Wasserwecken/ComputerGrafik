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
			
			testLayout.AddMappingGamePad(0, pad => pad.Buttons.A, act => act.BoolValue,	inp => inp == ButtonState.Pressed);
			testLayout.AddMappingGamePad(0, pad => pad.Triggers.Left, act => act.FloatValue,	inp => inp);

			testLayout.AddMappingGamePad(1, pad => pad.Buttons.A, act => act.BoolValue,	inp => inp == ButtonState.Pressed);
			testLayout.AddMappingGamePad(1, pad => pad.Triggers.Right, act => act.FloatValue, inp => inp);

			testLayout.AddMappingMouse(ms => ms.LeftButton,	act => act.BoolValue, inp => inp == ButtonState.Pressed);
			testLayout.AddMappingMouse(ms => ms.X, act => act.FloatValue, inp => inp);

			testLayout.AddMappingKeyboard(Key.Space, act => act.BoolValue, con => con);









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
