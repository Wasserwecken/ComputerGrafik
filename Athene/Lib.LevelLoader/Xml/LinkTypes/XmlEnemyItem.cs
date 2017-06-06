using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.LevelItems;

namespace Lib.LevelLoader.Xml.LinkTypes
{
    /// <summary>
    /// represents a enemy in the Enemies.xml
    /// </summary>
    [Serializable()]
    public class XmlEnemyItem : XmlLinkTypeBase
    {
        /// <summary>
        /// enemy type of the enemy
        /// </summary>
        [XmlAttribute("EnemyType")]
        public EnemyType EnemyType { get; set; }

        /// <summary>
        /// enemy type of the enemy
        /// </summary>
        [XmlAttribute("MovementType")]
        public MovementType MovementType { get; set; }

        /// <summary>
        /// path to the default animationm
        /// </summary>
        [XmlAttribute("DefaultAnimation")]
        public string DefaultAnimation { get; set; }

        /// <summary>
        /// length of the default animation
        /// </summary>
        [XmlAttribute("DefaultAnimationLength")]
        public int DefaultAnimationLength { get; set; }

        /// <summary>
        /// path to the attack animation
        /// </summary>
        [XmlAttribute("AttackAnimation")]
        public string AttackAnimation { get; set; }

        /// <summary>
        /// length of the attack animation
        /// </summary>
        [XmlAttribute("AttackAnimationLength")]
        public int AttackAnimationLength { get; set; }

        /// <summary>
        /// returns the first file of the default animation folder
        /// </summary>
        /// <returns></returns>
        public FileInfo GetFirstImage()
        {
            return new DirectoryInfo(Directory.GetCurrentDirectory() + @"\" + DefaultAnimation).GetFiles()[0];
        }
    }
}
