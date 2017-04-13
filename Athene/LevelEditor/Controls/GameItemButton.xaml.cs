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
using Simloader.Xml;

namespace LevelEditor.Controls
{
    /// <summary>
    /// GameItemButton represents a Block or an Enemy in the game.
    /// GameItemButton is used in the grid to edit the level.
    /// </summary>
    public partial class GameItemButton : UserControl
    {
        private string _path;

        /// <summary>
        /// Represents the relative path of the current Texture
        /// </summary>
        public string TexturePathRelative { get; set; }

        /// <summary>
        /// Represents the texture of a block in the XML file
        /// </summary>
        public string TextureId { get; private set; }

        /// <summary>
        /// Represents the X coordinate of a block in the XML file
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Represents the Y coordinate of a block in the XML file
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Represents the blocktype of a block in the XML file
        /// </summary>
        public BlockType BlockType { get; set; }

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
        /// GameItemButton represents a Block or an Enemy in the game
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="path">The Absolute Path of the Image, can also be null</param>
        public GameItemButton(int x, int y, string path = null)
        {
            InitializeComponent();

            X = x;
            Y = y;
            Path = path;
            TitleTextBlock.Text = X + " : " + Y;
            MainButton.Click += (s,e) => 
                OnClick(e);
        }

        /// <summary>
        /// Sets a Texture for the Button
        /// </summary>
        /// <param name="textureId">Texture Id (XML-Texture)</param>
        /// <param name="path">Absolute path of the Texture</param>
        public void SetTexture(string textureId, string path)
        {
            Path = path;
            TextureId = textureId;
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
