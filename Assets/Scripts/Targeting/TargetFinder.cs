using Units;
using UnityEngine;

namespace Targeting
{
    public static class TargetFinder
    {
        public static ITargetable FindClosestTarget(Vector3 position)
        {
            ITargetable closest = null;
            var minDistSq = float.MaxValue;
            foreach (var target in TargetRegistry.AllTargets)
            {
                if (target.IsDead) continue;
                var distSq = (target.Transform.position - position).sqrMagnitude;
                if (!(distSq < minDistSq)) continue;
                minDistSq = distSq;
                closest = target;
            }
            return closest;
        }

    }
}