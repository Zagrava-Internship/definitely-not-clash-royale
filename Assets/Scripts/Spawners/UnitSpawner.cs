using Units;
using UnityEngine;

namespace Spawners
{
    public static class UnitSpawner 
    {
        public static void Spawn(UnitConfig unitConfig, Vector3 position, string teamId)
        {
            var obj = Object.Instantiate(unitConfig.prefab, position, Quaternion.identity);
            var unit = obj.GetComponent<Unit>();
            unit.Initialize(unitConfig, teamId);
        }

    }
}