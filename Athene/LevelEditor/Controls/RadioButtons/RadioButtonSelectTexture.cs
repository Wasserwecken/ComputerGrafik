using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Lib.LevelLoader.Xml;

namespace LevelEditor.Controls.RadioButtons
{
    /// <summary>
    /// Represents a RadioButton on the left side of the application.
    /// With this control a texture can be selected 
    /// </summary>
    public class RadioButtonSelectTexture : RadioButtonBase
    {
      
        public XmlTexture XmlTexture { get; set; }

       

        /// <summary>
        /// Created a radiobutton which represents a texture
        /// </summary>
        /// <param name="fileInfo">fileinfo about texture</param>
        public RadioButtonSelectTexture(FileInfo fileInfo)
        {
            XmlTexture = new XmlTexture();
            TexturePath = fileInfo.FullName;
            Content = fileInfo.Name;
            ToolTip = fileInfo.FullName;
            XmlTexture.Path = fileInfo.FullName.Replace(Directory.GetCurrentDirectory(), string.Empty);

            if (XmlTexture.Path.StartsWith("/")) XmlTexture.Path = XmlTexture.Path.Remove(0, 1);
            if (XmlTexture.Path.StartsWith(@"\")) XmlTexture.Path = XmlTexture.Path.Remove(0, 1);

            XmlTexture.Id = fileInfo.Name.Replace(fileInfo.Extension, string.Empty);
        }
    }

    /// <summary>
    /// Actions for the RadioButtonSelectTexture
    /// </summary>
    public enum TextureRadioButtonAction
    {
        LoadTexture,
        Remove,
        Select,
        LoadAnimation,
        RemoveAttachedTexture
    }

   
}
