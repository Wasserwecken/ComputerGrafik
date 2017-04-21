using OpenTK;
using System;
using System.Collections.Generic;
using Lib.Logic;

namespace Lib.LevelLoader.LevelItems
{
	public class ForceObject
		: LevelItemBase
	{
		/// <summary>
		/// Boundaries of energy that is allowed
		/// </summary>
		public Vector2 EnergyLimit { get; set; }

		/// <summary>
		/// Available environments with the object behaviours
		/// </summary>
		public Dictionary<Enum, ForceObjectProperties> Properties { get; set; }

		/// <summary>
		/// Current energy of the object
		/// </summary>
		public Vector2 Energy { get; set; }


		/// <summary>
		/// Current used environment
		/// </summary>
		private Enum CurrentEnvironment { get; set; }

		/// <summary>
		/// Current used behaviour
		/// </summary>
		private ForceObjectProperties CurrentProperties { get; set; }

		/// <summary>
		/// Last invoked force
		/// </summary>
		public Vector2 ForceReference { get; set; }


		/// <summary>
		/// Initialises a forceable object
		/// </summary>
		/// <param name="properties"></param>
		/// <param name="energyLimit"></param>
		public ForceObject(Dictionary<Enum, ForceObjectProperties> properties, Vector2 energyLimit)
		{
			Properties = properties;
			EnergyLimit = energyLimit;
			Energy = Vector2.Zero;
		}


		/// <summary>
		/// Sets the enivornment for the object
		/// </summary>
		/// <param name="environment"></param>
		public void SetEnvironment(Enum environment)
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
			//An implus does not build up energy, its like a "dash"
			Energy = impuls;
			ApplyForce(impuls);
		}

		/// <summary>
		/// Applys a force to the object like pushing it.
		/// Builds up energy first
		/// </summary>
		/// <param name="force"></param>
		public void ApplyForce(Vector2 force)
		{
			//converts the force to energy and adding gravity
			//This will build up energy like "pusching" something
			SetEnergy(new Vector2(force.X, force.Y - CurrentProperties.Mass));

			//Apply force, calculate x and y again for easing
			//energy is used as reference
			float x = GetEasingValue(ForceReference.X, Energy.X) * Energy.X;
			float y = GetEasingValue(ForceReference.Y, Energy.Y) * Energy.Y;

			//Limit the energy
			CutVectorTo(ref x, ref y, EnergyLimit);

			//Set the new position
			X = X + x;
			Y = Y + y;
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
			float newEnergyX = ManipulateEnergy(Energy.X, force.X, ForceReference.X, CurrentProperties.Momentum.X);
			if (Math.Abs(newEnergyX) <= 0)
				ForceReference = new Vector2(0, ForceReference.Y);

			float newEnergyY = ManipulateEnergy(Energy.Y, force.Y, ForceReference.Y, CurrentProperties.Momentum.Y);
			if (Math.Abs(newEnergyY) <= 0)
				ForceReference = new Vector2(ForceReference.X, 0);

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
			float newForceX = ForceReference.X;
			float newForceY = ForceReference.Y;

			if (Math.Abs(force.X) > Math.Abs(ForceReference.X))
				newForceX = force.X;

			if (Math.Abs(force.Y) > Math.Abs(ForceReference.Y))
				newForceY = force.Y;

			ForceReference = new Vector2(newForceX, newForceY);
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

		/// <summary>
		/// Cuts a vector to the given limits. the limit has to be positive and sets limits for the positive and negative boundaries
		/// </summary>
		/// <returns></returns>
		private void CutVectorTo(ref float x, ref float y, Vector2 limits)
		{
			if (x < -limits.X)
				x = -limits.X;
			else if (x > limits.X)
				x = limits.X;

			if (y < -limits.Y)
				y = -limits.Y;
			else if (y > limits.Y)
				y = limits.Y;
		}
	}
}
