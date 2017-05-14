using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.Xml.LevelItems;
using Lib.LevelLoader.Xml.LinkTypes;

namespace Lib.LevelLoader.Xml.Root
{
    /// <summary>
    /// AnimationList represents a list of collectable items
    /// </summary>
    [Serializable()]
    [XmlRoot("CollectableList")]
    public class XmlCollectableList
    {
        /// <summary>
        /// animations
        /// </summary>
        [XmlArray("Items")]
        [XmlArrayItem("Collectable", typeof(XmlCollectableItem))]
        public List<XmlCollectableItem> Items { get; set; }
    }
}
