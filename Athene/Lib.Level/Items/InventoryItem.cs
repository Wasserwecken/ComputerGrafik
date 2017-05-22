using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.LevelItems;
using Lib.Visuals.Graphics;

namespace Lib.Level.Items
{
    public class InventoryItem
        : IInventoryItem
    {
        /// <summary>
        /// the sprite of the item
        /// </summary>
        public ISprite Sprite { get; set; }

        /// <summary>
        /// Type of the item in the inventory
        /// </summary>
        public ItemType ItemType { get; set; }


        /// <summary>
        /// Initialises a new inventoy item
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="itemType"></param>
        public InventoryItem(ISprite sprite, ItemType itemType)
        {
            Sprite = sprite;
            ItemType = itemType;
        }
    }
}
