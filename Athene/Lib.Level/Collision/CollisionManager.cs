using Lib.Level.Base;
using Lib.Level.Items;
using Lib.LevelLoader.LevelItems;
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
        public static CollisionReport HandleCollisions(Box2D sourceBox, List<IIntersectable> intersections)
        {
            var report = new CollisionReport();

            foreach (var item in intersections)
                report.Add(HandleCollision(sourceBox, item));

            EvaluateReport(sourceBox, report);

            return report;
        }

        /// <summary>
	    /// React to a collision with a block, may correcting it and returning the blocks alignment
	    /// </summary>
	    /// <param name="collidingItem">the colliding block</param>
	    private static CollisionReportItem HandleCollision(Box2D sourceBox, IIntersectable collidingItem)
        {
            var intersectSize = sourceBox.GetIntersectSize(collidingItem.HitBox);
            float intersectSizeX = intersectSize.X;
            float intersectSizeY = intersectSize.Y;
            bool correctedVertical = false;
            bool correctedHorizontal = false;

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
            if (collidingItem.HasCollisionCorrection && (intersectSizeY > 0 || intersectSizeX > 0))
            {
                if (intersectSizeX > intersectSizeY)
                {
                    //Correct Y axis
                    if (intersectSizeY > 0)
                    {
                        sourceBox.Position = new Vector2(sourceBox.Position.X, sourceBox.Position.Y + correctionY);
                        correctedVertical = true;
                    }
                }
                else
                {
                    //Correct X axis
                    if (intersectSizeX > 0)
                    {
                        sourceBox.Position = new Vector2(sourceBox.Position.X + correctionX, sourceBox.Position.Y);
                        correctedHorizontal = true;
                    }
                }
            }

            return new CollisionReportItem(collidingItem, correctedVertical, correctedHorizontal);
        }

        /// <summary>
        /// Creates a report where the intersection get alignments
        /// </summary>
        /// <param name="intersections"></param>
        /// <returns></returns>
        private static void EvaluateReport(Box2D sourceBox, CollisionReport report)
        {
            var sourceCenter = sourceBox.Center;

            foreach (var reportItem in report)
            {
                EvaluateAlignment(sourceCenter, reportItem);
                EvaluateSurroundings(report, reportItem);

                report.CorrectedVertical = report.CorrectedVertical || reportItem.CausedVerticalCorrection;
                report.CorrectedHorizontal = report.CorrectedHorizontal || reportItem.CausedHorizontalCorrection;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportItem"></param>
        private static void EvaluateSurroundings(CollisionReport report, CollisionReportItem reportItem)
        {
            if (reportItem.Item is Block blockItem)
            {
                switch(reportItem.ItemAlignment)
                {
                    case Alignment.Bottom:
                        if (blockItem.Environment == EnvironmentType.Water)
                            report.IsBottomWater = true;

                        if (blockItem.Environment == EnvironmentType.Solid)
                            report.IsSolidOnBottom = true;
                        break;

                    case Alignment.Left:
                    case Alignment.Right:
                        if (blockItem.Environment == EnvironmentType.Solid)
                            report.IsSolidOnSide = true;
                        break;
                }
            }

            else if (reportItem.Item.HasCollisionCorrection && reportItem.ItemAlignment == Alignment.Bottom)
                report.IsSolidOnBottom = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        private static void EvaluateAlignment(Vector2 sourceCenter, CollisionReportItem reportItem)
        {
            var itemCenter = reportItem.Item.HitBox.Center;

            if (Math.Abs(sourceCenter.X - itemCenter.X) > Math.Abs(sourceCenter.Y - itemCenter.Y))
            {
                if (itemCenter.X > sourceCenter.X)
                    reportItem.ItemAlignment = Alignment.Left;
                else
                    reportItem.ItemAlignment = Alignment.Right;
            }
            else
            {
                if (itemCenter.Y > sourceCenter.Y)
                    reportItem.ItemAlignment = Alignment.Top;
                else
                    reportItem.ItemAlignment = Alignment.Bottom;
            }
        }
    }
}
