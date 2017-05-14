using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lib.LevelLoader.Xml.LevelItems;

namespace LevelEditor.Controls.Tabs
{
    /// <summary>
    /// Interaktionslogik für CheckpointSettingsControl.xaml
    /// </summary>
    public partial class CheckpointSettingsControl : UserControl
    {
        /// <summary>
        /// a list of all checkpoints
        /// </summary>
        public ObservableCollection<XmlCheckpoint> Checkpoints { get; set; }

        /// <summary>
        /// Checkpoint Settings Control
        /// </summary>
        public CheckpointSettingsControl()
        {
            InitializeComponent();
            Checkpoints = new ObservableCollection<XmlCheckpoint>();
            ComboBoxItems.ItemsSource = Checkpoints;
        }

        /// <summary>
        /// the combo box
        /// </summary>
        public ComboBox ComboBoxItems => ComboBoxCheckpoints;

        public void AddCheckpoint(int x, int y)
        {
            int count = Checkpoints.Count(c => c.X == x && c.Y == y);

            if (count > 0)
                return;

            XmlCheckpoint checkpoint = new XmlCheckpoint()
            {
                X = x,
                Y = y
            };
            Checkpoints.Add(checkpoint);
        }


        private void ButtonDeleteCheckpoint_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = ComboBoxItems.SelectedValue;
            if(selectedItem != null && selectedItem.GetType() == typeof(XmlCheckpoint))
            {
                Checkpoints.Remove((XmlCheckpoint) selectedItem);
            }
        }

        private void ButtonAddCheckpoint_OnClick(object sender, RoutedEventArgs e)
        {
            var x = InputX.Text;
            var y = InputY.Text;

            if (String.IsNullOrWhiteSpace(x) || String.IsNullOrWhiteSpace(y))
            {
                MessageBox.Show("Bitte gültige Werte eingeben", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            AddCheckpoint(Convert.ToInt32(x), Convert.ToInt32(y));

        }

        private void Input_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
