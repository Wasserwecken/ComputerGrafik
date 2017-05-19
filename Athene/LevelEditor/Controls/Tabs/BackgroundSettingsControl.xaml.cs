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
using Lib.LevelLoader.Xml.LevelItems;

namespace LevelEditor.Controls.Tabs
{
    /// <summary>
    /// Interaktionslogik für BackgroundSettingsControl.xaml
    /// </summary>
    public partial class BackgroundSettingsControl : UserControl
    {
        public BackgroundSettingsControl()
        {
            InitializeComponent();

            ComboBoxBackgrounds.ItemsSource = BackgroundLoader.GetBackgrounds();
            if (ComboBoxBackgrounds.Items.Count > 0)
                ComboBoxBackgrounds.SelectedIndex = 0;

            AddBackgroundButton.Click += AddBackgroundButton_Click;
        }

        private void AddBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ComboBoxBackgrounds.SelectedItem;
            if (selectedItem != null && selectedItem is XmlBackgroundItem)
            {
                XmlBackgroundItem newItem = new XmlBackgroundItem()
                {
                    Index = 0,
                    Path = ((XmlBackgroundItem) selectedItem).Path
                };
                ItemsControlBackground.Items.Add(newItem);
            }
        }

        private void Index_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public List<XmlBackgroundItem> BackgroundItems
        {
            get
            {
                List<XmlBackgroundItem> returnList = new List<XmlBackgroundItem>();
                foreach (var item in ItemsControlBackground.Items)
                {
                    if(item is XmlBackgroundItem)
                        returnList.Add((XmlBackgroundItem)item);
                }
                return returnList;
            }
            set
            {
                foreach (var xmlBackgroundItem in value)
                {
                    ItemsControlBackground.Items.Add(xmlBackgroundItem);
                }
            }
        }

        private void ButtonDeleteXmlCheckpoint_OnClick(object sender, RoutedEventArgs e)
        {
            
            if (sender is Button && ((Button)sender).DataContext is XmlBackgroundItem)
            {
                ItemsControlBackground.Items.Remove(((Button)sender).DataContext as XmlBackgroundItem);
            }
        }
    }
}
