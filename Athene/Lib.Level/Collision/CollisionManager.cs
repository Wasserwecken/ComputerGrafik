using Lib.Level.Base;
using Lib.Tools;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Collision
{
    public static class CollisionManager
    {
        /// <summary>
        /// Handles all collisions, returns informations about the alignment of the given blocks
        /// </summary>
        /// <param name="sourceBox"></param>
        /// <param name="intersections"></param>
        /// <returns></returns>
        public static CollisionReport HandleCollisions(Box2D sourceBox, List<LevelItemBase> intersections, Action onCorrectionX, Action onCorrectionY)
        {
            foreach (var item in intersections)
                HandleCollision(sourceBox, item, onCorrectionX, onCorrectionY);

            var report = CreateCollisionReport(sourceBox, intersections);
            report.Analyse();

            return report;
        }

        /// <summary>
	    /// React to a collision with a block, may correcting it and returning the blocks alignment
	    /// </summary>
	    /// <param name="collidingItem">the colliding block</param>
	    private static void HandleCollision(Box2D sourceBox, LevelItemBase collidingItem, Action onCorrectionX, Action onCorrectionY)
        {
            var intersectSize = sourceBox.GetIntersectSize(collidingItem.HitBox);
            float intersectSizeX = intersectSize.X;
            float intersectSizeY = intersectSize.Y;

            // Check now in which direction the physic object has to be corrected. It depends on the center of the boxes.
            // Inverting here the intersectsize to achive the side decision
            // Creating here this vars, because there is a calculation behind the propertie "Center" to avoid multiple execution, by calling the prop
            var ownCenter = sourceBox.Center;
            var otherCenter = collidingItem.HitBox.Center;
            var correctionX = intersectSizeX;
            var correctionY = intersectSizeY;
            if (ownCenter.X < otherCenter.X)
                correctionX *= -1;
            if (ownCenter.Y < otherCenter.Y)
                correctionY *= -1;


            // Corret the position, check first if the collision has to be correct on the x or y axis
            // ignore for this check only the gravity, because this will be very time applied.
            // the intersect with smaller size has to be corrected
            if (collidingItem.Collision && (intersectSizeY > 0 || intersectSizeX > 0))
            {
                if (intersectSizeX > intersectSizeY)
                {
                    //Correct Y axis
                    if (intersectSizeY > 0)
                    {
                        sourceBox.Position = new Vector2(sourceBox.Position.X, sourceBox.Position.Y + correctionY);
                        onCorrectionY();
                    }
                }
                else
                {
                    //Correct X axis
                    if (intersectSizeX > 0)
                    {
                        sourceBox.Position = new Vector2(sourceBox.Position.X + correctionX, sourceBox.Position.Y);
                        onCorrectionX();
                    }
                }
            }
        }

        /// <summary>
        /// Creates a report where the intersection get alignments
        /// </summary>
        /// <param name="intersections"></param>
        /// <returns></returns>
        private static CollisionReport CreateCollisionReport(Box2D sourceBox, List<LevelItemBase> intersections)
        {
            CollisionReport report = new CollisionReport();

            var sourceCenter = sourceBox.Center;
            foreach (var item in intersections)
            {
                var itemCenter = item.HitBox.Center;

                if (Math.Abs(sourceCenter.X - itemCenter.X) > Math.Abs(sourceCenter.Y - itemCenter.Y))
                {
                    if (itemCenter.X > sourceCenter.X)
                        report.Add(new CollisionReportItem(Alignment.Left, item));
                    else
                        report.Add(new CollisionReportItem(Alignment.Right, item));
                }
                else
                {
                    if (itemCenter.Y > sourceCenter.Y)
                        report.Add(new CollisionReportItem(Alignment.Top, item));
                    else
                        report.Add(new CollisionReportItem(Alignment.Bottom, item));
                }
            }

            return report;
        }
    }
}
