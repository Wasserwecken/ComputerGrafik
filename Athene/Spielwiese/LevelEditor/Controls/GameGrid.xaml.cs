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
using SimeraExample.Code.Xml;

namespace LevelEditor.Controls
{
    /// <summary>
    /// Interaktionslogik für GameGrid.xaml
    /// </summary>
    public partial class GameGrid : UserControl
    {
        public SelectTextureRadioButton SelectedRadioButton { get; set; }

        /// <summary>
        /// Gamegrid shows the Management to create a new level
        /// </summary>
        public GameGrid()
        {
            InitializeComponent();
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                GridButtonClicked(sender, e);
            }
        }

        private void GridButtonClicked(object sender, EventArgs e)
        {
            GridButton button = sender as GridButton;

            if (SelectedRadioButton == null) return;

            button.Path = @SelectedRadioButton.Texture;
            button.TextureId = SelectedRadioButton.Id;
        }

        /// <summary>
        /// Inits the Textures to choose
        /// </summary>
        /// <param name="path">Path of Images</param>
        public void InitTextures(string path)
        {
            var files = Directory.GetFiles(path);
            string groupName = "textures";

            SelectTextureRadioButton nullButton = new SelectTextureRadioButton(null, "")
            {
                GroupName = groupName
            };
            nullButton.Checked += (s, e) => SelectedRadioButton = s as SelectTextureRadioButton;
            TextureWrapPanel.Children.Add(nullButton);

            foreach (string fileName in files)
            {
                SelectTextureRadioButton radioButton = new SelectTextureRadioButton(fileName, fileName.Replace(path, "").Replace(".png", ""));

                radioButton.Checked += (s, e) => SelectedRadioButton = s as SelectTextureRadioButton;

                radioButton.GroupName = groupName;
                TextureWrapPanel.Children.Add(radioButton);
            }

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
            int size = 50;

            for (int i = xStart; i < xEnd; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(size);
                MainGrid.ColumnDefinitions.Add(colDef);
            }

            for (int i = yStart; i < yEnd; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(size);
                MainGrid.RowDefinitions.Add(rowDef);
            }

            int rowIndex = 0;
            int colunmIndex = 0;

            for (int y = yEnd; y > yStart; y--)
            {
                for (int x = xStart; x < xEnd; x++)
                {
                    GridButton button = new GridButton(x, y);
                    button.Width = size;
                    button.Height = size;
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

            foreach (GridButton button in MainGrid.Children)
            {
                if(String.IsNullOrWhiteSpace(button.TextureId)) continue;
                if (levelReturn.Textures.Count(t => t.Path == button.Path) == 0)
                {
                    XmlTexture texture = new XmlTexture()
                    {
                        Id = button.TextureId,
                        Path = button.Path
                    };
                    levelReturn.Textures.Add(texture);
                }
                XmlBlock block = new XmlBlock()
                {
                    X = button.X,
                    Y = button.Y,
                    Texture = button.TextureId
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
                foreach (GridButton button in MainGrid.Children)
                {
                    if (button.X == block.X && button.Y == block.Y)
                    {
                        var texture = level.Textures.Where(t => t.Id == block.Texture).First();

                        button.TextureId = texture.Id;
                        button.Path = texture.Path;
                    }
                }
            }
        }

    }
}
