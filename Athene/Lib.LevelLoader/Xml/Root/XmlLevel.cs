using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

    }
}
