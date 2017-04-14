using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using Lib.LevelLoader.Xml;

namespace LevelEditor.Controls
{
    /// <summary>
    /// LevelItemButton represents a Block or an Enemy in the game.
    /// LevelItemButton is used in the grid to edit the level.
    /// </summary>
    public partial class LevelItemButton : UserControl
    {
        private string _path;

        public XmlLevelItem LevelItem { get; set; }


        /// <summary>
        /// Represents the relative path of the current Texture
        /// </summary>
        public string TexturePathRelative { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Represents the ImagePath of the Image of the Control
        /// </summary>
        public string Path
        {
            get { return _path; }
            private set
            {
                _path = value;

                if (_path != null)
                {
                    /* Update the image */
                    ImageSource src = new BitmapImage(new Uri(_path, UriKind.Relative));
                    ImageBrush brush = new ImageBrush(src);
                    MainButton.Background = brush;
                    TexturePathRelative = _path.Replace(Directory.GetCurrentDirectory(), string.Empty);
                    if (TexturePathRelative.StartsWith(@"\")) TexturePathRelative = TexturePathRelative.Remove(0, 1);
                }
                else
                {
                    /* remove the background */
                    MainButton.Background = null;
                }

            }

        }

        /// <summary>
        /// LevelItemButton represents a Block or an Enemy in the game
        /// </summary>
        /// <param name="x">XmlX coordinate</param>
        /// <param name="y">XmlY coordinate</param>
        /// <param name="path">The Absolute Path of the Image, can also be null</param>
        public LevelItemButton(int x, int y)
        {
            InitializeComponent();

            X = x;
            Y = y;
            TitleTextBlock.Text = X + " : " + Y;
            MainButton.Click += (s,e) => 
                OnClick(e);
        }

        public void SetXmlBlock(string textureId, string path, BlockType type)
        {
            LevelItem = new XmlBlock()
            {
                BlockType = type,
                X = this.X,
                Y = this.Y,
                Texture = textureId
            };
            Path = path;
        }

        public void ResetItem()
        {
            LevelItem = null;
        }




        /// <summary>
        /// Click Event for UserControl
        /// </summary>
        public event EventHandler Click;
        internal virtual void OnClick(EventArgs e)
        {
            var myEvent = Click;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        
      

    }
}
