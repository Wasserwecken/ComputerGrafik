using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Level.Base;
using Lib.Tools;
using Lib.Visuals.Graphics;
using OpenTK;
using Lib.LevelLoader.LevelItems;

namespace Lib.Level.Items
{
    public class Collectable : LevelItemBase, IDrawable, IIntersectable, IInventoryItem, IRemoveable, IMoveable
    {
        /// <summary>
        /// 
        /// </summary>
        public int ZLevel { get; set; }

        /// <summary>
        /// Determines if the collectable can be collected or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// type of the collectable
        /// </summary>
        public ItemType ItemType { get; set; }

        /// <summary>
        /// collission of the block
        /// </summary>
        public bool HasCollisionCorrection { get; set; }

        /// <summary>
        /// Removes the item from the level when set
        /// </summary>
        public bool Remove { get; set; }
        

        /// <summary>
        /// Maximum size which the collectable can reach for the animation
        /// </summary>
        private float AddedAnimatedZoom { get; set; }

        /// <summary>
        /// Amount of steps to reach the end of the animation
        /// </summary>
        private int AnimationLength { get; set; }

        /// <summary>
        /// current step of the animation
        /// </summary>
        private int CurrentAnimationStep { get; set; }

        /// <summary>
        /// Step counter which will be used to calculate the easing for the zoom factor
        /// </summary>
        private int CurrentEasingStep { get; set; }


        /// <summary>
        /// Initialises a clollectable
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="startPosition"></param>
        /// <param name="type"></param>
        public Collectable(ISprite sprite, Vector2 startPosition, ItemType type)
            : base(startPosition, new Vector2(1.5f, 1.5f))
        {
            IsActive = true;
            ItemType = type;

            if (ItemType == ItemType.Medikit)
            {
                HitBox.Size = new Vector2(0.75f, 0.75f);
            }

            ZLevel = 1;
            Sprite = sprite;
            Sprite.SetSize(HitBox.Size);

            AddedAnimatedZoom = .25f;
            AnimationLength = 120;
            CurrentAnimationStep = 0;
            CurrentEasingStep = 0;
        }


        /// <summary>
        /// Draws the collectable on the screen
        /// </summary>
        public void Draw()
        {
            foreach (var attachedSprite in AttachedSprites)
                attachedSprite.Draw(HitBox.Position, new Vector2(1f));


            var CurrentZoom = 1 + (AddedAnimatedZoom * Easing.Linear(CurrentEasingStep, AnimationLength));
            var spriteSizeDiff = (Sprite.Size * CurrentZoom) - Sprite.Size;
            var positionCorrection = new Vector2((spriteSizeDiff.X / 2), 0);

            Sprite.Draw(HitBox.Position - positionCorrection, Vector2.One * CurrentZoom);
        }
        
        /// <summary>
        /// Hovers the collectable to be more visible for the players
        /// </summary>
        public void Move()
        {
            if (CurrentAnimationStep >= AnimationLength)
                CurrentAnimationStep = 0;
            else
                CurrentAnimationStep++;

            if (CurrentAnimationStep < (AnimationLength / 2))
                CurrentEasingStep += 2;
            else
                CurrentEasingStep -= 2;

            CurrentEasingStep = Math.Min(CurrentEasingStep, AnimationLength);
            CurrentEasingStep = Math.Max(CurrentEasingStep, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intersectingItems"></param>
        public void HandleCollisions(List<IIntersectable> intersectingItems) { }

    }
}
