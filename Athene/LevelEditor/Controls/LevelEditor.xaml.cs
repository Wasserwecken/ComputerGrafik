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
using LevelEditor.Controls.RadioButtons;
using LevelEditor.Windows;
using Lib.LevelLoader.Xml;


namespace LevelEditor.Controls
{
    /// <summary>
    /// Interaktionslogik für LevelEditor.xaml
    /// </summary>
    public partial class LevelEditor : UserControl
    {
        /// <summary>
        /// Represents the current Selection in the OperateControl
        /// </summary>
        public RadioButtonBase SelectedRadioButton { get; set; }

        /// <summary>
        /// The size of the buttons in the grid
        /// </summary>
        public int GameItemButtonSize { get; set; }

        /// <summary>
        /// Window to show infos about the selected block
        /// </summary>
        public ShowBlockItemWindow ShowBlockWindow { get; set; }

        /// <summary>
        /// Gamegrid shows the Management to create a new level
        /// </summary>
        public LevelEditor()
        {
            InitializeComponent();
            GameItemButtonSize = 50;

            /* Initialize the OperateControl */
            OperateControl opControl = new OperateControl(this);
            ContentControlOperate.Content = opControl;

            opControl.InitOperateControl();
            opControl.InitAnimatedBlocks();
            opControl.InitDirectory(Directory.GetCurrentDirectory() + Properties.Settings.Default.ImageBaseFolder);
            /* Init the block types */
            BlockTypeComboBox.ItemsSource = Enum.GetValues(typeof(BlockType));
            BlockTypeComboBox.SelectedIndex = 0;
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
        /// When a button in the Grid is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridButtonClicked(object sender, EventArgs e)
        {
            LevelItemButton button = sender as LevelItemButton;
            if (SelectedRadioButton == null || button == null) return;

            if (SelectedRadioButton is RadioButtonSelectTexture)
            {
                var selectedBlockType = (BlockType)BlockTypeComboBox.SelectedItem;
                button.SetXmlBlock(((RadioButtonSelectTexture)SelectedRadioButton).XmlTexture, selectedBlockType);
            }
            if (SelectedRadioButton is RadioButtonSelectAnimation)
            {
                var selectedBlockType = (BlockType)BlockTypeComboBox.SelectedItem;
                button.SetXmlAnimatedBlock(
                    ((RadioButtonSelectAnimation) SelectedRadioButton).XmlAnimatedBlockInformation,
                    selectedBlockType);
            }
            if (SelectedRadioButton is RadioButtonTool)
            {
                if (((RadioButtonTool)SelectedRadioButton).Action == TextureRadioButtonAction.Remove)
                    button.ResetXmlItem();
                if (((RadioButtonTool)SelectedRadioButton).Action == TextureRadioButtonAction.Select)
                {
                    if (button.XmLLevelItem is XmlBlock)
                    {
                        var win = Helper.FindWindowHelper.IsWindowOpen<Window>("ShowBlockWindow");
                        if (win != null)
                        {
                            ((ShowBlockItemWindow)win).ShowBlock(button.XmLLevelItem as XmlBlock, button.XmlTexture);
                            win.Show();
                        }
                        else
                        {
                            win = new ShowBlockItemWindow() { Name = "ShowBlockWindow" };
                            ((ShowBlockItemWindow)win).ShowBlock(button.XmLLevelItem as XmlBlock, button.XmlTexture);
                            win.Show();
                        }
                    }
                }

            }
        }

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
            for (int i = xStart; i <= xEnd; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(GameItemButtonSize);
                MainGrid.ColumnDefinitions.Add(colDef);
            }

            for (int i = yStart; i <= yEnd; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(GameItemButtonSize);
                MainGrid.RowDefinitions.Add(rowDef);
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
                    MainGrid.Children.Add(button);
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

      

     

    }
}
