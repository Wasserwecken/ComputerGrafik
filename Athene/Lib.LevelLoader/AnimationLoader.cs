using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.Xml;

namespace Lib.LevelLoader
{
    public class AnimationLoader
    {
        public static XmlAnimationList GetBlockAnimations()
        {
            string path = @"Animations\BlockAnimations.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(XmlAnimationList));
            StreamReader reader = new StreamReader(path);
            XmlAnimationList xmlAnimationList = (XmlAnimationList)serializer.Deserialize(reader);
            return xmlAnimationList;
        }
    }
}
