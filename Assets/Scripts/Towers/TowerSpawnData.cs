using System;
using Targeting;
using UnityEngine;

namespace Towers
{
    [Serializable]
    public class TowerSpawnData
    {
        public Tower towerPrefab; 
        public Transform spawnPoint;   
        public Team teamId = Team.Team1; 
    }
}