using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Input.Mapping;
using Lib.LevelLoader.LevelItems;
using Lib.Visuals.Graphics;
using OpenTK;
using OpenTK.Input;
using Lib.Tools;
using Lib.Tools.QuadTree;
using Lib.Level.Items;
using Lib.Level.Base;
using Lib.LevelLoader.Xml;
using System.IO;
using Lib.LevelLoader;

namespace Lib.Level
{
    public class Level
    {
        /// <summary>
        /// List of all blocks drawn in the level, like walkable, water, lava, trees..
        /// </summary>
        public List<Block> Blocks { get; set; }

        /// <summary>
        /// List of all players in the level
        /// </summary>
        public List<Player> Players { get; set; }

        /// <summary>
        /// list of all checkpoints
        /// </summary>
        public List<Checkpoint> Checkpoints { get; set; }

        /// <summary>
        /// Poiint which should be the focus for the camera
        /// </summary>
        public Vector2 PlayersCenter { get; private set; }

        /// <summary>
        /// A box based on all active player positions
        /// </summary>
        public Box2D PlayersSpace { get; set; }

        /// <summary>
        /// QuadTree for all level Blocks
        /// </summary>
        private QuadTreeRoot BlocksQuadTree { get; set; }

        /// <summary>
        /// Startposition of players
        /// </summary>
        private Vector2 Startposition { get; set; }
        

        /// <summary>
        /// Initializes a empty level
        /// </summary>
        public Level(XmlLevel xmlLevel)
		{
            Players = new List<Player>();
            Blocks = new List<Block>();
            Checkpoints = new List<Checkpoint>();

            LoadLevelFromXmlLevel(xmlLevel);

            
            Players.Add(PlayerFactory.CreatePlayer(0, Startposition));
            Players.Add(PlayerFactory.CreatePlayer(1, Startposition));

           

           
            InitialiseQuadTree();
		}


        /// <summary>
        /// updates the level
        /// </summary>
        public void UpdateLogic()
        {
            //Moving all the characters first
            foreach (var player in Players)
            {
                player.UpdateLogic();

                var intersections = BlocksQuadTree.GetElementsIn(player.HitBox).ConvertAll(item => (LevelItemBase)item);
                if (intersections.Count > 0)
                    player.HandleIntersections(intersections);
            }

            //Camera settings
            CalculateCameraInformations();
        }
        
        /// <summary>
        /// Draws the level
        /// </summary>
        public void Draw(Box2D cameraFOV)
        {
            //Using the quadtree here because, only blocks in the camera view has to be drawn
            foreach (Block levelBlock in BlocksQuadTree.GetElementsIn(cameraFOV))
                levelBlock.Draw();

            //Draw players
            foreach (var player in Players)
                player.Draw();

            foreach (var checkpoint in Checkpoints)
            {
                checkpoint.Draw();
            }
        }


        /// <summary>
        /// Calculates the information which is needed for the camera
        /// Like zoom level and position
        /// </summary>
        private void CalculateCameraInformations()
        {
            Vector2 playersCenter = Vector2.Zero;
            float minPlayerXPos = 0;
            float minPlayerYPos = 0;
            float maxPlayerXPos = 0;
            float maxPlayerYPos = 0;

            foreach (var player in Players)
            {
                playersCenter += player.HitBox.Position;
                minPlayerXPos = Math.Min(minPlayerXPos, player.ViewPoint.X);
                minPlayerYPos = Math.Min(minPlayerYPos, player.ViewPoint.Y);
                maxPlayerXPos = Math.Max(maxPlayerXPos, player.ViewPoint.X);
                maxPlayerYPos = Math.Max(maxPlayerYPos, player.ViewPoint.Y);
            }

            PlayersSpace = new Box2D(minPlayerXPos, minPlayerYPos, maxPlayerXPos - minPlayerXPos, maxPlayerYPos - minPlayerXPos);
            PlayersCenter = playersCenter / Players.Count;
        }


