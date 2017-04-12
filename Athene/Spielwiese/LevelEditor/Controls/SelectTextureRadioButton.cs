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
        public SelectTextureRadioButton(string texture, string id)
        {
            if (texture == null)
                return;

            TexturePath = Directory.GetCurrentDirectory() + @"\" +  texture;
            Content = texture;
            ToolTip = TexturePath;
            Texture = texture;
            Id = id;
        }

        public string Id { get; set; }
        public string Texture { get; set; }

        public static readonly DependencyProperty TexturePathProperty =
             DependencyProperty.Register("TexturePath", typeof(String),
             typeof(SelectTextureRadioButton), new FrameworkPropertyMetadata(""));

        public string TexturePath
        {
            get { return (String)GetValue(TexturePathProperty); }
            set { SetValue(TexturePathProperty, value); }
        }



    }
}
