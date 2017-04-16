using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml
{
    [Serializable()]
    public class XmlAnimatedBlock : XmlLevelItem
    {
        [XmlAttribute("X")]
        public float X { get; set; }

        [XmlAttribute("Y")]
        public float Y { get; set; }

        [XmlAttribute("Animation")]
        public string Animation { get; set; }

        [XmlAttribute("BlockType")]
        public BlockType BlockType { get; set; }
    }
}
