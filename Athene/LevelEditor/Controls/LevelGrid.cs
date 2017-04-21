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
            var selectedBlockType = (BlockType) LevelEditor.BlockTypeComboBox.SelectedItem;

            if (selectedRadioButton == null || button == null) return;

            if (selectedRadioButton is RadioButtonSelectTexture)
            {
                button.SetXmlBlock(((RadioButtonSelectTexture)selectedRadioButton).XmlTexture, selectedBlockType);
            }
            if (selectedRadioButton is RadioButtonSelectAnimation)
            {
                button.SetXmlAnimatedBlock(
                    ((RadioButtonSelectAnimation)selectedRadioButton).XmlAnimation,
                    selectedBlockType);
            }
            if (selectedRadioButton is RadioButtonTool)
            {
                if (((RadioButtonTool)selectedRadioButton).Action == TextureRadioButtonAction.Remove)
                    button.ResetXmlItem();
                if (((RadioButtonTool)selectedRadioButton).Action == TextureRadioButtonAction.Select)
                {
                    if (button.XmLLevelItemBase is XmlBlock)
                    {
                        var win = Helper.FindWindowHelper.IsWindowOpen<Window>("ShowBlockWindow");
                        if (win != null)
                        {
                            ((ShowBlockItemWindow)win).ShowBlock(button.XmLLevelItemBase as XmlBlock, button.XmlTexture);
                            win.Show();
                        }
                        else
                        {
                            win = new ShowBlockItemWindow() { Name = "ShowBlockWindow" };
                            ((ShowBlockItemWindow)win).ShowBlock(button.XmLLevelItemBase as XmlBlock, button.XmlTexture);
                            win.Show();
                        }
                    }
                }
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
    }
}
