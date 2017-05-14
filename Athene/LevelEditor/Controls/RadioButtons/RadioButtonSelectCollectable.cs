using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.Xml;
using Lib.LevelLoader.Xml.LevelItems;
using Lib.LevelLoader.Xml.LinkTypes;

namespace LevelEditor.Controls.RadioButtons
{
    public class RadioButtonSelectCollectable : RadioButtonBase
    {
        public XmlCollectableItem XmlCollectable { get; set; }



        /// <summary>
        /// Created a radiobutton which represents a texture
        /// </summary>
        /// <param name="fileInfo">fileinfo about texture</param>
        public RadioButtonSelectCollectable(XmlCollectableItem collectableItem)
        {
            XmlCollectable = collectableItem;
            TexturePath = Directory.GetCurrentDirectory() + @"\" + collectableItem.Path;
            Content = collectableItem.Id;
            ToolTip = collectableItem.Id;
        }
    }
}
