using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.Xml;
using Lib.LevelLoader.Xml.LinkTypes;

namespace LevelEditor.Controls.RadioButtons
{
    class RadioButtonSelectCheckpoint : RadioButtonBase
    {
        public XmlCheckpointItem XmlCheckpointItem { get; set; }

        public RadioButtonSelectCheckpoint(XmlCheckpointItem xmlCheckpointItem)
        {
            this.XmlCheckpointItem = xmlCheckpointItem;


            TexturePath = xmlCheckpointItem.GetFirstImage().FullName;

            Content = xmlCheckpointItem.Id;
            ToolTip = xmlCheckpointItem.Id;
        }
    }
}
