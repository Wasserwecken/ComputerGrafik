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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LevelEditor.Controls;
using LevelEditor.Windows;
using Microsoft.Win32;

namespace LevelEditor
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            AddGridMenuItem.Click += AddGridMenuItem_Click;
            SaveGridMenu.Click += SaveGridMenu_Click;
            ImportGridMenuItem.Click += ImportGridMenuItem_Click;
        }

      
        public GameGrid GameGrid { get; set; }

        private void SaveGridMenu_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.FileName = "level";
            saveFileDialog.Filter = "XML-Files (.xml)|*.xml"; 
            if (saveFileDialog.ShowDialog() == true)
            {
                var xmlLevel = GameGrid.GetXmlLevel();
                XmlManager.ConvertToXml(xmlLevel, saveFileDialog.FileName);
            }
        }

        private void AddGridMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddGridWindow addWindow = new AddGridWindow();
            var result = addWindow.ShowDialog();

            if (result==true)
            {
                GameGrid = new GameGrid();
                GameGrid.InitNewGrid(addWindow.XStart, addWindow.XEnd, addWindow.YStart, addWindow.YEnd);
                GameGrid.InitSelectionArea(@"Pics\");
                GridContentControl.Content = GameGrid;
            }
        }

        private void ImportGridMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML-Files (.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                var level = XmlManager.LoadFromXml(openFileDialog.FileName);

                var maxX = level.Blocks.Max(block => block.X);
                var minX = level.Blocks.Min(block => block.X);
                var maxY = level.Blocks.Max(block => block.Y);
                var minY = level.Blocks.Min(block => block.Y);

                GameGrid = new GameGrid();
                GameGrid.InitNewGrid((int)minX, (int)maxX, (int)minY, (int)maxY);
                GameGrid.InitSelectionArea(@"Pics\");
                GridContentControl.Content = GameGrid;
                GameGrid.InitXmlLevel(level);
            }
        }

    }
}
