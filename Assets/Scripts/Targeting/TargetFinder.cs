using Units;
using UnityEngine;

namespace Targeting
{
    public static class TargetFinder
    {
        /// <summary>
        /// Finds the closest valid enemy target for the unit (by TeamId).
        /// </summary>
        public static ITargetable FindClosestTarget(Vector3 position, string myTeamId)
        {
            ITargetable closest = null;
            var minDistSq = float.MaxValue;
            foreach (var target in TargetRegistry.AllTargets)
            {
                if (target.IsDead) continue;
                if (target.TeamId == myTeamId) continue;
                var distSq = (target.Transform.position - position).sqrMagnitude;
                if (!(distSq < minDistSq)) continue;
                minDistSq = distSq;
                closest = target;
            }
            return closest;
        }

    }
}