using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.Xml.LinkTypes;

namespace Lib.LevelLoader.Xml.Root
{
    /// <summary>
    /// AnimationList represents a list of collectable items
    /// </summary>
    [Serializable()]
    [XmlRoot("EnemyList")]
    public class XmlEnemyList
    {
        /// <summary>
        /// enemies
        /// </summary>
        [XmlArray("Enemies")]
        [XmlArrayItem("Enemy", typeof(XmlEnemyItem))]
        public List<XmlEnemyItem> Enemies { get; set; }

    }
}
