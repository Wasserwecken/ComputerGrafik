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
using System.Windows.Shapes;

namespace LevelEditor.Windows
{
    /// <summary>
    /// Interaktionslogik für AddGridWindow.xaml
    /// </summary>
    public partial class AddGridWindow : Window
    {
        public AddGridWindow()
        {
            InitializeComponent();

            ButtonAddGrid.Click += (s, e) =>
            {
                int xStart, xEnd, yStart, yEnd;

                if (int.TryParse(InputXStart.Text, out xStart) && int.TryParse(InputXEnd.Text, out xEnd) &&
                    int.TryParse(InputYStart.Text, out yStart) && int.TryParse(InputYEnd.Text, out yEnd) &&
                    !String.IsNullOrWhiteSpace(InputXStart.Text) && !String.IsNullOrWhiteSpace(InputXEnd.Text) &&
                     !String.IsNullOrWhiteSpace(InputYStart.Text) && !String.IsNullOrWhiteSpace(InputYEnd.Text))
                {

                    XStart = xStart;
                    YStart = yStart;
                    XEnd = xEnd;
                    YEnd = yEnd;
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Bitte gültige Werte eingeben");
                }

            };
        }

        public int XStart { get; set; }
        public int YStart { get; set; }
        public int XEnd { get; set; }
        public int YEnd { get; set; }
    }
}
