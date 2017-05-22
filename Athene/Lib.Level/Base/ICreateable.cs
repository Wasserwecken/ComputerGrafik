using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Base
{
    public interface ICreateable
    {
        /// <summary>
        /// Returns all items that has been created
        /// </summary>
        /// <returns></returns>
        List<LevelItemBase> GetCreatedItems();

        /// <summary>
        /// Removes / forgets all created items
        /// </summary>
        void ClearCreatedItems();
    }
}
