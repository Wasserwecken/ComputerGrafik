using Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.LevelLoader.LevelItems
{
    public class CollisionReportItem
    {
        /// <summary>
        /// Alignment of the object to the checked object
        /// </summary>
        public Alignment ItemAlignment { get; set; }

        /// <summary>
        /// Item that is colliding with the checked object
        /// </summary>
        public LevelItemBase Item { get; set; }

        public CollisionReportItem(Alignment itemAlignment, LevelItemBase item)
        {
            ItemAlignment = itemAlignment;
            Item = item;
        }
    }
}
