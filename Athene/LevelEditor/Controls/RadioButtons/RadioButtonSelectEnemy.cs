using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.Xml.LinkTypes;

namespace LevelEditor.Controls.RadioButtons
{
    class RadioButtonSelectEnemy : RadioButtonBase
    {
        public XmlEnemyItem XmlEnemyItem { get; set; }

        public RadioButtonSelectEnemy(XmlEnemyItem xmlEnemyItem)
        {
            XmlEnemyItem = xmlEnemyItem;


            TexturePath = XmlEnemyItem.GetFirstImage().FullName;

            Content = XmlEnemyItem.Id;
            ToolTip = XmlEnemyItem.Id;
        }
    }
}
