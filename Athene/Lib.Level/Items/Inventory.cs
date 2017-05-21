using Lib.Level.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.LevelItems;

namespace Lib.Level.Items
{
    public class Inventory : IDrawable
    {
        /// <summary>
        /// the items of the inventory
        /// </summary>
        private List<IInventoryItem> Items { get; set; }

        /// <summary>
        /// initializes a new inventory
        /// </summary>
        public Inventory()
        {
            Items = new List<IInventoryItem>();
        }

        /// <summary>
        /// adds a item to the inventory
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(IInventoryItem item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// removes a item from the inventory
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(IInventoryItem item)
        {
            Items.Remove(item);
        }

        /// <summary>
        /// returns the first item of the type
        /// </summary>
        /// <param name="itemType">the item type</param>
        /// <returns></returns>
        public IInventoryItem GetFirstItemofType(ItemType itemType)
        {
            var returnInventoryItem = Items.FirstOrDefault(i => i.ItemType == itemType);
            return returnInventoryItem;
        }

        /// <summary>
        /// Renders the inventory
        /// </summary>
        public void Draw()
        {

        }


    }
}
