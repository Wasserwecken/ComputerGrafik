using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.Xml.LinkTypes;

namespace Lib.LevelLoader.Xml.Root
{
    /// <summary>
    /// AnimationList represents a list of collectable items
    /// </summary>
    [Serializable()]
    [XmlRoot("CheckpointList")]
    public class XmlCheckpointList
    {
        /// <summary>
        /// animations
        /// </summary>
        [XmlArray("Checkpoints")]
        [XmlArrayItem("Animation", typeof(XmlAnimation))]
        public List<XmlAnimation> CheckpointAnimations { get; set; }
    }
}
