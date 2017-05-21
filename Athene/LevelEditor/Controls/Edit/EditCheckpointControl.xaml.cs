using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lib.LevelLoader;
using Lib.LevelLoader.LevelItems;
using Lib.LevelLoader.Xml;
using Lib.LevelLoader.Xml.LevelItems;
using Lib.LevelLoader.Xml.LinkTypes;

namespace LevelEditor.Controls.Edit
{
    /// <summary>
    /// Interaktionslogik für EditCheckpointControl.xaml
    /// </summary>
    public partial class EditCheckpointControl : UserControl
    {
        /// <summary>
        /// the current selected animation
        /// </summary>
        public XmlCheckpointItem CurrrentXmlCheckpointItem { get; set; }

        /// <summary>
        /// the current XmlCheckpoint
        /// </summary>
        public XmlCheckpoint CurrentXmlCheckpoint { get; set; }

        /// <summary>
        /// the current selected button
        /// </summary>
        private LevelItemButton CurrentButton { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public EditCheckpointControl()
        {
            InitializeComponent();
            ButtonSave.Click += ButtonSave_Click;
            ButtonDeleteAttachedLink.Click += (s, e) =>
            {
                CurrentButton.AttachLink(BlockLinkType.Image, null);
            };
            ButtonDelete.Click += (s, e) =>
            {
                CurrentButton.ResetXmlItem();
                CloseButton_OnClick(s,e);
            };
        }

        /// <summary>
        /// buttonSave click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var startX = Convert.ToInt32(InputEditStartX.Text);
                var startY = Convert.ToInt32(InputEditStartY.Text);
                var destinationX = Convert.ToInt32(InputEditDestinationX.Text);
                var destinationY = Convert.ToInt32(InputEditDestinationY.Text);

                CurrentXmlCheckpoint.X = startX;
                CurrentXmlCheckpoint.Y = startY;
                CurrentXmlCheckpoint.DestinationX = destinationX;
                CurrentXmlCheckpoint.DestinationY = destinationY;
            }
            catch
            {
                MessageBox.Show("Bitte gültige Werte eingeben", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
           
        }

        /// <summary>
        /// close button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Parent != null && Parent is ContentControl)
            {
                ContentControl parent = Parent as ContentControl;
                parent.Content = null;
            }
        }

        /// <summary>
        /// shows the checkpoint in the control
        /// </summary>
        /// <param name="checkpoint"></param>
        /// <param name="animation"></param>
        /// <param name="senderButton"></param>
        public void ShowCheckpoint(XmlCheckpoint checkpoint, LevelItemButton senderButton)
        {
            CurrentXmlCheckpoint = checkpoint;


            XmlCheckpointItem xmlCheckpointItem =
                CheckpointLoader.GetCheckpoints().Checkpoints.FirstOrDefault(c => c.Id == checkpoint.Link);

            if(xmlCheckpointItem == null)
                throw new Exception("xmlAnimation not found in the checkpoint list");

            CurrrentXmlCheckpointItem = xmlCheckpointItem;
            CurrentButton = senderButton;

            TextBlockCoordinates.Text = "(" + checkpoint.X + "|" + checkpoint.Y + ")";

        
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(xmlCheckpointItem.GetFirstImage().FullName, UriKind.Absolute);
            logo.EndInit();
            ImageBlock.Source = logo;

            TextBlockLinkType.Text = "Animation";
            TextBlockLinkName.Text = xmlCheckpointItem.Id;
            

            InputEditStartX.Text = checkpoint.X.ToString();
            InputEditStartY.Text = checkpoint.Y.ToString();
            InputEditDestinationX.Text = checkpoint.DestinationX.ToString();
            InputEditDestinationY.Text = checkpoint.DestinationY.ToString();
        }

        private void Input_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("^-?[^0-9]+");
            //e.Handled = regex.IsMatch(e.Text);
        }

    }
}
