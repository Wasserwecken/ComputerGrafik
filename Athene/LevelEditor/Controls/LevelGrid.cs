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
        public LevelEditorControl LevelEditor { get; set; }

        /// <summary>
        /// The size of the buttons in the grid
        /// </summary>
        public int GameItemButtonSize { get; set; }

        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        public LevelGrid(Controls.LevelEditorControl editor)
        {
            LevelEditor = editor;
            ShowGridLines = true;
            GameItemButtonSize = 50;

            
        }



        public void InitNewGrid(int xStart, int xEnd, int yStart, int yEnd)
        {
            MaxX = xEnd;
            MaxY = yEnd;
            MinX = xStart;
            MinY = yStart;

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
                    button.Click += LevelEditor.GridButtonClicked;
                    button.MouseMove += Button_MouseMove;
                    colunmIndex++;
                }
                colunmIndex = 0;
                rowIndex++;
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
                LevelEditor.GridButtonClicked(sender, e);
            }
        }
       

        

    }
}
