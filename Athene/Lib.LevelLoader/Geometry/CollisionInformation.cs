using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.LevelItems;

namespace Lib.LevelLoader.Geometry
{
    /// <summary>
    /// represents information about a collision
    /// </summary>
    public static class CollisionInformation
    {
        /// <summary>
        ///  returns the type of a collision
        /// </summary>
        /// <param name="block1"></param>
        /// <param name="block2"></param>
        /// <returns></returns>
        public static CollisionType FromBlocks(LevelItemBase block1, LevelItemBase block2)
        {
            if (block2.Position.X == Math.Round(block1.Position.X) && block2.Position.Y < block1.Position.Y)
            {
                return CollisionType.Bottom;
            }
            else if (block2.Position.X == Math.Round(block1.Position.X) && block2.Position.Y > block1.Position.Y)
            {
                return CollisionType.Top;
            }
            else if (block2.Position.X > block1.Position.X && block2.Position.Y == Math.Round(block1.Position.Y))
            {
                return CollisionType.Right;
            }
            else if (block2.Position.X < block1.Position.X && block2.Position.Y == Math.Round(block1.Position.Y))
            {
                return CollisionType.Left;
            }
            else if (block2.Position.X == Math.Round(block1.Position.X) && block2.Position.Y == Math.Round(block1.Position.Y))
            {
                return CollisionType.Behind;
            }
            else
            {
                return CollisionType.MoreCollision;
             
            }


        }

        
    }
}
