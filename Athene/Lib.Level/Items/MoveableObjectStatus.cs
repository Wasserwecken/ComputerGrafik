using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Items
{
    public class MoveableObjectStatus
    {
        /// <summary>
        /// Determines if the object is falling
        /// </summary>
        public bool IsFalling { get; set; }

        /// <summary>
        /// Determines if the object is jumping
        /// </summary>
        public bool IsJumping { get; set; }

        /// <summary>
        /// Determines teh direction where the object has been moved
        /// </summary>
        public Vector2 MoveDirection { get; set; }

        /// <summary>
        /// Determines if the object stands on someting
        /// </summary>
        public bool IsGrounded { get; set; }
    }
}
