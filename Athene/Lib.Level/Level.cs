using System;
using System.Collections.Generic;
using System.Linq;
using Lib.LevelLoader.LevelItems;
using Lib.Visuals.Graphics;
using OpenTK;
using Lib.Tools;
using Lib.Level.Items;
using Lib.Level.Base;
using Lib.LevelLoader.Xml;
using System.IO;
using Lib.LevelLoader;
using Lib.Level.QuadTree;

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
        /// Boundaries of the level
        /// </summary>
        public Box2D LevelSize { get; set; }


        /// <summary>
        /// Blocks which can change their status / appereance / position
        /// </summary>
        private List<LevelItemBase> DynamicObjects { get; set; }

        /// <summary>
        /// Static objects which are loaded
        /// </summary>
        private List<LevelItemBase> EnvironmentObjects { get; set; }

        /// <summary>
        /// Players which are participating in the level
        /// </summary>
        private List<Player> ActivePlayers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private ParalaxBackground Background { get; set; }

        /// <summary>
        /// QuadTree for static ojects
        /// </summary>
        private QuadTreeRoot EnvironmentQuadTree { get; set; }
        
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
            EnvironmentObjects = new List<LevelItemBase>();
            ActivePlayers = new List<Player>();


            LoadLevelFromXmlLevel(xmlLevel);

            var player1 = PlayerFactory.CreatePlayer(0, SpawnPosition);
            var player2 = PlayerFactory.CreatePlayer(1, SpawnPosition + new Vector2(1,1));

            DynamicObjects.Add(player1);
            DynamicObjects.Add(player2);
            ActivePlayers.Add(player1);
            ActivePlayers.Add(player2);
		}


        /// <summary>
        /// updates the level
        /// </summary>
        public void UpdateLogic()
        {
            var newCreatedItems = new List<LevelItemBase>();
            var removedItems = new List<LevelItemBase>();

            foreach (var item in DynamicObjects)
            {
                if (item is IMoveable moveItem)
                    moveItem.Move();


                if (item is IIntersectable intersecItem)
                {
                    List<IIntersectable> intersectingItems = GetIntersectingItems(intersecItem.HitBox);
                    intersectingItems.Remove(intersecItem);
                    intersecItem.HandleCollisions(intersectingItems);
                }


                if (item is IInteractable interactItem)
                {
                    List<IIntersectable> intersectingItems = GetIntersectingItems(interactItem.InteractionBox);
                    intersectingItems.Remove((IIntersectable)item);
                    interactItem.HandleInteractions(intersectingItems);
                }


                if (item is ICreateable creatingItem)
                {
                    newCreatedItems.AddRange(creatingItem.GetCreatedItems());
                    creatingItem.ClearCreatedItems();
                }


                if (item is IRemoveable removeableItem && removeableItem.Remove)
                    removedItems.Add((LevelItemBase)removeableItem);
            }


            DynamicObjects.AddRange(newCreatedItems);
            foreach (var item in removedItems)
                DynamicObjects.Remove(item);


            CalculateCameraInformations();
        }
        
        /// <summary>
        /// Draws the level
        /// </summary>
        public void Draw(Box2D cameraFOV)
        {
            Background.Draw(PlayersCenter);

            //Using the quadtree here because, only blocks in the camera view has to be drawn
            foreach (LevelItemBase item in EnvironmentQuadTree.GetElementsIn(cameraFOV))
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
        /// Gets a list with all interesecting items in the level
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private List<IIntersectable> GetIntersectingItems(Box2D range)
        {
            var intersections = new List<IIntersectable>();

            //Static things
            intersections.AddRange(EnvironmentQuadTree.GetElementsIn(range));

            //dynamic things
            foreach(var dynamicItem in DynamicObjects)
            {
                if (dynamicItem is IIntersectable && dynamicItem.HitBox.IntersectsWith(range))
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
                    sprite = new SpriteStatic(Vector2.One, xmlTexture.Path);
                    if (xmlBlock.IsScrolling)
                    {
                        ((SpriteStatic)sprite).StartTextureScroll(new Vector2(xmlBlock.ScrollingDirectionX, xmlBlock.ScrollingDirectionY), xmlBlock.ScrollingLength);
                    }
                    if (xmlBlock.AttachedLink != null)
                    {
                        attachedSprite = Helper.AttachedSpriteHelper.GetAttachedSprite(xmlBlock, xmlLevel);
                    }
                }
                if (xmlBlock.LinkType == BlockLinkType.Animation)
                {
                    sprite = new SpriteAnimated(Vector2.One);
                    var xmlAnimation = AnimationLoader.GetBlockAnimations().Animations.First(a => a.Id == xmlBlock.Link);
                    if (xmlAnimation == null)
                        throw new Exception("Animation not found");
                    ((SpriteAnimated)sprite).AddAnimation(xmlAnimation.Path, xmlAnimation.AnimationLength);
                    animatedSpriteList.Add((SpriteAnimated)sprite, new DirectoryInfo(xmlAnimation.Path).Name);

                }
                var startPosition = new Vector2(xmlBlock.X, xmlBlock.Y);
                var block = new Block(startPosition, sprite, xmlBlock.EnvironmentType, xmlBlock.Collision, xmlBlock.Damage);
                if (attachedSprite != null)
                    block.AttachedSprites.Add(attachedSprite);

                EnvironmentObjects.Add(block);
            }

            // start animations of all animated blocks in the level
            foreach (var anSprite in animatedSpriteList)
            {
                anSprite.Key.StartAnimation(anSprite.Value);
            }

            foreach (var xmlLevelCheckpoint in xmlLevel.Checkpoints)
            {
                var xmlCheckpointItem =
                    CheckpointLoader.GetCheckpoints()
                        .Checkpoints.FirstOrDefault(c => c.Id == xmlLevelCheckpoint.Link);

                if (xmlCheckpointItem == null)
                    throw new Exception("Checkpoint in xml Datei nicht gefunden");
                
                Checkpoint checkPoint = new Checkpoint(new Vector2(xmlLevelCheckpoint.X, xmlLevelCheckpoint.Y),
                    new Vector2(xmlLevelCheckpoint.DestinationX, xmlLevelCheckpoint.DestinationY),
                    (ItemType)Enum.Parse(typeof(ItemType), xmlCheckpointItem.CollectableItemType), xmlCheckpointItem.Path);
                
                DynamicObjects.Add(checkPoint);
            }

            foreach (var xmlLevelCollectable in xmlLevel.Collectables)
            {
                var xmlCollectable =
                    CollectableLoader.GetCollectables().Items.FirstOrDefault(c => c.Id == xmlLevelCollectable.Link);

                if(xmlCollectable == null)
                    throw  new Exception("Collectable not found");

                SpriteStatic sprite = new SpriteStatic(Vector2.One * 2, xmlCollectable.Path);

                Collectable collectable = new Collectable(sprite, new Vector2(xmlLevelCollectable.X, xmlLevelCollectable.Y), xmlCollectable.ItemType);


                /* create the attached Sprites */
                if (xmlLevelCollectable.AttachedLink != null)
                {
                    ISprite attachedSprite = Helper.AttachedSpriteHelper.GetAttachedSprite(xmlLevelCollectable, xmlLevel);
                    collectable.AttachedSprites.Add(attachedSprite);
                }


                DynamicObjects.Add(collectable);
            }

            foreach (var xmlLevelEnemy in xmlLevel.Enemies)
            {
                var xmlEnemyItem = EnemyLoader.GetEnemies().Enemies.FirstOrDefault(e => e.Id == xmlLevelEnemy.Link);

                if (xmlEnemyItem == null)
                    throw new Exception("Enemy not found");

                SpriteAnimated enemySprite = new SpriteAnimated(Vector2.One);
                enemySprite.AddAnimation(xmlEnemyItem.DefaultAnimation, xmlEnemyItem.DefaultAnimationLength);
                enemySprite.StartAnimation(new DirectoryInfo(xmlEnemyItem.DefaultAnimation).Name);
                
                Enemy enemy = new Enemy(new Vector2(xmlLevelEnemy.X, xmlLevelEnemy.Y), enemySprite, xmlEnemyItem.EnemyType, xmlEnemyItem.MovementType);

                DynamicObjects.Add(enemy);


            }

            InitialiseQuadTree();
            Background = new ParalaxBackground(LevelSize.Position, LevelSize.Size, LevelSize.Size.Y, new Vector2(.1f, -.1f), xmlLevel.Backgrounds);
        }


        /// <summary>
        /// Builds up a quadtree for the environment
        /// </summary>
        private void InitialiseQuadTree()
        {
            var quadList = new List<IIntersectable>();
            float MinX = 0;
            float MinY = 0;
            float MaxX = 0;
            float MaxY = 0;

            foreach (LevelItemBase levelBlock in EnvironmentObjects)
            {
                if (levelBlock is IIntersectable)
                {
                    if (levelBlock.HitBox.Position.X < MinX)
                        MinX = levelBlock.HitBox.Position.X;

                    if (levelBlock.HitBox.MaximumX > MaxX)
                        MaxX = levelBlock.HitBox.MaximumX;

                    if (levelBlock.HitBox.Position.Y < MinY)
                        MinY = levelBlock.HitBox.Position.Y;

                    if (levelBlock.HitBox.MaximumY > MaxY)
                        MaxY = levelBlock.HitBox.MaximumY;

                    quadList.Add((IIntersectable)levelBlock);
                }
            }

            LevelSize = new Box2D(MinX, MinY, MaxX - MinX, MaxY - MinY);
            EnvironmentQuadTree = new QuadTreeRoot(LevelSize, 4, quadList);
        }
    }
}
