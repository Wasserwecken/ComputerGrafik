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
        : LevelItemBase
	{
		/// <summary>
		/// Current position of the player in the level
		/// </summary>
		public new Box2D HitBox => Physics.HitBox;

		/// <summary>
		/// Sets the values for the offset where the camera should point on
		/// </summary>
		public Vector2 ViewPoint { get; private set; }
        

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
        /// Determines if the player can execute a jump
        /// </summary>
        private bool IsJumpAllowed { get; set; }
        
	   
		/// <summary>
		/// Initialises a player
		/// </summary>
		public LevelItemPlayer(
			Vector2 startPosition,
			InputMapList<LevelItemPlayerActions> inputMapping,
			SpriteAnimated sprite)
            : base(startPosition, new Vector2(0.75f, 0.75f))
		{
			//Bind the input
			InputValues = new LevelItemPlayerActions();
			InputLayout = new InputLayout<LevelItemPlayerActions>(InputValues, inputMapping);

			//set physic behaviour
			var physicProps = new Dictionary<BlockType, LevelItemPhysicBodyProperties>
            {
                {BlockType.Air, new LevelItemPhysicBodyProperties(6f, 30f, 0.1f, 0.2f)},
                {BlockType.Solid, new LevelItemPhysicBodyProperties(6f, 30f, 0.1f, 0.2f)},
                {BlockType.Ladder, new LevelItemPhysicBodyProperties(6f, 6f, 0.1f, 0f)},
                {BlockType.Water, new LevelItemPhysicBodyProperties(30f, 30f, 0.06f, -0.01f)},
                {BlockType.Lava, new LevelItemPhysicBodyProperties(15f, 15f, 0.025f, 0f)}
            };
			Physics = new LevelItemPhysicBody(physicProps, BlockType.Air, base.HitBox);

			//set graphics
			Sprite = sprite;
		}


		/// <summary>
		/// Updates all interactions of the player for a single step
		/// </summary>
		public void UpdateLogic(List<LevelItemBase> intersections)
		{
			ProcessInput();

            Physics.UpdatePhysics(intersections);

            UpdateOffsetViewPoint();
		}

        /// <summary>
        /// Draws the player on the screen
        /// </summary>
        public void Draw()
		{
		    Sprite.FlipTextureHorizontal = Physics.Energy.X > 0;
		    Sprite.Draw(Physics.HitBox.Position, new Vector2(0.8f));
		}


		/// <summary>
		/// Process the input values
		/// </summary>
		private void ProcessInput()
		{
			// tries to move the player in a given direction.
			// The direction values should be between -1 and 1 for x and y
			var moveDirection = new Vector2(InputValues.MoveRight - InputValues.MoveLeft, InputValues.MoveUp - InputValues.MoveDown);
			Physics.ApplyForce(moveDirection);
            
			// tries to execute a jump of the player. In some environments or sitiations
			// it will be not allowed to jump (e.g. water)
            if (InputValues.Jump && (IsJumpAllowed))
            {
				Physics.ApplyImpulse(new Vector2(0, 0.2f));
            }
		}

		/// <summary>
		/// Calcs the view point for the camera
		/// </summary>
		private void UpdateOffsetViewPoint()
		{
			float offsetValue = 2f;
			var x = Physics.HitBox.Position.X + offsetValue;
			var y = Physics.HitBox.Position.Y + offsetValue;

			ViewPoint = new Vector2(x, y);
		}
    }
}
