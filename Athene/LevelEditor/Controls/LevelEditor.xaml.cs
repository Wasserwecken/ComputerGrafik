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
using Simloader.Xml;

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
        public SelectTextureRadioButton SelectedRadioButton { get; set; }
        public int GameItemButtonSize { get; set; }

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
            opControl.InitDirectory(Directory.GetCurrentDirectory() + Properties.Settings.Default.ImageBaseFolder);
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
            GameItemButton button = sender as GameItemButton;
            if (SelectedRadioButton == null || button == null) return;

            if (SelectedRadioButton.Action == TextureRadioButtonAction.Remove)
            {
                button.SetTexture(null, null);
            }
            else if (SelectedRadioButton.Action == TextureRadioButtonAction.Select)
            {
                return;
            }
            else if (SelectedRadioButton.Action == TextureRadioButtonAction.LoadTexture)
            {
                button.SetTexture(SelectedRadioButton.Id, @SelectedRadioButton.TexturePath);
            }

        }

        /// <summary>
        /// When a new object is selected in the operate control, the current SelectedRadioButton needs to update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TextureRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectedRadioButton = sender as SelectTextureRadioButton;
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
            for (int i = xStart; i < xEnd; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(GameItemButtonSize);
                MainGrid.ColumnDefinitions.Add(colDef);
            }

            for (int i = yStart; i < yEnd; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(GameItemButtonSize);
                MainGrid.RowDefinitions.Add(rowDef);
            }

            int rowIndex = 0;
            int colunmIndex = 0;

            for (int y = yEnd; y > yStart; y--)
            {
                for (int x = xStart; x < xEnd; x++)
                {
                    GameItemButton button = new GameItemButton(x, y);
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

        /// <summary>
        /// Returns the XmlLevel of the current Grid
        /// </summary>
        /// <returns></returns>
        public XmlLevel GetXmlLevel()
        {
            XmlLevel levelReturn = new XmlLevel();
            levelReturn.Blocks = new List<XmlBlock>();
            levelReturn.Textures = new List<XmlTexture>();

            foreach (GameItemButton button in MainGrid.Children)
            {
                if(String.IsNullOrWhiteSpace(button.TextureId)) continue;
                if (levelReturn.Textures.Count(t => t.Path == button.TexturePathRelative) == 0)
                {
                    XmlTexture texture = new XmlTexture()
                    {
                        Id = button.TextureId,
                        Path = button.TexturePathRelative
                    };
                    levelReturn.Textures.Add(texture);
                }
                XmlBlock block = new XmlBlock()
                {
                    X = button.X,
                    Y = button.Y,
                    Texture = button.TextureId,
                    BlockType = BlockType.Walkable
                };
                levelReturn.Blocks.Add(block);
            }
            return levelReturn;
        }

        /// <summary>
        /// Initializes a new level on the grid
        /// </summary>
        /// <param name="level"></param>
        public void InitXmlLevel(XmlLevel level)
        {
            foreach (var block in level.Blocks)
            {
                foreach (GameItemButton button in MainGrid.Children)
                {
                    if (button.X == block.X && button.Y == block.Y)
                    {
                        var texture = level.Textures.First(t => t.Id == block.Texture);
                        
                        if(!File.Exists(texture.Path))
                        {
                            MessageBox.Show(texture.Path + " wurde nicht gefunden, Level kann nicht geladen werden",
                                "XmlLevel laden fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
                            MainGrid.Children.Clear();
                            IsEnabled = false;
                            return;
                        }

                        button.SetTexture(texture.Id, @texture.Path);
                    }
                }
            }
        }
    }
}
