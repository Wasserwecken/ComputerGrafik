using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml.LinkTypes
{
    /// <summary>
    /// represents a collectable item in CollectableItems.xml
    /// </summary>
    [Serializable()]
    public class XmlCollectableItem : XmlLinkTypeBase
    {
        /// <summary>
        /// path to the texture of the item
        /// </summary>
        [XmlAttribute("Path")]
        public string Path { get; set; }

        /// <summary>
        /// ItemType of the item
        /// </summary>
        [XmlAttribute("ItemType")]
        public XMlCollectableItemType ItemType { get; set; }
    }
}
