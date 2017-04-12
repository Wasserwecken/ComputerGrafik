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
                XmlExporter.ConvertToXml(xmlLevel, saveFileDialog.FileName);
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
                GameGrid.InitTextures(@"Pics\");
                GridContentControl.Content = GameGrid;
            }
        }
    }
}
