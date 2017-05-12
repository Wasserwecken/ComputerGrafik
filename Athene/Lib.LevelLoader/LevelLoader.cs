using Lib.LevelLoader.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Lib.LevelLoader.LevelItems;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.LevelLoader
{
	public class LevelLoader
    {
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
    }
}
