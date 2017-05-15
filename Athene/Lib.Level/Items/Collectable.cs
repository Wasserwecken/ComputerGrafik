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
    public class Collectable : LevelItemBase
    {
        public Collectable(ISprite sprite, Vector2 startPosition)
            : base(startPosition, new Vector2(0.75f, 0.75f))
        {
            Sprite = sprite;
        }

        /// <summary>
        /// Draws the collectable on the screen
        /// </summary>
        public void Draw()
        {

            Sprite.Draw(HitBox.Position, new Vector2(0.8f));
        }
    }
}
