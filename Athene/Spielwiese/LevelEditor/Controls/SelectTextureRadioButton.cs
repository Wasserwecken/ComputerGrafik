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
    public class SelectTextureRadioButton : RadioButton
    {
        public SelectTextureRadioButton(string texture, string id, TextureRadioButtonAction action, string title = "")
        {
            if (texture == null)
                return;
            Action = action;
            TexturePath = Directory.GetCurrentDirectory() + @"\" +  texture;


            Content = texture;
            if (title != "") Content = title;


            ToolTip = TexturePath;
            Texture = texture;
            Id = id;
        }

        /// <summary>
        /// The Action of the RadioButton
        /// </summary>
        public TextureRadioButtonAction Action { get; set; }

        /// <summary>
        /// Returns the ID of the TextureRadioButton (Id für XML)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Relative Path of texture
        /// </summary>
        public string Texture { get; set; }

        public static readonly DependencyProperty TexturePathProperty =
             DependencyProperty.Register("TexturePath", typeof(String),
             typeof(SelectTextureRadioButton), new FrameworkPropertyMetadata(""));

        /// <summary>
        /// Absolute Path of texture
        /// </summary>
        public string TexturePath
        {
            get { return (String)GetValue(TexturePathProperty); }
            set { SetValue(TexturePathProperty, value); }
        }
    }

    public enum TextureRadioButtonAction
    {
        LoadTexture,
        Remove,
        Select
    }

   
}
