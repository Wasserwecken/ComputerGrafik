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
using System.Windows.Shapes;
using LevelEditor.Controls.LevelItemPresenter;
using LevelEditor.Controls.RadioButtons;
using LevelEditor.Editor;
using LevelEditor.Windows;
using Lib.LevelLoader.LevelItems;
using Lib.LevelLoader.Xml;
using Lib.LevelLoader.Xml.LevelItems;


namespace LevelEditor.Controls
{
    /// <summary>
    /// Interaktionslogik für LevelEditorControl.xaml
    /// </summary>
    public partial class LevelEditorControl : UserControl
    {
        #region private members & properties

        /// <summary>
        /// Represents the current Selection in the OperateControl
        /// </summary>
        public RadioButtonBase SelectedRadioButton { get; set; }

        /// <summary>
        /// The grid for the level
        /// </summary>
        public LevelGrid LevelGrid { get; set; }

        /// <summary>
        /// Window to show infos about the selected block
        /// </summary>
        public ShowBlockItemWindow ShowBlockWindow { get; set; }

        /// <summary>
        /// the operate control
        /// </summary>
        public OperateControl OperateControl { get; set; }

        /// <summary>
        /// Editor mode
        /// </summary>
        public EditorMode EditorMode { get; set; }


        #endregion

        #region constructor

        /// <summary>
        /// Gamegrid shows the Management to create a new level
        /// </summary>
        public LevelEditorControl()
        {
            InitializeComponent();

            /* Initialize the OperateControl */
            OperateControl = new OperateControl(this);
            ContentControlOperate.Content = OperateControl;

            OperateControl.InitOperateControl();
            OperateControl.InitCheckpoints();
            OperateControl.InitCollectableItems();
            OperateControl.InitAnimatedBlocks();
            OperateControl.InitDirectory(Directory.GetCurrentDirectory() + Properties.Settings.Default.ImageBaseFolder);
            /* Init the block types */

            SettingsBlockControl.BlockTypeComboBox.ItemsSource = Enum.GetValues(typeof(BlockType));
            SettingsBlockControl.BlockTypeComboBox.SelectedIndex = 0;

            RadioButtonEditorMode.IsChecked = true;
        }

        #endregion

        #region methods and eventhandler

        /// <summary>
        /// When a new object is selected in the operate control, the current SelectedRadioButton needs to update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TextureRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectedRadioButton = sender as RadioButtonBase;
        }

        /// <summary>
        /// Initializes a Grid to create a new level
        /// </summary>
        /// <param name="xStart">start x</param>
        /// <param name="xEnd">end x</param>
        /// <param name="yStart">start y</param>
        /// <param name="yEnd">end y</param>
        public void InitNewGrid(int xStart, int xEnd, int yStart, int yEnd)
        {
            LevelGrid = new LevelGrid(this);
            LevelGrid.InitNewGrid(xStart, xEnd, yStart, yEnd);
            LevelGridScrollViewer.Content = LevelGrid;


        }

        /// <summary>
        /// When a button in the Grid is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GridButtonClicked(object sender, EventArgs e)
        {
            LevelItemButton button = sender as LevelItemButton;
            var selectedRadioButton = SelectedRadioButton;


            if (selectedRadioButton == null || button == null) return;

            if (selectedRadioButton is RadioButtonSelectTexture)
            {
                HandleTextureRadioButton(selectedRadioButton as RadioButtonSelectTexture, button);
            }
            if (selectedRadioButton is RadioButtonSelectAnimation)
            {
                HandleAnimationRadioButton(selectedRadioButton as RadioButtonSelectAnimation, button);
            }
            if (selectedRadioButton is RadioButtonTool)
            {
                HandleRadioButtonTool(selectedRadioButton as RadioButtonTool, button);
            }
            if (selectedRadioButton is RadioButtonSelectCollectable)
            {
                HandleRadioButtonCollectable(selectedRadioButton as RadioButtonSelectCollectable, button);
            }
            if (selectedRadioButton is RadioButtonSelectCheckpoint)
            {
                HandleCheckpointRadioButton(selectedRadioButton as RadioButtonSelectCheckpoint, button);
            }
        }

        #endregion

        #region radioButton handler

        /// <summary>
        /// handles a animation radio button
        /// </summary>
        /// <param name="radioButton"></param>
        /// <param name="button"></param>
        private void HandleAnimationRadioButton(RadioButtonSelectAnimation radioButton, LevelItemButton button)
        {

            var selectedBlockType = (BlockType) SettingsBlockControl.BlockTypeComboBox.SelectedItem;
            var selectedCollision = (bool) SettingsBlockControl.CollisionYesRadioButton.IsChecked;
            var selectedDamage = (int)SettingsBlockControl.DamageSlider.Value;

            if (EditorMode == EditorMode.AttachMode)
            {
                if (button.ItemPresenter != null)
                {
                    button.AttachLink(BlockLinkType.Animation, radioButton.XmlAnimation);
                }
            }
            else if (EditorMode == EditorMode.EditorMode)
            {
                button.SetXmlAnimatedBlock(radioButton.XmlAnimation,
                selectedBlockType, selectedCollision, selectedDamage);
            }


        }

