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
using System.Windows.Shapes;
using Lib.LevelLoader.LevelItems;
using Lib.LevelLoader.Xml;

namespace LevelEditor.Windows
{
    /// <summary>
    /// Interaktionslogik für ShowBlockItemWindow.xaml
    /// </summary>
    public partial class ShowBlockItemWindow : Window
    {
        private XmlBlock _currentXmlBlock;
        private XmlTexture _currentXmlTexture;

        public ShowBlockItemWindow()
        {
            InitializeComponent();

            ComboBoxBlockTypes.ItemsSource = Enum.GetValues(typeof(BlockType));
            ButtonSave.Click += ButtonSave_Click;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            BlockType newBlockType = (BlockType)ComboBoxBlockTypes.SelectedItem;
            var selectedCollision = (bool)CollisionYesRadioButton.IsChecked;
            var selectedDamage = (int)DamageSlider.Value;

            _currentXmlBlock.BlockType = newBlockType;
            _currentXmlBlock.Damage = selectedDamage;
            _currentXmlBlock.Collision = selectedCollision;
        }


        public void ShowBlock(XmlBlock block, XmlTexture texture)
        {
            _currentXmlBlock = block;
            _currentXmlTexture = texture;

            TextBlockCoordinates.Text = "(" + block.X + "|" + block.Y + ")";

            /* Update the image */
            if (texture != null)
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(@"\" + texture.Path, UriKind.Relative);
                logo.EndInit();
                ImageBlock.Source = logo;
            }
            ComboBoxBlockTypes.SelectedItem = block.BlockType;
            CollisionYesRadioButton.IsChecked = block.Collision;
            DamageSlider.Value = block.Damage;
        }


    }
}
