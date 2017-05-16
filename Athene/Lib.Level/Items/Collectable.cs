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
    public class Collectable : LevelItemBase, IInventoryItem
    {
        public bool IsActive { get; set; }

        public Collectable(ISprite sprite, Vector2 startPosition)
            : base(startPosition, new Vector2(0.75f, 0.75f))
        {
            Sprite = sprite;
            IsActive = true;
        }

        /// <summary>
        /// Draws the collectable on the screen
        /// </summary>
        public void Draw()
        {
            if(IsActive)
                Sprite.Draw(HitBox.Position, new Vector2(0.8f));
        }

        
        
    }
}
