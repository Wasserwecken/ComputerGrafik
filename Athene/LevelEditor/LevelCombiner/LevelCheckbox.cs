using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Lib.LevelLoader.Xml;

namespace LevelEditor.LevelCombiner
{
    class LevelCheckbox : CheckBox
    {
        public XmlLevel XmlLevel { get; set; }

        public LevelCheckbox(XmlLevel level, object content)
        {
            XmlLevel = level;
            Content = content;
        }
    }
}
