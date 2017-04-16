using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LevelEditor.Controls.RadioButtons
{
    public class RadioButtonBase : RadioButton
    {
        /// <summary>
        /// Absolute ImagePath of texture
        /// </summary>
        public string TexturePath
        {
            get { return (String)GetValue(TexturePathProperty); }
            set { SetValue(TexturePathProperty, value); }
        }
        public static readonly DependencyProperty TexturePathProperty = DependencyProperty.Register("TexturePath", typeof(String), typeof(RadioButtonSelectTexture), new FrameworkPropertyMetadata(""));

       
        public RadioButtonBase()
        {
            Style = FindResource("RadioButtonBaseStyle") as Style;
            GroupName = "operateRadioButton";
        }
    }
}
