using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.Xml;

namespace LevelEditor.Controls.RadioButtons
{
    class RadioButtonTool : RadioButtonBase
    {
        /// <summary>
        /// Creates a tool radio button
        /// </summary>
        /// <param name="title"></param>
        /// <param name="texturePath"></param>
        /// <param name="action"></param>
        public RadioButtonTool(string title, string texturePath, TextureRadioButtonAction action)
        {
            Action = action;
            TexturePath = texturePath;
            Content = title;
        }

        /// <summary>
        /// The Action of the RadioButton
        /// </summary>
        public TextureRadioButtonAction Action { get; set; }

    }
}
