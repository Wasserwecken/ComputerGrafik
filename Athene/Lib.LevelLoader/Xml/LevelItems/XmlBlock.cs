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
        [XmlAttribute("Id")]
        public int Id { get; set; }

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
        /// scrolling of the block
        /// </summary>
        [XmlAttribute("IsScrolling")]
        public bool IsScrolling { get; set; }

        /// <summary>
        /// scrolling length of the block
        /// </summary>
        [XmlAttribute("ScrollingLength")]
        public int ScrollingLength { get; set; }


        /// <summary>
        /// scrolling direction x of the block
        /// </summary>
        [XmlAttribute("ScrollingDirectionX")]
        public float ScrollingDirectionX { get; set; }

        /// <summary>
        /// scrolling direction y of the block
        /// </summary>
        [XmlAttribute("ScrollingDirectionY")]
        public float ScrollingDirectionY { get; set; }

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
