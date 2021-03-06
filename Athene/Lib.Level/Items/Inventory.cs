﻿using Lib.Level.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.LevelItems;
using OpenTK;

namespace Lib.Level.Items
{
    public class Inventory : IDrawable
    {
        /// <summary>
        /// 
        /// </summary>
        public int ZLevel { get; set; }

        /// <summary>
        /// Position of the inventory
        /// </summary>
        public Vector2 Position { get; set; }


        /// <summary>
        /// the items of the inventory
        /// </summary>
        private List<InventoryItem> Items { get; set; }

        /// <summary>
        /// Margin between the items
        /// </summary>
        private float ItemMargin { get; set; }

        /// <summary>
        /// Size of one icon, which will be shown
        /// </summary>
        private float IconSize { get; set; }

        /// <summary>
        /// Defines how many items will be rendered in one line befor wraping it
        /// </summary>
        private int ItemsPerLine { get; set; }

        /// <summary>
        /// Calculated width of a line of the inventory
        /// </summary>
        private float InventoryLineWidth { get; set; }

        /// <summary>
        /// Distantce between the items
        /// </summary>
        private float SingleItemStep { get; set; }


        /// <summary>
        /// initializes a new inventory
        /// </summary>
        public Inventory(float iconSize, float itemMargin)
        {
            ZLevel = 1;
            ItemMargin = itemMargin;
            IconSize = iconSize;
            ItemsPerLine = 1;
            SingleItemStep = (IconSize + (2 * ItemMargin));

            Items = new List<InventoryItem>();
        }

        /// <summary>
        /// adds a item to the inventory
        /// </summary>
        /// <param name="newItem"></param>
        public void AddItem(InventoryItem newItem)
        {
            //check if the inventory has already enough items of the type
            int counter = 0;
            foreach(var inventoryItem in Items)
            {
                if (inventoryItem.ItemType == newItem.ItemType)
                    counter++;
            }

            int allowedAmout = 0;
            switch(newItem.ItemType)
            {
                case ItemType.CrystalBlue:
                case ItemType.CrystalGreen:
                case ItemType.CrystalRed:
                case ItemType.CrystalYellow:
                case ItemType.Weapon:
                    allowedAmout = 1;
                    break;

                case ItemType.Softice:
                case ItemType.Medikit:
                    allowedAmout = 3;
                    break;

                default:
                    allowedAmout = 0;
                    break;
            }

            if (counter < allowedAmout)
            {
                newItem.Sprite.SetSize(Vector2.One * IconSize);
                Items.Add(newItem);
                ItemsPerLine = (int)Math.Sqrt(Items.Count);
                InventoryLineWidth = SingleItemStep * ItemsPerLine;
            }
        }

        /// <summary>
        /// removes all items which are loosable
        /// </summary>
        public void RemoveLooseableItems()
        {
            Items.RemoveAll(i =>
                (i.ItemType != ItemType.CrystalBlue) &&
                (i.ItemType != ItemType.CrystalGreen) &&
                (i.ItemType != ItemType.CrystalRed) &&
                (i.ItemType != ItemType.CrystalYellow));
        }

        public List<InventoryItem> GetLoosableItems()
        {
            return Items.Where(i =>
               (i.ItemType != ItemType.CrystalBlue) &&
               (i.ItemType != ItemType.CrystalGreen) &&
               (i.ItemType != ItemType.CrystalRed) &&
               (i.ItemType != ItemType.CrystalYellow)).ToList();
        }

        /// <summary>
        /// removes a item from the inventory
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(InventoryItem item)
        {
            Items.Remove(item);
        }

        /// <summary>
        /// returns the first item of the type
        /// </summary>
        /// <param name="itemType">the item type</param>
        /// <returns></returns>
        public InventoryItem GetFirstItemofType(ItemType itemType)
        {
            var returnInventoryItem = Items.FirstOrDefault(i => i.ItemType == itemType);
            return returnInventoryItem;
        }

        /// <summary>
        /// Renders the inventory
        /// </summary>
        public void Draw()
        {
            int lineCounter = -1;
            for (int index = 0; index < Items.Count; index++)
            {
                if (index % ItemsPerLine == 0)
                    lineCounter++;

                var itemPositionX = SingleItemStep * (index % ItemsPerLine) - (InventoryLineWidth / 2);
                var itemPositionY = SingleItemStep * lineCounter;
                var positionAbsolute = Position + new Vector2(itemPositionX, itemPositionY);

                Items[index].Sprite.Draw(positionAbsolute, Vector2.One);
            }
        }
    }
}
