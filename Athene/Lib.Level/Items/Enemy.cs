using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Level.Base;
using Lib.Level.Collision;
using Lib.Level.Physics;
using Lib.LevelLoader.LevelItems;
using Lib.Tools;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.Level.Items
{
    public class Enemy : LevelItemBase, IDrawable, IMoveable, IIntersectable, IRemoveable
    {
        /// <summary>
        /// 
        /// </summary>
        public int ZLevel { get; set; }

        /// <summary>
        /// the type of the enemy
        /// </summary>
        public EnemyType EnemyType { get; set; }

        /// <summary>
        /// The physical position and movement of the enemy
        /// </summary>
        private PhysicBody Physics { get; }

        /// <summary>
        /// the enemy status
        /// </summary>
        public EnemyStatus Status { get; set; }

        /// <summary>
        /// Range where interactions can be requested by other items
        /// </summary>
        public Box2D InteractionBox { get; set; }

        /// <summary>
        /// collission of the enemy
        /// </summary>
        public bool HasCollisionCorrection { get; set; }

        /// <summary>
        /// walking direction, 1 = right; -1 = left
        /// </summary>
        public int WalkDirection { get; set; }

        /// <summary>
        /// the commands of the enemy
        /// </summary>
        public EnemyCommands Commands { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MovementType MovementType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Remove { get; set; }


        /// <summary>
        /// initializes a player
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="sprite"></param>
        /// <param name="enemyType"></param>
        /// <param name="movementType"></param>
        public Enemy(Vector2 startPosition, SpriteAnimated sprite, EnemyType enemyType, MovementType movementType) : base(startPosition, new Vector2(0.75f, 0.75f))
        {
            ZLevel = 1;
            Sprite = sprite;
            EnemyType = enemyType;
            Status = new EnemyStatus {MoveDirection = new Vector2(1f, 0)};
            MovementType = movementType;
            WalkDirection = 1;
            Commands = new EnemyCommands();

            Physics = EnemyPhysicsFactory.GetPhysicsByEnemyType(enemyType);
            InteractionBox = HitBox;
            Damage = 2;
        }

        public void Draw()
        {
            if (Physics.Energy.X < 0)
                Sprite.FlipTextureHorizontal = false;
            if (Physics.Energy.X > 0)
                Sprite.FlipTextureHorizontal = true;

            Sprite.Draw(HitBox.Position, new Vector2(0.8f));
        }

        public void Move()
        {
            if (Commands.TurnAround)
            {
                WalkDirection = WalkDirection * -1;
                Commands.TurnAround = false;
            }

            var directionHorizontal = 0.5f * WalkDirection;
            var directionVertical = 0f;

        

            Physics.ApplyForce(new Vector2(directionHorizontal, directionVertical));

           
            if (Commands.Jump && Status.IsJumpAllowed && !Status.IsJumping)
            {
                Physics.ApplyImpulse(new Vector2(0, 0.2f));
                Status.IsJumpAllowed = false;
                Commands.Jump = false;
            }
          
            Status.MoveDirection = Physics.Process(HitBox.Position);
            HitBox.Position += Status.MoveDirection;
        }


        /// <summary>
        /// Sets the environment for the physics body, based on all intersections
        /// </summary>
        /// <param name="intersectingItems"></param>
        private void SetEnvironment(List<IIntersectable> intersectingItems)
        {
            var environment = EnvironmentType.Air;
            foreach (IIntersectable item in intersectingItems)
            {
                if (item is Block && item.HitBox.Contains(HitBox.Center))
                    environment = ((Block)item).Environment;
            }

            Status.Environment = environment;
            Physics.SetEnvironment(environment);
        }

        /// <summary>
        /// handles the collision with intersecting items
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
                Remove = (item is Bullet);
                break;
            }


            /* if enemy is walking and on side is a block, then the enemy has to jump */
            if (report.IsSolidOnSide && MovementType == MovementType.Walk)
            {
                Commands.Jump = true;
            }
            /* if enemy is flying or swimming and on side is a block, then the enemy has to turn around */
            if (report.IsSolidOnSide && (MovementType == MovementType.Fly ||
                MovementType == MovementType.Swim))
            {
                Commands.TurnAround = true;
            }

            /* when enemy can't jump above a block, he has to turn around */
            if (report.Count(i => i.Item.HitBox.Position.Y > HitBox.Position.Y) >= 1)
            {
                Commands.TurnAround = true;
            }


            SetEnvironment(intersectingItems);
            SetEnemyStatus(report);
        }

        private void SetEnemyStatus(CollisionReport report)
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
            

            if (Status.Environment == EnvironmentType.Air)
                Status.IsJumpAllowed = (report.IsBottomWater && report.IsSolidOnSide) || Status.IsGrounded;

        }

    }
}
