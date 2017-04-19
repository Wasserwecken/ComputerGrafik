using OpenTK;
using System;
using System.Collections.Generic;

namespace Lib.Logic
{
	public class DynamicObject
	{
		/// <summary>
		/// Current energy of the object
		/// </summary>
		public Vector2 Energy { get; set; }

		/// <summary>
		/// Boundaries of energy that is allowed
		/// </summary>
		public Vector2 EnergyLimit { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Vector2 Position { get; set; }


		/// <summary>
		/// Available environments with the object behaviours
		/// </summary>
		private Dictionary<Enum, ObjectProperties> Properties { get; set; }

		/// <summary>
		/// Current used environment
		/// </summary>
		private Enum CurrentEnvironment { get; set; }

		/// <summary>
		/// Current used behaviour
		/// </summary>
		private ObjectProperties CurrentProperties { get; set; }


		/// <summary>
		/// Initialises a dynamic object
		/// </summary>
		/// <param name="properties"></param>
		/// <param name="energyLimit"></param>
		public DynamicObject(Dictionary<Enum, ObjectProperties> properties, Vector2 energyLimit)
		{
			Properties = properties;
			EnergyLimit = energyLimit;
			Energy = Vector2.Zero;
		}

		/// <summary>
		/// Applys a force to the object and moves it
		/// </summary>
		/// <param name="environment"></param>
		/// <param name="force"></param>
		public void ApplyForce(Enum environment, Vector2 force)
		{
			var x = Energy.X;
			var y = Energy.Y;

			if (!environment.Equals(CurrentEnvironment))
			{
				CurrentEnvironment = environment;
				CurrentProperties = Properties[environment];
			}

			//Apply interia
			x = x - (x / CurrentProperties.Interia.X);
			y = y - (y / CurrentProperties.Interia.Y);

			//Apply gravity
			y = y - CurrentProperties.Mass;

			//Apply given force
			x = x + force.X;
			y = y + force.Y;

			//Limit the energy
			if (x < -EnergyLimit.X)
				x = -EnergyLimit.X;
			else if (x > EnergyLimit.X)
				x = EnergyLimit.X;

			if (y < -EnergyLimit.Y)
				y = -EnergyLimit.Y;
			else if (y > EnergyLimit.Y)
				y = EnergyLimit.Y;

			//set new position and maniulate energy
			Energy = new Vector2(x, y);
			Position = Position + Energy;
		}
	}
}
