using Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Base
{
    public interface IInteractable
        : IIntersectable
    {
        /// <summary>
        /// Range where the interaction can be triggered
        /// </summary>
        float InteractionRadius { get; set; }

        /// <summary>
        /// Interacts with the given objects
        /// </summary>
        /// <param name="interactableItem"></param>
        void HandleInteractions(List<IInteractable> interactableItem);
    }
}
