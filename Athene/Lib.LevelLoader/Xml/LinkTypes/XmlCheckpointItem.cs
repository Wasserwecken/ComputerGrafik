using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.LevelItems;

namespace Lib.LevelLoader.Xml.LinkTypes
{

    /// <summary>
    /// represents a checkpoint item in Checkpoints.xml
    /// </summary>
    [Serializable()]
    public class XmlCheckpointItem : XmlLinkTypeBase
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
        /// path to the frames of the animation
        /// </summary>
        [XmlAttribute("ActivationPath")]
        public string ActivationPath { get; set; }

        /// <summary>
        /// length of the animation
        /// </summary>
        [XmlAttribute("ActivationAnimationLength")]
        public int ActivationAnimationLength { get; set; }


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
