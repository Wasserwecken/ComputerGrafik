﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib.LevelLoader.Xml
{
    /// <summary>
    /// parent class for XmlLevelElements
    /// </summary>
    public class XmlLevelItemBase
    {
        /// <summary>
        /// y coordinate
        /// </summary>
        [XmlAttribute("X")]
        public float X { get; set; }

        /// <summary>
        /// x coordinate
        /// </summary>
        [XmlAttribute("Y")]
        public float Y { get; set; }

        /// <summary>
        /// attached link type of the block
        /// </summary>
        [XmlAttribute("AttachedLinkType")]
        public string AttachedLinkType { get; set; }

        /// <summary>
        /// attached link of the block
        /// </summary>
        [XmlAttribute("AttachedLink")]
        public string AttachedLink { get; set; }
    }
}
