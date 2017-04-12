using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SimeraExample.Code.Xml
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

    }
}
