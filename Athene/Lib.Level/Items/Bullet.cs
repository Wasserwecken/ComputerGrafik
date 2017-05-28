using Lib.Level.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Lib.Level.Physics;
using Lib.LevelLoader.LevelItems;
using Lib.Level.Collision;
using Lib.Visuals.Graphics;

namespace Lib.Level.Items
{
    public class Bullet
        : LevelItemBase, IIntersectable, IMoveable, IRemoveable, IDrawable
    {
        /// <summary>
        /// 
        /// </summary>
        public bool HasCollisionCorrection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Remove { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private EnergyObject Physics { get; set; }


        /// <summary>
        /// Initialises a new bullet
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="boxSize"></param>
        public Bullet(Vector2 startPosition, Vector2 moveDirection) : base(startPosition, Vector2.One * 0.3f)
        {
            var bulletSpeed = 0.3f;
            var bulletMass = 0.5f;
            var bulletPhysicProps = new Dictionary<EnvironmentType, EnergyObjectProperties>
            {
                {EnvironmentType.Air, new EnergyObjectProperties    (200, 200, bulletSpeed, bulletMass)},
                {EnvironmentType.Ladder, new EnergyObjectProperties (200, 200, bulletSpeed, bulletMass)},
                {EnvironmentType.Lava, new EnergyObjectProperties   (200, 200, bulletSpeed, bulletMass)},
                {EnvironmentType.Solid, new EnergyObjectProperties  (200, 200, bulletSpeed, bulletMass)},
                {EnvironmentType.Water, new EnergyObjectProperties  (200, 200, bulletSpeed, bulletMass)}
            };
            Physics = new EnergyObject(bulletPhysicProps);
            Physics.Energy += moveDirection * bulletSpeed;
            Physics.SetLastInput(moveDirection * bulletSpeed);

            Sprite = new SpriteStatic(HitBox.Size, "Images/Objects/Bullets/bullet.png");
            HasCollisionCorrection = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intersectingItems"></param>
        public void HandleCollisions(List<IIntersectable> intersectingItems)
        {
            var report = CollisionManager.HandleCollisions(HitBox, intersectingItems);

            if (report.CorrectedHorizontal)
                Physics.BounceOnAxisX();
            if (report.CorrectedVertical)
                Physics.BounceOnAxisY();

            foreach(var item in intersectingItems)
            {
                if (item is Teleporter teleporter)
                {
                    HitBox.Position = teleporter.DestinationPosition;
                    //Manipulating the position in the direction where the player is moving, else the 
                    //player would be teleported immidiatly back
                    HitBox.Position += new Vector2(Math.Sign(Physics.Energy.X), Math.Sign(Physics.Energy.Y));
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Move()
        {
            Physics.ProcessEnergyInput();
            HitBox.Position += Physics.Energy;
        }

        public void Draw()
        {
            Sprite.Draw(HitBox.Position, Vector2.One);
        }
    }
}
