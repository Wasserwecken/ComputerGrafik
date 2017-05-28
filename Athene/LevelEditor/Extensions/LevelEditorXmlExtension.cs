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
using Lib.LevelLoader.Xml.LevelItems;
using Lib.LevelLoader.Xml.LinkTypes;


namespace LevelEditor.Extensions
{
    public static class LevelEditorXmlExtension
    {
        /// <summary>
        /// Returns the XmlLevel of the current Grid
        /// </summary>
        /// <returns></returns>
        public static XmlLevel GetXmlLevel(this Controls.LevelEditorControl levelEditor)
        {
            LevelGrid grid = levelEditor.LevelGrid;

            XmlLevel levelReturn = new XmlLevel
            {
                Blocks = new List<XmlBlock>(),
                Textures = new List<XmlTexture>(),
                Collectables = new List<XmlCollectable>(),
                Checkpoints = new List<XmlCheckpoint>(),
                Enemies = new List<XmlEnemy>(),
                MinX = grid.MinX,
                MinY = grid.MinY,
                MaxX = grid.MaxX,
                MaxY = grid.MaxY,
                SpawnX = levelEditor.SettingsLevelControl.SpawnX,
                SpawnY = levelEditor.SettingsLevelControl.SpawnY,
                Backgrounds = levelEditor.SettingsBackgroundControl.BackgroundItems
            };

           


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
                else if (button.ItemPresenter.XmLLevelItemBase is XmlCollectable)
                {
                    XmlCollectable xnlCollectable = button.ItemPresenter.XmLLevelItemBase as XmlCollectable;
                    HandleXmlCollectable(xnlCollectable, levelReturn, button.ItemPresenter.XmlAttachedLink);
                }
                else if (button.ItemPresenter.XmLLevelItemBase is XmlCheckpoint)
                {
                    XmlCheckpoint xmlCheckpoint = button.ItemPresenter.XmLLevelItemBase as XmlCheckpoint;
                    HandleXmlCheckpoint(xmlCheckpoint, levelReturn, button.ItemPresenter.XmlAttachedLink);
                }
                else if (button.ItemPresenter.XmLLevelItemBase is XmlEnemy)
                {
                    XmlEnemy xmlEnemy = button.ItemPresenter.XmLLevelItemBase as XmlEnemy;
                    HandleXmlEnemy(xmlEnemy, levelReturn, button.ItemPresenter.XmlAttachedLink);
                }



            }
            return levelReturn;
        }


