using Lib.Level.Base;
using Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Collision
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
        public IIntersectable Item { get; set; }


        /// <summary>
        /// Initialises the object with values
        /// </summary>
        /// <param name="itemAlignment"></param>
        /// <param name="item"></param>
        public CollisionReportItem(Alignment itemAlignment, IIntersectable item)
        {
            ItemAlignment = itemAlignment;
            Item = item;
        }
    }
}
