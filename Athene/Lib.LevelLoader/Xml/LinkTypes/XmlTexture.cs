using System;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml.LinkTypes
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
