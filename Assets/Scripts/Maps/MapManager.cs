using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform[] spawnPoints;

    public Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

}
