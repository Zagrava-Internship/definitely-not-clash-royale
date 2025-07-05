using Units;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public MapManager mapManager;
    public static UnitSpawner Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Vector2Int followingPosition = Vector2Int.zero;

    public void Spawn(UnitData unitData)
    {
        var spawnPoint = mapManager.GetRandomSpawnPoint();
        var obj = Instantiate(unitData.prefab, spawnPoint.position, Quaternion.identity);
        obj.GetComponent<Unit>().Initialize(unitData, followingPosition);
    }
    
    public static void Spawn(UnitData unitData, Vector3 position)
    {
        var obj = Instantiate(unitData.prefab, position, Quaternion.identity);
        obj.GetComponent<Unit>().Initialize(unitData,Instance.followingPosition);
    }

}