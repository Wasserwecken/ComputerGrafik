using Lib.LevelLoader.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Lib.LevelLoader.LevelItems;
using Lib.Visuals.Graphics;

namespace Lib.LevelLoader
{
	public class LevelLoader
    {
        /// <summary>
        /// Loads a level
        /// </summary>
        /// <param name="level">number of the level (File must be in "Level(level).xml" Format</param>
        /// <returns>Returns the level</returns>
        public static Level LoadLevel(int level)
        {
            string path = @"Level\Level" + level + ".xml";
            XmlSerializer serializer = new XmlSerializer(typeof(XmlLevel));
            StreamReader reader = new StreamReader(path);
            XmlLevel xmllevel = (XmlLevel)serializer.Deserialize(reader);
            return LoadLevelFromXmlLevel(xmllevel);
        }

        /// <summary>
        /// Converts a XmlLevel to a xml file
        /// </summary>
        /// <param name="level">The level as xmlLevel</param>
        /// <param name="destination">Destination file name</param>
        public static void ConvertToXml(XmlLevel level, string destination)
        {
            XmlSerializer writer = new XmlSerializer(typeof(XmlLevel));
            FileStream file = File.Create(destination);
            writer.Serialize(file, level);
            file.Close();
        }

        /// <summary>
        /// Loads a XmlLevel from a xml file
        /// </summary>
        /// <param name="source">path of the xml file</param>
        /// <returns></returns>
        public static XmlLevel LoadFromXml(string source)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlLevel));
            StreamReader reader = new StreamReader(source);
            XmlLevel xmllevel = (XmlLevel)serializer.Deserialize(reader);
            return xmllevel;
        }



        /// <summary>
        /// Loads a level from the infos of a XmlLevel
        /// </summary>
        /// <param name="xmlLevel">XmlLevel</param>
        /// <returns>Returns the level</returns>
        private static Level LoadLevelFromXmlLevel(XmlLevel xmlLevel)
        {
            if(xmlLevel == null)
                throw new Exception("XmLLevel is null");

            Level returnLevel = new Level();

            // all animated sprites should start at the same time.
            // all animated sprites are later started from the list
            // spriteAnimated is the sprite, string is the animation name
            var animatedSpriteList = new Dictionary<SpriteAnimated, string>();
           
            foreach (var xmlBlock in xmlLevel.Blocks)
            {
                ISprite sprite = null;

                if (xmlBlock.LinkType == BlockLinkType.Image)
                {
                    var xmlTexture = xmlLevel.Textures.FirstOrDefault(t => t.Id == xmlBlock.Link);
                    if (xmlTexture == null)
                        throw new Exception("Texture not found in XML file");
                    sprite = new SpriteStatic(xmlTexture.Path);
                }
                if (xmlBlock.LinkType == BlockLinkType.Animation)
                {
                    sprite = new SpriteAnimated();
                    var xmlAnimation = AnimationLoader.GetBlockAnimations().Animations.First(a => a.Id == xmlBlock.Link);
                    if(xmlAnimation == null)
                        throw new Exception("Animation not found");
                    ((SpriteAnimated)sprite).AddAnimation(xmlAnimation.Path, xmlAnimation.AnimationLength);
                    animatedSpriteList.Add((SpriteAnimated)sprite, new DirectoryInfo(xmlAnimation.Path).Name);
   
                }
                var block = new Block(xmlBlock.X, xmlBlock.Y, sprite, xmlBlock.BlockType, xmlBlock.Collision, xmlBlock.Damage);
                returnLevel.Blocks.Add(block);
            }

            // start animations
            foreach (var anSprite in animatedSpriteList)
            {
                anSprite.Key.StartAnimation(anSprite.Value);
            }

            return returnLevel;
        }
    }
}
