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
using System.Linq;

namespace Lib.Level.Items
{
    public class Player
        : LevelItemBase, IDrawable, IMoveable, IInteractable, IIntersectable
	{
		/// <summary>
		/// Sets the values for the offset where the camera should point on
		/// </summary>
		public Vector2 ViewPoint { get; private set; }
        
        /// <summary>
        /// List of inventory items
        /// </summary>
        public List<IInventoryItem> Inventory { get; set; }

		/// <summary>
		/// The physical position and movement of the player
		/// </summary>
		private PhysicBody Physics { get; }

		/// <summary>
		/// Setted values from the input
		/// </summary>
		private PlayerActions InputValues { get; }

        /// <summary>
        /// Status of the player
        /// </summary>
        private PlayerStatus Status { get; set; }

        /// <summary>
        /// Input layout for the player
        /// </summary>
        private InputLayout<PlayerActions> InputLayout { get; set; }
        
        /// <summary>
        /// Range where interactions can be requested by other items
        /// </summary>
        public Box2D InteractionBox { get; set; }


        /// <summary>
        /// Initialises a player
        /// </summary>
        public Player(
			    Vector2 startPosition,
			    InputMapList<PlayerActions> inputMapping,
			    SpriteAnimated sprite,
                Dictionary<EnvironmentType, EnergyObjectProperties> impulseProperties,
                Dictionary<EnvironmentType, EnergyObjectProperties> forceProperties)
            : base(startPosition, new Vector2(0.75f, 0.75f))
		{
            Status = new PlayerStatus();
			InputValues = new PlayerActions();
			InputLayout = new InputLayout<PlayerActions>(InputValues, inputMapping);
            Inventory = new List<IInventoryItem>();
			Physics = new PhysicBody(impulseProperties, forceProperties);
            
			Sprite = sprite;
            HasCollisionCorrection = true;
            InteractionBox = HitBox;

        }
                

        /// <summary>
        /// Draws the player on the screen
        /// </summary>
        public void Draw()
		{
            SetAnimation();

            float offsetValue = 2f;
            var x = HitBox.Position.X + offsetValue;
            var y = HitBox.Position.Y + offsetValue;

            ViewPoint = new Vector2(x, y);


            if (Physics.Energy.X > 0)
                Sprite.FlipTextureHorizontal = false;
            if (Physics.Energy.X < 0)
                Sprite.FlipTextureHorizontal = true;
            
            Sprite.Draw(HitBox.Position, new Vector2(0.8f));
		}

        /// <summary>
        /// Executes the momvement logic of the player
        /// </summary>
        public void Move()
        {
            // tries to move the player in a given direction.
            // The direction values should be between -1 and 1 for x and y
            var directionHorizontal = InputValues.MoveRight - InputValues.MoveLeft;
            var directionVertical = InputValues.MoveUp - InputValues.MoveDown;

            //restrict the move direction by environment (in air, the player is not allowed to manipulate its force in the y axis)
            if (Physics.CurrentEnvironment == EnvironmentType.Air)
                directionVertical = 0;

            Physics.ApplyForce(new Vector2(directionHorizontal, directionVertical));


            // tries to execute a jump of the player. In some environments or situations
            // it will be not allowed to jump (e.g. water)
            if (InputValues.Jump && Status.IsJumpAllowed && !Status.IsJumping)
            {
                Physics.ApplyImpulse(new Vector2(0, 0.3f));
                Status.IsJumpAllowed = false;
            }

            if (InputValues.Helping && Status.IsJumpAllowed && !Status.IsJumping)
            {
                Physics.ApplyImpulse(new Vector2(0.5f, 0.4f));
                Status.IsJumpAllowed = false;
            }

            //Apply now the added energy
            Status.MoveDirection = Physics.Process(HitBox.Position);
            HitBox.Position += Status.MoveDirection;
        }

        /// <summary>
        /// Execute interactions with other items
        /// </summary>
        /// <param name="interactableItem"></param>
        public void HandleInteractions(List<IIntersectable> intersectionItems)
        {
            foreach(var item in intersectionItems)
            {
                if (item is Collectable && ((Collectable)item).IsActive)
                {
                    Inventory.Add((Collectable)item);
                    ((Collectable)item).IsActive = false;
                }
            }
        }

        /// <summary>
        /// Reacts to intersections with other items
        /// </summary>
        /// <param name="intersectingItems"></param>
        public void HandleCollisions(List<IIntersectable> intersectingItems)
        {
            var report = CollisionManager.HandleCollisions(HitBox, intersectingItems);

            if (report.CorrectedHorizontal)
                Physics.StopBodyOnAxisX();
            if (report.CorrectedVertical)
                Physics.StopBodyOnAxisY();

            foreach (var item in intersectingItems)
            {
                /* look for checkpoints to activate */
                if (item is Checkpoint checkpoint && !((Checkpoint)item).IsActivated)
                {
                    var result = Inventory.FirstOrDefault(i => i.TypeId == checkpoint.ActivationItemType.ToString());
                    if (result != null)
                    {
                        checkpoint.Activate();
                        Inventory.Remove(result);
                    }
                }

                /* check teleporter */
                if (item is Teleporter teleporter && ((Teleporter)item).IsActivated)
                {
                    teleporter.Teleport(this);
                 
                }
            }

            SetEnvironment(intersectingItems);
            SetPlayerStatus(report);
        }


        /// <summary>
        /// Evaluates the collision report and sets values for the player
        /// </summary>
        /// <param name="intersectingItems"></param>
        private void SetPlayerStatus(CollisionReport report)
        {
            if (Physics.Energy.Y < 0)
            {
                Status.IsFalling = true;
                Status.IsJumpAllowed = false;
                Status.IsGrounded = false;
            }
            else if (Physics.Energy.Y > 0)
            {
                Status.IsFalling = false;
                Status.IsJumping = true;
                Status.IsGrounded = false;
            }
            else
            {
                Status.IsFalling = false;
                Status.IsJumping = false;
                Status.IsGrounded = true;
            }
            

            Status.IsIdle = (Math.Abs(Physics.Energy.X) <= 0 && Math.Abs(Physics.Energy.Y) <= 0);                


            if (Status.Environment == EnvironmentType.Air)
                Status.IsJumpAllowed = (report.IsBottomWater && report.IsSolidOnSide) || Status.IsGrounded;
        }
        
        /// <summary>
        /// Sets the current animation for the player that should be played
        /// </summary>
        private void SetAnimation()
        {
            var playerSprite = (SpriteAnimated)Sprite;

            if (Status.Environment == EnvironmentType.Water)
                playerSprite.StartAnimation("swim");

            if (Status.Environment == EnvironmentType.Air)
                playerSprite.StartAnimation("walk");
        }

        /// <summary>
        /// Sets the environment for the physics body, based on all intersections
        /// </summary>
        /// <param name="intersectingItems"></param>
        private void SetEnvironment(List<IIntersectable> intersectingItems)
        {
            //Set the environment, because if there is no collision, the player has to adapt the default environment
            var playerEnvironment = EnvironmentType.Air;
            foreach (IIntersectable item in intersectingItems)
            {
                if (item is Block && item.HitBox.Contains(HitBox.Center))
                    playerEnvironment = ((Block)item).Environment;
            }

            Status.Environment = playerEnvironment;
            Physics.SetEnvironment(playerEnvironment);
        }
    }
}
