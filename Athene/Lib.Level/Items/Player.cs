using Lib.Input;
using Lib.Input.Mapping;
using Lib.Level.Base;
using Lib.Level.Collision;
using Lib.Level.Physics;
using Lib.LevelLoader.LevelItems;
using Lib.Tools;
using Lib.Visuals.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Lib.Level.Items
{
    public class Player
        : LevelItemBase
	{
		/// <summary>
		/// Sets the values for the offset where the camera should point on
		/// </summary>
		public Vector2 ViewPoint { get; private set; }
        

		/// <summary>
		/// The physical position and movement of the player
		/// </summary>
		private PhysicBody Physics { get; }

		/// <summary>
		/// Setted values from the input
		/// </summary>
		private PlayerActions InputValues { get; }

		/// <summary>
		/// Input layout for the player
		/// </summary>
		private InputLayout<PlayerActions> InputLayout { get; set; }
        
        /// <summary>
        /// Determines if the player is allowed to execute a jump
        /// </summary>
        private bool IsJumpAllowed { get; set; }

        /// <summary>
        /// Determines if the player is currently jumping
        /// </summary>
        private bool IsJumping { get { return Physics.Energy.Y > 0; } }
        
	   
		/// <summary>
		/// Initialises a player
		/// </summary>
		public Player(
			Vector2 startPosition,
			InputMapList<PlayerActions> inputMapping,
			SpriteAnimated sprite)
            : base(startPosition, new Vector2(0.75f, 0.75f))
		{
			//Bind the input
			InputValues = new PlayerActions();
			InputLayout = new InputLayout<PlayerActions>(InputValues, inputMapping);

            //set physic behaviour
            var impulseProps = new Dictionary<BlockType, EnergyObjectProperties>
            {
                {BlockType.Air, new EnergyObjectProperties(30f, 30f, 0.1f, 0f)},
                {BlockType.Solid, new EnergyObjectProperties(30f, 30f, 0.1f, 0f)},
                {BlockType.Ladder, new EnergyObjectProperties(10f, 10f, 0.1f, 0f)},
                {BlockType.Water, new EnergyObjectProperties(30f, 30f, 0.06f, 0f)},
                {BlockType.Lava, new EnergyObjectProperties(15f, 15f, 0.025f, 0f)}
            };
            var forceProps = new Dictionary<BlockType, EnergyObjectProperties>
            {
                {BlockType.Air, new EnergyObjectProperties(6f, 30f, 0.1f, 0.2f)},
                {BlockType.Solid, new EnergyObjectProperties(6f, 30f, 0.1f, 0.2f)},
                {BlockType.Ladder, new EnergyObjectProperties(6f, 6f, 0.1f, 0f)},
                {BlockType.Water, new EnergyObjectProperties(30f, 30f, 0.06f, -0.01f)},
                {BlockType.Lava, new EnergyObjectProperties(15f, 15f, 0.025f, 0f)}
            };
			Physics = new PhysicBody(impulseProps, forceProps);

			//set other
			Sprite = sprite;
            Collision = true;
            IsJumpAllowed = false;
		}


		/// <summary>
		/// Updates all interactions of the player for a single step
		/// </summary>
		public void UpdateLogic()
		{
			ProcessInput();

            //Physics
            HitBox.Position = Physics.ProcessInput(HitBox.Position);
            
            //View things
            UpdateOffsetViewPoint();
		}


        /// <summary>
        /// reacts to intersections
        /// </summary>
        /// <param name="intersections"></param>
        public void HandleIntersections(List<LevelItemBase> intersections)
        {
            //Set the environment, because if there is no collision, the player has to adapt the default environment
            Physics.SetEnvironment(BlockType.Air);
            foreach (LevelItemBase item in intersections)
            {
                if (item.HitBox.Contains(HitBox.Center))
                    Physics.SetEnvironment(item.BlockType);
            }

            //Collisions
            var report = CollisionManager.HandleCollisions(HitBox, intersections, () => Physics.StopBodyOnAxisX(), () => Physics.StopBodyOnAxisY());
            ProcessIntersectionReport(report);
        }
        

        /// <summary>
        /// Draws the player on the screen
        /// </summary>
        public void Draw()
		{
            if (Physics.Energy.X > 0)
                Sprite.FlipTextureHorizontal = false;
            if (Physics.Energy.X < 0)
                Sprite.FlipTextureHorizontal = true;
            
            Sprite.Draw(HitBox.Position, new Vector2(0.8f));
		}


        /// <summary>
        /// Processing the collision report to analyze what the player is able / allowed to do
        /// </summary>
        /// <param name="intersections"></param>
        private void ProcessIntersectionReport(CollisionReport report)
        {
            if (Physics.CurrentEnvironment == BlockType.Air)
            {
                IsJumpAllowed = false;

                //This will allow the player to jump out of water
                if(report.IsBottomWater && report.IsSolidOnSide)
                   IsJumpAllowed = true;

                //defining the normal jump
                if (report.IsSolidOnBottom)
                    IsJumpAllowed = true;
            }
        }

		/// <summary>
		/// Process the input values
		/// </summary>
		private void ProcessInput()
		{
            // tries to move the player in a given direction.
            // The direction values should be between -1 and 1 for x and y
            var directionHorizontal = InputValues.MoveRight - InputValues.MoveLeft;
            var directionVertical = InputValues.MoveUp - InputValues.MoveDown;

            //restrict the move direction by environment (in air, the player is not allowed to manipulate its force in the y axis)
            if (Physics.CurrentEnvironment == BlockType.Air)
                directionVertical = 0;
            
			Physics.ApplyForce(new Vector2(directionHorizontal, directionVertical));
            

			// tries to execute a jump of the player. In some environments or situations
			// it will be not allowed to jump (e.g. water)
            if (InputValues.Jump && IsJumpAllowed && !IsJumping)
            {
				Physics.ApplyImpulse(new Vector2(0, 0.3f));
                IsJumpAllowed = false;
            }

            if (InputValues.Helping && IsJumpAllowed && !IsJumping)
            {
                Physics.ApplyImpulse(new Vector2(0.5f, 0.4f));
                IsJumpAllowed = false;
            }
		}

		/// <summary>
		/// Calcs the view point for the camera
		/// </summary>
		private void UpdateOffsetViewPoint()
		{
			float offsetValue = 2f;
			var x = HitBox.Position.X + offsetValue;
			var y = HitBox.Position.Y + offsetValue;

			ViewPoint = new Vector2(x, y);
		}
    }
}
