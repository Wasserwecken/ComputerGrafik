using System;
using OpenTK;
using Lib.Input;
using Lib.Input.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Lib.LevelLoader.Geometry;
using Lib.Visuals.Graphics;
using OpenTK.Graphics.OpenGL;

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
		/// The physical position and movement of the player
		/// </summary>
		public LevelItemPhysicBody Physics { get; }

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
				{BlockType.Water, new LevelItemPhysicBodyProperties(30f, 30f, 0.05f, -0.005f)},
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

            //var oldPosition = new Vector2(Physics.Position.X, Physics.Position.Y);

			Physics.UpdateLogic();
		    //CheckCollision(level, oldPosition);
			UpdateOffsetViewPoint();
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

		   
            if (InputValues.Jump && PlayerIsOnSolid(level))
				 Physics.ApplyImpulse(new Vector2(0, 0.5f));
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

        /// <summary>
        /// returns if the player stands on a solid block or not
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private bool PlayerIsOnSolid(Level level)
	    {
            var blockBottom = level.Blocks.FirstOrDefault(b => (b.Position.X == Math.Round(Position.X)) && (b.Position.Y < Math.Round(Physics.Position.Y)));
	        if (blockBottom != null)
	        {
                return blockBottom.Box2D.MaxY == Physics.Box2D.Y;
            }
	        return false;

	    }

    }
}
