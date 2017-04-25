using Lib.LevelLoader.Xml;
using Lib.LevelLoader.Xml.LinkTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor.Controls.LevelItemPresenter
{
    public class LevelItemPresenterBase
    {
        public XmlLevelItemBase XmLLevelItemBase { get; set; }

        /// <summary>
        /// XmlTexture is for Blocks (Texture of Blocks)
        /// </summary>
        public XmlLinkTypeBase XmlAttachedLink { get; set; }
    }
}
