using Lib.LevelLoader.Xml.LinkTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml
{
    /// <summary>
    /// represents a animation in animations.xml
    /// </summary>
    [Serializable()]
    public class XmlAnimation : XmlLinkTypeBase
    {
         /// <summary>
        /// path to the frames of the animation
        /// </summary>
        [XmlAttribute("Path")]
        public string Path { get; set; }

        /// <summary>
        /// length of the animation
        /// </summary>
        [XmlAttribute("AnimationLength")]
        public int AnimationLength { get; set; }

        /// <summary>
        /// returns the first file of the animation folder
        /// </summary>
        /// <returns></returns>
        public FileInfo GetFirstImage()
        {
            return new DirectoryInfo(Directory.GetCurrentDirectory() + @"\" + Path).GetFiles()[0];
        }
    }
}
