using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimeraExample.Code
{
    class Level
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

        public void InitTestData()
		{
			Blocks.Add(new Block(0, 1, new Sprite(TextureLoader.LoadFromFile("Pics/trophy.png"))));
			Blocks.Add(new Block(1, 1, new Sprite(TextureLoader.LoadFromFile("Pics/bigtree.png"))));

			Blocks.Add(new Block(-1, 0, new Sprite(TextureLoader.LoadFromFile("Pics/dirt_end_left.png"))));
			Blocks.Add(new Block(0, 0, new Sprite(TextureLoader.LoadFromFile("Pics/dirt_middle.png"))));
			Blocks.Add(new Block(1, 0, new Sprite(TextureLoader.LoadFromFile("Pics/dirt_end_right.png"))));
			Blocks.Add(new Block(-1, -1, new Sprite(TextureLoader.LoadFromFile("Pics/dirt.png"))));
			Blocks.Add(new Block(0, -1, new Sprite(TextureLoader.LoadFromFile("Pics/dirt.png"))));
			Blocks.Add(new Block(1, -1, new Sprite(TextureLoader.LoadFromFile("Pics/dirt.png"))));

			var anList = TextureLoader.LoadAnimationImages("Pics/Worm/frame-", 1, 8, ImageExtension.png);
			Blocks.Add(new Block(-1, 1, new AnimatedSprite(anList, 1)));
		}

    }
}
