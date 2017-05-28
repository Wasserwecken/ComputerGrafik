using Lib.Level.Base;
using Lib.LevelLoader.Xml.LevelItems;
using Lib.Tools;
using Lib.Visuals.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Items
{
    public class ParalaxBackground
    {
        /// <summary>
        /// All sprites of the background
        /// </summary>
        private List<ParalaxBackgroundItem> Layers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Vector2 LevelSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Vector2 BackgroundTileSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Vector2 Position { get; set; }
        

        /// <summary>
        /// Initialises a new parallax background
        /// </summary>
        /// <param name="parentLevel"></param>
        public ParalaxBackground(Vector2 position, Vector2 levelSize, float backgroundHeight, Vector2 paralaxFactor, List<XmlBackgroundItem> givenLayers)
        {
            Position = position;
            LevelSize = levelSize;

            var levelAspectRatio = (LevelSize.X / LevelSize.Y);
            BackgroundTileSize = new Vector2(backgroundHeight * levelAspectRatio, backgroundHeight); // size of one tile of the background which will be repeated on the sprite

            Layers = PrepareBackgroundItems(givenLayers);
            Layers = Layers.OrderByDescending(f => f.Index).ToList();

            foreach(var item in Layers)
                item.DisplacementFactor = Easing.Linear(givenLayers.Count - 1 - item.Index, givenLayers.Count - 1) * paralaxFactor;
        }


        /// <summary>
        /// Render the background
        /// </summary>
        public void Draw(Vector2 viewPoint)
        {
            //displacement will be relative to the position of the player to the level center
            Vector2 displacementFactor = new Vector2(viewPoint.X / (LevelSize.X / 2), viewPoint.Y / (LevelSize.Y / 2));
            Vector2 displacementValue = BackgroundTileSize * displacementFactor; // applying the factor to get the absolute displacement

            foreach (var layer in Layers)
                layer.Sprite.Draw((Position) + (displacementValue * layer.DisplacementFactor), Vector2.One);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<ParalaxBackgroundItem> PrepareBackgroundItems(List<XmlBackgroundItem> givenLayers)
        {
            var layers = new List<ParalaxBackgroundItem>();

            for (int index = 0; index < givenLayers.Count; index++)
            {
                SpriteStatic sprite = GetBackgroundSprite(givenLayers[index].Path);
                int layerIndex = givenLayers[index].Index;
                layers.Add(new ParalaxBackgroundItem(layerIndex, Vector2.Zero, sprite));
            }

            return layers;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BackgroundSize"></param>
        /// <returns></returns>
        public SpriteStatic GetBackgroundSprite(string imagePath)
        {
            SpriteStatic sprite = new SpriteStatic(LevelSize, imagePath, TextureWrapMode.MirroredRepeat, TextureWrapMode.ClampToEdge);
            //respect the texture aspect ratio to avoid verzerrte textures
            float textureAspectRatio = sprite.SpriteTexture.Width / sprite.SpriteTexture.Height;
            float tileHeightCoverFactor = LevelSize.Y / BackgroundTileSize.Y; // calculate how often a tile has space on the sprite
            float tileCoverFactorX = tileHeightCoverFactor;
            float tileCoverFactorY = tileHeightCoverFactor * textureAspectRatio;

            //doing strange things, but it works :-/
            sprite.SetTextureCoordinates(-tileCoverFactorX / 4, -tileCoverFactorY / 2.5f, tileCoverFactorX, tileCoverFactorY);

            return sprite;
        }
    }
}
