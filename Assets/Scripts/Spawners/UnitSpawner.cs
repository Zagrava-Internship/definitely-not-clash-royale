using Units;
using UnityEngine;

namespace Spawners
{
    public class UnitSpawner : MonoBehaviour
    {
        public MapManager mapManager;
        public static UnitSpawner Instance { get; private set; }

        public Vector2Int followingPosition = Vector2Int.zero;
    
        public void Spawn(UnitData unitData, Vector3 position)
        {
            var obj = Instantiate(unitData.prefab, position, Quaternion.identity);
            obj.GetComponent<Unit>().Initialize(unitData, followingPosition);
        }

    }
}