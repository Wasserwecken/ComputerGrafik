using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.LevelLoader
{
    public class Level
    {
        public List<Block> Blocks { get; set; }

        public Level(List<Block> blocks)
        {
            Blocks = blocks;
        }

        public  Level()
		{
			Blocks = new List<Block>();
		}

        public void Draw()
        {
            foreach (var block in Blocks)
            {
                block.Draw();
            }
        }

    }
}
