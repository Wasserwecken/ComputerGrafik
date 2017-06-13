using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Input.Mapping;
using Lib.Level.Base;
using Lib.LevelLoader.LevelItems;
using Lib.Tools;
using System.Diagnostics;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.Level.Items
{
    public class Checkpoint : LevelItemBase, IDrawable, IIntersectable, ICreateable, IRemoveable, IMoveable
    {
        /// <summary>
        /// 
        /// </summary>
        public int ZLevel { get; set; }

        /// <summary>
        /// collission of the block
        /// </summary>
        public bool HasCollisionCorrection { get; set; }

        /// <summary>
        /// ActivationItemType of the block
        /// (string, for example: crystal_red)
        /// </summary>
        public ItemType ActivationItemType { get; set; }

        /// <summary>
        /// Defines if the checkpoint has been activated
        /// </summary>
        public bool IsActive { get; private set; }
                
        /// <summary>
        /// the teleporter to the destination
        /// </summary>
        public Teleporter Teleporter { get; set; }

        /// <summary>
        /// the back teleporter
        /// </summary>
        public Teleporter BackTeleporter { get; set; }

        /// <summary>
        /// Defines if the item has to be removed from the level
        /// </summary>
        public bool Remove { get { return ActivationWatch.ElapsedMilliseconds > ActivationTime; } set { } }


        /// <summary>
        /// Watch to control the activation time
        /// </summary>
        private Stopwatch ActivationWatch { get; set; }

        /// <summary>
        /// Destination of the checkpoint
        /// </summary>
        private Vector2 DestinationPosition { get; set; }

        /// <summary>
        /// Time until the checkpoint is active
        /// </summary>
        private int ActivationTime { get; set; }

        /// <summary>
        /// Position of the checkpoint on levelstart
        /// </summary>
        public Vector2 OriginalPosition { get; set; }



        /// <summary>
        /// Initialises a checkpoint
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="destinationPosition"></param>
        /// <param name="activationItemType"></param>
        /// <param name="path">path to the files (disabled & activating folders must be there)</param>
        public Checkpoint(Vector2 startPosition, Vector2 destinationPosition, ItemType activationItemType, string path)
            : base(startPosition, new Vector2(0.75f, 0.75f))
        {
            OriginalPosition = startPosition;
            DestinationPosition = destinationPosition;
            ActivationItemType = activationItemType;
            ActivationWatch = new Stopwatch();
            ActivationTime = 1000;


            SpriteAnimated teleporterSprite = new SpriteAnimated(new Vector2(0.75f));
            teleporterSprite.AddAnimation("Animations/portal/wobble", 1000);


            var checkPointAnimations = new SpriteAnimated(Vector2.One);
            checkPointAnimations.AddAnimation(path + "/activating", ActivationTime);
            checkPointAnimations.AddAnimation(path + "/disabled", 2000);
            checkPointAnimations.StartAnimation("disabled");

            Sprite = checkPointAnimations;
            ZLevel = 1;
        }

        /// <summary>
        /// Draws the checkpoint on the screen
        /// </summary>
        public void Draw()
        {
            Sprite.Draw(HitBox.Position, Vector2.One);
        }

        /// <summary>
        /// Activates the checkpoint
        /// </summary>
        public void Activate()
        {
            if (IsActive == false)
            {
                ((SpriteAnimated)Sprite).StartAnimation("activating");
                ActivationWatch.Start();
            }

            IsActive = true;
        }

        /// <summary>
        /// Reacts to intersections with items
        /// </summary>
        /// <param name="intersectingItems"></param>
        public void HandleCollisions(List<IIntersectable> intersectingItems) { }

        /// <summary>
        /// Returns two teleporters if activated
        /// </summary>
        /// <returns></returns>
        public List<LevelItemBase> GetCreatedItems()
        {
            var teleporters = new List<LevelItemBase>();

            SpriteAnimated teleporterSprite = new SpriteAnimated(new Vector2(0.75f));
            teleporterSprite.AddAnimation("Animations/portal/wobble", 1000);
            teleporterSprite.StartAnimation("wobble");
            Teleporter = new Teleporter(HitBox.Position, DestinationPosition, Vector2.One, teleporterSprite);
            BackTeleporter = new Teleporter(DestinationPosition, HitBox.Position, Vector2.One, teleporterSprite);

            if (IsActive && ActivationWatch.ElapsedMilliseconds > ActivationTime)
            {
               
                teleporters.Add(Teleporter);
                teleporters.Add(BackTeleporter);
            }

            return teleporters;
        }

        /// <summary>
        /// This will never happen, after item creation, this item will die
        /// </summary>
        public void ClearCreatedItems() { }

        /// <summary>
        /// Moves the checkpoint upwards for the activation
        /// </summary>
        public void Move()
        {
            if (IsActive)
            {
                var activationProgress = Easing.Linear(ActivationWatch.ElapsedMilliseconds , ActivationTime);
                HitBox.Position = OriginalPosition + new Vector2(0, 2 * activationProgress);
            }
        }
    }
}
