using Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Base
{
    public interface IInteractable
    {
        /// <summary>
        /// 
        /// </summary>
        Box2D InteractionBox { get; set; }

        /// <summary>
        /// Interacts with the given objects
        /// </summary>
        /// <param name="interactableItem"></param>
        void HandleInteractions(List<IIntersectable> interactableItem);
    }
}
