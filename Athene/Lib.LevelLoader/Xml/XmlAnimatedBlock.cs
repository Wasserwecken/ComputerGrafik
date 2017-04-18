using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml
{
    /// <summary>
    /// class represents an animated block in the level.xml
    /// </summary>
    [Serializable()]
    public class XmlAnimatedBlock : XmlLevelItem
    {
        /// <summary>
        /// y coordinate
        /// </summary>
        [XmlAttribute("X")]
        public float X { get; set; }

        /// <summary>
        /// x coordinate
        /// </summary>
        [XmlAttribute("Y")]
        public float Y { get; set; }

        /// <summary>
        /// name of the animation
        /// </summary>
        [XmlAttribute("Animation")]
        public string Animation { get; set; }

        /// <summary>
        /// type of the block
        /// </summary>
        [XmlAttribute("BlockType")]
        public BlockType BlockType { get; set; }


    }
}
