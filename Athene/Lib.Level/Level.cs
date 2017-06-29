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
        /// 
        /// </summary>
        private List<LevelItemBase> LevelItems { get; set; }

        /// <summary>
        /// Blocks which can change their status / appereance / position
        /// </summary>
        private List<LevelItemBase> DynamicObjects { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        private ParalaxBackground Background { get; set; }

        /// <summary>
        /// QuadTree for static ojects
        /// </summary>
        private QuadTreeRoot LevelItemsQuadTree { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Box2D CameraFOV { get; set; }


        /// <summary>
        /// Initializes a empty level
        /// </summary>
        public Level(XmlLevel xmlLevel)
		{
            DynamicObjects = new List<LevelItemBase>();

            LevelItems = LoadLevelFromXmlLevel(xmlLevel);
            LevelSize = GetLevelSize(LevelItems);
            LevelItemsQuadTree = InitialiseQuadTree(LevelSize, LevelItems);

            foreach(var item in LevelItems)
            {
                if (!(item is Block))
                    DynamicObjects.Add(item);
            }

            Background = new ParalaxBackground(LevelSize.Position, LevelSize.Size, LevelSize.Size.Y, new Vector2(.1f, -.1f), xmlLevel.Backgrounds);
        }


        /// <summary>
        /// updates the level
        /// </summary>
        public void UpdateLogic(Box2D cameraFOV)
        {
            Vector2 playersPositionSum = Vector2.Zero;
            float minPlayerXPos = 50000;
            float minPlayerYPos = 50000;
            float maxPlayerXPos = -50000;
            float maxPlayerYPos = -50000;
            int playerCount = 0;

            var newCreatedItems = new List<LevelItemBase>();
            var removedItems = new List<LevelItemBase>();
            CameraFOV = cameraFOV;

            
            foreach (var item in DynamicObjects)
            {
                if (item is Player || item.HitBox.IntersectsWith(CameraFOV.AddRange(10f)))
                {
                    var updateQuadTree = (item is IIntersectable) && (item is IMoveable);


                    if (updateQuadTree)
                        LevelItemsQuadTree.RemoveElement((IIntersectable)item);


                    if (item is IMoveable moveItem)
                        moveItem.Move();


                    if (item is IIntersectable intersecItem)
                    {
                        List<IIntersectable> intersectingItems = LevelItemsQuadTree.GetElementsIn(intersecItem.HitBox);
                        intersectingItems.Remove(intersecItem);
                        intersecItem.HandleCollisions(intersectingItems);
                    }


                    if (item is IInteractable interactItem)
                    {
                        List<IIntersectable> intersectingItems = LevelItemsQuadTree.GetElementsIn(interactItem.InteractionBox);
                        intersectingItems.Remove((IIntersectable)interactItem);
                        interactItem.HandleInteractions(intersectingItems);
                    }


                    if (updateQuadTree)
                        LevelItemsQuadTree.InsertElement((IIntersectable)item);


                    if (item is ICreateable creatingItem)
                    {
                        newCreatedItems.AddRange(creatingItem.GetCreatedItems());
                        creatingItem.ClearCreatedItems();
                    }


                    if (item is IRemoveable removeableItem && removeableItem.Remove)
                        removedItems.Add((LevelItemBase)removeableItem);
                }


                if (item is Player player)
                {
                    playerCount++;
                    playersPositionSum += player.HitBox.Position;
                    minPlayerXPos = Math.Min(minPlayerXPos, player.ViewPoint.X);
                    minPlayerYPos = Math.Min(minPlayerYPos, player.ViewPoint.Y);
                    maxPlayerXPos = Math.Max(maxPlayerXPos, player.ViewPoint.X);
                    maxPlayerYPos = Math.Max(maxPlayerYPos, player.ViewPoint.Y);
                }
            }

            MaxPlayersDistance = new Vector2(maxPlayerXPos - minPlayerXPos, maxPlayerYPos - minPlayerYPos);
            PlayersCenter = playersPositionSum / playerCount;


            foreach (var item in newCreatedItems)
            {
                DynamicObjects.Add(item);
                if (item is IIntersectable)
                    LevelItemsQuadTree.InsertElement((IIntersectable)item);
            }

            foreach (var item in removedItems)
            {
                DynamicObjects.Remove(item);
                if (item is IIntersectable)
                    LevelItemsQuadTree.RemoveElement((IIntersectable)item);
            }
        }
        
        /// <summary>
        /// Draws the level
        /// </summary>
        public void Draw()
        {
            if (CameraFOV != null)
            {
                Background.Draw(PlayersCenter);

                //Using the quadtree here because, only blocks in the camera view has to be drawn
                List<IIntersectable> drawCandiates = LevelItemsQuadTree.GetElementsIn(CameraFOV);
                List<IDrawable> drawableItems = new List<IDrawable>();
                foreach (var item in drawCandiates)
                {
                    if (item is IDrawable)
                        drawableItems.Add((IDrawable)item);
                }
                DrawByZLevel(drawableItems, 0);
            }
        }


        /// <summary>
        /// Draws recursivly items based on their Z level
        /// </summary>
        /// <param name="items"></param>
        private void DrawByZLevel(List<IDrawable> items, int zLevel)
        {
            List<IDrawable> drawableItems = new List<IDrawable>();
            foreach (var item in items)
            {
                if (item.ZLevel > zLevel)
                    drawableItems.Add(item);
                else
                    item.Draw();
            }

            if (drawableItems.Count > 0)
                DrawByZLevel(drawableItems, zLevel + 1);
        }

        /// <summary>
        /// Loads a level from the infos of a XmlLevel
        /// </summary>
        /// <param name="xmlLevel">XmlLevel</param>
        /// <returns>Returns the level</returns>
        private List<LevelItemBase> LoadLevelFromXmlLevel(XmlLevel xmlLevel)
        {
            var Elements = new List<LevelItemBase>();

            if (xmlLevel == null)
                throw new Exception("XmLLevel is null");


            var spawnPosition = new Vector2(xmlLevel.SpawnX, xmlLevel.SpawnY);

            var player1 = PlayerFactory.CreatePlayer(0, spawnPosition);
            var player2 = PlayerFactory.CreatePlayer(1, spawnPosition + new Vector2(1, 1));

            
            

            player1.CommunicationPlayer = player2;
            player2.CommunicationPlayer = player1;

            Elements.Add(player1);
            Elements.Add(player2);


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

                Elements.Add(block);
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

                Elements.Add(checkPoint);
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

                Elements.Add(collectable);
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

                Elements.Add(enemy);
            }

            return Elements;
        }


        /// <summary>
        /// Builds up a quadtree for the environment
        /// </summary>
        private QuadTreeRoot InitialiseQuadTree(Box2D levelSize, List<LevelItemBase> Elements)
        {
            var quadList = new List<IIntersectable>();

            Box2D quadSize;
            //calculate a dedicated size which will be quadratic for the quadtree
            //this will be more cleaner for splitting the nodes
            if (levelSize.Size.X > levelSize.Size.Y)
                quadSize = new Box2D(levelSize.Position.X, levelSize.Center.Y - (levelSize.Size.X / 2), LevelSize.Size.X, LevelSize.Size.X);
            else
                quadSize = new Box2D(levelSize.Center.X - (levelSize.Size.Y / 2), levelSize.Position.Y, LevelSize.Size.Y, LevelSize.Size.Y);


            foreach (var levelBlock in Elements)
            {
                if (levelBlock is IIntersectable)
                    quadList.Add((IIntersectable)levelBlock);
            }
            
            return new QuadTreeRoot(quadSize, 15, quadList);
        }

        /// <summary>
        /// Gets the boundaries of the level, based on the levelitems it contians
        /// </summary>
        /// <param name="Elements"></param>
        /// <returns></returns>
        private Box2D GetLevelSize(List<LevelItemBase> Elements)
        {
            float MinX = 0;
            float MinY = 0;
            float MaxX = 0;
            float MaxY = 0;

            foreach (var levelBlock in Elements)
            {
                if (levelBlock.HitBox.Position.X < MinX)
                    MinX = levelBlock.HitBox.Position.X;

                if (levelBlock.HitBox.MaximumX > MaxX)
                    MaxX = levelBlock.HitBox.MaximumX;

                if (levelBlock.HitBox.Position.Y < MinY)
                    MinY = levelBlock.HitBox.Position.Y;

                if (levelBlock.HitBox.MaximumY > MaxY)
                    MaxY = levelBlock.HitBox.MaximumY;
            }

            return new Box2D(MinX, MinY, MaxX - MinX, MaxY - MinY);
        }
    }
}
