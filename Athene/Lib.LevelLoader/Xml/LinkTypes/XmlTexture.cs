using Lib.LevelLoader.Xml.LinkTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml
{
    /// <summary>
    /// represents a texture in the level.xml
    /// </summary>
    [Serializable()]
    public class XmlTexture : XmlLinkTypeBase
    {
        [XmlAttribute("Path")]
        public string Path { get; set; }

        
    }
}
