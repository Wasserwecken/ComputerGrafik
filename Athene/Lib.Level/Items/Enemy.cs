using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Level.Base;
using Lib.LevelLoader.LevelItems;
using Lib.Tools;
using Lib.Visuals.Graphics;
using OpenTK;

namespace Lib.Level.Items
{
    public class Enemy : LevelItemBase, IDrawable, IMoveable, ICreateable, IIntersectable, IInteractable
    {
        /// <summary>
        /// the type of the enemy
        /// </summary>
        public EnemyType EnemyType { get; set; }

        /// <summary>
        /// Range where interactions can be requested by other items
        /// </summary>
        public Box2D InteractionBox { get; set; }

        /// <summary>
        /// collission of the enemy
        /// </summary>
        public bool HasCollisionCorrection { get; set; }

        /// <summary>
        /// initializes a player
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="sprite"></param>
        /// <param name="enemyType"></param>
        public Enemy(Vector2 startPosition, SpriteAnimated sprite, EnemyType enemyType) : base(startPosition, new Vector2(0.75f, 0.75f))
        {
            Sprite = sprite;
            EnemyType = enemyType;

            InteractionBox = HitBox;
        }

        public void Draw()
        {
            Sprite.Draw(HitBox.Position, new Vector2(0.8f));
        }

        public void Move()
        {
            
        }

        public List<LevelItemBase> GetCreatedItems()
        {
            return new List<LevelItemBase>();
        }

        public void ClearCreatedItems()
        {
            
        }

        /// <summary>
        /// handles the collision with intersecting items
        /// </summary>
        /// <param name="intersectingItems"></param>
        public void HandleCollisions(List<IIntersectable> intersectingItems)
        {
            
        }

        /// <summary>
        /// handles the interactions with intersectable items
        /// </summary>
        /// <param name="interactableItem"></param>
        public void HandleInteractions(List<IIntersectable> interactableItem)
        {
           
        }
    }
}
