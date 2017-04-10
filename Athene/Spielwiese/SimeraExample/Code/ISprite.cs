using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SimeraExample.Code
{
    public  interface ISprite
    {
        void Draw(Vector2 position, Vector2 scale);
    }
}
