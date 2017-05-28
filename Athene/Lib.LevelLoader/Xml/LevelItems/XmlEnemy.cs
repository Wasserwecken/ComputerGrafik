using System;
using System.Xml.Serialization;
using Lib.LevelLoader.LevelItems;

namespace Lib.LevelLoader.Xml.LevelItems
{
    /// <summary>
    /// represents a enemy in a level.xml
    /// </summary>
    [Serializable()]
    public class XmlEnemy : XmlLevelItemBase
    {
        /// <summary>
        /// name of the enemy type
        /// </summary>
        [XmlAttribute("Link")]
        public string Link { get; set; }
    }
}
