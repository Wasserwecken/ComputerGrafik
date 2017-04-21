using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader;
using Lib.LevelLoader.LevelItems;
using Lib.Visuals.Graphics;
using OpenTK;

namespace SimeraExample
{
	class FakePlayer
	{
		public Vector2 Position => new Vector2(PlayerPhysics.X, PlayerPhysics.Y);
		public Vector2 Energy => PlayerPhysics.Energy;
		public Vector2 Force => PlayerPhysics.ForceReference;

		private LevelItemPhysicBody PlayerPhysics { get; set; }
		private SpriteStatic Sprite { get; set; }
		private Vector2 ForceInput { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public FakePlayer()
		{
			Sprite = new SpriteStatic("Pics/trophy.png");

			//set physics
			var forceProps = new Dictionary<BlockType, LevelItemPhysicBodyProperties>
			{
				{BlockType.Solid, new LevelItemPhysicBodyProperties(1f, 30f, 0.2f)},
				{BlockType.Liquid, new LevelItemPhysicBodyProperties(30f, 30f, -0.005f)}
			};

			PlayerPhysics = new LevelItemPhysicBody(forceProps, BlockType.Solid);
			PlayerPhysics.X = 0;
			PlayerPhysics.Y = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		public void ExecuteLogic()
		{
			PlayerPhysics.ApplyForce(ForceInput);
			ForceInput = Vector2.Zero;

			if (Position.X > 0)
			{
				PlayerPhysics.SetEnvironment(BlockType.Solid);
				if (Position.Y < 0)
				{
					PlayerPhysics.Energy = new Vector2(PlayerPhysics.Energy.X, 0);
					PlayerPhysics.Y = 0;
				}
			}
			else
			{
				if (Position.Y < 0)
					PlayerPhysics.SetEnvironment(BlockType.Liquid);
				else
					PlayerPhysics.SetEnvironment(BlockType.Solid);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Move(float directionX, float directionY)
		{
			float inputForce = 0.1f;
		 	ForceInput = ForceInput + new Vector2(directionX * inputForce, directionY * inputForce);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Jump()
		{
			PlayerPhysics.ApplyImpulse(new Vector2(0, 0.2f));
		}

		/// <summary>
		/// render things
		/// </summary>
		public void Draw()
		{
			Sprite.Draw(new Vector2(PlayerPhysics.X, PlayerPhysics.Y), new Vector2(0.8f));
		}
	}
}
