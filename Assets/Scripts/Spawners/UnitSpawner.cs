using Units;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public MapManager mapManager;

    public void Spawn(UnitData unitData)
    {
        var spawnPoint = mapManager.GetRandomSpawnPoint();
        var obj = Instantiate(unitData.prefab, spawnPoint.position, Quaternion.identity);
        obj.GetComponent<Unit>().Initialize(unitData);
    }
    
    public void Spawn(UnitData unitData, Vector3 position)
    {
        var obj = Instantiate(unitData.prefab, position, Quaternion.identity);
        obj.GetComponent<Unit>().Initialize(unitData);
    }

}