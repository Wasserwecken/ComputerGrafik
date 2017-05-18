﻿using Lib.LevelLoader.LevelItems;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Items
{
    public class PlayerStatus
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
        /// Defines if the player is allowed to jump
        /// </summary>
        public bool IsJumpAllowed { get; set; }

        /// <summary>
        /// Defines if the object has note been moved
        /// </summary>
        public bool IsIdle { get; set; }

        /// <summary>
        /// Determines teh direction where the object has been moved
        /// </summary>
        public Vector2 MoveDirection { get; set; }

        /// <summary>
        /// Determines if the object stands on someting
        /// </summary>
        public bool IsGrounded { get; set; }

        /// <summary>
        /// Environment wher the object is with in
        /// </summary>
        public BlockType Environment { get; set; }
    }
}