using Lib.Level.Base;
using Lib.Level.Items;
using Lib.LevelLoader.LevelItems;
using Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Collision
{

    public class CollisionReport
        : List<CollisionReportItem>
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsBottomWater { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsSolidOnSide { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSolidOnBottom { get; private set; }


        /// <summary>
        /// Initialises the report with standard values
        /// </summary>
        public CollisionReport()
        {
            IsBottomWater = false;
            IsSolidOnSide = false;
            IsSolidOnBottom = false;
        }
        

        /// <summary>
        /// Adding a new item to the report
        /// </summary>
        /// <param name="item"></param>
        public new void Add(CollisionReportItem item)
        {
            base.Add(item);

            if (item.Item is LevelItemBase)
            {
                if (item.ItemAlignment == Alignment.Bottom)
                {
                    if (((LevelItemBase)item.Item).BlockType == BlockType.Water)
                        IsBottomWater = true;

                    if (item.Item is Player)
                        IsSolidOnBottom = true;
                }


                if (((LevelItemBase)item.Item).BlockType == BlockType.Solid)
                {
                    if (item.ItemAlignment == Alignment.Left || item.ItemAlignment == Alignment.Right)
                        IsSolidOnSide = true;

                    if (item.ItemAlignment == Alignment.Bottom)
                        IsSolidOnBottom = true;
                }
            }
        }
    }
}
