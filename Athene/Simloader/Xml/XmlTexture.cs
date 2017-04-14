using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml
{
    [Serializable()]
    public class XmlTexture
    {
        [XmlAttribute("Path")]
        public string Path { get; set; }

        [XmlAttribute("Id")]
        public string Id { get; set; }
    }
}
