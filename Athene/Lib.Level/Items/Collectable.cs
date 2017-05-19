using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Level.Base;
using Lib.Tools;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.Level.Items
{
    public class Collectable : LevelItemBase, IDrawable, IIntersectable, IInventoryItem
    {
        /// <summary>
        /// Determines if the collectable can be collected or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// type of the collectable
        /// </summary>
        public CollectableItemType Type { get; set; }


        /// <summary>
        /// Initialises a clollectable
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="startPosition"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intersectingItems"></param>
        public void HandleCollisions(List<IIntersectable> intersectingItems) { }
    }
}
