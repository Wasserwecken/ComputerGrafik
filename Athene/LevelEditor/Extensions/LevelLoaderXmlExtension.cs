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
using Lib.LevelLoader.LevelItems;
using LevelEditor.Controls.LevelItemPresenter;
using Lib.LevelLoader.Xml.LinkTypes;

namespace LevelEditor.Extensions
{
    public static class LevelLoaderXmlExtension
    {
        /// <summary>
        /// Returns the XmlLevel of the current Grid
        /// </summary>
        /// <returns></returns>
        public static XmlLevel GetXmlLevel(this Controls.LevelGrid grid)
        {
            XmlLevel levelReturn = new XmlLevel();
            levelReturn.Blocks = new List<XmlBlock>();
            levelReturn.Textures = new List<XmlTexture>();


          
            foreach (LevelItemButton button in grid.Children)
            {
                if (button.ItemPresenter == null) continue;

                /* Each button needs to be checked which type the XmLLevelItemBase is */
                if (button.ItemPresenter.XmLLevelItemBase is XmlBlock)
                {
                    XmlBlock xmlBlock = button.ItemPresenter.XmLLevelItemBase as XmlBlock;
                    if (xmlBlock.LinkType == BlockLinkType.Image)
                    {
                        XmlTexture xmlTexture = (button.ItemPresenter as XmlBlockPresenter).XmlTexture;

                        /* handle attached link */
                       


                        XmlLinkTypeBase attachedLink = button.ItemPresenter.XmlAttachedLink;
                        HandleXmlBlock(xmlBlock, xmlTexture, attachedLink, levelReturn);
                    }
                    else if ((button.ItemPresenter.XmLLevelItemBase as XmlBlock).LinkType == BlockLinkType.Animation)
                    {
                        HandleXmlBlock(xmlBlock, null, button.ItemPresenter.XmlAttachedLink, levelReturn);
                    }
                        
                }
                    

            }
            return levelReturn;
        }


        /// <summary>
        /// Initializes a new level on the grid
        /// </summary>
        /// <param name="level"></param>
        public static void InitXmlLevel(this Controls.LevelGrid grid, XmlLevel level)
        {
            foreach (LevelItemButton button in grid.Children)
            {
                var block = level.Blocks.FirstOrDefault(t => t.X == button.X && t.Y == button.Y);
                if (block != null)
                {
                    if (block.LinkType == BlockLinkType.Image)
                    {
                        var texture = level.Textures.First(t => t.Id == block.Link);


                        /* get the attachedLink */
                        XmlLinkTypeBase attachedLink = null;
                        if (block.AttachedLink != null)
                        {
                            if (block.AttachedLinkType == BlockLinkType.Image.ToString())
                            {
                                attachedLink = level.Textures.FirstOrDefault(t => t.Id == block?.AttachedLink);
                            }
                            if (block.AttachedLinkType == BlockLinkType.Animation.ToString())
                            {
                                attachedLink = AnimationLoader.GetBlockAnimations().Animations.First(a => a.Id == block.AttachedLink);
                            }
                        }


                        if (!File.Exists(texture.Path))
                        {
                            MessageBox.Show(texture.Path + " wurde nicht gefunden, Level kann nicht geladen werden",
                                "XmlLevel laden fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
                            grid.Children.Clear();
                            grid.IsEnabled = false;
                            return;
                        }
                        button.SetXmlBlock(texture, block.BlockType, block.Collision, block.Damage, block.IsScrolling, block.ScrollingLength, block.ScrollingDirectionX, block.ScrollingDirectionY, attachedLink);
                    }
                    else if (block.LinkType == BlockLinkType.Animation)
                    {
                        var animation = AnimationLoader.GetBlockAnimations().Animations.First(a => a.Id == block.Link);
                        if (!Directory.Exists(animation.Path))
                        {
                            MessageBox.Show(animation.Path + " wurde nicht gefunden, Level kann nicht geladen werden",
                                "XmlLevel laden fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
                            grid.Children.Clear();
                            grid.IsEnabled = false;
                        }
                        button.SetXmlAnimatedBlock(animation, block.BlockType, block.Collision, block.Damage);
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
        private static void HandleXmlBlock(XmlBlock block, XmlLinkTypeBase link, XmlLinkTypeBase attachedLink, XmlLevel level)
        {
            /* handle the link */
            if(link is XmlTexture && level.Textures.Count(t => t.Id == block.Link) == 0)
            {
                XmlTexture texture = new XmlTexture()
                {
                    Id = block.Link,
                    Path = ((XmlTexture)link).Path
                };
                level.Textures.Add(texture);
            }

            //if (block.LinkType == BlockLinkType.Image && level.Textures.Count(t => t.Id == block.Link) == 0)
            //{
            //    XmlTexture texture = new XmlTexture()
            //    {
            //        Id = block.Link,
            //        Path = texturePath
            //    };
            //    level.Textures.Add(texture);
            //}

            /* handle the attachedLink */
            if(attachedLink is XmlTexture)
            {
                XmlTexture texture = new XmlTexture()
                {
                    Id = block.AttachedLink,
                    Path = ((XmlTexture)attachedLink).Path
                };
                level.Textures.Add(texture);
            }

            //if(block.AttachedTexture != null && level.Textures.Count(t => t.Id == block.AttachedTexture) == 0)
            //{
            //    XmlTexture attachedTexture = new XmlTexture()
            //    {
            //        Id = block.AttachedTexture,
            //        Path = attachedTexturePath
            //    };
            //    level.Textures.Add(attachedTexture);
            //}
            level.Blocks.Add(block);
        }


    }
}