        /// <summary>
        /// handles a collectable radio button
        /// </summary>
        /// <param name="radioButton"></param>
        /// <param name="button"></param>
        private void HandleRadioButtonCollectable(RadioButtonSelectCollectable radioButton, LevelItemButton button)
        {
            button.SetXmlCollectable(radioButton.XmlCollectable);
        }

        /// <summary>
        /// handles a tool radio button
        /// </summary>
        /// <param name="radioButton"></param>
        /// <param name="button"></param>
        private void HandleRadioButtonTool(RadioButtonTool radioButton, LevelItemButton button)
        {

            if (radioButton.Action == TextureRadioButtonAction.Remove)
                button.ResetXmlItem();
            if (radioButton.Action == TextureRadioButtonAction.RemoveAttachedTexture)
                button.AttachLink(BlockLinkType.Image, null);
            if (radioButton.Action == TextureRadioButtonAction.Select && button.ItemPresenter != null)
            {
                if (button.ItemPresenter.GetType() == typeof(XmlBlockPresenter) || button.ItemPresenter.GetType() == typeof(XmlAnimatedBlockPresenter))
                {
                    var editControl = new Edit.EditBlockControl();
                    XmlTexture texture = null;
                    if (button.ItemPresenter is XmlBlockPresenter)
                        texture = ((XmlBlockPresenter)button.ItemPresenter).XmlTexture;
                    editControl.ShowBlock(button.ItemPresenter.XmLLevelItemBase as XmlBlock, texture, button);
                    ContentControlInfo.Content = editControl;
                }
                else if (button.ItemPresenter.GetType() == typeof(XmlCheckpointPresenter))
                {
                    var editControl = new Edit.EditCheckpointControl();
                    editControl.ShowCheckpoint(button.ItemPresenter.XmLLevelItemBase as XmlCheckpoint, button);
                    ContentControlInfo.Content = editControl;
                }
                else
                {
                    TextBlock infoText = new TextBlock()
                    {
                        Text = "Nicht verfügbar",
                        FontSize = 20,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(35)
                    };
                    ContentControlInfo.Content = infoText;
                }
            }
        }

        /// <summary>
        /// handles a texture radio button
        /// </summary>
        /// <param name="radioButton"></param>
        /// <param name="button"></param>
        private void HandleTextureRadioButton(RadioButtonSelectTexture radioButton, LevelItemButton button)
        {
            var selectedRadioButton = SelectedRadioButton;
            var selectedBlockType = (BlockType)SettingsBlockControl.BlockTypeComboBox.SelectedItem;
            var selectedCollision = SettingsBlockControl.CollisionYesRadioButton.IsChecked != null && (bool)SettingsBlockControl.CollisionYesRadioButton.IsChecked;
            var selectedDamage = (int)SettingsBlockControl.DamageSlider.Value;
            var selectedIsScrolling = SettingsBlockControl.ScrollingActiveCheckBox.IsChecked != null && (bool)SettingsBlockControl.ScrollingActiveCheckBox.IsChecked;
            var selectedScrollingLength = Convert.ToInt32(SettingsBlockControl.ScrollingIntervallTextBox.Text);
            var selectedScrollingDirectionX = (float)Convert.ToDouble(SettingsBlockControl.ScrollingXDirectionTextBox.Text);
            var selectedScrollingDirectionY = (float)Convert.ToDouble(SettingsBlockControl.ScrollingYDirectionTextBox.Text);

            if (EditorMode == EditorMode.AttachMode)
            {
                if (button.ItemPresenter != null)
                {
                    button.AttachLink(BlockLinkType.Image, radioButton.XmlTexture);
                }
            }
            else if (EditorMode == EditorMode.EditorMode)
            {
                button.SetXmlBlock(radioButton.XmlTexture, selectedBlockType, selectedCollision, selectedDamage,
                selectedIsScrolling, selectedScrollingLength, selectedScrollingDirectionX, selectedScrollingDirectionY);
            }
        }

        private void HandleCheckpointRadioButton(RadioButtonSelectCheckpoint radioButton, LevelItemButton button)
        {
                button.SetXmlCheckpoint(radioButton.XmlCheckpointItem);
        }

        #endregion

        /// <summary>
        /// sets the correct editor mode 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonLevelEditorMode_OnChecked(object sender, RoutedEventArgs e)
        {
            if (Equals(sender, RadioButtonEditorMode))
            {
                EditorMode = EditorMode.EditorMode;
                foreach (var control in OperateControl.TextureWrapPanel.Children)
                {
                    if (control is TextureFolderControl)
                    {
                        TextureFolderControl folder = control as TextureFolderControl;
                        folder.Visibility = Visibility.Visible;

                    }
                }
            }
            else if (Equals(sender, RadioButtonAttachMode))
            {
                EditorMode = EditorMode.AttachMode;
                foreach (var control in OperateControl.TextureWrapPanel.Children)
                {
                    if (control is TextureFolderControl)
                    {
                        TextureFolderControl folder = control as TextureFolderControl;
                        if (folder.Attachable)
                            folder.Visibility = Visibility.Visible;
                        else folder.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
    }
}
