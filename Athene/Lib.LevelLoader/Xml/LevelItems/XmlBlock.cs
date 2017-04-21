using Lib.LevelLoader.LevelItems;
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
    public class XmlBlock : XmlLevelItemBase
    {
        /// <summary>
        /// Texture name of the block
        /// </summary>
        [XmlAttribute("Link")]
        public string Link { get; set; }

        /// <summary>
        /// Texture name of the block
        /// </summary>
        [XmlAttribute("LinkType")]
        public BlockLinkType LinkType { get; set; }

        /// <summary>
        /// blocktype of the block
        /// </summary>
        [XmlAttribute("BlockType")]
        public BlockType BlockType { get; set; }

        /// <summary>
        /// collission of the block
        /// </summary>
        [XmlAttribute("Collision")]
        public bool Collision { get; set; }

        /// <summary>
        /// damage of the block
        /// </summary>
        [XmlAttribute("Damage")]
        public int Damage { get; set; }
    }

  
}