        /// <summary>
        /// Initializes a new level on the grid
        /// </summary>
        /// <param name="levelEditor"></param>
        /// <param name="level"></param>
        public static void InitXmlLevel(this Controls.LevelEditorControl levelEditor, XmlLevel level)
        {
            if (level != null)
            {
                levelEditor.SettingsLevelControl.InputStartpositionX.Text = level.SpawnX.ToString();
                levelEditor.SettingsLevelControl.InputStartpositionY.Text = level.SpawnY.ToString();
                levelEditor.SettingsBackgroundControl.BackgroundItems = level.Backgrounds;

            }




            LevelGrid grid = levelEditor.LevelGrid;

            foreach (LevelItemButton button in grid.Children)
            {
                /* check if a block with these coordinates exist */
                /* later check if other items exist */
                var block = level.Blocks.FirstOrDefault(t => t.X == button.X && t.Y == button.Y);
                if (block != null)
                {
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


                    if (block.LinkType == BlockLinkType.Image)
                    {
                        var texture = level.Textures.First(t => t.Id == block.Link);
                        /* get the attachedLink */
                        
                        
                        if (!File.Exists(texture.Path))
                        {
                            MessageBox.Show(texture.Path + " wurde nicht gefunden, Level kann nicht geladen werden",
                                "XmlLevel laden fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
                            grid.Children.Clear();
                            grid.IsEnabled = false;
                            return;
                        }
                        button.SetXmlBlock(texture, block.EnvironmentType, block.Collision, block.Damage, block.IsScrolling, block.ScrollingLength, block.ScrollingDirectionX, block.ScrollingDirectionY, attachedLink);
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
                        button.SetXmlAnimatedBlock(animation, block.EnvironmentType, block.Collision, block.Damage, attachedLink);
                    }
                }

                var collectable = level.Collectables.FirstOrDefault(c => c.X == button.X && c.Y == button.Y);
                if (collectable != null)
                {
                    var collectableItem = CollectableLoader.GetCollectables().Items.First(a => a.Id == collectable.Link);
                    XmlLinkTypeBase attachedLink = null;

                    if (!File.Exists(collectableItem.Path))
                    {
                        MessageBox.Show(collectableItem.Path + " wurde nicht gefunden, Level kann nicht geladen werden",
                            "XmlLevel laden fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
                        grid.Children.Clear();
                        grid.IsEnabled = false;
                    }

                    if (collectable.AttachedLink != null)
                    {
                        if (collectable.AttachedLinkType == BlockLinkType.Image.ToString())
                        {
                            attachedLink = level.Textures.FirstOrDefault(t => t.Id == block?.AttachedLink);
                        }
                        if (collectable.AttachedLinkType == BlockLinkType.Animation.ToString())
                        {
                            attachedLink = AnimationLoader.GetBlockAnimations().Animations.First(a => a.Id == block.AttachedLink);
                        }
                        
                    }

                    button.SetXmlCollectable(collectableItem, attachedLink);
                    // if (attachedLink != null) button.AttachLink((BlockLinkType)Enum.Parse(typeof(BlockLinkType), checkpoint.AttachedLinkType), attachedLink);
                }


                var checkpoint = level.Checkpoints.FirstOrDefault(c => c.X == button.X && c.Y == button.Y);
                if (checkpoint != null)
                {
                    var xmlCheckpointItem = CheckpointLoader.GetCheckpoints().Checkpoints.FirstOrDefault(a => a.Id == checkpoint.Link);
                    XmlLinkTypeBase attachedLink = null;

                    if (checkpoint.AttachedLink != null)
                    {
                        if (checkpoint.AttachedLinkType == BlockLinkType.Image.ToString())
                        {
                            attachedLink = level.Textures.FirstOrDefault(t => t.Id == checkpoint?.AttachedLink);
                        }
                        if (checkpoint.AttachedLinkType == BlockLinkType.Animation.ToString())
                        {
                            attachedLink = AnimationLoader.GetBlockAnimations().Animations.FirstOrDefault(a => a.Id == checkpoint.AttachedLink);
                        }
                    }
                    button.SetXmlCheckpoint(xmlCheckpointItem, checkpoint.DestinationX, checkpoint.DestinationY, attachedLink);
                   
                }

                var enemy = level.Enemies.FirstOrDefault(e => e.X == button.X && e.Y == button.Y);
                if (enemy != null)
                {
                    var xmlEnemyItem = EnemyLoader.GetEnemies().Enemies.FirstOrDefault(e => e.Id == enemy.Link);
                    XmlLinkTypeBase attachedLink = null;

                    if (enemy.AttachedLink != null)
                    {
                        if (enemy.AttachedLinkType == BlockLinkType.Image.ToString())
                        {
                            attachedLink = level.Textures.FirstOrDefault(t => t.Id == block?.AttachedLink);
                        }
                        if (enemy.AttachedLinkType == BlockLinkType.Animation.ToString())
                        {
                            attachedLink = AnimationLoader.GetBlockAnimations().Animations.First(a => a.Id == block.AttachedLink);
                        }
                    }
                    button.SetXmlEnemy(xmlEnemyItem, attachedLink);


                }

            }
        }







        private static void HandleXmlCollectable(XmlCollectable collecatable, XmlLevel level, XmlLinkTypeBase attachedLink)
        {
            level.Collectables.Add(collecatable);

            /* handle the attachedLink */
            if (attachedLink is XmlTexture && level.Textures.Count(t => t.Id == attachedLink.Id) == 0)
            {
                XmlTexture texture = new XmlTexture()
                {
                    Id = collecatable.AttachedLink,
                    Path = ((XmlTexture)attachedLink).Path
                };
                level.Textures.Add(texture);
            }
        }

        /// <summary>
        /// Inserts an XmlBlock into a level
        /// </summary>
        /// <param name="block">The Block</param>
        /// <param name="link"></param>
        /// <param name="attachedLink"></param>
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

            /* handle the attachedLink */
            if(attachedLink is XmlTexture && level.Textures.Count(t => t.Id == attachedLink.Id) == 0)
            {
                XmlTexture texture = new XmlTexture()
                {
                    Id = block.AttachedLink,
                    Path = ((XmlTexture)attachedLink).Path
                };
                level.Textures.Add(texture);
            }
            level.Blocks.Add(block);
        }

        private static void HandleXmlEnemy(XmlEnemy enemy, XmlLevel level, XmlLinkTypeBase attachedLink)
        {
            level.Enemies.Add(enemy);

            /* handle the attachedLink */
            if (attachedLink is XmlTexture && level.Textures.Count(t => t.Id == attachedLink.Id) == 0)
            {
                XmlTexture texture = new XmlTexture()
                {
                    Id = enemy.AttachedLink,
                    Path = ((XmlTexture)attachedLink).Path
                };
                level.Textures.Add(texture);
            }
        }

        private static void HandleXmlCheckpoint(XmlCheckpoint checkpoint, XmlLevel level, XmlLinkTypeBase attachedLink)
        {
            level.Checkpoints.Add(checkpoint);

            /* handle the attachedLink */
            if (attachedLink is XmlTexture && level.Textures.Count(t => t.Id == attachedLink.Id) == 0)
            {
                XmlTexture texture = new XmlTexture()
                {
                    Id = checkpoint.AttachedLink,
                    Path = ((XmlTexture)attachedLink).Path
                };
                level.Textures.Add(texture);
            }
        }

    }
}
