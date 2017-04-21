using OpenTK;
using System;
using System.Collections.Generic;
using Lib.Logic;

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
		/// Available environments with the object behaviours
		/// </summary>
		private Dictionary<BlockType, LevelItemPhysicBodyProperties> Properties { get; set; }

		/// <summary>
		/// Current energy of the object
		/// </summary>
		private Vector2 Energy { get; set; }

		/// <summary>
		/// Current used environment
		/// </summary>
		private Enum CurrentEnvironment { get; set; }

		/// <summary>
		/// Current used behaviour
		/// </summary>
		private LevelItemPhysicBodyProperties CurrentProperties { get; set; }

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
		public LevelItemPhysicBody(Dictionary<BlockType, LevelItemPhysicBodyProperties> properties, BlockType startEnvironment, Vector2 startPosition)
			: base(startPosition)
		{
			Properties = properties;
			EnergyLimit = Vector2.Zero;
			Energy = Vector2.Zero;

			SetEnvironment(startEnvironment);
		}

		/// <summary>
		/// Updates all values for one step
		/// </summary>
		public void UpdateLogic()
		{
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
		}

		/// <summary>
		/// Sets the enivornment for the object
		/// </summary>
		/// <param name="environment"></param>
		public void SetEnvironment(BlockType environment)
		{
			//check for the environment
			if (!environment.Equals(CurrentEnvironment))
			{
				CurrentEnvironment = environment;
				CurrentProperties = Properties[environment];
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="impuls"></param>
		public void ApplyImpulse(Vector2 impuls)
		{
			//An implus does not build up energy, its directly added
			Energy = Energy + impuls;
			SetForceReference(impuls);
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
		/// Sets the new energy by a given force. Limits the maximums to the given force
		/// </summary>
		/// <param name="force"></param>
		private void SetEnergy(Vector2 force)
		{
			//sets the reference, ignoring 0's in x and y
			SetForceReference(force);

			//Calc energy
			float newEnergyX = ManipulateEnergy(Energy.X, force.X, LastProcessedForce.X, CurrentProperties.Momentum.X);
			if (Math.Abs(newEnergyX) <= 0)
				LastProcessedForce = new Vector2(0, LastProcessedForce.Y);

			float newEnergyY = ManipulateEnergy(Energy.Y, force.Y, LastProcessedForce.Y, CurrentProperties.Momentum.Y);
			if (Math.Abs(newEnergyY) <= 0)
				LastProcessedForce = new Vector2(LastProcessedForce.X, 0);

			Energy = new Vector2(newEnergyX, newEnergyY);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="forceReference"></param>
		/// <param name="energy"></param>
		private float GetEasingValue(float forceReference, float energy)
		{
			float easingValue;

			if (Math.Abs(energy) <= 0)
				easingValue = 0;
			else
			{
				float easingStep = Math.Abs(energy / forceReference);
				easingStep = easingStep.LimitToRange(0, 1);

				easingValue = Easing.CubicOut(easingStep, 1);
			}

			return easingValue;
		}

		/// <summary>
		/// Sets the last given force, if the force should be 0 in x or y, the force remains the same
		/// </summary>
		/// <param name="force"></param>
		private void SetForceReference(Vector2 force)
		{
			float newForceX = LastProcessedForce.X;
			float newForceY = LastProcessedForce.Y;

			if (Math.Abs(force.X) > Math.Abs(LastProcessedForce.X))
				newForceX = force.X;

			if (Math.Abs(force.Y) > Math.Abs(LastProcessedForce.Y))
				newForceY = force.Y;

			LastProcessedForce = new Vector2(newForceX, newForceY);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="force"></param>
		/// <param name="energy"></param>
		/// <param name="forceReference"></param>
		/// <param name="momentum"></param>
		/// <returns></returns>
		private float ManipulateEnergy(float energy, float force, float forceReference, float momentum)
		{
			float stepSize = forceReference / momentum;

			//Check for increasing or decreasing the energy
			if (force * energy >= 0 && Math.Abs(force) >= Math.Abs(energy))
			{
				energy += stepSize;
				if (energy > 0 && energy > forceReference)
					energy = forceReference;
				if (energy < 0 && energy < forceReference)
					energy = forceReference;
			}
			else
				energy = energy.ReduceToZero(stepSize);
			
			return energy;
		}
	}
}
