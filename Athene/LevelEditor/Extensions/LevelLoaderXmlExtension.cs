using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelEditor.Controls;
using Lib.LevelLoader.Xml;
using System.IO;
using System.Windows;
using Lib.LevelLoader;

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
            levelReturn.AnimatedBlocks = new List<XmlAnimatedBlock>();

          
            foreach (LevelItemButton button in leveleditor.MainGrid.Children)
            {
                if (button.XmLLevelItem == null) continue;

                /* Each button needs to be checked which type the XmLLevelItem is */
                if (button.XmLLevelItem is XmlBlock)
                    HandleXmlBlock(button.XmLLevelItem as XmlBlock, button.XmlTexture.Path, levelReturn);
                if (button.XmLLevelItem is XmlAnimatedBlock)
                    HandleAnimatedXmlBlock(button.XmLLevelItem as XmlAnimatedBlock, levelReturn);

            }
            return levelReturn;
        }


        /// <summary>
        /// Initializes a new level on the grid
        /// </summary>
        /// <param name="level"></param>
        public static void InitXmlLevel(this Controls.LevelEditor leveleditor, XmlLevel level)
        {
            foreach (LevelItemButton button in leveleditor.MainGrid.Children)
            {
                var block = level.Blocks.FirstOrDefault(t => t.X == button.X && t.Y == button.Y);
                if (block != null)
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
                    button.SetXmlBlock(texture, block.BlockType);
                }

                var animatedBlock = level.AnimatedBlocks.FirstOrDefault(t => t.X == button.X && t.Y == button.Y);
                if (animatedBlock != null)
                {
                    var animation = AnimationLoader.GetBlockAnimations().Animations.First(a => a.Id == animatedBlock.Animation);

                    if (!Directory.Exists(animation.Path))
                    {
                        MessageBox.Show(animation.Path + " wurde nicht gefunden, Level kann nicht geladen werden",
                            "XmlLevel laden fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
                        leveleditor.MainGrid.Children.Clear();
                        leveleditor.IsEnabled = false;
                    }

                    button.SetXmlAnimatedBlock(animation, animatedBlock.BlockType);
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

        private static void HandleAnimatedXmlBlock(XmlAnimatedBlock block, XmlLevel level)
        {
            level.AnimatedBlocks.Add(block);
        }
    }
}
