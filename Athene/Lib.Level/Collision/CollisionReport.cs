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
        /// Checks if the item has been corrected
        /// </summary>
        public bool CorrectedVertical { get; set; }

        /// <summary>
        /// Checks if the item has been corrected
        /// </summary>
        public bool CorrectedHorizontal { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public bool IsBottomWater { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSolidOnBottom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPlayerOnBottom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSolidOnSide { get; set; }


        /// <summary>
        /// Initialises the report with standard values
        /// </summary>
        public CollisionReport()
        {
            IsBottomWater = false;
            IsSolidOnSide = false;
            CorrectedHorizontal = false;
            CorrectedVertical = false;
        }
    }
}
