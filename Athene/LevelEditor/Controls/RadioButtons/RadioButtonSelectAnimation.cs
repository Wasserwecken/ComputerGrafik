using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.Xml;

namespace LevelEditor.Controls.RadioButtons
{
    class RadioButtonSelectAnimation : RadioButtonBase
    {
        public XmlAnimation XmlAnimation { get; set; }

        public RadioButtonSelectAnimation(XmlAnimation animation)
        {
            XmlAnimation = animation;
           
            TexturePath = animation.GetFirstImage().FullName;
            Content = animation.Id;
            ToolTip = animation.Id;
        }


    }
}
