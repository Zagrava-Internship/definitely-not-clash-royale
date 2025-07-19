using Units;
using UnityEngine;

namespace Targeting
{
    public static class TargetFinder
    {
        /// <summary>
        /// Finds the closest valid enemy target for the unit (by Team).
        /// </summary>
        public static ITargetable FindClosestEnemy(Vector3 position, Team myTeam)
        {
            ITargetable closest = null;
            var minDistSq = float.MaxValue;
            foreach (var target in TargetRegistry.AllTargets)
            {
                if (target.IsDead) continue;
                if (target.Team == myTeam) continue;
                
                var distSq = (target.Transform.position - position).sqrMagnitude;
                if (!(distSq < minDistSq)) continue;
                
                minDistSq = distSq;
                closest = target;
            }
            return closest;
        }

    }
}