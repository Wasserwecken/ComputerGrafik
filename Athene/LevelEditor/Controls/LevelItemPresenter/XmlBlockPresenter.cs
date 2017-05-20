using Lib.LevelLoader.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.Xml.LinkTypes;

namespace LevelEditor.Controls.LevelItemPresenter
{
    public class XmlBlockPresenter : LevelItemPresenterBase
    {
        /// <summary>
        /// XmlTexture is for Blocks (Texture of Blocks)
        /// </summary>
        public XmlTexture XmlTexture { get; set; }
    }
}
