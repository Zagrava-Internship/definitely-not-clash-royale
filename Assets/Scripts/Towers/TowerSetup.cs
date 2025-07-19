using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Towers
{
    public class TowerSetup: MonoBehaviour
    {
        [Header("Parent Object for Towers")]
        [SerializeField]
        private Transform parentObject;
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
            if (data.towerPrefab is null || data.spawnPoint is null)
            {
                Debug.LogWarning("Data for tower spawn is incomplete. TowerPrefab or SpawnPoint is null.");
                return;
            }

            var newTowerInstance = Instantiate(
                data.towerPrefab,
                data.spawnPoint.position,
                data.spawnPoint.rotation,
                parentObject.transform
            );
            
            var towerConfig = newTowerInstance.GetComponent<UnitStats>().Config;
            
            newTowerInstance.Initialize(towerConfig, data.teamId);

            newTowerInstance.name = $"{data.towerPrefab.name} ({data.teamId})";
        }
    }
}