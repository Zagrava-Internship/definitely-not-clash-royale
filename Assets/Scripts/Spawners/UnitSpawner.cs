using Targeting;
using Units;
using Units.Factories;
using UnityEngine;
using Unit = Units.Unit;

namespace Spawners
{
    public static class UnitSpawner 
    {
        public static void Spawn(UnitConfig unitConfig, Vector3 position, Team team)
        {
            if (unitConfig is null)
                throw new System.ArgumentNullException(nameof(unitConfig), "UnitSpawner: unitConfig is null.");
            
            var obj = Object.Instantiate(unitConfig.prefab, position, Quaternion.identity); 
            MovementFactory.AddMovement(obj, unitConfig.movementType);
            AttackFactory.AddAttack(obj, unitConfig.attackType, unitConfig);
            var unit = obj.GetComponent<Unit>();
            
            unit.InitializeUnit(unitConfig, team);
        }

    }
}