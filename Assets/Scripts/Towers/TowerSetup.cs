using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Towers
{
    public class TowerSetup: MonoBehaviour
    {
        [Header("Towers Setup")]
        [SerializeField]
        private List<TowerSpawnData> towersToSpawn;

        private void Start()
        {
            foreach (var spawnData in towersToSpawn)
            {
                SpawnTower(spawnData);
            }
        }

        private void SpawnTower(TowerSpawnData data)
        {
            if (data.towerPrefab == null || data.spawnPoint == null)
            {
                Debug.LogWarning("Data for tower spawn is incomplete. TowerPrefab or SpawnPoint is null.");
                return;
            }

            var newTowerInstance = Instantiate(
                data.towerPrefab,
                data.spawnPoint.position,
                data.spawnPoint.rotation
            );
            
            var towerConfig = newTowerInstance.GetComponent<UnitStats>();
            
            //newTowerInstance.Initialize(towerConfig, data.teamId);

            newTowerInstance.name = $"{data.towerPrefab.name} ({data.teamId})";
        }
    }
}