using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml.LinkTypes
{
    public class XmlLinkTypeBase
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }
    }
}
