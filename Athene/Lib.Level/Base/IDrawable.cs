using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Base
{
    public interface IDrawable
    {
        /// <summary>
        /// 
        /// </summary>
        int ZLevel { get; set; }

        /// <summary>
        /// Draws the item on the screen
        /// </summary>
        void Draw();
    }
}
