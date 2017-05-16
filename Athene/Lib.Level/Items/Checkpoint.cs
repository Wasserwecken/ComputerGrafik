using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Input.Mapping;
using Lib.Level.Base;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.Level.Items
{
    public class Checkpoint : LevelItemBase
    {
        public Checkpoint(Vector2 startPosition, Vector2 destinationPosition, ISprite sprite)
            : base(startPosition, new Vector2(0.75f, 0.75f))
        {
            DestinationPosition = destinationPosition;
            Sprite = sprite;
        }

        /// <summary>
        /// Destination of the checkpoint
        /// </summary>
        public Vector2 DestinationPosition { get; set; }

        /// <summary>
        /// Draws the checkpoint on the screen
        /// </summary>
        public void Draw()
        {
            Sprite.Draw(HitBox.Position, new Vector2(0.8f));
        }
    }
}
