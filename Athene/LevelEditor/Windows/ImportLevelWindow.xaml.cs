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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lib.LevelLoader;
using Lib.LevelLoader.Xml;

namespace LevelEditor.Windows
{
    /// <summary>
    /// Interaktionslogik für ImportLevelWindow.xaml
    /// </summary>
    public partial class ImportLevelWindow : Window
    {
        public ImportLevelWindow(string fileName)
        {
            InitializeComponent();

            Title = "Level importieren: " + new FileInfo(fileName).Name;

            var level = LevelLoader.LoadFromXml(fileName);

            InputXStart.Text = level.MinX.ToString();
            InputYStart.Text = level.MinY.ToString();
            InputXEnd.Text = level.MaxX.ToString();
            InputYEnd.Text = level.MaxY.ToString();
            Level = level;

            TextBlockFoundBlocks.Text = "Es wurden " + level.Blocks.Count + " Blöcke gefunden";

            ButtonImportLevel.Click += ButtonImportLevel_Click;
        }

        private void ButtonImportLevel_Click(object sender, RoutedEventArgs e)
        {
            int xStart, xEnd, yStart, yEnd;

            if (int.TryParse(InputXStart.Text, out xStart) && int.TryParse(InputXEnd.Text, out xEnd) &&
                int.TryParse(InputYStart.Text, out yStart) && int.TryParse(InputYEnd.Text, out yEnd) &&
                !String.IsNullOrWhiteSpace(InputXStart.Text) && !String.IsNullOrWhiteSpace(InputXEnd.Text) &&
                !String.IsNullOrWhiteSpace(InputYStart.Text) && !String.IsNullOrWhiteSpace(InputYEnd.Text))
            {
                Level.MinX = xStart;
                Level.MinY = yStart;
                Level.MaxX = xEnd;
                Level.MaxY = yEnd;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Bitte gültige Werte eingeben", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public XmlLevel Level { get; set; }

    }
}
