using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml.LevelItems
{
    [Serializable()]
    public class XmlBackgroundItem
    {
        /// <summary>
        /// Index of the background
        /// </summary>
        [XmlAttribute("Index")]
        public int Index { get; set; }

        /// <summary>
        /// Path of the Background
        /// </summary>
        [XmlAttribute("Path")]
        public string Path { get; set; }

        public string AbsolutePath
        {
            get { return Directory.GetCurrentDirectory() + @"/" + Path; }
        }

        public string Name
        {
            get
            {
                return new FileInfo(AbsolutePath).Name;
            }
        }
    }
}
