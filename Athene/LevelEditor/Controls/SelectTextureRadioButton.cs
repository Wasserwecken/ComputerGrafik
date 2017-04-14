using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LevelEditor.Controls
{
    /// <summary>
    /// Represents a RadioButton on the left side of the application.
    /// With this control a texture can be selected 
    /// </summary>
    public class SelectTextureRadioButton : RadioButton
    {
        /// <summary>
        /// The Action of the RadioButton
        /// </summary>
        public TextureRadioButtonAction Action { get; set; }
        /// <summary>
        /// Returns the ID of the TextureRadioButton (XML-ID)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Relative Path of texture (XML-Path)
        /// </summary>
        public string TextureRelative { get; set; }

        /// <summary>
        /// Absolute Path of texture
        /// </summary>
        public string TexturePath
        {
            get { return (String)GetValue(TexturePathProperty); }
            set { SetValue(TexturePathProperty, value); }
        }
        public static readonly DependencyProperty TexturePathProperty = DependencyProperty.Register("TexturePath", typeof(String), typeof(SelectTextureRadioButton), new FrameworkPropertyMetadata(""));

        /// <summary>
        /// Creates a RadioButton with an image
        /// </summary>
        /// <param name="texturePath">Absolute Path of the file</param>
        /// <param name="id">Id for the texture, !equals ID in XML</param>
        /// <param name="action">Action of the button</param>
        /// <param name="title">Text of the button</param>
        public SelectTextureRadioButton(string texturePath, string id, TextureRadioButtonAction action, string title = "")
        {
            Action = action;
            TexturePath = texturePath;
            Content = title;
            ToolTip = texturePath;
            TextureRelative = texturePath.Replace(Directory.GetCurrentDirectory(), string.Empty);
            Id = id;
        }
    }

    /// <summary>
    /// Actions for the SelectTextureRadioButton
    /// </summary>
    public enum TextureRadioButtonAction
    {
        LoadTexture,
        Remove,
        Select
    }

   
}
