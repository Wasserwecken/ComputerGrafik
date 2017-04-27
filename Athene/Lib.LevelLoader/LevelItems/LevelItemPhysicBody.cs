using Lib.Tools;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Lib.LevelLoader.LevelItems
{
    public class LevelItemPhysicBody
		: LevelItemBase
	{
		/// <summary>
		/// Boundaries of energy that is allowed
		/// </summary>
		public Vector2 EnergyLimit { get; set; }

		/// <summary>
		/// Current energy of the object
		/// </summary>
		public Vector2 Energy { get; private set; }

		/// <summary>
		/// Current used behaviour
		/// </summary>
		public LevelItemPhysicBodyProperties CurrentProperties { get; private set; }

		/// <summary>
		/// Current used environment
		/// </summary>
		public BlockType CurrentEnvironment { get; private set; }

        /// <summary>
        /// Environment which will be set if there is no collision
        /// </summary>
        public BlockType DefaultEnvironment { get; set; }


		/// <summary>
		/// Available environments with the object behaviours
		/// </summary>
		private Dictionary<BlockType, LevelItemPhysicBodyProperties> Properties { get; set; }
        
		/// <summary>
		/// Last invoked force
		/// </summary>
		private Vector2 LastProcessedForce { get; set; }

		/// <summary>
		/// This force will be processed every step and reseted afterwords
		/// </summary>
		private Vector2 ForceToProcess { get; set; }


		/// <summary>
		/// Initialises a forceable object
		/// </summary>
		public LevelItemPhysicBody(Dictionary<BlockType, LevelItemPhysicBodyProperties> properties, BlockType defaultEnvironment, Vector2 startPosition)
			: base(startPosition, new Vector2(0.75f, 0.75f))
		{
			Properties = properties;
			EnergyLimit = Vector2.Zero;
			Energy = Vector2.Zero;

            DefaultEnvironment = defaultEnvironment;
            CurrentEnvironment = defaultEnvironment;
            CurrentProperties = Properties[defaultEnvironment];
        }

		/// <summary>
		/// Updates all values for one step
		/// </summary>
		public void UpdateLogic()
		{
            //multiply the speed with the current force to get the fitting speed for the enivornment
            ForceToProcess = ForceToProcess * CurrentProperties.MovementSpeed;

			//converts the force to energy and adding gravity
			//This will build up energy like "pusching" something
			SetEnergy(new Vector2(ForceToProcess.X, ForceToProcess.Y - CurrentProperties.Mass));
			ForceToProcess = Vector2.Zero;

			//Apply force, calculate x and y again for easing
			//energy is used as reference
			float x = GetEasingValue(LastProcessedForce.X, Energy.X) * Energy.X;
			float y = GetEasingValue(LastProcessedForce.Y, Energy.Y) * Energy.Y;

			//Limit the energy
			if (EnergyLimit.X > 0)
				x = x.LimitToRange(-EnergyLimit.X, -EnergyLimit.X);

			if (EnergyLimit.Y > 0)
				y = y.LimitToRange(-EnergyLimit.Y, -EnergyLimit.Y);

			//Set the new position
			Position = Position + new Vector2(x, y);

            // update the 2DBox
            HitBox.Postion = Position;

            //setting the default environment
            SetEnvironment(DefaultEnvironment);
        }

		/// <summary>
		/// Sets the enivornment for the object
		/// </summary>
		/// <param name="environment"></param>
		public void SetEnvironment(BlockType environment)
		{
			CurrentEnvironment = environment;
			CurrentProperties = Properties[environment];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="impuls"></param>
		public void ApplyImpulse(Vector2 impuls)
		{
			//An implus does not build up energy, its directly added
			Energy = Energy + impuls;
			SetLastProcessedForce(impuls);
		}

		/// <summary>
		/// Applys a force to the object like pushing it.
		/// </summary>
		/// <param name="force"></param>
		public void ApplyForce(Vector2 force)
		{
			ForceToProcess = ForceToProcess + force;
		}

        /// <summary>
        /// Stops the body immidiatly on the x axis, by removing all energy and force
        /// </summary>
        public void StopBodyOnAxisX()
        {
            ForceToProcess = new Vector2(0, ForceToProcess.Y);
            Energy = new Vector2(0, Energy.Y);
            LastProcessedForce = new Vector2(0, LastProcessedForce.Y);
        }

        /// <summary>
        /// Stops the body immidiatly on the x axis, by removing all energy and force
        /// </summary>
        public void StopBodyOnAxisY()
        {
            ForceToProcess = new Vector2(ForceToProcess.X, 0);
            Energy = new Vector2(Energy.X, 0);
            LastProcessedForce = new Vector2(LastProcessedForce.X, 0);
        }


        /// <summary>
        /// Sets the new energy by a given force. Limits the maximums to the given force
        /// </summary>
        /// <param name="forceToProcess"></param>
        private void SetEnergy(Vector2 forceToProcess)
		{
			//sets the reference, ignoring 0's in x and y
			SetLastProcessedForce(forceToProcess);

			//Calc energy, also resets the last processed force if there is no input anymore and all energy has been build down
			float newEnergyX = ManipulateEnergy(Energy.X, forceToProcess.X, LastProcessedForce.X, CurrentProperties.Momentum.X);
			if (Math.Abs(newEnergyX) <= 0)
				LastProcessedForce = new Vector2(0, LastProcessedForce.Y);

			float newEnergyY = ManipulateEnergy(Energy.Y, forceToProcess.Y, LastProcessedForce.Y, CurrentProperties.Momentum.Y);
			if (Math.Abs(newEnergyY) <= 0)
				LastProcessedForce = new Vector2(LastProcessedForce.X, 0);

			Energy = new Vector2(newEnergyX, newEnergyY);
		}

		/// <summary>
		/// Caclulates from the energy level the easing value for a smooth movement
		/// </summary>
		/// <param name="lastProcessedForce"></param>
		/// <param name="energy"></param>
		private float GetEasingValue(float lastProcessedForce, float energy)
		{
			float easingValue;

			if (Math.Abs(energy) <= 0)
				easingValue = 0;
			else
			{
				float easingStep = Math.Abs(energy / lastProcessedForce);
				easingStep = easingStep.LimitToRange(0, 1);

				easingValue = Easing.CubicOut(easingStep, 1);
			}

			return easingValue;
		}

		/// <summary>
		/// Sets the last given force, if the force should be 0 in x or y, the force remains the same
		/// </summary>
		/// <param name="forceToProcess"></param>
		private void SetLastProcessedForce(Vector2 forceToProcess)
		{
			float newForceRefX = LastProcessedForce.X;
			float newForceRefY = LastProcessedForce.Y;

			if (Math.Abs(forceToProcess.X) > Math.Abs(LastProcessedForce.X))
				newForceRefX = forceToProcess.X;

			if (Math.Abs(forceToProcess.Y) > Math.Abs(LastProcessedForce.Y))
				newForceRefY = forceToProcess.Y;

			LastProcessedForce = new Vector2(newForceRefX, newForceRefY);
		}

		/// <summary>
		/// Decides if the energy has to be increasing or decreasing. Also determines the values and speed for that
		/// </summary>
		/// <param name="forceToProcess"></param>
		/// <param name="energy"></param>
		/// <param name="lastProcessedForce"></param>
		/// <param name="momentum"></param>
		/// <returns></returns>
		private float ManipulateEnergy(float energy, float forceToProcess, float lastProcessedForce, float momentum)
		{
            //Calculating how much energy will be added for one tick, more momentum -> less energy
			float stepSize = lastProcessedForce / momentum;

			//Check for increasing or decreasing the energy
			if (forceToProcess * energy >= 0 && Math.Abs(forceToProcess) >= Math.Abs(energy))
			{
				energy += stepSize;

				if (energy > 0 && energy > lastProcessedForce)
					energy = lastProcessedForce;
				if (energy < 0 && energy < lastProcessedForce)
					energy = lastProcessedForce;
			}
			else
				energy = energy.ReduceToZero(stepSize);
			
			return energy;
		}

	    /// <summary>
	    /// React to a collision with a block
	    /// </summary>
	    /// <param name="collidingBlock">the colliding block</param>
        /// <returns>Returns true if the body got hit from bottom, fals if it is another direction</returns>
	    public override CollisionInformation HandleCollision(LevelItemBase collidingBlock)
        {
            var infos = new CollisionInformation() { CollisionOnBottom = false, CollisionOnLeft = false, CollisionOnRight = false, CollisionOnTop = false };
            var myBox = HitBox;
            var otherBox = collidingBlock.HitBox;
            float intersectSizeX = 0;
            float intersectSizeY = 0;


            // calculate the intersect of the two boxes, for later corrections
            // y_overlap = y12 < y21 || y11 > y22 ? 0 : Math.min(y12, y22) - Math.max(y11, y21);
            // x_overlap = x12 < x21 || x11 > x22 ? 0 : Math.min(x12, x22) - Math.max(x11, x21),
            // https://math.stackexchange.com/questions/99565/simplest-way-to-calculate-the-intersect-area-of-two-rectangles
            if (!(myBox.MaximumX < otherBox.Postion.X || myBox.Postion.X > otherBox.MaximumX))
                intersectSizeX = Math.Min(myBox.MaximumX, otherBox.MaximumX) - Math.Max(myBox.Postion.X, otherBox.Postion.X);

            if (!(myBox.MaximumY < otherBox.Postion.Y || myBox.Postion.Y > otherBox.MaximumY))
                intersectSizeY = Math.Min(myBox.MaximumY, otherBox.MaximumY) - Math.Max(myBox.Postion.Y, otherBox.Postion.Y);

            //Check if the collision has to be corrected
            if (collidingBlock.Collision && (intersectSizeX > 0 || intersectSizeY > 0))
            {
                // Check now in which direction the physic object has to be corrected. It depends on the center of the boxes.
                // Inverting here the intersectsize to achive the side decision
                // Creating here this vars, because there is a calculation behind the propertie "Center" to avoid multiple execution, by calling the prop
                var ownCenter = HitBox.Center;
                var otherCenter = collidingBlock.HitBox.Center;
                if (ownCenter.X < otherCenter.X)
                    intersectSizeX *= -1;
                if (ownCenter.Y < otherCenter.Y)
                    intersectSizeY *= -1;

                // Corret the position, check first if the collision has to be correct on the x or y axis
                // ignore for this check only the gravity, because this will be very time applied.
                // the intersect with smaller size has to be corrected
                if (Math.Abs(intersectSizeX) > Math.Abs(intersectSizeY))
                {
                    //Correct Y axis
                    Position = new Vector2(Position.X, Position.Y + intersectSizeY);
                    HitBox.Postion = new Vector2(Position.X, Position.Y + intersectSizeY);
                    StopBodyOnAxisY();

                    infos.CollisionOnBottom = (intersectSizeY > 0);
                    infos.CollisionOnTop = (intersectSizeY < 0);
                }
                else
                {
                    //Correct X axis
                    Position = new Vector2(Position.X + intersectSizeX, Position.Y);
                    HitBox.Postion = new Vector2(Position.X + intersectSizeX, Position.Y);
                    StopBodyOnAxisX();

                    infos.CollisionOnLeft = (intersectSizeX > 0);
                    infos.CollisionOnRight = (intersectSizeX < 0);
                }
            }
            
            return infos;
        }
    }
}
