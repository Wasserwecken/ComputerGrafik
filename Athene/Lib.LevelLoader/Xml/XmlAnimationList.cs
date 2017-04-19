using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml
{
    /// <summary>
    /// AnimationList represents a list of animations
    /// </summary>
    [Serializable()]
    [XmlRoot("AnimationList")]
    public class XmlAnimationList
    {
        /// <summary>
        /// animations
        /// </summary>
        [XmlArray("Animations")]
        [XmlArrayItem("Animation", typeof(XmlAnimation))]
        public List<XmlAnimation> Animations { get; set; }
    }
}
