using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class CheckpointSettingsControl : UserControl, INotifyPropertyChanged
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
            DataContext = this;
        }

        /// <summary>
        /// the combo box
        /// </summary>
        public ComboBox ComboBoxItems => ComboBoxCheckpoints;

        public void AddCheckpoint(int x, int y, int destinationX, int destinationY)
        {
            int count = Checkpoints.Count(c => c.X == x && c.Y == y);

            if (count > 0)
                return;

            XmlCheckpoint checkpoint = new XmlCheckpoint()
            {
                X = x,
                Y = y,
                DestinationX = destinationX,
                DestinationY = destinationY
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
            var startX = InputAddStartX.Text;
            var startY = InputAddStartY.Text;
            var destinationX = InputAddDestinationX.Text;
            var destinationY = InputAddDestinationY.Text;

            if (String.IsNullOrWhiteSpace(startX) || String.IsNullOrWhiteSpace(startY) || String.IsNullOrWhiteSpace(destinationX) || String.IsNullOrWhiteSpace(destinationY))
            {
                MessageBox.Show("Bitte gültige Werte eingeben", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
                AddCheckpoint(Convert.ToInt32(startX),
                Convert.ToInt32(startY),
                Convert.ToInt32(destinationX),
                Convert.ToInt32(destinationY));
            }
            catch (Exception exception)
            {
                MessageBox.Show("Bitte gültige Werte eingeben", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            

        }

        private void Input_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ButtonSaveCheckpoint_OnClick(object sender, RoutedEventArgs e)
        {
            var startX = InputEditStartX.Text;
            var startY = InputEditStartY.Text;
            var destinationX = InputEditDestinationX.Text;
            var destinationY = InputEditDestinationY.Text;

            var selectedItem = ComboBoxItems.SelectedValue;
            if (selectedItem != null && selectedItem.GetType() == typeof(XmlCheckpoint))
            {
                XmlCheckpoint checkpoint = (XmlCheckpoint) selectedItem;
                checkpoint.X = Convert.ToInt32(startX);
                checkpoint.Y = Convert.ToInt32(startY);
                checkpoint.DestinationX = Convert.ToInt32(destinationX);
                checkpoint.DestinationY = Convert.ToInt32(destinationY);


                checkpoint.NotifyPropertyChanged("Description");
            }

        }


    

        private void ComboBoxCheckpoints_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            var selectedItem = e.AddedItems[0];
            if (selectedItem != null && selectedItem.GetType() == typeof(XmlCheckpoint))
            {
                XmlCheckpoint checkpoint = (XmlCheckpoint) selectedItem;
                InputEditStartX.Text = checkpoint.X.ToString();
                InputEditStartY.Text = checkpoint.Y.ToString();
                InputEditDestinationX.Text = checkpoint.DestinationX.ToString();
                InputEditDestinationY.Text = checkpoint.DestinationY.ToString();

            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
