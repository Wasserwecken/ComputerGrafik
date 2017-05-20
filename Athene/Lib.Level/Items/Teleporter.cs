using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Level.Base;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.Level.Items
{
    public class Teleporter : LevelItemBase, IDrawable, IIntersectable
    {
        /// <summary>
        /// Destination Position of the Teleporter
        /// </summary>
        public Vector2 DestinationPosition { get; set; }

        /// <summary>
        /// says if the teleporter is activated
        /// </summary>
        public bool IsActivated { get; set; }

        public Teleporter(Vector2 startPosition, Vector2 destinationPosition, Vector2 boxSize, ISprite sprite)
            : base(startPosition, boxSize)
        {
            DestinationPosition = destinationPosition;
            Sprite = sprite;
        }

        /// <summary>
        /// Draws the checkpoint on the screen
        /// </summary>
        public void Draw()
        {
            if(IsActivated)
                Sprite.Draw(HitBox.Position, new Vector2(0.8f));
        }

        /// <summary>
        /// teleports a item to the destination
        /// </summary>
        /// <param name="item"></param>
        public void Teleport(LevelItemBase item)
        {
            item.HitBox.Position = DestinationPosition;
        }

        /// <summary>
        /// Reacts to intersections with items
        /// </summary>
        /// <param name="intersectingItems"></param>
        public void HandleCollisions(List<IIntersectable> intersectingItems) { }
    }
}
