using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simuals.Graphics;

namespace Simloader
{
    public class Block
    {
        public float X { get; set; }
        public float Y { get; set; }
        public ISprite Sprite { get; set; }

        public Block()
        {
            
        }

        public Block(float x, float y, ISprite sprite)
        {
            X = x;
            Y = y;
            Sprite = sprite;
        }

        public void Draw()
        {
            Sprite.Draw(new Vector2(X, Y), new Vector2(1f));
        }
    }
}
