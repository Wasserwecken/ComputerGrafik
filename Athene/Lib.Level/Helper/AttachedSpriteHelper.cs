using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader;
using Lib.LevelLoader.LevelItems;
using Lib.LevelLoader.Xml;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.Level.Helper
{
    public class AttachedSpriteHelper
    {
        public static ISprite GetAttachedSprite(XmlLevelItemBase item, XmlLevel xmlLevel)
        {
            ISprite attachedSprite = null;
            if (item.AttachedLink != null)
            {
                if (item.AttachedLinkType == BlockLinkType.Image.ToString())
                {
                    var attachedTexture = xmlLevel.Textures.FirstOrDefault(t => t.Id == item.AttachedLink);
                    attachedSprite = new SpriteStatic(Vector2.One, attachedTexture?.Path);
                }
                if (item.AttachedLinkType == BlockLinkType.Animation.ToString())
                {
                    attachedSprite = new SpriteAnimated(Vector2.One);
                    var xmlAnimation = AnimationLoader.GetBlockAnimations().Animations.First(a => a.Id == item.AttachedLink);
                    if (xmlAnimation == null)
                        throw new Exception("Animation not found");
                    ((SpriteAnimated)attachedSprite).AddAnimation(xmlAnimation.Path, xmlAnimation.AnimationLength);
                    // start animation
                    ((SpriteAnimated)attachedSprite).StartAnimation(new DirectoryInfo(xmlAnimation.Path).Name);
                }
            }

            return attachedSprite;
        }
    }
}
