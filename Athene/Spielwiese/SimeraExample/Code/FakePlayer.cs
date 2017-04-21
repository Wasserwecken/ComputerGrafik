using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.LevelLoader;
using Lib.Visuals.Graphics;
using OpenTK;

namespace SimeraExample
{
	class FakePlayer
	{
		public Vector2 Position => PlayerPhysics.Position;
		public Vector2 Energy => PlayerPhysics.Energy;
		public Vector2 Force => PlayerPhysics.ForceReference;

		private ForceObject PlayerPhysics { get; set; }
		private SpriteStatic Sprite { get; set; }
		private Vector2 ForceInput { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public FakePlayer()
		{
			Sprite = new SpriteStatic("Pics/trophy.png");

			//set physics
			var forceProps = new Dictionary<Enum, ForceObjectProperties>
			{
				{Movement.Walk, new ForceObjectProperties(1f, 30f, 0.2f)},
				{Movement.Swimming, new ForceObjectProperties(30f, 30f, -0.005f)}
			};
			PlayerPhysics = new ForceObject(forceProps, new Vector2(200.0f));
			
			PlayerPhysics.SetEnvironment(Movement.Walk);
			PlayerPhysics.Position = new Vector2(-2, 1);
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
				PlayerPhysics.SetEnvironment(Movement.Walk);
				if (Position.Y < 0)
				{
					PlayerPhysics.Energy = new Vector2(PlayerPhysics.Energy.X, 0);
					PlayerPhysics.Position = new Vector2(PlayerPhysics.Position.X, 0);
				}
			}
			else
			{
				if (Position.Y < 0)
					PlayerPhysics.SetEnvironment(Movement.Swimming);
				else
					PlayerPhysics.SetEnvironment(Movement.Walk);
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
			Sprite.Draw(PlayerPhysics.Position, new Vector2(0.8f));
		}
	}
}
