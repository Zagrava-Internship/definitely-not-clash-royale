using Combat;
using Combat.Interfaces;
using Health;
using Targeting;
using Towers.Animation;
using Units;
using Units.Strategies.Attack;
using Units.Strategies.Movement;
using UnityEngine;

namespace Towers
{
    [RequireComponent(typeof(StaticMovement))]
    public class Tower : Unit
    {
        public void InitializeTower(UnitConfig towerConfig, Team team)
        {
            InitializeUnit(towerConfig, team);
        }
        
    }
}