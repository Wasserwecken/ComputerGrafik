using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader.LevelItems;

namespace Lib.Level.Items
{
    public interface IInventoryItem
    {
        ItemType ItemType { get; }
    }
}
