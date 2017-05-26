using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Input.Mapping;
using Lib.Level.Base;
using Lib.LevelLoader.LevelItems;
using Lib.Tools;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.Level.Items
{
    public class Checkpoint : LevelItemBase, IDrawable, IIntersectable
    {
        /// <summary>
        /// collission of the block
        /// </summary>
        public bool HasCollisionCorrection { get; set; }

        /// <summary>
        /// Destination of the checkpoint
        /// </summary>
        public Vector2 DestinationPosition { get; set; }

        /// <summary>
        /// ActivationItemType of the block
        /// (string, for example: crystal_red)
        /// </summary>
        public ItemType ActivationItemType { get; set; }

        /// <summary>
        /// returns if the Checkpoint is activated
        /// </summary>
        public bool IsActivated { get; private set; }

        /// <summary>
        /// the sprite when the checkpoint is activated
        /// </summary>
        public ISprite SpriteActivated { get; set; }

        /// <summary>
        /// the teleporter to the destination
        /// </summary>
        public Teleporter Teleporter { get; set; }

        /// <summary>
        /// the back teleporter
        /// </summary>
        public Teleporter BackTeleporter { get; set; }

        /// <summary>
        /// Initialises a checkpoint
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="destinationPosition"></param>
        /// <param name="damageSprite"></param>
        /// <param name="activationItemType"></param>
        /// <param name="activatedSprite"></param>
        /// <param name="teleporter"></param>
        /// <param name="backTeleporter"></param>
        public Checkpoint(Vector2 startPosition, Vector2 destinationPosition, ISprite damageSprite, ISprite activatedSprite, ItemType activationItemType, Teleporter teleporter, Teleporter backTeleporter)
            : base(startPosition, new Vector2(0.75f, 0.75f))
        {
            DestinationPosition = destinationPosition;
            Sprite = damageSprite;
            ActivationItemType = activationItemType;
            SpriteActivated = activatedSprite;
            Teleporter = teleporter;
            BackTeleporter = backTeleporter;
        }

        /// <summary>
        /// activates the checkpoint
        /// </summary>
        public void Activate()
        {
            IsActivated = true;
            Teleporter.IsActivated = true;
            BackTeleporter.IsActivated = true;
        }

        /// <summary>
        /// Draws the checkpoint on the screen
        /// </summary>
        public void Draw()
        {
            foreach (var attachedSprite in AttachedSprites)
                attachedSprite.Draw(HitBox.Position, new Vector2(1f));

            if (!IsActivated)
                Sprite.Draw(HitBox.Position, new Vector2(0.8f));
            else
            {
                SpriteActivated.Draw(HitBox.Position, new Vector2(0.8f));
            }

         

        }

        /// <summary>
        /// Reacts to intersections with items
        /// </summary>
        /// <param name="intersectingItems"></param>
        public void HandleCollisions(List<IIntersectable> intersectingItems) { }
    }
}
