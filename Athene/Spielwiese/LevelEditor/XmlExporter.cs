using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using SimeraExample.Code.Xml;

namespace LevelEditor
{
    public static class XmlExporter
    {
        public static bool ConvertToXml(XmlLevel level, string destination)
        {
           XmlSerializer writer = new XmlSerializer(typeof(XmlLevel));

           
            FileStream file = File.Create(destination);

            writer.Serialize(file, level);
            file.Close();

            return false;
        }
    }
}
