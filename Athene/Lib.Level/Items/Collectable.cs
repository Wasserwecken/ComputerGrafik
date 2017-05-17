using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Level.Base;
using Lib.LevelLoader.LevelItems;
using Lib.LevelLoader.Xml.LinkTypes;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.Level.Items
{
    public class Collectable : LevelItemBase, IInventoryItem
    {
        /// <summary>
        /// is active state of the collectable
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// type of the collectable
        /// </summary>
        public CollectableItemType Type { get; set; }

        public Collectable(ISprite sprite, Vector2 startPosition, CollectableItemType type)
            : base(startPosition, new Vector2(0.75f, 0.75f))
        {
            Sprite = sprite;
            IsActive = true;
            Type = type;
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
