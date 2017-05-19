using Lib.Level.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Items
{
    public class Inventory
        : IDrawable
    {
        List<IInventoryItem> Items;

        /// <summary>
        /// Renders the inventory
        /// </summary>
        public void Draw()
        {

        }
    }
}
