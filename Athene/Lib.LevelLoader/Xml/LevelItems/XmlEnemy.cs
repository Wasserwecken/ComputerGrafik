using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml
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
        [XmlAttribute("Type")]
        public string Type { get; set; }
    }
}
