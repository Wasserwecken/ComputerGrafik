using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.Xml.Root;

namespace Lib.LevelLoader
{
    public class CheckpointLoader
    {
        public static XmlCheckpointList GetCheckpoints()
        {
            string path = @"Animations\Checkpoints.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(XmlCheckpointList));
            StreamReader reader = new StreamReader(path);
            XmlCheckpointList xmlCheckpointList = (XmlCheckpointList)serializer.Deserialize(reader);
            return xmlCheckpointList;
        }
    }
}
