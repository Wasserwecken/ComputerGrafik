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
using System.Diagnostics;
using System.Linq;

namespace Lib.Level.Items
{
    public class Player
        : LevelItemBase, IDrawable, IMoveable, IInteractable, IIntersectable, ICreateable
    {
        /// <summary>
        /// 
        /// </summary>
        public int ZLevel { get; set; }

        /// <summary>
        /// Sets the values for the offset where the camera should point on
        /// </summary>
        public Vector2 ViewPoint { get; private set; }
        
        /// <summary>
        /// List of inventory items
        /// </summary>
        public Inventory Inventory { get; set; }

        /// <summary>
        /// collission of the block
        /// </summary>
        public bool HasCollisionCorrection { get; set; }

        /// <summary>
        /// life of the player
        /// </summary>
        private int Life { get; set; }

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
        /// Delay for the the next shoot in logic ticks
        /// </summary>
        private int ReloadTime { get; set; }


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
            Inventory = new Inventory(0.5f, 0.01f, 1);
            Physics = new PhysicBody(impulseProperties, forceProperties);
            
            Sprite = sprite;
            HasCollisionCorrection = true;
            ReloadTime = 0;
            ZLevel = 2;
		    Life = 100;
            //float interactionSizeFactor = 2f;
            //float interactionSizeX = HitBox.Size.X * interactionSizeFactor;
            //float interactionSizeY = HitBox.Size.Y * interactionSizeFactor;

            //InteractionBox = new Box2D(HitBox.Position.X - (interactionSizeX / 2), HitBox.Position.Y - (interactionSizeY / 2), interactionSizeX, interactionSizeY);
            InteractionBox = HitBox;
        }

	    private void TakeDamage(int damage)
	    {
	        Life -= damage;
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

            if (Status.ViewDirection > 0)
                Sprite.FlipTextureHorizontal = false;
            else if (Status.ViewDirection < 0)
                Sprite.FlipTextureHorizontal = true;
            

            Sprite.Draw(HitBox.Position, Vector2.One);

            Inventory.Position = new Vector2(HitBox.Center.X, HitBox.MaximumY + 0.1f);
            Inventory.Draw();
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

            //Apply now the added energy
            Status.MoveDirection = Physics.Process(HitBox.Position);

            if (Status.MoveDirection.X > 0)
                Status.ViewDirection = 1;
            else if (Status.MoveDirection.X < 0)
                Status.ViewDirection = -1;

            HitBox.Position += Status.MoveDirection;

            //Shooting things
            ReloadTime = Math.Max(0, ReloadTime - 1);
        }

	    /// <summary>
	    /// Execute interactions with other items
	    /// </summary>
	    /// <param name="intersectionItems"></param>
	    public void HandleInteractions(List<IIntersectable> intersectionItems)
        {
            foreach(var item in intersectionItems)
            {
                if (item is Collectable collectable && collectable.IsActive)
                {
                    collectable.Remove = true;
                    collectable.IsActive = false;
                    Inventory.AddItem(new InventoryItem(collectable.Sprite, collectable.ItemType));
                }

                /* look for checkpoints to activate */
                if (item is Checkpoint checkpoint && !checkpoint.IsActive)
                {
                    var getItem = Inventory.GetFirstItemofType(checkpoint.ActivationItemType);
                    if (getItem != null)
                    {
                        checkpoint.Activate();
                        Inventory.RemoveItem(getItem);
                    }
                }

                /* check teleporter */
                if (item is Teleporter teleporter)
                {
                    HitBox.Position = teleporter.DestinationPosition;
                    //Manipulating the position in the direction where the player is moving, else the 
                    //player would be teleported immidiatly back
                    HitBox.Position += new Vector2(Math.Sign(Physics.Energy.X), Math.Sign(Physics.Energy.Y));
                }


                if (item is Player otherPlayer)
                {
                    if (InputValues.Helping && !otherPlayer.InputValues.Helping)
                    {
                        otherPlayer.HitBox.Position += new Vector2(0, 1);

                        if (otherPlayer.Status.IsGrounded)
                            otherPlayer.Physics.ApplyImpulse(new Vector2(0.3f * Status.ViewDirection, 0.6f));
                    }
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
            
            SetEnvironment(intersectingItems);
            SetPlayerStatus(report);
        }

        /// <summary>
        /// Retruns all items that has been created from the player
        /// </summary>
        /// <returns></returns>
        public List<LevelItemBase> GetCreatedItems()
        {
            var bulletList = new List<LevelItemBase>();

            if (InputValues.Shoot && ReloadTime <= 0)
            {
                var direction = new Vector2(Status.ViewDirection, 0.3f);
                bulletList.Add(new Bullet(HitBox.Center + direction, direction));
                ReloadTime = 5;
            }

            return bulletList;
        }

        /// <summary>
        /// Clears all created items
        /// </summary>
        public void ClearCreatedItems()
        {
        }


        /// <summary>
        /// Evaluates the collision report and sets values for the player
        /// </summary>
        /// <param name="report"></param>
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
                Status.IsGrounded = report.IsSolidOnBottom;
            }
            
            if (Status.IsGrounded && Math.Abs(Physics.Energy.X) <= 0)
            {
                Status.IsWalking = false;
                Status.IsIdle = true;
            }
            else
            {
                Status.IsWalking = true;
                Status.IsIdle = false;
            }

            if (Status.Environment == EnvironmentType.Air)
                Status.IsJumpAllowed = (report.IsBottomWater && report.IsSolidOnSide) || Status.IsGrounded;
        }
        
        /// <summary>
        /// Sets the current animation for the player that should be played
        /// </summary>
        private void SetAnimation()
        {
            var playerSprite = (SpriteAnimated)Sprite;

            switch(Status.Environment)
            {
                case EnvironmentType.Water:
                    if (Status.IsIdle)
                        playerSprite.StopAnimation();
                    else
                        playerSprite.StartAnimation("swim");
                    break;

                case EnvironmentType.Ladder:
                    if (Status.IsIdle)
                        playerSprite.StopAnimation();
                    else
                        playerSprite.StartAnimation("climb");
                    break;

                case EnvironmentType.Air:
                    if (Status.IsIdle)
                        playerSprite.StartAnimation("idle");

                    else if (Status.IsFalling || Status.IsJumping)
                        playerSprite.StartAnimation("fall");

                    else if (Status.IsWalking)
                        playerSprite.StartAnimation("walk");
                    break;

            }
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
