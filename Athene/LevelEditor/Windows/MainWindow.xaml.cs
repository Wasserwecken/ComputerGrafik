﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LevelEditor.Controls;
using LevelEditor.Extensions;
using LevelEditor.LevelCombiner;
using LevelEditor.Windows;
using Lib.LevelLoader;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

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
            CombineLevelMenuItem.Click += CombineLevelMenuItem_Click;
        }

        private void CombineLevelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog objDialog = new FolderBrowserDialog();
            objDialog.Description = Directory.GetCurrentDirectory();
            objDialog.SelectedPath = Directory.GetCurrentDirectory();
            DialogResult objResult = objDialog.ShowDialog();
            if (objResult == System.Windows.Forms.DialogResult.OK)
            {
                GridContentControl.Content = new CombineLevelControl(objDialog.SelectedPath);
            }
        }

        public Controls.LevelEditorControl LevelEditor { get; set; }

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
                LevelEditor = new Controls.LevelEditorControl();
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
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\workspace";
            if (openFileDialog.ShowDialog() == true)
            {
                var window = new ImportLevelWindow(openFileDialog.FileName);
                window.Owner = this;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (window.ShowDialog() == true)
                {
                    LevelEditor = new LevelEditorControl();
                    LevelEditor.InitNewGrid(window.Level.MinX, window.Level.MaxX, window.Level.MinY, window.Level.MaxY);
                    LevelEditor.InitXmlLevel(window.Level);
                    GridContentControl.Content = LevelEditor;
                   
                }
            }
        }
    }
}
