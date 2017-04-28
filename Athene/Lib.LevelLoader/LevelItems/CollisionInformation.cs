using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.LevelLoader.LevelItems
{
    public struct CollisionReport
    {
        /// <summary>
        /// 
        /// </summary>
        public bool CollisionOnTop { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool CollisionOnBottom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool CollisionOnLeft { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool CollisionOnRight { get; set; }
    }
}
