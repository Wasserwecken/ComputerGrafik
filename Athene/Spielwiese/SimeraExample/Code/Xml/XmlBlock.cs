using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SimeraExample.Code.Xml
{
    [Serializable()]
    public class XmlBlock
    {
        [XmlAttribute("X")]
        public float X { get; set; }

        [XmlAttribute("Y")]
        public float Y { get; set; }

        [XmlAttribute("Texture")]
        public string Texture { get; set; }
    }
}
