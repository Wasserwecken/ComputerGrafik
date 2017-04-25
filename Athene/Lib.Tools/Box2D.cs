using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Lib.Tools
{
    public class Box2D
    {
        /// <summary>
        /// Position of the box in the level
        /// </summary>
        public Vector2 Postion { get; set; }

        /// <summary>
        /// Dimension of the box
        /// </summary>
        public Vector2 Size { get; set; }
        
        /// <summary>
        /// Center point of the box
        /// </summary>
        public Vector2 Center => new Vector2(Postion.X + (0.5f * Size.X), Postion.Y + (0.5f * Size.Y));

        /// <summary>
        /// Absolute left ending of the box
        /// </summary>
        public float MaximumX => Postion.X + Size.X;

        /// <summary>
        /// Absolute top ending of the box
        /// </summary>
        public float MaximumY => Postion.Y + Size.Y;


        /// <summary>
        /// creates an AABR, an 2D axis aligned bounding box
        /// </summary>
        /// <param name="x">left side x coordinate</param>
        /// <param name="y">bottom side y coordinate</param>
        /// <param name="sizeX">width</param>
        /// <param name="sizeY">height</param>
        public Box2D(float x, float y, float sizeX, float sizeY)
        {
            Postion = new Vector2(x, y);
            Size = new Vector2(sizeX, sizeY);
        }


        /// <summary>
        /// Checks if another box intersecs with this box
        /// </summary>
        /// <param name="otherBox"></param>
        /// <returns></returns>
        public bool IntersectsWith(Box2D otherBox)
        {

            bool noXintersect = (MaximumX <= otherBox.Postion.X) || (Postion.X >= otherBox.MaximumX);
            bool noYintersect = (MaximumY <= otherBox.Postion.Y) || (Postion.Y >= otherBox.MaximumY);
            
            return !(noXintersect || noYintersect);
        }

        /// <summary>
        /// Check if a box is inside of this box
        /// </summary>
        /// <param name="otherBox"></param>
        /// <returns></returns>
        public bool IsInside(Box2D otherBox)
        {
            if (Postion.X <= otherBox.Postion.X)
                return false;

            if (MaximumX >= otherBox.MaximumX)
                return false;

            if (Postion.X <= otherBox.Postion.Y)
                return false;

            if (MaximumY >= otherBox.MaximumY)
                return false;

            return true;
        }
    }
}
