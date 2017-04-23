using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.LevelItems;
using OpenTK;

namespace Lib.LevelLoader
{
    public class Level
    {
        /// <summary>
        /// List of all blocks drawn in the level, like walkable, water, lava, trees..
        /// </summary>
        public List<Block> Blocks { get; set; }

        /// <summary>
        /// List of all players in the level
        /// </summary>
        public List<LevelItemPlayer> Players { get; set; }

        /// <summary>
        /// Initializes a empty level
        /// </summary>
        public  Level()
		{
			Blocks = new List<Block>();
            Players = new List<LevelItemPlayer>();
		}

        /// <summary>
        /// updates the level
        /// </summary>
        public void UpdateLogic()
        {
          

            foreach (var player in Players)
            {
                player.UpdateLogic(this);


                
            }
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
            foreach (var player in Players)
            {
                player.Draw();
            }
        }

    }
}
