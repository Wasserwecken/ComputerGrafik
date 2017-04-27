using Lib.Input;
using Lib.Input.Mapping;
using Lib.Tools;
using Lib.Visuals.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.LevelLoader.LevelItems
{
    public class LevelItemPlayer
	{
		/// <summary>
		/// Current position of the player in the level
		/// </summary>
		public Vector2 Position => Physics.Position;

		/// <summary>
		/// Sets the values for the offset where the camera should point on
		/// </summary>
		public Vector2 ViewPoint { get; private set; }

        /// <summary>
        /// Hitbox of the player
        /// </summary>
        public Box2D HitBox => Physics.HitBox;


		/// <summary>
		/// The physical position and movement of the player
		/// </summary>
		private LevelItemPhysicBody Physics { get; }

		/// <summary>
		/// Setted values from the input
		/// </summary>
		private LevelItemPlayerActions InputValues { get; }

		/// <summary>
		/// Input layout for the player
		/// </summary>
		private InputLayout<LevelItemPlayerActions> InputLayout { get; set; }

		/// <summary>
		/// Sprite of the player
		/// </summary>
		private SpriteAnimated Sprite { get; }

        /// <summary>
        /// Determines if the player can execute a jump
        /// </summary>
        private bool IsJumpAllowed { get; set; }

        /// <summary>
        /// Determines if the player can execute a second jump
        /// </summary>
        private bool IsDoubleJumpAllowed { get; set; }

	   
		/// <summary>
		/// Initialises a player
		/// </summary>
		public LevelItemPlayer(
			Vector2 startPosition,
			BlockType startEnvironment,
			InputMapList<LevelItemPlayerActions> inputMapping,
			SpriteAnimated sprite)
		{
			//Bind the input
			InputValues = new LevelItemPlayerActions();
			InputLayout = new InputLayout<LevelItemPlayerActions>(InputValues, inputMapping);

			//set physic behaviour
			var physicProps = new Dictionary<BlockType, LevelItemPhysicBodyProperties>
            {
                {BlockType.Solid, new LevelItemPhysicBodyProperties(6f, 30f, 0.1f, 0.2f)},
                {BlockType.Ladder, new LevelItemPhysicBodyProperties(6f, 6f, 0.1f, 0f)},
                {BlockType.Water, new LevelItemPhysicBodyProperties(30f, 30f, 0.06f, -0.01f)},
                {BlockType.Lava, new LevelItemPhysicBodyProperties(15f, 15f, 0.025f, 0f)}
            };
			Physics = new LevelItemPhysicBody(physicProps, startEnvironment, startPosition);

			//set graphics
			Sprite = sprite;
		}


		/// <summary>
		/// Updates all interactions of the player for a single step
		/// </summary>
		public void UpdateLogic(Level level)
		{
			ProcessInput(level);
            
			Physics.UpdateLogic();
		    
            UpdateOffsetViewPoint();
		}

        /// <summary>
        /// Handles all collisions with the giving blocks
        /// </summary>
        /// <param name="collidingBlock">the colliding block</param>
        /// <returns>Returns true if the body got hit from bottom, fals if it is another direction</returns>
        public void HandleCollisions(List<LevelItemBase> collidingItems)
        {
            //Setting the standard environment (will stay if there is not collision)
            var playersEnvironment = BlockType.Solid;
            IsJumpAllowed = false;

            foreach(LevelItemBase item in collidingItems)
            {
                // check for the environment and may reset the jump
                if (item.HitBox.Contains(HitBox.Center))
                    playersEnvironment = item.BlockType;

                HandleCollision(item);
            }

            //finally
            Physics.SetEnvironment(playersEnvironment);
        }

        /// <summary>
        /// React to a collision with a block
        /// </summary>
        /// <param name="collidingItem">the colliding block</param>
        /// <returns>Returns true if the body got hit from bottom, fals if it is another direction</returns>
        private void HandleCollision(LevelItemBase collidingItem)
        {
            // React to the collision with the body
            var collisionInfo = Physics.HandleCollision(collidingItem);
            if (collisionInfo.CollisionOnBottom)
            {
                //resetting the possibility to jump
                if (Physics.CurrentEnvironment == BlockType.Solid)
                    IsJumpAllowed = true;
            }
        }

        /// <summary>
        /// Draws the player on the screen
        /// </summary>
        public void Draw()
		{
		    Sprite.FlipTextureHorizontal = Physics.Energy.X > 0;

		    Sprite.Draw(Physics.Position, new Vector2(0.8f));
		}


		/// <summary>
		/// Process the input values
		/// </summary>
		private void ProcessInput(Level level)
		{
			// tries to move the player in a given direction.
			// The direction values should be between -1 and 1 for x and y
			var moveDirection = new Vector2(InputValues.MoveRight - InputValues.MoveLeft, InputValues.MoveUp - InputValues.MoveDown);
			Physics.ApplyForce(moveDirection);
            
			// tries to execute a jump of the player. In some environments or sitiations
			// it will be not allowed to jump (e.g. water)
            if (InputValues.Jump && (IsJumpAllowed || IsDoubleJumpAllowed))
            {
				Physics.ApplyImpulse(new Vector2(0, 0.2f));
                IsJumpAllowed = false;
            }
		}

		/// <summary>
		/// Calcs the view point for the camera
		/// </summary>
		private void UpdateOffsetViewPoint()
		{
			float offsetValue = 1f;
			var x = Physics.Position.X + offsetValue;
			var y = Physics.Position.Y + offsetValue;

			ViewPoint = new Vector2(x, y);
		}
    }
}
