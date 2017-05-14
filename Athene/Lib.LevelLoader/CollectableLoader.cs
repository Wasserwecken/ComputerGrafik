using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.Xml;
using Lib.LevelLoader.Xml.Root;

namespace Lib.LevelLoader
{
    public class CollectableLoader
    {
        public static XmlCollectableList GetCollectables()
        {
            string path = @"Items\CollectableItems.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(XmlCollectableList));
            StreamReader reader = new StreamReader(path);
            XmlCollectableList xmlCollectableList = (XmlCollectableList)serializer.Deserialize(reader);
            return xmlCollectableList;
        }
    }
}
