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
                var xmlLevel = LevelEditor.GetXmlLevel();
                LevelLoader.ConvertToXml(xmlLevel, saveFileDialog.FileName);
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
                var level = LevelLoader.LoadFromXml(openFileDialog.FileName);

                var maxX = Math.Max(level.Blocks.Max(block => block.X), level.AnimatedBlocks.Max(block => block.X));
                var minX = Math.Min(level.Blocks.Min(block => block.X), level.AnimatedBlocks.Min(block => block.X));
                var maxY = Math.Max(level.Blocks.Max(block => block.Y), level.AnimatedBlocks.Max(block => block.Y));
                var minY = Math.Min(level.Blocks.Min(block => block.Y), level.AnimatedBlocks.Min(block => block.Y));



                //try
                //{
                    LevelEditor = new Controls.LevelEditor();
                    LevelEditor.InitNewGrid((int)minX, (int)maxX, (int)minY, (int)maxY);
                    GridContentControl.Content = LevelEditor;
                    LevelEditor.InitXmlLevel(level);
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(
                //    "Beim Laden des XmLlevels ist ein Fehler aufgetreten. Eventuell sind nicht alle Texturen, die benötigt werden, vorhanden.",
                //    "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                //}

               
            }
        }

    }
}
