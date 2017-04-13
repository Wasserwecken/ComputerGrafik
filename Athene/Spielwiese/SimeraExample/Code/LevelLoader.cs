using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SimeraExample.Code.Xml;
using Simuals.Graphics;

namespace SimeraExample.Code
{
    public class LevelLoader
    {
        /// <summary>
        /// Loads a level
        /// </summary>
        /// <param name="level">number of the level</param>
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
        /// Loads a level from the infos of a XmlLevel
        /// </summary>
        /// <param name="xmlLevel">XmlLevel</param>
        /// <returns>Returns the level</returns>
        private static Level LoadLevelFromXmlLevel(XmlLevel xmlLevel)
        {
            if(xmlLevel == null)
                throw new Exception("XmLLevel is null");

            Level returnLevel = new Level();

            foreach (var xmlBlock in xmlLevel.Blocks)
            {
                var texturePath = xmlLevel.Textures.FirstOrDefault(t => t.Id == xmlBlock.Texture);

                if(texturePath == null)
                    throw new Exception("Texture not found in XML file");

                StaticSprite sprite = new StaticSprite(texturePath.Path);
                var block = new Block(xmlBlock.X, xmlBlock.Y, sprite);
                returnLevel.Blocks.Add(block);
            }

            return returnLevel;
        }
    }
}
