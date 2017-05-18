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
        private MoveableObjectStatus Status { get; set; }

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
        /// Range where interactions can be requested by other items
        /// </summary>
        public float InteractionRadius { get; set; }


        /// <summary>
        /// Initialises a player
        /// </summary>
        public Player(
			    Vector2 startPosition,
			    InputMapList<PlayerActions> inputMapping,
			    SpriteAnimated sprite,
                Dictionary<BlockType, EnergyObjectProperties> impulseProperties,
                Dictionary<BlockType, EnergyObjectProperties> forceProperties)
            : base(startPosition, new Vector2(0.75f, 0.75f))
		{
            Status = new MoveableObjectStatus();
			InputValues = new PlayerActions();
			InputLayout = new InputLayout<PlayerActions>(InputValues, inputMapping);
            Inventory = new List<IInventoryItem>();
			Physics = new PhysicBody(impulseProperties, forceProperties);
            
			Sprite = sprite;
            HasCollisionCorrection = true;
            IsJumpAllowed = false;
		}

        
        /// <summary>
        /// Adds a item to the inventory
        /// </summary>
        /// <param name="item"></param>
	    public void PickUp(Collectable item)
	    {
	        Inventory.Add(item);
	        item.IsActive = false;
	    }
        

        /// <summary>
        /// Draws the player on the screen
        /// </summary>
        public void Draw()
		{
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

            //Apply now the added energy
            Status.MoveDirection = Physics.Process(HitBox.Position);
            HitBox.Position += Status.MoveDirection;
        }

        /// <summary>
        /// Execute interactions with other items
        /// </summary>
        /// <param name="interactableItem"></param>
        public void HandleInteractions(List<IInteractable> interactableItem) { }

        /// <summary>
        /// Reacts to intersections with other items
        /// </summary>
        /// <param name="intersectingItems"></param>
        public void HandleCollisions(List<IIntersectable> intersectingItems)
        {
            //Set the environment, because if there is no collision, the player has to adapt the default environment
            Physics.SetEnvironment(BlockType.Air);
            foreach (IIntersectable item in intersectingItems)
            {
                if (item is Block && item.HitBox.Contains(HitBox.Center))
                {
                    Physics.SetEnvironment(((Block)item).BlockType);
                }
            }

            //Collisions
            var report = CollisionManager.HandleCollisions(HitBox, intersectingItems);

            if (report.CorrectedHorizontal)
                Physics.StopBodyOnAxisX();
            if (report.CorrectedVertical)
                Physics.StopBodyOnAxisY();

            if (Physics.CurrentEnvironment == BlockType.Air)
            {
                ((SpriteAnimated)Sprite).StartAnimation("walk");
                IsJumpAllowed = false;

                //This will allow the player to jump out of water
                if (report.IsBottomWater && report.IsSolidOnSide)
                    IsJumpAllowed = true;

                //defining the normal jump
                if (report.IsSolidOnBottom)
                    IsJumpAllowed = true;
            }
            if (Physics.CurrentEnvironment == BlockType.Water)
                ((SpriteAnimated)Sprite).StartAnimation("swim");
        }
    }
}
