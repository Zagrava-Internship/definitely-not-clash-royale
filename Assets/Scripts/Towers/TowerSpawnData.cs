using System;
using UnityEngine;

namespace Towers
{
    [Serializable]
    public class TowerSpawnData
    {
        public Tower towerPrefab; 
        public Transform spawnPoint;   
        public string teamId = "Team_1"; 
    }
}