using Units;
using UnityEngine;

namespace Spawners
{
    public static class UnitSpawner 
    {
        public static void Spawn(UnitData unitData, Vector3 position)
        {
            var obj = Object.Instantiate(unitData.prefab, position, Quaternion.identity);
            obj.GetComponent<Unit>().Initialize(unitData);
        }

    }
}