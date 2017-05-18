using Lib.LevelLoader.LevelItems;
using Lib.Tools;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Level.Physics
{
    public class PhysicBody
	{
        /// <summary>
        /// Anergy of the body
        /// </summary>
        public Vector2 Energy { get; private set; }

        /// <summary>
        /// Current used environment
        /// </summary>
        public BlockType CurrentEnvironment { get; private set; }


        /// <summary>
        /// Energy which is infuenced by impulses
        /// </summary>
        private EnergyObject ImpulseObject { get; set; }

        /// <summary>
        /// Energy which is affected by force
        /// </summary>
        private EnergyObject ForceObject { get; set; }


        /// <summary>
        /// Initialises a forceable object
        /// </summary>
        public PhysicBody(Dictionary<BlockType, EnergyObjectProperties> impulseProperties, Dictionary<BlockType, EnergyObjectProperties> forceProperties)
		{
            ImpulseObject = new EnergyObject(impulseProperties);
            ForceObject = new EnergyObject(forceProperties);

            SetEnvironment(impulseProperties.First().Key);
        }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="impuls"></param>
		public void ApplyImpulse(Vector2 impuls)
		{
			//An implus does not build up energy, its directly added
			ImpulseObject.Energy += impuls;
            ImpulseObject.SetLastInput(impuls);
		}

		/// <summary>
		/// Applys a force to the object like pushing it.
		/// </summary>
		/// <param name="force"></param>
		public void ApplyForce(Vector2 force)
		{
            ForceObject.ApplyInput(force);
		}

		/// <summary>
		/// Updates the bodys position, in order of applyed force and gravity. Also corrects analysis collisions
		/// </summary>
		public Vector2 Process(Vector2 position)
        {
            ForceObject.ProcessEnergyInput();
            ImpulseObject.ProcessEnergyInput();

            SetEnergy();
            return Energy;
        }

       /// <summary>
        /// Stops the body immidiatly on the x axis, by removing all energy and force
        /// </summary>
        public void StopBodyOnAxisX()
        {
            ImpulseObject.StopOnAxisX();
            ForceObject.StopOnAxisX();
            SetEnergy();
        }

        /// <summary>
        /// Stops the body immidiatly on the x axis, by removing all energy and force
        /// </summary>
        public void StopBodyOnAxisY()
        {
            ImpulseObject.StopOnAxisY();
            ForceObject.StopOnAxisY();
            SetEnergy();
        }

        /// <summary>
        /// Sets the enivornment for the object
        /// </summary>
        /// <param name="environment"></param>
        public void SetEnvironment(BlockType environment)
        {
            CurrentEnvironment = environment;

            ImpulseObject.SetEnvironment(environment);
            ForceObject.SetEnvironment(environment);
        }


        /// <summary>
        /// Calculates the energy of the body from both objects
        /// </summary>
        private void SetEnergy()
        {
            Energy = ImpulseObject.Energy + ForceObject.Energy;
        }
    }
}
