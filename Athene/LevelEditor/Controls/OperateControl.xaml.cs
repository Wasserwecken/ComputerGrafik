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
using LevelEditor.Controls.RadioButtons;
using Lib.LevelLoader;

namespace LevelEditor.Controls
{
    /// <summary>
    /// OperateControl represents the Items on the left side, contains also many RadioButtons for the LevelEditorControl
    /// </summary>
    public partial class OperateControl : UserControl
    {
        private LevelEditorControl _parentLevelEditor;

        public OperateControl(LevelEditorControl parentLevelEditor)
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
            LoadTextureEntries(directory, listOut);
            foreach (var item in listOut)
            {
                TextureWrapPanel.Children.Add(item);
            }
        }

        public void InitOperateControl()
        {
            TextureFolderControl folder = new TextureFolderControl("Werkzeuge");

            RadioButtonTool nullButton = new RadioButtonTool("Block entfernen",
                Directory.GetCurrentDirectory() + @"/CommonImages/Delete-96.png", TextureRadioButtonAction.Remove);

            RadioButtonTool selectButton = new RadioButtonTool("Auswählen",
                Directory.GetCurrentDirectory() + @"/CommonImages/Cursor-96.png", TextureRadioButtonAction.Select);

            RadioButtonTool removeAttachedButton = new RadioButtonTool("Angehängte Textur\nentfernen",
                Directory.GetCurrentDirectory() + @"/CommonImages/Delete-File-96.png", TextureRadioButtonAction.RemoveAttachedTexture);


            nullButton.Checked += _parentLevelEditor.TextureRadioButton_Checked;
            selectButton.Checked += _parentLevelEditor.TextureRadioButton_Checked;
            removeAttachedButton.Checked += _parentLevelEditor.TextureRadioButton_Checked;
           

            folder.AddRadioButton(nullButton);
            folder.AddRadioButton(selectButton);
            folder.AddRadioButton(removeAttachedButton);
            TextureWrapPanel.Children.Add(folder);
        }

        public void InitCheckpoints()
        {
            var checkPoints = CheckpointLoader.GetCheckpoints().Checkpoints;
            var folder = new TextureFolderControl("Checkpoints");

            foreach (var xmlCheckpointItem in checkPoints)
            {
                RadioButtonSelectCheckpoint radioButton = new RadioButtonSelectCheckpoint(xmlCheckpointItem);
                radioButton.Checked += _parentLevelEditor.TextureRadioButton_Checked;
                folder.AddRadioButton(radioButton);
            }
            TextureWrapPanel.Children.Add(folder);


        }

        public void InitEnemies()
        {
            var enemyList = EnemyLoader.GetEnemies();
            var folder = new TextureFolderControl("Enemies");

            foreach (var xmlEnemy in enemyList.Enemies)
            {
                RadioButtonSelectEnemy radioButton = new RadioButtonSelectEnemy(xmlEnemy);
                radioButton.Checked += _parentLevelEditor.TextureRadioButton_Checked;
                folder.AddRadioButton(radioButton);
            }
            TextureWrapPanel.Children.Add(folder);
        }

        public void InitCollectableItems()
        {
            var itemList = CollectableLoader.GetCollectables();
            var folder = new TextureFolderControl("Collectable");

            foreach (var xmlCollectable in itemList.Items)
            {
                RadioButtonSelectCollectable radioButton = new RadioButtonSelectCollectable(xmlCollectable);
                radioButton.Checked += _parentLevelEditor.TextureRadioButton_Checked;
                folder.AddRadioButton(radioButton);
            }
            TextureWrapPanel.Children.Add(folder);

        }

        public void InitAnimatedBlocks()
        {
            var animationList = AnimationLoader.GetBlockAnimations();
            var folder = new TextureFolderControl("Animierte Blöcke", true);

            foreach (var xmlAnimatedBlock in animationList.Animations)
            {
                RadioButtonSelectAnimation radioButton = new RadioButtonSelectAnimation(xmlAnimatedBlock);
                radioButton.Checked += _parentLevelEditor.TextureRadioButton_Checked;
                folder.AddRadioButton(radioButton);
            }
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
                var texFolderControl = new TextureFolderControl(dir.Name + " (Blöcke)", true);
                foreach (var fileInfo in files)
                {
                    /* Only png files */
                    if (fileInfo.Extension != ".png")
                        continue;
                    RadioButtonSelectTexture button = new RadioButtonSelectTexture(fileInfo);
                    button.Checked += _parentLevelEditor.TextureRadioButton_Checked;
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
