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

            var maxX = Math.Max(level.Blocks.Max(block => block.X), level.AnimatedBlocks.Max(block => block.X));
            var minX = Math.Min(level.Blocks.Min(block => block.X), level.AnimatedBlocks.Min(block => block.X));
            var maxY = Math.Max(level.Blocks.Max(block => block.Y), level.AnimatedBlocks.Max(block => block.Y));
            var minY = Math.Min(level.Blocks.Min(block => block.Y), level.AnimatedBlocks.Min(block => block.Y));

            InputXStart.Text = minX.ToString();
            InputYStart.Text = minY.ToString();
            InputXEnd.Text = maxX.ToString();
            InputYEnd.Text = maxY.ToString();
            Level = level;

            TextBlockFoundBlocks.Text = "Es wurden " + (level.Blocks.Count + level.AnimatedBlocks.Count) +
                                        " Blöcke gefunden";

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
                MinX = xStart;
                MinY = yStart;
                MaxX = xEnd;
                MaxY = yEnd;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Bitte gültige Werte eingeben", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public XmlLevel Level { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public int MinX { get; set; }
        public int MinY { get; set; }

    }
}
