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
        /// QuadTree for all level Blocks
        /// </summary>
        private QuadTreeRoot BlocksQuadTree { get; set; }


        /// <summary>
        /// Initializes a empty level
        /// </summary>
        public Level(XmlLevel xmlLevel)
		{
			Blocks = new List<Block>();
            Players = new List<Player>();

            LoadLevelFromXmlLevel(xmlLevel);
            InitialiseQuadTree();
            CreateTestPlayer();
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

            // start animations
            foreach (var anSprite in animatedSpriteList)
            {
                anSprite.Key.StartAnimation(anSprite.Value);
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


        /// <summary>
        /// 
        /// </summary>
        private void CreateTestPlayer()
        {
            Player player = null;

            SpriteAnimated playerSprite = new SpriteAnimated();
            playerSprite.AddAnimation("Pics/Worm/walk", 1000);
            playerSprite.AddAnimation("Pics/Worm/idle", 1000);
            playerSprite.StartAnimation("walk");

            var mapList = new InputMapList<PlayerActions>();

            mapList.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveLeft, (inval, curval) => inval.Length > 0.01 && inval.X > 0 ? -inval.X : 0);
            mapList.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveRight, (inval, curval) => inval.Length > 0.01 && inval.X < 0 ? inval.X : 0);
            mapList.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveUp, (inval, curval) => inval.Length > 0.01 && inval.Y > 0 ? inval.Y : 0);
            mapList.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveDown, (inval, curval) => inval.Length > 0.01 && inval.Y < 0 ? -inval.Y : 0);
            mapList.AddMappingGamePad(0, pad => pad.Buttons.A, inp => inp.Jump, (inval, curval) => inval == ButtonState.Pressed);

            mapList.AddMappingKeyboard(Key.Left, inp => inp.MoveLeft, (inval, curval) => inval ? +1 : 0);
            mapList.AddMappingKeyboard(Key.Right, inp => inp.MoveRight, (inval, curval) => inval ? +1 : 0);
            mapList.AddMappingKeyboard(Key.Up, inp => inp.MoveUp, (inval, curval) => inval ? +1 : 0);
            mapList.AddMappingKeyboard(Key.Down, inp => inp.MoveDown, (inval, curval) => inval ? +1 : 0);
            mapList.AddMappingKeyboard(Key.Space, inp => inp.Jump, (inval, curval) => inval);

            player = new Player(Vector2.Zero, mapList, playerSprite);
            Players.Add(player);

        }

        /// <summary>
        /// Draws the level
        /// </summary>
        public void Draw(Box2D cameraFOV)
        {
            //Using the quadtree here because, only blocks in the camera view has to be drawn
            foreach(Block levelBlock in BlocksQuadTree.GetElementsIn(cameraFOV))
                levelBlock.Draw();
            
            //Draw players
            foreach (var player in Players)
                player.Draw();
        }

    }
}
