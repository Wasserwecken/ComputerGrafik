using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.LevelLoader
{
    public class Level
    {
        /// <summary>
        /// List of all blocks drawn in the level, like walkable, water, lava, trees..
        /// </summary>
        public List<Block> Blocks { get; set; }

        /// <summary>
        /// Initializes a level with a list of blocks
        /// </summary>
        /// <param name="blocks"></param>
        public Level(List<Block> blocks)
        {
            Blocks = blocks;
        }

        /// <summary>
        /// Initializes a empty level
        /// </summary>
        public  Level()
		{
			Blocks = new List<Block>();
		}

        /// <summary>
        /// Draws the level
        /// </summary>
        public void Draw()
        {
            foreach (var block in Blocks)
            {
                block.Draw();
            }
        }

    }
}
