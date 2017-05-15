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
        /// list of all collectables
        /// </summary>
        public List<Collectable> Collectables { get; set; }

        /// <summary>
        /// Poiint which should be the focus for the camera
        /// </summary>
        public Vector2 PlayersCenter { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 MaxPlayersDistance { get; set; }

        /// <summary>
        /// QuadTree for all level Blocks
        /// </summary>
        private QuadTreeRoot BlocksQuadTree { get; set; }

        /// <summary>
        /// Startposition of players
        /// </summary>
        private Vector2 SpawnPosition { get; set; }
        

        /// <summary>
        /// Initializes a empty level
        /// </summary>
        public Level(XmlLevel xmlLevel)
		{
            Players = new List<Player>();
            Blocks = new List<Block>();
            Checkpoints = new List<Checkpoint>();
            Collectables = new List<Collectable>();

            LoadLevelFromXmlLevel(xmlLevel);
            
            Players.Add(PlayerFactory.CreatePlayer(0, SpawnPosition));
            Players.Add(PlayerFactory.CreatePlayer(1, SpawnPosition));
           
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

                var intersections = new List<LevelItemBase>();
                intersections.AddRange(BlocksQuadTree.GetElementsIn(player.HitBox).ConvertAll(item => (LevelItemBase)item));

                foreach (var otherPlayer in Players)
                {
                    if (!player.Equals(otherPlayer) && player.HitBox.IntersectsWith(otherPlayer.HitBox))
                        intersections.Add(otherPlayer);
                }

                foreach (var checkpoint in Checkpoints)
                {
                    if (player.HitBox.IntersectsWith(checkpoint.HitBox))
                        Console.WriteLine("Player " + Players.IndexOf(player) + " is on Checkpoint");
                }

                foreach (var collectable in Collectables)
                {
                    if (player.HitBox.IntersectsWith(collectable.HitBox))
                        Console.WriteLine("Player " + Players.IndexOf(player) + " can pick up a item");
                }



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


            foreach (var collectable in Collectables)
            {
                collectable.Draw();
            }
        }


        /// <summary>
        /// Calculates the information which is needed for the camera
        /// Like zoom level and position
        /// </summary>
        private void CalculateCameraInformations()
        {
            Vector2 playersPositionSum = Players[0].HitBox.Position;
            float minPlayerXPos = Players[0].ViewPoint.X;
            float minPlayerYPos = Players[0].ViewPoint.Y;
            float maxPlayerXPos = Players[0].ViewPoint.X;
            float maxPlayerYPos = Players[0].ViewPoint.Y;

            for (int index = 1; index < Players.Count; index ++)
            {
                playersPositionSum = playersPositionSum + Players[index].HitBox.Position;
                minPlayerXPos = Math.Min(minPlayerXPos, Players[index].ViewPoint.X);
                minPlayerYPos = Math.Min(minPlayerYPos, Players[index].ViewPoint.Y);
                maxPlayerXPos = Math.Max(maxPlayerXPos, Players[index].ViewPoint.X);
                maxPlayerYPos = Math.Max(maxPlayerYPos, Players[index].ViewPoint.Y);
            }


            MaxPlayersDistance = new Vector2(maxPlayerXPos - minPlayerXPos, maxPlayerYPos - minPlayerYPos);
            PlayersCenter = playersPositionSum / Players.Count;
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

            SpawnPosition = new Vector2(xmlLevel.SpawnX, xmlLevel.SpawnY);

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

            foreach (var xmlLevelCollectable in xmlLevel.Collectables)
            {
                var xmlCollectable =
                    CollectableLoader.GetCollectables().Items.FirstOrDefault(c => c.Id == xmlLevelCollectable.Link);

                if(xmlCollectable == null)
                    throw  new Exception("Collectable not found");

                SpriteStatic sprite = new SpriteStatic(xmlCollectable.Path);

                Collectable collectable = new Collectable(sprite, new Vector2(xmlLevelCollectable.X, xmlLevelCollectable.Y));
                Collectables.Add(collectable);
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
