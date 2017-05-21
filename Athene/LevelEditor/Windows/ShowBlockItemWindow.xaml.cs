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
        private XmlTexture _currentXmlTexture;
        public XmlBlock CurrentXmlBlock { get; set; }

        public ShowBlockItemWindow()
        {
            InitializeComponent();

            ComboBoxBlockTypes.ItemsSource = Enum.GetValues(typeof(EnvironmentType));
            ButtonSave.Click += ButtonSave_Click;
        }

        /// <summary>
        /// buttonSave click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            EnvironmentType newBlockType = (EnvironmentType)ComboBoxBlockTypes.SelectedItem;
            var selectedCollision = CollisionYesRadioButton.IsChecked != null && (bool)CollisionYesRadioButton.IsChecked;
            var selectedDamage = (int)DamageSlider.Value;
            var selectedIsScrolling = ScrollingActiveCheckBox.IsChecked != null && (bool) ScrollingActiveCheckBox.IsChecked;
            var selectedScrollingIntervall = (int) Convert.ToInt32(ScrollingIntervallTextBox.Text);
            var selectedScrollingXDirection = (float) Convert.ToDouble(ScrollingXDirectionTextBox.Text);
            var selectedScrollingYDirection = (float)Convert.ToDouble(ScrollingYDirectionTextBox.Text);

            CurrentXmlBlock.BlockType = newBlockType;
            CurrentXmlBlock.Damage = selectedDamage;
            CurrentXmlBlock.Collision = selectedCollision;
            CurrentXmlBlock.IsScrolling = selectedIsScrolling;
            CurrentXmlBlock.ScrollingLength = selectedScrollingIntervall;
            CurrentXmlBlock.ScrollingDirectionX = selectedScrollingXDirection;
            CurrentXmlBlock.ScrollingDirectionY = selectedScrollingYDirection;
        }

        /// <summary>
        /// Shows the block in the view
        /// </summary>
        /// <param name="block"></param>
        /// <param name="texture"></param>
        public void ShowBlock(XmlBlock block, XmlTexture texture)
        {
            CurrentXmlBlock = block;
            _currentXmlTexture = texture;

            TextBlockCoordinates.Text = "(" + block.X + "|" + block.Y + ")";

            /* Update the image */
            if (texture != null)
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\" +  texture.Path, UriKind.Absolute);
                logo.EndInit();
                ImageBlock.Source = logo;
            }
            ComboBoxBlockTypes.SelectedItem = block.BlockType;
            CollisionYesRadioButton.IsChecked = block.Collision;
            DamageSlider.Value = block.Damage;
            ScrollingActiveCheckBox.IsChecked = block.IsScrolling;
            ScrollingIntervallTextBox.Text = block.ScrollingLength.ToString();
            ScrollingXDirectionTextBox.Text = block.ScrollingDirectionX.ToString();
            ScrollingYDirectionTextBox.Text = block.ScrollingDirectionY.ToString();
        }


    }
}
