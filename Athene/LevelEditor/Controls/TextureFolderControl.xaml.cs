using System;
using System.Collections.Generic;
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

namespace LevelEditor.Controls
{
    /// <summary>
    /// Represents a folder in the OperateControl
    /// </summary>
    public partial class TextureFolderControl : UserControl
    {
        /// <summary>
        /// Folder for operate control
        /// </summary>
        /// <param name="title">Name of folder</param>
        public TextureFolderControl(string title)
        {
            InitializeComponent();
            Title = title;
            TextBlockTitle.Text = title;

            ExpandInpandButton.Click += (s, e) =>
            {
                if (MainWrapPanel.Visibility == Visibility.Collapsed)
                {
                    MainWrapPanel.Visibility = Visibility.Visible;
                    ExpandInpandButton.Content = "Einklappen";
                }
                else
                {
                    MainWrapPanel.Visibility = Visibility.Collapsed;
                    ExpandInpandButton.Content = "Ausklappen";
                }
            };
        }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Adds a RadioButton to the folder
        /// </summary>
        /// <param name="button"></param>
        public void AddRadioButton(RadioButton button)
        {
            MainWrapPanel.Children.Add(button);
        }

        /// <summary>
        /// Returns the amount of radio buttons
        /// </summary>
        /// <returns></returns>
        public int CountRadioButtons()
        {
            return MainWrapPanel.Children.Count;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
