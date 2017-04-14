using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Simloader.Xml
{
    [Serializable()]
    public class XmlBlock
    {
        [XmlAttribute("X")]
        public float X { get; set; }

        [XmlAttribute("Y")]
        public float Y { get; set; }

        [XmlAttribute("Texture")]
        public string Texture { get; set; }

        [XmlAttribute("BlockType")]
        public BlockType BlockType { get; set; }
    }

    public enum BlockType
    {
        Collectable,
        Water,
        Lava,
        Plant,
        Walkable
    };
}
