using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
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
    /// OperateControl represents the Items on the left side, contains also many RadioButtons for the LevelEditor
    /// </summary>
    public partial class OperateControl : UserControl
    {
        private LevelEditor _parentLevelEditor;
        private readonly string _groupName = "textures";

        public OperateControl(LevelEditor parentLevelEditor)
        {
            InitializeComponent();

            _parentLevelEditor = parentLevelEditor;
        }

        /// <summary>
        /// Initializes a directory with all its images to the Control
        /// </summary>
        /// <param name="directory"></param>
        public void InitDirectory(string directory)
        {
            List<TextureFolderControl> listOut = new List<TextureFolderControl>();
            LoadTextureEntries(Directory.GetCurrentDirectory() + @"\Pics\", listOut);
            foreach (var item in listOut)
            {
                TextureWrapPanel.Children.Add(item);
            }
        }

        public void InitOperateControl()
        {
            TextureFolderControl folder = new TextureFolderControl("Werkzeuge");

            SelectTextureRadioButton nullButton = new SelectTextureRadioButton(Directory.GetCurrentDirectory() + @"/CommonImages/Delete-96.png", "", TextureRadioButtonAction.Remove, "Bild entfernen")
            {
                GroupName = _groupName
            };
            SelectTextureRadioButton selectButton = new SelectTextureRadioButton(Directory.GetCurrentDirectory() + @"/CommonImages/Cursor-96.png", "", TextureRadioButtonAction.Select, "Auswählen")
            {
                GroupName = _groupName
            };
            nullButton.Checked += _parentLevelEditor.TextureRadioButton_Checked;
            selectButton.Checked += _parentLevelEditor.TextureRadioButton_Checked;

            folder.AddRadioButton(nullButton);
            folder.AddRadioButton(selectButton);
            TextureWrapPanel.Children.Add(folder);
        }


        /// <summary>
        /// Loads a list of TextureFolderControls
        /// </summary>
        /// <param name="sDir">source directory</param>
        /// <param name="textureFolderControls">the list</param>
        public void LoadTextureEntries(string sDir, List<TextureFolderControl> textureFolderControls)
        {
            try
            {
                var dir = new DirectoryInfo(sDir);
                var files = dir.GetFiles();
                var texFolderControl = new TextureFolderControl(dir.Name);
                foreach (var fileInfo in files)
                {
                    /* Only png files */
                    if (fileInfo.Extension != ".png")
                        continue;
                    SelectTextureRadioButton button = new SelectTextureRadioButton(fileInfo.FullName, fileInfo.Name.Replace(fileInfo.Extension, string.Empty), TextureRadioButtonAction.LoadTexture, fileInfo.Name);
                    button.Checked += _parentLevelEditor.TextureRadioButton_Checked;
                    button.GroupName = _groupName;
                    texFolderControl.AddRadioButton(button);
                }
                if(texFolderControl.CountRadioButtons() > 0)
                    textureFolderControls.Add(texFolderControl);
                foreach (var d in dir.GetDirectories())
                {
                    LoadTextureEntries(d.FullName, textureFolderControls);
                }
            }
            catch (Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }
    }
}
