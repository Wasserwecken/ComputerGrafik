using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelEditor.Controls;
using Lib.LevelLoader.Xml;
using System.IO;
using System.Windows;

namespace LevelEditor.Extensions
{
    public static class LevelLoaderXmlExtension
    {
        /// <summary>
        /// Returns the XmlLevel of the current Grid
        /// </summary>
        /// <returns></returns>
        public static XmlLevel GetXmlLevel(this Controls.LevelEditor leveleditor)
        {
            XmlLevel levelReturn = new XmlLevel();
            levelReturn.Blocks = new List<XmlBlock>();
            levelReturn.Textures = new List<XmlTexture>();

          
            foreach (LevelItemButton button in leveleditor.MainGrid.Children)
            {
                if (button.LevelItem == null) continue;

                /* Each button needs to be checked which type the LevelItem is */
                if (button.LevelItem is XmlBlock)
                    HandleXmlBlock(button.LevelItem as XmlBlock, button.TexturePathRelative, levelReturn);


            }
            return levelReturn;
        }


        /// <summary>
        /// Initializes a new level on the grid
        /// </summary>
        /// <param name="level"></param>
        public static void InitXmlLevel(this Controls.LevelEditor leveleditor, XmlLevel level)
        {
            foreach (var block in level.Blocks)
            {
                foreach (LevelItemButton button in leveleditor.MainGrid.Children)
                {
                    if (button.X == block.X && button.Y == block.Y)
                    {
                        var texture = level.Textures.First(t => t.Id == block.Texture);

                        if (!File.Exists(texture.Path))
                        {
                            MessageBox.Show(texture.Path + " wurde nicht gefunden, Level kann nicht geladen werden",
                                "XmlLevel laden fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
                            leveleditor.MainGrid.Children.Clear();
                            leveleditor.IsEnabled = false;
                            return;
                        }
                        button.SetXmlBlock(texture.Id, texture.Path, block.BlockType);
                    }
                }
            }
        }

        /// <summary>
        /// Inserts an XmlBlock into a level
        /// </summary>
        /// <param name="block">The Block</param>
        /// <param name="texturePath">The texture relative path</param>
        /// <param name="level">the level</param>
        private static void HandleXmlBlock(XmlBlock block, string texturePath, XmlLevel level)
        {
            if (level.Textures.Count(t => t.Id == block.Texture) == 0)
            {
                XmlTexture texture = new XmlTexture()
                {
                    Id = block.Texture,
                    Path = texturePath
                };
                level.Textures.Add(texture);
            }
            level.Blocks.Add(block);

        }
    }
}
