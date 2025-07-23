using Targeting;
using Units;
using Units.Strategies.Movement;
using UnityEngine;

namespace Towers
{
    [RequireComponent(typeof(StaticMovement))]
    public class Tower : Unit,ITower
    {
        public void InitializeTower(UnitConfig towerConfig, Team team)
        {
            InitializeUnit(towerConfig, team);
        }
        
    }
}