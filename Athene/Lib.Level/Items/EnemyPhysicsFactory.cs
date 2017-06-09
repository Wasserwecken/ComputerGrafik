using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Level.Physics;
using Lib.LevelLoader.LevelItems;

namespace Lib.Level.Items
{
    public class EnemyPhysicsFactory
    {
        /// <summary>
        /// returns the Physics of a enemy type
        /// </summary>
        /// <param name="enemyType"></param>
        /// <returns></returns>
        public static PhysicBody GetPhysicsByEnemyType(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.GreenFly:
                    return GetWalkPhysics();
                case EnemyType.BoneCruncherMonster:
                    return GetWalkPhysics();
                case EnemyType.GreenMonster:
                    return GetSwimPhysics();
                case  EnemyType.GrumpyFly:
                    return GetFlyPhysics();
                default:
                    return null;
            }


        }

        private static PhysicBody GetWalkPhysics()
        {
            var impulseProps = new Dictionary<EnvironmentType, EnergyObjectProperties>
            {
                {EnvironmentType.Air, new EnergyObjectProperties(30f, 30f, 0.1f, 0f)},
                {EnvironmentType.Solid, new EnergyObjectProperties(30f, 30f, 0.1f, 0f)},
                {EnvironmentType.Ladder, new EnergyObjectProperties(10f, 10f, 0.1f, 0f)},
                {EnvironmentType.Water, new EnergyObjectProperties(30f, 30f, 0.06f, 0f)},
                {EnvironmentType.Lava, new EnergyObjectProperties(15f, 15f, 0.025f, 0f)}
            };
            var forceProps = new Dictionary<EnvironmentType, EnergyObjectProperties>
            {
                {EnvironmentType.Air, new EnergyObjectProperties(6f, 30f, 0.1f, 0.2f)},
                {EnvironmentType.Solid, new EnergyObjectProperties(6f, 30f, 0.1f, 0.2f)},
                {EnvironmentType.Ladder, new EnergyObjectProperties(6f, 6f, 0.1f, 0f)},
                {EnvironmentType.Water, new EnergyObjectProperties(30f, 30f, 0.06f, -0.01f)},
                {EnvironmentType.Lava, new EnergyObjectProperties(15f, 15f, 0.025f, 0f)}
            };
            var physics = new PhysicBody(impulseProps, forceProps);

            return physics;
        }

        private static PhysicBody GetFlyPhysics()
        {
            var impulseProps = new Dictionary<EnvironmentType, EnergyObjectProperties>
            {
                {EnvironmentType.Air, new EnergyObjectProperties(30f, 30f, 0.1f, 0f)},
                {EnvironmentType.Solid, new EnergyObjectProperties(30f, 30f, 0.1f, 0f)},
                {EnvironmentType.Ladder, new EnergyObjectProperties(10f, 10f, 0.1f, 0f)},
                {EnvironmentType.Water, new EnergyObjectProperties(30f, 30f, 0.06f, 0f)},
                {EnvironmentType.Lava, new EnergyObjectProperties(15f, 15f, 0.025f, 0f)}
            };
            var forceProps = new Dictionary<EnvironmentType, EnergyObjectProperties>
            {
                {EnvironmentType.Air, new EnergyObjectProperties(6f, 0.01f, 0.1f, 0f)},
                {EnvironmentType.Solid, new EnergyObjectProperties(6f, 30f, 0.1f, 0.2f)},
                {EnvironmentType.Ladder, new EnergyObjectProperties(6f, 6f, 0.1f, 0f)},
                {EnvironmentType.Water, new EnergyObjectProperties(30f, 30f, 0.06f, -0.01f)},
                {EnvironmentType.Lava, new EnergyObjectProperties(15f, 15f, 0.025f, 0f)}
            };
            var physics = new PhysicBody(impulseProps, forceProps);

            return physics;
        }

        private static PhysicBody GetSwimPhysics()
        {
            var impulseProps = new Dictionary<EnvironmentType, EnergyObjectProperties>
            {
                {EnvironmentType.Air, new EnergyObjectProperties(30f, 30f, 0.1f, 0f)},
                {EnvironmentType.Solid, new EnergyObjectProperties(30f, 30f, 0.1f, 0f)},
                {EnvironmentType.Ladder, new EnergyObjectProperties(10f, 10f, 0.1f, 0f)},
                {EnvironmentType.Water, new EnergyObjectProperties(30f, 30f, 0.06f, 0f)},
                {EnvironmentType.Lava, new EnergyObjectProperties(15f, 15f, 0.025f, 0f)}
            };
            var forceProps = new Dictionary<EnvironmentType, EnergyObjectProperties>
            {
                {EnvironmentType.Air, new EnergyObjectProperties(6f, 0.01f, 0.1f, 0f)},
                {EnvironmentType.Solid, new EnergyObjectProperties(6f, 30f, 0.1f, 0.2f)},
                {EnvironmentType.Ladder, new EnergyObjectProperties(6f, 6f, 0.1f, 0f)},
                {EnvironmentType.Water, new EnergyObjectProperties(30f, 30f, 0.06f, 0f)},
                {EnvironmentType.Lava, new EnergyObjectProperties(15f, 15f, 0.025f, 0f)}
            };
            var physics = new PhysicBody(impulseProps, forceProps);

            return physics;
        }
    }
}
