using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.LevelItems;

namespace Lib.LevelLoader.Xml.LevelItems
{
    /// <summary>
    /// class represents a checkpoint in the level.xml
    /// </summary>
    [Serializable()]
    public class XmlCheckpoint : XmlLevelItemBase, INotifyPropertyChanged
    {
        /// <summary>
        /// y coordinate
        /// </summary>
        [XmlAttribute("DestinationX")]
        public float DestinationX { get; set; }

        /// <summary>
        /// x coordinate
        /// </summary>
        [XmlAttribute("DestinationY")]
        public float DestinationY { get; set; }

        /// <summary>
        /// link name of the block
        /// </summary>
        [XmlAttribute("Link")]
        public string Link { get; set; }

        public string Description => "Checkpoint (" + X + "|" + Y + ")";


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
