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
        public XmlAnimation XmlAnimation { get; set; }

        public RadioButtonSelectCheckpoint(XmlAnimation XmlAnimation)
        {
            this.XmlAnimation = XmlAnimation;


            TexturePath = XmlAnimation.GetFirstImage().FullName;

            Content = XmlAnimation.Id;
            ToolTip = XmlAnimation.Id;
        }
    }
}
