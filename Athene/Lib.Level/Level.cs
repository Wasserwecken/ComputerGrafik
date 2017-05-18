using System;
using System.Collections.Generic;
using System.Linq;
using Lib.LevelLoader.LevelItems;
using Lib.Visuals.Graphics;
using OpenTK;
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
        /// Poiint which should be the focus for the camera
        /// </summary>
        public Vector2 PlayersCenter { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 MaxPlayersDistance { get; set; }


        /// <summary>
        /// Blocks which can change their status / appereance / position
        /// </summary>
        private List<LevelItemBase> DynamicObjects { get; set; }

        /// <summary>
        /// Static objects which are loaded
        /// </summary>
        private List<LevelItemBase> StaticObjects { get; set; }

        /// <summary>
        /// Players which are participating in the level
        /// </summary>
        private List<Player> ActivePlayers { get; set; }

        /// <summary>
        /// QuadTree for static ojects
        /// </summary>
        private QuadTreeRoot StaticObjectsQuadTree { get; set; }
        
        /// <summary>
        /// Startposition of players
        /// </summary>
        private Vector2 SpawnPosition { get; set; }
        

        /// <summary>
        /// Initializes a empty level
        /// </summary>
        public Level(XmlLevel xmlLevel)
		{
            DynamicObjects = new List<LevelItemBase>();
            StaticObjects = new List<LevelItemBase>();
            ActivePlayers = new List<Player>();


            LoadLevelFromXmlLevel(xmlLevel);

            var player1 = PlayerFactory.CreatePlayer(0, SpawnPosition);
            var player2 = PlayerFactory.CreatePlayer(1, SpawnPosition + new Vector2(1,1));

            DynamicObjects.Add(player1);
            DynamicObjects.Add(player2);
            ActivePlayers.Add(player1);
            ActivePlayers.Add(player2);

            InitialiseQuadTree();
		}


        /// <summary>
        /// updates the level
        /// </summary>
        public void UpdateLogic()
        {
            foreach(var item in StaticObjects)
            {
                UpdateLevelItem(item);
            }

            foreach(var item in DynamicObjects)
            {
                UpdateLevelItem(item);
            }

            CalculateCameraInformations();
        }
        
        /// <summary>
        /// Draws the level
        /// </summary>
        public void Draw(Box2D cameraFOV)
        {
            //Using the quadtree here because, only blocks in the camera view has to be drawn
            foreach (LevelItemBase item in StaticObjectsQuadTree.GetElementsIn(cameraFOV))
            {
                if (item is IDrawable)
                    ((IDrawable)item).Draw();
            }

            foreach (LevelItemBase item in DynamicObjects)
            {
                if (item is IDrawable)
                    ((IDrawable)item).Draw();
            }
        }


        /// <summary>
        /// Updates a level item based on his type
        /// </summary>
        /// <param name="item"></param>
        private void UpdateLevelItem(LevelItemBase item)
        {
            List<IIntersectable> intersectingItems = null;

            if (item is IMoveable)
                ((IMoveable)item).Move();

            if (item is IIntersectable)
            {
                if (intersectingItems == null)
                    intersectingItems = GetIntersectingItems(((IIntersectable)item));

                ((IIntersectable)item).HandleCollisions(intersectingItems);
            }

            if (item is IInteractable)
            {
                List<IInteractable> interactableItems = new List<IInteractable>();

                if (intersectingItems == null)
                    intersectingItems = GetIntersectingItems(((IIntersectable)item));

                foreach(var intersecItem in intersectingItems)
                {
                    if (intersecItem is IInteractable)
                        interactableItems.Add((IInteractable)intersecItem);
                }
                
                ((IInteractable)item).HandleInteractions(interactableItems);
            }
        }


        /// <summary>
        /// Gets a list with all interesecting items in the level
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private List<IIntersectable> GetIntersectingItems(IIntersectable item)
        {
            List<IIntersectable> intersections = new List<IIntersectable>();

            //Static things
            intersections.AddRange(StaticObjectsQuadTree.GetElementsIn(item.HitBox).ConvertAll(f => (IIntersectable)f ));

            //dynamic things
            foreach(var dynamicItem in DynamicObjects)
            {
                if (dynamicItem is IIntersectable && !dynamicItem.Equals(item) && dynamicItem.HitBox.IntersectsWith(item.HitBox))
                    intersections.Add(((IIntersectable)dynamicItem));
            }

            return intersections;
        }


        /// <summary>
        /// Calculates the information which is needed for the camera
        /// Like zoom level and position
        /// </summary>
        private void CalculateCameraInformations()
        {
            Vector2 playersPositionSum = ActivePlayers[0].HitBox.Position;
            float minPlayerXPos = ActivePlayers[0].ViewPoint.X;
            float minPlayerYPos = ActivePlayers[0].ViewPoint.Y;
            float maxPlayerXPos = ActivePlayers[0].ViewPoint.X;
            float maxPlayerYPos = ActivePlayers[0].ViewPoint.Y;

            for (int index = 1; index < ActivePlayers.Count; index ++)
            {
                playersPositionSum = playersPositionSum + ActivePlayers[index].HitBox.Position;
                minPlayerXPos = Math.Min(minPlayerXPos, ActivePlayers[index].ViewPoint.X);
                minPlayerYPos = Math.Min(minPlayerYPos, ActivePlayers[index].ViewPoint.Y);
                maxPlayerXPos = Math.Max(maxPlayerXPos, ActivePlayers[index].ViewPoint.X);
                maxPlayerYPos = Math.Max(maxPlayerYPos, ActivePlayers[index].ViewPoint.Y);
            }


            MaxPlayersDistance = new Vector2(maxPlayerXPos - minPlayerXPos, maxPlayerYPos - minPlayerYPos);
            PlayersCenter = playersPositionSum / ActivePlayers.Count;
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

                StaticObjects.Add(block);
            }

            // start animations of all animated blocks in the level
            foreach (var anSprite in animatedSpriteList)
            {
                anSprite.Key.StartAnimation(anSprite.Value);
            }

            foreach (var xmlLevelCheckpoint in xmlLevel.Checkpoints)
            {
                var xmlCheckpointAnimation =
                    CheckpointLoader.GetCheckpoints()
                        .CheckpointAnimations.FirstOrDefault(c => c.Id == xmlLevelCheckpoint.Link);

                if (xmlCheckpointAnimation == null)
                    throw new Exception("Checkpoint Animation in xml Datei nicht gefunden");

                SpriteAnimated sprite = new SpriteAnimated();
                sprite.AddAnimation(xmlCheckpointAnimation.Path, xmlCheckpointAnimation.AnimationLength);
                sprite.StartAnimation(xmlCheckpointAnimation.Id);

                Checkpoint checkPoint = new Checkpoint(new Vector2(xmlLevelCheckpoint.X, xmlLevelCheckpoint.Y), 
                    new Vector2(xmlLevelCheckpoint.DestinationX, xmlLevelCheckpoint.DestinationY),
                    sprite);
                StaticObjects.Add(checkPoint);
            }

            foreach (var xmlLevelCollectable in xmlLevel.Collectables)
            {
                var xmlCollectable =
                    CollectableLoader.GetCollectables().Items.FirstOrDefault(c => c.Id == xmlLevelCollectable.Link);

                if(xmlCollectable == null)
                    throw  new Exception("Collectable not found");

                SpriteStatic sprite = new SpriteStatic(xmlCollectable.Path);

                Collectable collectable = new Collectable(sprite, new Vector2(xmlLevelCollectable.X, xmlLevelCollectable.Y));
                StaticObjects.Add(collectable);
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

            foreach (LevelItemBase levelBlock in StaticObjects)
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
            StaticObjectsQuadTree = new QuadTreeRoot(levelSize, 4, quadList);
        }
    }
}
