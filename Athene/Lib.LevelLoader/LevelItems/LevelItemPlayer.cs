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
		/// Current movmement speed
		/// </summary>
		private float MovementSpeed { get; set; }

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
				{BlockType.Solid, new LevelItemPhysicBodyProperties(1f, 30f, 0.2f)},
				{BlockType.Liquid, new LevelItemPhysicBodyProperties(30f, 30f, -0.005f)}
			};
			Physics = new LevelItemPhysicBody(physicProps, startEnvironment, startPosition);

			//Set basic values
			MovementSpeed = 0.1f;

			//set graphics
			Sprite = sprite;
		}


		/// <summary>
		/// Updates all interactions of the player for a single step
		/// </summary>
		public void UpdateLogic(Level level)
		{
			ProcessInput(level);

            var oldPosition = new Vector2(Physics.Position.X, Physics.Position.Y);

			Physics.UpdateLogic();
		    CheckCollision(level, oldPosition);
			UpdateOffsetViewPoint();
		}

	    public void CheckCollision(Level level, Vector2 oldPosition)
	    {

	        var blockInformations = GetPlayerBlockEnvironment(level);

	        if (blockInformations.BlockBottom != null)
	        {
                var collision = Physics.Box2D.Intersects(blockInformations.BlockBottom.Box2D);
                if (collision)
                {
                    //Console.WriteLine("bottom collision");
                    Physics.Position = new Vector2(Physics.Position.X, blockInformations.BlockBottom.Box2D.MaxY);
                    Physics.Box2D = new Box2D(Physics.Position.X, blockInformations.BlockBottom.Box2D.MaxY, Physics.Box2D.SizeX, Physics.Box2D.SizeY);
                }
            }
	        if (blockInformations.BlockLeft != null)
	        {
                var collision = Physics.Box2D.Intersects(blockInformations.BlockLeft.Box2D);
                if (collision)
                {
                    Console.WriteLine("left collision");
                    Physics.Position = new Vector2(blockInformations.BlockLeft.Box2D.MaxX, Physics.Position.Y);
                    Physics.Box2D = new Box2D(blockInformations.BlockLeft.Box2D.MaxX, Physics.Position.Y, Physics.Box2D.SizeX, Physics.Box2D.SizeY);
                }
            }
            if (blockInformations.BlockRight != null)
            {
                var collision = Physics.Box2D.Intersects(blockInformations.BlockRight.Box2D);
                if (collision)
                {
                    Console.WriteLine("right collision");
                    Physics.Position = new Vector2(blockInformations.BlockRight.Position.X - Physics.Box2D.SizeX, Physics.Position.Y);
                    Physics.Box2D = new Box2D(blockInformations.BlockRight.Position.X - Physics.Box2D.SizeX, Physics.Position.Y, Physics.Box2D.SizeX, Physics.Box2D.SizeY);
                }
            }
            if (blockInformations.BlockTop != null)
            {
                var collision = Physics.Box2D.Intersects(blockInformations.BlockTop.Box2D);
                if (collision)
                {
                    //Console.WriteLine("bottom collision");
                    Physics.Position = new Vector2(Physics.Position.X, blockInformations.BlockTop.Box2D.Y - blockInformations.BlockTop.Box2D.SizeY);
                    Physics.Box2D = new Box2D(Physics.Position.X, blockInformations.BlockTop.Box2D.Y - blockInformations.BlockTop.Box2D.SizeY, Physics.Box2D.SizeX, Physics.Box2D.SizeY);
                }
            }
            if (blockInformations.BlockBehind != null)
            {
                var collision = Physics.Box2D.Intersects(blockInformations.BlockBehind.Box2D);
                if (collision)
                {
                    Console.WriteLine("behind collision");
                    Physics.Position = oldPosition;
                    Physics.Box2D = new Box2D(oldPosition.X, oldPosition.Y, Physics.Box2D.SizeX, Physics.Box2D.SizeY);
                }
            }

          


        }

		/// <summary>
		/// Draws the player on the screen
		/// </summary>
		public void Draw()
		{
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

			moveDirection *= MovementSpeed;
			Physics.ApplyForce(moveDirection);


			// tries to execute a jump of the player. In some environments or sitiations
			// it will be not allowed to jump (e.g. water)

		    bool collision = false;

		    var blockList = GetPlayerBlockEnvironment(level);
		    collision = blockList.BlockBottom?.Box2D.MaxY == Physics.Box2D.Y;

            if (InputValues.Jump && collision)
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
        /// returns a block information set about blocks near
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
	    private PlayerBlockEnvironmentSet GetPlayerBlockEnvironment(Level level)
	    {
	        PlayerBlockEnvironmentSet returnInformation = new PlayerBlockEnvironmentSet();

            float xBottomPositionRounding;
            //if (InputValues.MoveLeft > 0)
            //    xBottomPositionRounding = (float)Math.Ceiling(Physics.Position.X);
            //else if (InputValues.MoveRight > 0)
            //    xBottomPositionRounding = (float)Math.Floor(Physics.Position.X);
            //else
            xBottomPositionRounding = (float)Math.Round(Physics.Position.X);



            // set the bottom block
            returnInformation.BlockBottom = level.Blocks.FirstOrDefault( b => (b.Position.X == xBottomPositionRounding) && (b.Position.Y < Math.Round(Physics.Position.Y)));
            // set the top block
            returnInformation.BlockTop = level.Blocks.FirstOrDefault( b =>(b.Position.X == xBottomPositionRounding) && (b.Position.Y > Math.Round(Physics.Position.Y)));

            var blocksLeft = (from block in level.Blocks where (block.Position.Y == Math.Round(Physics.Position.Y)) && (block.Position.X < Math.Round(Physics.Position.X))
                              orderby block.Position.X descending
                              select block).ToArray();
            // set the left block
	        if (blocksLeft.Length > 0)
	            returnInformation.BlockLeft = blocksLeft[0];

            
            var blocksRight = (from block in level.Blocks where (block.Position.Y == Math.Round(Physics.Position.Y)) && (block.Position.X > Math.Round(Physics.Position.X))
                               orderby block.Position.X
                               select block).ToArray();
            // set the right block
            if (blocksRight.Length > 0)
                returnInformation.BlockRight = blocksRight[0];


            var blocksTopRight = (from block in level.Blocks where (block.Position.Y > Math.Round(Physics.Position.Y)) && (block.Position.X > Math.Round(Physics.Position.X))
                               orderby block.Position.X
                               select block).ToArray();
            //if (blocksTopRight.Length > 0)
            //    returnInformation.BlockTopRight = blocksTopRight[0];

            var blocksBottomRight = (from block in level.Blocks
                                  where (block.Position.Y < Math.Round(Physics.Position.Y)) && (block.Position.X > Math.Round(Physics.Position.X))
                                  orderby block.Position.X
                                  select block).ToArray();
            //if (blocksBottomRight.Length > 0)
            //    returnInformation.BlockBottomRight = blocksBottomRight[0];

            returnInformation.BlockBehind = level.Blocks.FirstOrDefault(b => (b.Position.X == Math.Round(Physics.Position.X)) && (b.Position.Y == Math.Round(Physics.Position.Y)));

            return returnInformation;
	    }
    }
}
