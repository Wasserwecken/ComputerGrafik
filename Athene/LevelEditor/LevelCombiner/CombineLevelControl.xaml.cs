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
using Lib.LevelLoader;
using Lib.LevelLoader.Xml;
using Lib.LevelLoader.Xml.LevelItems;
using Lib.LevelLoader.Xml.LinkTypes;
using Microsoft.Win32;

namespace LevelEditor.LevelCombiner
{
    /// <summary>
    /// Interaktionslogik für CombineLevelControl.xaml
    /// </summary>
    public partial class CombineLevelControl : UserControl
    {
        public CombineLevelControl(string folderpath)
        {
            InitializeComponent();

            InitFolder(folderpath);
            ButtonCombine.Click += ButtonCombine_Click;
        }


        private XmlLevel GetCombinedLevel(List<XmlLevel> levels)
        {
            XmlLevel level = new XmlLevel()
            {
                Checkpoints = new List<XmlCheckpoint>(),
                Blocks = new List<XmlBlock>(),
                Collectables = new List<XmlCollectable>(),
                Enemies = new List<XmlEnemy>(),
                Textures = new List<XmlTexture>(),
                Backgrounds = new List<XmlBackgroundItem>()
            };

            foreach (var xmlLevel in levels)
            {
                foreach (var obj in xmlLevel.Checkpoints)
                {
                    level.Checkpoints.Add(obj);
                }
                foreach (var obj in xmlLevel.Blocks)
                {
                    level.Blocks.Add(obj);
                }
                foreach (var obj in xmlLevel.Collectables)
                {
                    level.Collectables.Add(obj);
                }
                foreach (var obj in xmlLevel.Enemies)
                {
                    level.Enemies.Add(obj);
                }
                foreach (var obj in xmlLevel.Textures)
                {
                    level.Textures.Add(obj);
                }
                foreach (var obj in xmlLevel.Backgrounds)
                {
                    level.Backgrounds.Add(obj);
                }

                if (xmlLevel.MaxX > level.MaxX)
                    level.MaxX = xmlLevel.MaxX;

                if (xmlLevel.MaxY > level.MaxY)
                    level.MaxY = xmlLevel.MaxY;

                if (xmlLevel.MinX < level.MinX)
                    level.MinX = xmlLevel.MinX;

                if (xmlLevel.MinY < level.MinY)
                    level.MinY = xmlLevel.MinY;

                if (xmlLevel.SpawnX != 0)
                    level.SpawnX = xmlLevel.SpawnX;

                if (xmlLevel.SpawnY != 0)
                    level.SpawnY = xmlLevel.SpawnY;
            }

            return level;
        }
       

        private void ButtonCombine_Click(object sender, RoutedEventArgs e)
        {
            List<XmlLevel> xmlLevels = new List<XmlLevel>();

            foreach (var child in StackPanelCheckbox.Children)
            {
                if (child is LevelCheckbox checkbox)
                {
                    if (checkbox.IsChecked == true)
                    {
                        xmlLevels.Add(checkbox.XmlLevel);
                    }
                }
            }






            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.FileName = "Level";
            saveFileDialog.Filter = "XML-Files (.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                var xmlLevel = GetCombinedLevel(xmlLevels);
                LevelLoader.ConvertToXml(xmlLevel, saveFileDialog.FileName);
                MessageBox.Show("Exportieren abgeschlossen", "Erfolgreich", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void InitFolder(string folderpath)
        {
            StackPanelCheckbox.Children.Clear();

            DirectoryInfo dirInfo = new DirectoryInfo(folderpath);

            foreach (var fileInfo in dirInfo.GetFiles())
            {
                if (fileInfo.Extension.Contains("xml"))
                {
                    try
                    {
                        XmlLevel level = LevelLoader.LoadFromXml(fileInfo.FullName);
                        LevelCheckbox checkBox = new LevelCheckbox(level, fileInfo.Name);
                        StackPanelCheckbox.Children.Add(checkBox);
                    }
                    catch (Exception e)
                    {
                        
                    }
                   
                    
                }
            }

        }
    }
}
