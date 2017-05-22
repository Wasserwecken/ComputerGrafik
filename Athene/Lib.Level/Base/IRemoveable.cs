using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Base
{
    public interface IRemoveable
    {
        /// <summary>
        /// Determines if the item can be removed from the game
        /// </summary>
        bool Remove { get; set; }
    }
}
