using Units;
using UnityEngine;

namespace Spawners
{
    public static class UnitSpawner 
    {
        public static void Spawn(UnitData unitData, Vector3 position, string teamId)
        {
            var obj = Object.Instantiate(unitData.prefab, position, Quaternion.identity);
            var unit = obj.GetComponent<Unit>();
            unit.Initialize(unitData, teamId);
        }

    }
}