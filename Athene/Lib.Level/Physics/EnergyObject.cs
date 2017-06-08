using Lib.LevelLoader.LevelItems;
using Lib.Tools;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Physics
{
    public class EnergyObject
    {
        /// <summary>
        /// Current energy of the object
        /// </summary>
        public Vector2 Energy { get; set; }

        /// <summary>
        /// Current used behaviour
        /// </summary>
        public EnergyObjectProperties CurrentProperties { get; private set; }

        /// <summary>
        /// Current used environment
        /// </summary>
        public EnvironmentType CurrentEnvironment { get; private set; }


        /// <summary>
        /// Available environments with the object behaviours
        /// </summary>
        private Dictionary<EnvironmentType, EnergyObjectProperties> Properties { get; set; }

        /// <summary>
        /// Input which will be processed
        /// </summary>
        private Vector2 Input { get; set; }

        /// <summary>
        /// Last invoked force
        /// </summary>
        private Vector2 LastInput { get; set; }


        /// <summary>
        /// Initialises the energy object
        /// </summary>
        public EnergyObject(Dictionary<EnvironmentType, EnergyObjectProperties> properties)
        {
            Properties = properties;
            Energy = Vector2.Zero;

            SetEnvironment(Properties.First().Key);
        }


        /// <summary>
        /// Changes the properties which the energy object has to use to calculate their values
        /// </summary>
        /// <param name="type"></param>
        public void SetEnvironment(EnvironmentType environment)
        {
            CurrentEnvironment = environment;
            CurrentProperties = Properties[environment];
        }
        
        /// <summary>
        /// Stops the body immidiatly on the x axis, by removing all energy
        /// </summary>
        public void StopOnAxisX()
        {
            Energy = new Vector2(0, Energy.Y);
            LastInput = new Vector2(0, LastInput.Y);
        }

        /// <summary>
        /// Stops the body immidiatly on the x axis, by removing all energy
        /// </summary>
        public void StopOnAxisY()
        {
            Energy = new Vector2(Energy.X, 0);
            LastInput = new Vector2(LastInput.X, 0);
        }

        /// <summary>
        /// Inverts the energy on the x axis to simulate a bounce
        /// </summary>
        public void BounceOnAxisX()
        {
            Energy = new Vector2(Energy.X * -1, Energy.Y);
            LastInput = Energy;
        }

        /// <summary>
        /// Inverts the energy on the x axis to simulate a bounce
        /// </summary>
        public void BounceOnAxisY()
        {
            Energy = new Vector2(Energy.X, Energy.Y * -1);
            LastInput = Energy;
        }

        /// <summary>
        /// Gives an input for the energy
        /// </summary>
        /// <param name="input"></param>
        public void ApplyInput(Vector2 input)
        {
            Input += input;
        }

        /// <summary>
        /// Processes an energy input, returns 
        /// </summary>
        /// <param name="input"></param>
        public void ProcessEnergyInput()
        {
            //multiply the speed with the current force to get the fitting speed for the enivornment
            Input = Input * CurrentProperties.Speed;
            Input = new Vector2(Input.X, Input.Y - CurrentProperties.Mass);

            //Sets the last input reference, 0 values will be ignored
            SetLastInput(Input);

            //Calc energy, also resets the last processed force if there is no input anymore and all energy has been build down
            float newEnergyX = GetNewEnergyValue(Energy.X, Input.X, LastInput.X, CurrentProperties.Momentum.X);
            if (Math.Abs(newEnergyX) <= 0.0001)
            {
                StopOnAxisX();
                newEnergyX = 0;
            }

            float newEnergyY = GetNewEnergyValue(Energy.Y, Input.Y, LastInput.Y, CurrentProperties.Momentum.Y);
            if (Math.Abs(newEnergyY) <= 0.0001)
            {
                StopOnAxisY();
                newEnergyY = 0;
            }

            //Adding the energy level to the position to get the new calculated position
            Input = Vector2.Zero;
            Energy = new Vector2(newEnergyX, newEnergyY);
        }

                /// <summary>
        /// Sets the last given force, if the force should be 0 in x or y, the force remains the same
        /// </summary>
        /// <param name="input"></param>
        public void SetLastInput(Vector2 input)
        {
            float newInputRefX = LastInput.X;
            float newInputRefY = LastInput.Y;

            if (Math.Abs(input.X) > Math.Abs(LastInput.X))
                newInputRefX = input.X;

            if (Math.Abs(input.Y) > Math.Abs(LastInput.Y))
                newInputRefY = input.Y;

            LastInput = new Vector2(newInputRefX, newInputRefY);
        }


        /// <summary>
        /// Calculates the new current energy value
        /// </summary>
        private float GetNewEnergyValue(float currentEnergy, float input, float lastInput, float momentum)
        {
            //Calculating how much energy will be added for one tick, more momentum -> less energy
            float stepSize = lastInput / momentum;

            //Check for increasing or decreasing the energy
            if (input * currentEnergy >= 0 && Math.Abs(input) >= Math.Abs(currentEnergy))
            {
                currentEnergy += stepSize;

                if (currentEnergy > 0 && currentEnergy > lastInput)
                    currentEnergy = lastInput;
                if (currentEnergy < 0 && currentEnergy < lastInput)
                    currentEnergy = lastInput;
            }
            else
                currentEnergy = currentEnergy.ReduceToZero(stepSize);

            return currentEnergy;
        }
    }
}