        /// <summary>
        /// Loads a level from the infos of a XmlLevel
        /// </summary>
        /// <param name="xmlLevel">XmlLevel</param>
        /// <returns>Returns the level</returns>
        private void LoadLevelFromXmlLevel(XmlLevel xmlLevel)
        {
            if (xmlLevel == null)
                throw new Exception("XmLLevel is null");
            
            // all animated sprites should start at the same time.
            // all animated sprites are later started from the list
            // spriteAnimated is the sprite, string is the animation name
            var animatedSpriteList = new Dictionary<SpriteAnimated, string>();

            Startposition = new Vector2(xmlLevel.SpawnX, xmlLevel.SpawnY);

            foreach (var xmlBlock in xmlLevel.Blocks)
            {
                ISprite sprite = null;
                ISprite attachedSprite = null;

                if (xmlBlock.LinkType == BlockLinkType.Image)
                {
                    var xmlTexture = xmlLevel.Textures.FirstOrDefault(t => t.Id == xmlBlock.Link);
                    if (xmlTexture == null)
                        throw new Exception("Texture not found in XML file");
                    sprite = new SpriteStatic(xmlTexture.Path);
                    if (xmlBlock.IsScrolling)
                    {
                        ((SpriteStatic)sprite).StartTextureScroll(new Vector2(xmlBlock.ScrollingDirectionX, xmlBlock.ScrollingDirectionY), xmlBlock.ScrollingLength);
                    }
                    if (xmlBlock.AttachedLink != null)
                    {
                        if (xmlBlock.AttachedLinkType == BlockLinkType.Image.ToString())
                        {
                            var attachedTexture = xmlLevel.Textures.FirstOrDefault(t => t.Id == xmlBlock.AttachedLink);
                            attachedSprite = new SpriteStatic(attachedTexture.Path);
                        }
                        if (xmlBlock.AttachedLinkType == BlockLinkType.Animation.ToString())
                        {

                            attachedSprite = new SpriteAnimated();
                            var xmlAnimation = AnimationLoader.GetBlockAnimations().Animations.First(a => a.Id == xmlBlock.AttachedLink);
                            if (xmlAnimation == null)
                                throw new Exception("Animation not found");
                            ((SpriteAnimated)attachedSprite).AddAnimation(xmlAnimation.Path, xmlAnimation.AnimationLength);
                            // start animation
                            ((SpriteAnimated)attachedSprite).StartAnimation(new DirectoryInfo(xmlAnimation.Path).Name);
                        }


                    }
                }
                if (xmlBlock.LinkType == BlockLinkType.Animation)
                {
                    sprite = new SpriteAnimated();
                    var xmlAnimation = AnimationLoader.GetBlockAnimations().Animations.First(a => a.Id == xmlBlock.Link);
                    if (xmlAnimation == null)
                        throw new Exception("Animation not found");
                    ((SpriteAnimated)sprite).AddAnimation(xmlAnimation.Path, xmlAnimation.AnimationLength);
                    animatedSpriteList.Add((SpriteAnimated)sprite, new DirectoryInfo(xmlAnimation.Path).Name);

                }
                var startPosition = new Vector2(xmlBlock.X, xmlBlock.Y);
                var block = new Block(startPosition, sprite, xmlBlock.BlockType, xmlBlock.Collision, xmlBlock.Damage);
                if (attachedSprite != null)
                    block.AttachedSprites.Add(attachedSprite);

                Blocks.Add(block);
            }

            // start animations of all animated blocks in the level
            foreach (var anSprite in animatedSpriteList)
            {
                anSprite.Key.StartAnimation(anSprite.Value);
            }

            foreach (var xmlLevelCheckpoint in xmlLevel.Checkpoints)
            {
                Checkpoint checkPoint = new Checkpoint(new Vector2(xmlLevelCheckpoint.X, xmlLevelCheckpoint.Y));
                Checkpoints.Add(checkPoint);
            }
        }


        /// <summary>
        /// Builds up a quadtree for the environment
        /// </summary>
        private void InitialiseQuadTree()
        {
            var quadList = new List<IQuadTreeElement>();
            float MinX = 0;
            float MinY = 0;
            float MaxX = 0;
            float MaxY = 0;

            foreach (Block levelBlock in Blocks)
            {
                if (levelBlock.HitBox.Position.X < MinX)
                    MinX = levelBlock.HitBox.Position.X;

                if (levelBlock.HitBox.MaximumX > MaxX)
                    MaxX = levelBlock.HitBox.MaximumX;

                if (levelBlock.HitBox.Position.Y < MinY)
                    MinY = levelBlock.HitBox.Position.Y;

                if (levelBlock.HitBox.MaximumY > MaxY)
                    MaxY = levelBlock.HitBox.MaximumY;

                quadList.Add(levelBlock);
            }

            var levelSize = new Box2D(MinX, MinY, MaxX - MinX, MaxY - MinY);
            BlocksQuadTree = new QuadTreeRoot(levelSize, 4, quadList);
        }
    }
}
