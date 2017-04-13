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
using System.Windows.Resources;
using System.Windows.Shapes;
using SimeraExample.Code.Xml;

namespace LevelEditor.Controls
{
    /// <summary>
    /// Interaktionslogik für GridButton.xaml
    /// </summary>
    public partial class GridButton : UserControl
    {
        public GridButton(int x, int y, string path = null)
        {
            InitializeComponent();

            X = x;
            Y = y;

            if(path != null)
                Path = path;

            MainButton.Content = X + " : " + Y;

            MainButton.Click += (s,e) => 
                OnClick(e);
        }

        public string TextureId { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public BlockType BlockType { get; set; }

        private string path;

        public string Path
        {
            get { return path; }
            private set
            {
                path = value;

                if (path != null)
                {
                    ImageSource src = new BitmapImage(new Uri(path, UriKind.Relative));
                    ImageBrush brush = new ImageBrush(src);
                    MainButton.Background = brush;
                }
                else
                {
                    MainButton.Background = null;
                }
                
            }
            
        }


        public event EventHandler Click;

        internal virtual void OnClick(EventArgs e)
        {
            var myEvent = Click;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public void SetTexture(string textureId, string path)
        {
            Path = path;
            TextureId = textureId;
        }
      

    }
}
