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
        /// Analyses the report and sets the property values
        /// </summary>
        public void Analyse()
        {
            foreach(var item in this)
            {
                if (item.Item.BlockType == BlockType.Water && item.ItemAlignment == Alignment.Bottom)
                    IsBottomWater = true;

                if (item.Item.BlockType == BlockType.Solid)
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
