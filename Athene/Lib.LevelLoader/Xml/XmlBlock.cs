using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml
{
    /// <summary>
    /// class represents a block in the level.xml
    /// </summary>
    [Serializable()]
    public class XmlBlock : XmlLevelItem
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

    /// <summary>
    /// block type
    /// </summary>
    public enum BlockType
    {
        Collision,
        NoCollision,
        Water,
        Lava
    };
}
