using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.Xml.LevelItems;

namespace Lib.LevelLoader.Xml
{
    [Serializable()]
    [XmlRoot("Level")]
    public class XmlLevel
    {
        [XmlArray("Blocks")]
        [XmlArrayItem("Block", typeof(XmlBlock))]
        public List<XmlBlock> Blocks { get; set; }

        [XmlArray("Textures")]
        [XmlArrayItem("Texture", typeof(XmlTexture))]
        public List<XmlTexture> Textures { get; set; }

        [XmlArray("Enemies")]
        [XmlArrayItem("Enemy", typeof(XmlEnemy))]
        public List<XmlEnemy> Enemies { get; set; }

        [XmlArray("Collectables")]
        [XmlArrayItem("Collectable", typeof(XmlCollectable))]
        public List<XmlCollectable> Collectables { get; set; }

        [XmlArray("Checkpoints")]
        [XmlArrayItem("Checkpoint", typeof(XmlCheckpoint))]
        public List<XmlCheckpoint> Checkpoints { get; set; }

        [XmlArray("Backgrounds")]
        [XmlArrayItem("Background", typeof(XmlBackgroundItem))]
        public List<XmlBackgroundItem> Backgrounds { get; set; }

        [XmlAttribute("MaxX")]
        public int MaxX { get; set; }

        [XmlAttribute("MaxY")]
        public int MaxY { get; set; }

        [XmlAttribute("MinX")]
        public int MinX { get; set; }

        [XmlAttribute("MinY")]
        public int MinY { get; set; }

        [XmlAttribute("SpawnX")]
        public int SpawnX { get; set; }

        [XmlAttribute("SpawnY")]
        public int SpawnY { get; set; }
    }
}
