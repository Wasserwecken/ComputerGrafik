using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.LevelLoader.Xml.LevelItems
{
    /// <summary>
    /// class represents a checkpoint in the level.xml
    /// </summary>
    [Serializable()]
    public class XmlCheckpoint : XmlLevelItemBase
    {
        public override string ToString()
        {
            return "Checkpoint (" + X + "|" + Y + ")";
        }
    }
}
