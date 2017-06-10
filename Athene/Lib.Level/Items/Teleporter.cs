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
        /// 
        /// </summary>
        public int ZLevel { get; set; }

        /// <summary>
        /// collission of the block
        /// </summary>
        public bool HasCollisionCorrection { get; set; }

        /// <summary>
        /// Destination Position of the Teleporter
        /// </summary>
        public Vector2 DestinationPosition { get; set; }


        /// <summary>
        /// Initialises a new teleporter
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="destinationPosition"></param>
        /// <param name="boxSize"></param>
        /// <param name="sprite"></param>
        public Teleporter(Vector2 startPosition, Vector2 destinationPosition, Vector2 boxSize, ISprite sprite)
            : base(startPosition, boxSize)
        {
            DestinationPosition = destinationPosition;
            Sprite = sprite;
            ZLevel = 1;
        }

        /// <summary>
        /// Draws the checkpoint on the screen
        /// </summary>
        public void Draw()
        {
            Sprite.Draw(HitBox.Position, new Vector2(0.8f));
        }


        /// <summary>
        /// Reacts to intersections with items
        /// </summary>
        /// <param name="intersectingItems"></param>
        public void HandleCollisions(List<IIntersectable> intersectingItems) { }
    }
}
