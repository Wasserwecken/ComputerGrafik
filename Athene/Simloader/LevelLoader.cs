using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.Xml;
using Simuals.Graphics;

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

            foreach (var xmlBlock in xmlLevel.Blocks)
            {
     
                var xmlTexture = xmlLevel.Textures.FirstOrDefault(t => t.Id == xmlBlock.Texture);

                if(xmlTexture == null)
                    throw new Exception("Texture not found in XML file");

                SpriteStatic sprite = new SpriteStatic(xmlTexture.Path);
                var block = new Block(xmlBlock.X, xmlBlock.Y, sprite);
                returnLevel.Blocks.Add(block);
            }

            return returnLevel;
        }
    }
}
