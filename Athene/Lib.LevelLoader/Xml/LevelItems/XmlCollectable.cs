using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml.LevelItems
{
    /// <summary>
    /// represents a collectable item in the level.xml
    /// </summary>
    [Serializable()]
    public class XmlCollectable : XmlLevelItemBase
    {

        /// <summary>
        /// Collectable item name of the link
        /// in Collectable.xml
        /// </summary>
        [XmlAttribute("Link")]
        public string Link { get; set; }
    }
}
