using Lib.LevelLoader.Xml;
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
using Lib.LevelLoader.LevelItems;
using LevelEditor.Controls.LevelItemPresenter;

namespace LevelEditor.Controls
{
    /// <summary>
    /// LevelItemButton represents a Block or an Enemy in the game.
    /// LevelItemButton is used in the grid to edit the level.
    /// </summary>
    public partial class LevelItemButton : UserControl
    {
        /// <summary>
        /// XmlLevelItemBase represents the current Item in the grid
        /// </summary>
        public LevelItemPresenterBase ItemPresenter { get; private set; }

       

        /// <summary>
        /// x coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// y coordinate
        /// </summary>
        public int Y { get; set; }

        

        /// <summary>
        /// LevelItemButton represents a Block or an Enemy in the game
        /// </summary>
        /// <param name="x">XmlX coordinate</param>
        /// <param name="y">XmlY coordinate</param>
        public LevelItemButton(int x, int y)
        {
            InitializeComponent();

            X = x;
            Y = y;
            TitleTextBlock.Text = X + " : " + Y;
            MainButton.Click += (s,e) => 
                OnClick(e);
        }

        /// <summary>
        /// Sets an xmlBlock to the Button
        /// </summary>
        /// <param name="texture">Texture of the Block</param>
        /// <param name="type">Blocktype</param>
        /// <param name="collision">collision</param>
        /// <param name="damage">damage</param>
        public void SetXmlBlock(XmlTexture texture, BlockType type, bool collision, int damage, bool isScrolling = false, int scrollingLength = 0, float scrollingDirectionX = 0, float scrollingDirectionY = 0)
        {
            var xmlBlock = new XmlBlock()
            {
                BlockType = type,
                X = X,
                Y = Y,
                Link = texture.Id,
                LinkType = BlockLinkType.Image,
                Damage =  damage,
                Collision = collision,
                IsScrolling = isScrolling,
                ScrollingLength = scrollingLength,
                ScrollingDirectionX = scrollingDirectionX,
                ScrollingDirectionY = scrollingDirectionY
            };
            ItemPresenter = new XmlBlockPresenter()
            {
                XmlTexture = texture,
                XmLLevelItemBase = xmlBlock
            };
            SetImage(texture.Path);
        }

        /// <summary>
        ///  Sets an XmlAnimatedBlock to the Button
        /// </summary>
        /// <param name="animation">animation</param>
        /// <param name="type">Blocktype</param>
        /// <param name="collision">collision</param>
        /// <param name="damage">damage</param>
        public void SetXmlAnimatedBlock(XmlAnimation animation, BlockType type, bool collision, int damage)
        {
            var xmlBlock = new XmlBlock()
            {
                BlockType = type,
                X = X,
                Y = Y,
                Link = animation.Id,
                LinkType = BlockLinkType.Animation,
                Collision = collision,
                Damage = damage
            };
            ItemPresenter = new XmlBlockPresenter()
            {
                XmLLevelItemBase = xmlBlock
            };
            SetImage(animation.GetFirstImage().FullName, UriKind.Absolute);
        }

        /// <summary>
        /// Resets the current xml item
        /// </summary>
        public void ResetXmlItem()
        {
            ItemPresenter = null;
            SetImage(null);
        }

        /// <summary>
        /// Sets the ImageSource
        /// </summary>
        /// <param name="path">Path to image</param>
        /// <param name="uriKind">Path kind</param>
        private void SetImage(string path, UriKind uriKind = UriKind.Relative)
        {
            if (path != null)
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = uriKind == UriKind.Relative ? new Uri(Directory.GetCurrentDirectory() + "/" + path, UriKind.Absolute) : new Uri(path, UriKind.Absolute);
                logo.EndInit();
                ImageBrush brush = new ImageBrush(logo);
                MainButton.Background = brush;
            }
            else
            {
                MainButton.Background = null;
            }

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
