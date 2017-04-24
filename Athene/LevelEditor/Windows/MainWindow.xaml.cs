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
using LevelEditor.Extensions;
using LevelEditor.Windows;
using Lib.LevelLoader;
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

            AddLevelMenuItem.Click += AddLevelMenuItem_Click;
            ExportGridMenuItem.Click += ExportGridMenuItem_Click;
            ImportGridMenuItem.Click += ImportLevelMenuItem_Click;
        }

      
        public Controls.LevelEditor LevelEditor { get; set; }

        /// <summary>
        /// Export a level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportGridMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (LevelEditor == null)
            {
                MessageBox.Show("Kein Level zum exportieren vorhanden", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.FileName = "level";
            saveFileDialog.Filter = "XML-Files (.xml)|*.xml"; 
            if (saveFileDialog.ShowDialog() == true)
            {
                var xmlLevel = LevelEditor.LevelGrid.GetXmlLevel();
                LevelLoader.ConvertToXml(xmlLevel, saveFileDialog.FileName);
                MessageBox.Show("Exportieren abgeschlossen", "Erfolgreich", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Open the AddLevel Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLevelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddLevelWindow addWindow = new AddLevelWindow()
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            if (addWindow.ShowDialog() == true)
            {
                LevelEditor = new Controls.LevelEditor();
                LevelEditor.InitNewGrid(addWindow.XStart, addWindow.XEnd, addWindow.YStart, addWindow.YEnd);
                GridContentControl.Content = LevelEditor;
            }
        }

        /// <summary>
        /// Import a level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportLevelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML-Files (.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                var window = new ImportLevelWindow(openFileDialog.FileName);
                window.Owner = this;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (window.ShowDialog() == true)
                {
                    LevelEditor = new Controls.LevelEditor();
                    LevelEditor.InitNewGrid(window.MinX, window.MaxX, window.MinY, window.MaxY);
                    GridContentControl.Content = LevelEditor;
                    LevelEditor.LevelGrid.InitXmlLevel(window.Level);
                }
            }
        }
    }
}
