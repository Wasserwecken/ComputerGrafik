using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LevelEditor.Controls;
using LevelEditor.Controls.RadioButtons;
using LevelEditor.Windows;
using Lib.LevelLoader.LevelItems;
using Lib.LevelLoader.Xml;
using LevelEditor.Controls.LevelItemPresenter;

namespace LevelEditor.Controls
{
    public class LevelGrid : Grid
    {
        public Controls.LevelEditor LevelEditor { get; set; }

        /// <summary>
        /// The size of the buttons in the grid
        /// </summary>
        public int GameItemButtonSize { get; set; }

        public LevelGrid(Controls.LevelEditor editor)
        {
            LevelEditor = editor;
            ShowGridLines = true;
            GameItemButtonSize = 50;
        }



        public void InitNewGrid(int xStart, int xEnd, int yStart, int yEnd)
        {
            for (int i = xStart; i <= xEnd; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(GameItemButtonSize);
                ColumnDefinitions.Add(colDef);
            }

            for (int i = yStart; i <= yEnd; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(GameItemButtonSize);
                RowDefinitions.Add(rowDef);
            }

            int rowIndex = 0;
            int colunmIndex = 0;

            for (int y = yEnd; y >= yStart; y--)
            {
                for (int x = xStart; x <= xEnd; x++)
                {
                    LevelItemButton button = new LevelItemButton(x, y);
                    button.Width = GameItemButtonSize;
                    button.Height = GameItemButtonSize;
                    Children.Add(button);
                    Grid.SetRow(button, rowIndex);
                    Grid.SetColumn(button, colunmIndex);
                    button.Click += GridButtonClicked;
                    button.MouseMove += Button_MouseMove;
                    colunmIndex++;
                }
                colunmIndex = 0;
                rowIndex++;
            }
        }


        /// <summary>
        /// When a button in the Grid is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridButtonClicked(object sender, EventArgs e)
        {
            LevelItemButton button = sender as LevelItemButton;
            var selectedRadioButton = LevelEditor.SelectedRadioButton;
           

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
        }

        /// <summary>
        /// If mouse is hold while going over buttons, they are saved like you click on them
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">events</param>
        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                GridButtonClicked(sender, e);
            }
        }
        /// <summary>
        /// handles a texture radio button
        /// </summary>
        /// <param name="radioButton"></param>
        /// <param name="button"></param>
        private void HandleTextureRadioButton(RadioButtonSelectTexture radioButton, LevelItemButton button)
        {
            var selectedRadioButton = LevelEditor.SelectedRadioButton;
            var selectedBlockType = (BlockType)LevelEditor.BlockTypeComboBox.SelectedItem;
            var selectedCollision = (bool)LevelEditor.CollisionYesRadioButton.IsChecked;
            var selectedDamage = (int)LevelEditor.DamageSlider.Value;
            var selectedIsScrolling = (bool)LevelEditor.ScrollingActiveCheckBox.IsChecked;
            var selectedScrollingLength = Convert.ToInt32(LevelEditor.ScrollingIntervallTextBox.Text);
            var selectedScrollingDirectionX = (float)Convert.ToDouble(LevelEditor.ScrollingXDirectionTextBox.Text);
            var selectedScrollingDirectionY = (float)Convert.ToDouble(LevelEditor.ScrollingYDirectionTextBox.Text);

            if (LevelEditor.AppendTextureCheckBox.IsChecked == true)
            {
                if (button.ItemPresenter != null)
                {
                    button.AttachLink(BlockLinkType.Image, radioButton.XmlTexture);
                }
            }
            else
            {
                button.SetXmlBlock(radioButton.XmlTexture, selectedBlockType, selectedCollision, selectedDamage,
                selectedIsScrolling, selectedScrollingLength, selectedScrollingDirectionX, selectedScrollingDirectionY);
            }
        }

        /// <summary>
        /// handles a animation radio button
        /// </summary>
        /// <param name="radioButton"></param>
        /// <param name="button"></param>
        private void HandleAnimationRadioButton(RadioButtonSelectAnimation radioButton, LevelItemButton button)
        {
            var selectedRadioButton = LevelEditor.SelectedRadioButton;
            var selectedBlockType = (BlockType)LevelEditor.BlockTypeComboBox.SelectedItem;
            var selectedCollision = (bool)LevelEditor.CollisionYesRadioButton.IsChecked;
            var selectedDamage = (int)LevelEditor.DamageSlider.Value;

            if (LevelEditor.AppendTextureCheckBox.IsChecked == true)
            {
                if (button.ItemPresenter != null)
                {
                    button.AttachLink(BlockLinkType.Animation, radioButton.XmlAnimation);
                }
            }
            else
            {
                button.SetXmlAnimatedBlock(radioButton.XmlAnimation,
                selectedBlockType, selectedCollision, selectedDamage);
            }

          
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
            if (radioButton.Action == TextureRadioButtonAction.Select)
            {
                if (button.ItemPresenter is XmlBlockPresenter || button.ItemPresenter is XmlAnimatedBlockPresenter)
                {
                    var win = Helper.FindWindowHelper.IsWindowOpen<Window>("ShowBlockWindow");

                    XmlTexture texture = null;
                    if (button.ItemPresenter is XmlBlockPresenter)
                        texture = ((XmlBlockPresenter)button.ItemPresenter).XmlTexture;
                    if (win != null)
                    {
                        ((ShowBlockItemWindow)win).ShowBlock(button.ItemPresenter.XmLLevelItemBase as XmlBlock, texture);
                        win.Show();
                    }
                    else
                    {
                        win = new ShowBlockItemWindow() { Name = "ShowBlockWindow" };
                        ((ShowBlockItemWindow)win).ShowBlock(button.ItemPresenter.XmLLevelItemBase as XmlBlock, texture);
                        win.Show();
                    }
                }
            }
        }

    }
}
