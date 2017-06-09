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

namespace LevelEditor.Controls.Tabs
{
    /// <summary>
    /// Interaktionslogik für LevelSettingsControl.xaml
    /// </summary>
    public partial class LevelSettingsControl : UserControl
    {
        public LevelSettingsControl()
        {
            InitializeComponent();
        }

        public int SpawnX => Convert.ToInt32(InputStartpositionX.Text);
        public int SpawnY => Convert.ToInt32(InputStartpositionY.Text);

        public int FinishX => Convert.ToInt32(InputFinishX.Text);
        public int FinishY => Convert.ToInt32(InputFinishY.Text);
    }
}
