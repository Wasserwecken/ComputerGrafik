using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.Creation;

namespace LevelEditor.Controls.RadioButtons
{
    class RadioButtonSelectAnimation : RadioButtonBase
    {
        public AnimatedBlockInformation XmlAnimatedBlockInformation { get; set; }

        public RadioButtonSelectAnimation(AnimatedBlockInformation aimatedBlockInformation)
        {
            XmlAnimatedBlockInformation = aimatedBlockInformation;
           
            TexturePath = XmlAnimatedBlockInformation.GetFirstImage().FullName;
            Content = aimatedBlockInformation.Name;
            ToolTip = aimatedBlockInformation.Name;
        }


    }
}
