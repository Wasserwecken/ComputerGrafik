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
using Lib.LevelLoader.LevelItems;
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
        /// The grid for the level
        /// </summary>
        public  LevelGrid LevelGrid { get; set; }

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
    }
}
