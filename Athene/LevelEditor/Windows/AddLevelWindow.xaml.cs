using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace LevelEditor.Windows
{
    /// <summary>
    /// Interaktionslogik für AddLevelWindow.xaml
    /// </summary>
    public partial class AddLevelWindow : Window
    {
        public int XStart { get; set; }
        public int YStart { get; set; }
        public int XEnd { get; set; }
        public int YEnd { get; set; }
        public string ImageSource { get; set; }

        public AddLevelWindow()
        {
            InitializeComponent();
            ButtonAddLevel.Click += ButtonAddLevel_Click;
            InitDefaultValues();
            InputXStart.Focus();
        }

        private void InitDefaultValues()
        {
            InputXStart.Text = Properties.Settings.Default.addNewLevelMinX.ToString();
            InputXEnd.Text = Properties.Settings.Default.addNewLevelMaxX.ToString();
            InputYStart.Text = Properties.Settings.Default.addNewLevelMinY.ToString();
            InputYEnd.Text = Properties.Settings.Default.addNewLevelMaxY.ToString();
        }

        private void ButtonAddLevel_Click(object sender, RoutedEventArgs e)
        {
            int xStart, xEnd, yStart, yEnd;

            if (int.TryParse(InputXStart.Text, out xStart) && int.TryParse(InputXEnd.Text, out xEnd) &&
                int.TryParse(InputYStart.Text, out yStart) && int.TryParse(InputYEnd.Text, out yEnd) &&
                !String.IsNullOrWhiteSpace(InputXStart.Text) && !String.IsNullOrWhiteSpace(InputXEnd.Text) &&
                 !String.IsNullOrWhiteSpace(InputYStart.Text) && !String.IsNullOrWhiteSpace(InputYEnd.Text))
            {

                XStart = xStart;
                YStart = yStart;
                XEnd = xEnd;
                YEnd = yEnd;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Bitte gültige Werte eingeben", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
